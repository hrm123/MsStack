using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace TaskReminder
{
    // ── DTOs ──────────────────────────────────────────────────────────────

    public record OllamaMessage(
        [property: JsonPropertyName("role")]    string Role,
        [property: JsonPropertyName("content")] string Content);

    public record OllamaChatRequest(
        [property: JsonPropertyName("model")]    string Model,
        [property: JsonPropertyName("messages")] List<OllamaMessage> Messages,
        [property: JsonPropertyName("stream")]   bool Stream = true);

    // Partial chunk returned per line when streaming
    file record StreamChunk(
        [property: JsonPropertyName("message")] OllamaMessage? Message,
        [property: JsonPropertyName("done")]     bool Done);

    // ── Client ────────────────────────────────────────────────────────────

    /// <summary>
    /// Thin wrapper around the Ollama /api/chat endpoint.
    /// Supports streaming (token by token) and single-shot calls.
    /// </summary>
    public sealed class OllamaClient : IDisposable
    {
        private readonly HttpClient _http;
        public string Model { get; set; }

        public OllamaClient(string baseUrl = "http://localhost:11434", string model = "llama3")
        {
            Model = model;
            _http = new HttpClient { BaseAddress = new Uri(baseUrl), Timeout = TimeSpan.FromMinutes(5) };
        }

        // ── Health check ──────────────────────────────────────────────────

        /// <summary>Returns true if Ollama is reachable and the model is available.</summary>
        public async Task<(bool ok, string error)> CheckAsync(CancellationToken ct = default)
        {
            try
            {
                var resp = await _http.GetAsync("/api/tags", ct);
                if (!resp.IsSuccessStatusCode)
                    return (false, $"Ollama returned HTTP {(int)resp.StatusCode}");

                var json = await resp.Content.ReadAsStringAsync(ct);
                if (!json.Contains($"\"{Model}\""))
                    return (false, $"Model '{Model}' not found. Run:  ollama pull {Model}");

                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, $"Cannot reach Ollama at {_http.BaseAddress}.\n{ex.Message}");
            }
        }

        // ── Streaming chat ────────────────────────────────────────────────

        /// <summary>
        /// Streams response tokens one by one via IAsyncEnumerable.
        /// Caller assembles tokens into the full reply.
        /// </summary>
        public async IAsyncEnumerable<string> StreamChatAsync(
            List<OllamaMessage> history,
            [EnumeratorCancellation] CancellationToken ct = default)
        {
            var body    = JsonSerializer.Serialize(new OllamaChatRequest(Model, history, Stream: true));
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            using var request  = new HttpRequestMessage(HttpMethod.Post, "/api/chat") { Content = content };
            using var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            using var reader = new System.IO.StreamReader(stream);

            while (!reader.EndOfStream && !ct.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                StreamChunk? chunk = null;
                try { chunk = JsonSerializer.Deserialize<StreamChunk>(line); }
                catch { continue; }

                if (chunk?.Done == true) yield break;

                var token = chunk?.Message?.Content;
                if (!string.IsNullOrEmpty(token))
                    yield return token;
            }
        }

        // ── Non-streaming convenience ─────────────────────────────────────

        /// <summary>Collects the full streamed reply into one string.</summary>
        public async Task<string> ChatAsync(List<OllamaMessage> history, CancellationToken ct = default)
        {
            var sb = new StringBuilder();
            await foreach (var token in StreamChatAsync(history, ct))
                sb.Append(token);
            return sb.ToString();
        }

        public void Dispose() => _http.Dispose();
    }
}

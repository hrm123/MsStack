using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FolderSizeCalculator
{
    public class FolderWalker
    {
        public void Walk(string rootPath, int maxDepth, string outputPath)
        {
            using (var writer = new StreamWriter(outputPath))
            {
                writer.WriteLine($"Folder Size Report for: {rootPath}");
                writer.WriteLine($"Generated on: {DateTime.Now}");
                writer.WriteLine("--------------------------------------------------");

                Console.WriteLine($"Report file created at: {outputPath}");
                ProcessDirectory(rootPath, 0, maxDepth, writer);
            }
            Console.WriteLine("Scan complete.");
        }

        private void ProcessDirectory(string currentPath, int currentDepth, int maxDepth, StreamWriter writer)
        {
            if (currentDepth > maxDepth) return;

            // Show progress for top-level folders to indicate activity
            if (currentDepth <= 1)
            {
                Console.WriteLine($"Scanning: {currentPath} ...");
            }

            long size = GetDirectorySize(currentPath);
            string indentation = new string(' ', currentDepth * 4);
            string folderName = Path.GetFileName(currentPath);
            if (string.IsNullOrEmpty(folderName)) folderName = currentPath;

            writer.WriteLine($"{indentation}{folderName} - {FormatBytes(size)}");
            writer.Flush(); // Ensure content is written immediately

            if (currentDepth < maxDepth)
            {
                try
                {
                    foreach (var directory in Directory.GetDirectories(currentPath))
                    {
                        ProcessDirectory(directory, currentDepth + 1, maxDepth, writer);
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    writer.WriteLine($"{indentation}    [Access Denied: {Path.GetFileName(currentPath)}]");
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"{indentation}    [Error: {ex.Message}]");
                }
            }
        }

        private long GetDirectorySize(string path)
        {
            long size = 0;
            try
            {
                // Add file sizes for current directory
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    size += new FileInfo(file).Length;
                }

                // Add sizes for subdirectories (recursive)
                string[] subDirs = Directory.GetDirectories(path);
                foreach (string subDir in subDirs)
                {
                    size += GetDirectorySize(subDir);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Ignore inaccessible folders
            }
            catch (Exception)
            {
                // Ignore other errors for size calculation
            }
            return size;
        }

        private string FormatBytes(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = bytes;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }
            return string.Format("{0:0.##} {1}", len, sizes[order]);
        }
    }
}

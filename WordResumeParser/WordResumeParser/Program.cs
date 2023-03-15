// See https://aka.ms/new-console-template for more information
using WordResumeParser;

Console.WriteLine("Hello, World!");

ResumeParser resumeParser = new ResumeParser("C:\\Users\\hramm\\Downloads\\RAMMOHAN_HOLAGUNDI.docx");
resumeParser.Parse();
using System;
using System.IO;

namespace FolderSizeCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string folderPath;
            int maxDepth = 3; // Default value

            if (args.Length > 0)
            {
                folderPath = args[0];
                
                if (args.Length > 1)
                {
                    if (int.TryParse(args[1], out int parsedDepth))
                    {
                        maxDepth = parsedDepth;
                    }
                    else
                    {
                        Console.WriteLine("Invalid depth parameter. Using default: 3");
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter the folder path:");
                folderPath = Console.ReadLine();
                
                // Allow user to enter depth interactively if no args provided
                Console.WriteLine("Enter max depth (default 3):");
                string depthInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(depthInput) && int.TryParse(depthInput, out int parsedDepth))
                {
                    maxDepth = parsedDepth;
                }
            }

            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
            {
                Console.WriteLine("Invalid directory path provided.");
                return;
            }

            Console.WriteLine($"Analyzing folder: {folderPath} with max depth: {maxDepth}...");
            
            var walker = new FolderWalker();
            string outputFile = "folder_sizes.txt";
            
            walker.Walk(folderPath, maxDepth, outputFile);
            
            Console.WriteLine("Done.");
        }
    }
}

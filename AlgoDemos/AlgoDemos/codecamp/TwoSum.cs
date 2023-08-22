using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Text;
using System;
using AlgoDemos.ExpressionTree;

class Result
{

    /*
     * Complete the 'icecreamParlor' function below.
     *
     * The function is expected to return an INTEGER_ARRAY.
     * The function accepts following parameters:
     *  1. INTEGER m
     *  2. INTEGER_ARRAY arr
     */

    public static List<int> icecreamParlor(int m, List<int> arr)
    {
        Dictionary<int, string> costIndexArray = new Dictionary<int, string>();
        List<int> response = new List<int>();

        //add array values to hash table for constant time retrieval
        // we can use dictionary since keys will be unique (sincel only one unique
        // solution exists)
        for (int j = 0; j < arr.Count(); j++)
        {
            if (!costIndexArray.ContainsKey(arr[j]))
            {
                costIndexArray.Add(arr[j], (j + 1).ToString()); // add one since we want 
                // to return 1 based index
            }
            else
            {
                costIndexArray[arr[j]] = costIndexArray[arr[j]] + "," + (j + 1).ToString();
            }
        }

        int matchingIndex = -1;
        for (int j = 0; j < arr.Count(); j++)
        {
            if (costIndexArray.ContainsKey(m - arr[j]))
            {
                if (costIndexArray[m - arr[j]].IndexOf(",") == -1)
                {
                    matchingIndex = Int32.Parse(costIndexArray[m - arr[j]]);
                }
                else
                {
                    //take one that is not current index
                    var matching = costIndexArray[m - arr[j]].Split(",");
                    for (int j1 = 0; j1 < matching.Length; j1++)
                    {
                        if (matching[j1] != (j + 1).ToString())
                        {
                            matchingIndex = Int32.Parse(matching[j1]);
                            break;
                        }
                    }
                }
                if (matchingIndex != j + 1) //ignore if mathing index is same as current index
                {
                    response.Add(matchingIndex > j + 1 ? j + 1 : matchingIndex);
                    response.Add(matchingIndex > j + 1 ? matchingIndex : j + 1);
                }
                break;
            }
        }
        return response;
    }

}

class TwoSum
{
    public void Run()
    {
        string fileNameOutput = @"D:\code\MsftStack\AlgoDemos\AlgoDemos\codecamp\twosum_" + System.Guid.NewGuid().ToString() + ".txt";
        TextWriter textWriter = new StreamWriter(fileNameOutput, true);
        string fileName = @"D:\code\MsftStack\AlgoDemos\AlgoDemos\codecamp\twosum_scenarios.txt";  // @"D:\code\MsftStack\AlgoDemos\AlgoDemos\codecamp\twosum_failing_scenario.txt"; 
        string[] lines = File.ReadAllLines(fileName);

        int t = Convert.ToInt32(lines[0]);
        int iter = 1, testcasenumber = 0;
        while (iter < lines.Length)
        {
            testcasenumber++;
            int m = Convert.ToInt32(lines[iter++]);

            int n = Convert.ToInt32(lines[iter++]);

            List<int> arr = lines[iter++].TrimEnd().Split(' ').ToList().Select(arrTemp => Convert.ToInt32(arrTemp)).ToList();
            List<int> result = Result.icecreamParlor(m, arr);
            /*
            if(testcasenumber == 47) {
                Console.WriteLine(String.Join(" ", arr));

            }
            */
            textWriter.WriteLine(String.Join(" ", result));
        }

        textWriter.Flush();
        textWriter.Close();

        
        //validation
            
        string[] generated = File.ReadAllLines(fileNameOutput);
        string[] answer = File.ReadAllLines(@"D:\code\MsftStack\AlgoDemos\AlgoDemos\codecamp\twosum_ExpectedOutput.txt");  // File.ReadAllLines(@"D:\code\MsftStack\AlgoDemos\AlgoDemos\codecamp\twosum_ExpectedOutput_failing.txt");  // 
        if (answer.Length == generated.Length)
        {
            for (int i = 0; i < answer.Length; i++)
            {
                if (generated[i] != answer[i])
                {
                    Console.WriteLine("indexes for" + i + "-" + generated[i] + " - not equals - " + answer[i]);
                    var gnrtd = generated[i].Split(" ");
                    var ansr  = answer[i].Split(" ");
                    string[] currentArray = lines[i * 3 + 3].Split(" ");
                    Console.WriteLine("values - " + (gnrtd.Length > 1 ? (currentArray[int.Parse(gnrtd[0])] + "," + currentArray[int.Parse(gnrtd[1])]) : "") + 
                        " - not equals - " + currentArray[int.Parse(ansr[0])] + "," + currentArray[int.Parse(ansr[1])]);
                }
            }
        }
        
    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.String
{
    public class PhoneCombinations
    {
        List<string> _responses = null;
        Dictionary<char, char[]> _phoneMapping = new Dictionary<char, char[]>
        {
            ['2'] = new char[] {'a', 'b', 'c' },
            ['3'] = new char[] {'d', 'e', 'f' },
            ['4'] = new char[] {'g', 'h', 'i' },
            ['5'] = new char[] {'j', 'k', 'l' },
            ['6'] = new char[] {'m', 'n', 'o' },
            ['7'] = new char[] {'p', 'q', 'r','s' },
            ['8'] = new char[] {'t', 'u', 'v' },
            ['9'] = new char[] {'w', 'x', 'y','z' }
            };
        string _digits;

        /// <summary>
        /// 1ms beats 37.6%. 47.6 MB beats 77.58%.
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public IList<string> LetterCombinationsDFS(string digits)
        {
            _digits = digits;
            _responses = new List<string>();
            RecursiveDFS(0, "");
            return _responses;
        }

        private void RecursiveDFS(int depth, string current)
        {
            if(depth==_digits.Length)
            {
                _responses.Add(current);
                return;
            }
            foreach(char c in _phoneMapping[_digits[depth]])
            {
                RecursiveDFS(depth + 1, current + c);
            }
        }

        /// <summary>
        /// 47.34 MB beats 92%. 5 ms beats 3.56%
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        public IList<string> LetterCombinations(string digits)
        {
            Dictionary<char, (char, char, char, char)> phoneMapping = new();
            phoneMapping['2'] = ('a', 'b', 'c', ' ');
            phoneMapping['3'] = ('d', 'e', 'f', ' ');
            phoneMapping['4'] = ('g', 'h', 'i', ' ');
            phoneMapping['5'] = ('j', 'k', 'l', ' ');
            phoneMapping['6'] = ('m', 'n', 'o', ' ');
            phoneMapping['7'] = ('p', 'q', 'r', 's');
            phoneMapping['8'] = ('t', 'u', 'v', ' ');
            phoneMapping['9'] = ('w', 'x', 'y', 'z');

            Queue<string> temp2 = null;
            var dgits = digits.ToArray();


            var (item1, item2, item3, item4) = phoneMapping[dgits[0]];
            var temp1 = new Queue<string>();
            temp1.Enqueue(item1.ToString());
            temp1.Enqueue(item2.ToString());
            temp1.Enqueue(item3.ToString());
            if (item4 != ' ')
            {
                temp1.Enqueue(item4.ToString());
            }
            string cur = "";
            for (int y = 1; y < dgits.Count(); y++)
            {
                var (itm1, itm2, itm3, itm4) = phoneMapping[dgits[y]];
                // only one of the 2 queues will be null
                if (temp1 != null)
                {
                    temp2 = new Queue<string>();
                    while (temp1.Count > 0)
                    {
                        cur = temp1.Dequeue();
                        temp2.Enqueue(cur + itm1);
                        temp2.Enqueue(cur + itm2);
                        temp2.Enqueue(cur + itm3);
                        if (itm4 != ' ')
                        {
                            temp2.Enqueue(cur + itm4);
                        }
                    }
                    temp1 = null;
                }
                else
                {
                    temp1 = new Queue<string>();
                    while (temp2.Count > 0)
                    {
                        cur = temp2.Dequeue();
                        temp1.Enqueue(cur + itm1);
                        temp1.Enqueue(cur + itm2);
                        temp1.Enqueue(cur + itm3);
                        if (itm4 != ' ')
                        {
                            temp1.Enqueue(cur + itm4);
                        }
                    }
                    temp2 = null;
                }
            }
            return temp1 != null ? temp1.ToList() : temp2.ToList();
        }

        public static void Demo()
        {
            PhoneCombinations ps = new PhoneCombinations();
            string[] answer = [];
            answer = ["ad", "ae", "af", "bd", "be", "bf", "cd", "ce", "cf"];
            Console.WriteLine($"Answer answer={string.Join(",",ps.LetterCombinations("23"))} should be {string.Join(",", answer)}");
            // answer = ["g", "h", "i"];
            // Console.WriteLine($"Answer ={string.Join(",", ps.LetterCombinationsDFS("4"))} should be {string.Join(",", answer)}");
        }
    }

}

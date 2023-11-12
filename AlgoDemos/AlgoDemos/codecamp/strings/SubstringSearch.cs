using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.strings
{
    

    public class SubstringSearch
    {



        public static void Testcase()
        {
            

            String[] arr = { "bar", "foo", "the" };
            String text = "barfoofoobarthefoobarman";
            string textTimeLimit = "fxgpwfxehfushbiwzqbrxbrjmrsprlkzlqohynnfsjxbkhajxddweuftoakfnugamwjklwsdgrhszjeyohvudcprezzstfsiccunamtgecbfnffbmcxphnjlmidinwqramhazcjfekmtptudwtufgdqtludbsvsuhbuiddpjpbcexyjvseayhqtvmwwgtqadsxlxabhqejwdigyamyavruqdsmmofmrxjcwmfdemnapviovuovqfrilmvxacjrbvvxhbblponejyuguldkqxvdjsajumcvhxsqytdpjswuqqaldgxwfszokazeobbxyunzpsozkmtdfwoienejggzsgyzwbatzwiamarnmdirigftkvxvbduvlsuhvvcxrkxfmqsbwdsgjazwvxrycmtqgozclczcmykbrccnhutrwtprsgpwulmdbofmqgqegafxfhkmeefazusrdjpxwcaxqhlincmwestlzeydkfgjcycfjrcgvfdnmrvctkyzetlfxlljqrsupwzyqegnjmbxdybwkzvotiustfcmwwglvgllksavgbsjcufovcdqlxlgcpsalznifatruumaxgjbcqqytvqgsbmwsfyeelmllicgeogduqhxgxxbspjmtwkldirjveugowoumxxipehxdgzxwklpgxclfuuayojomziawhjloqjrzkrvucmbaoohijkizvymuimrqxeeqnxpxqmsvyvxojexuqhungiiowbgufmkvkzsanmvlhsztzynrihfqrpgosnixzuulvnoizlvjgwehvryfbfvsubimqqmskgdhvtpxfkglhksedsjgygzujhjcavumdzuztlwywejdqdjvvucvadqlmznmwhbzhvaciyaeeljwqssloujozzclapywzbxiyykgwmlbxwxnrrafnjwmkyympyiozgwxgifeuqftpfggyjnmulyhjwtzucpvkouswawtsemhlvpjefvaqwhasbbpfsmrghjfhhyblmjwhailfdmwbvywwbndtzwcifjrnhtatezdehqmmekoaikylxcgyaihgryvarxwvwbcdroqwyewiqutnyblzmshfteikbeiyhkdyatuyzqipndeneqoqkxwfdgsfmaftmirmemmdeuvtzzjdrzwexujaabhmdofpzwddkrgpldikdtqwtoaqiksincwtqbipzoukfdbtglfjfxgyliqtukdimzbfclwcbrmhuncaahmffudqcvmxgphspkztxwvznjgjyilzrxewrkpwfbvlscghofdldyksksbianjumqsgahchkxvdbbmpipcmqngafikixucjmwpoetvqsqtttikcxnqncwphgardwtpkseywyvosokmhsylrndhpawziyqmwmbgehgchcqaqaoxsmlkcotrqpqzqhnmtovxxbbhdrhvsxvmlgynurfljxmzezlzpiymkztprcllbgznmaaithvcfhbjgplxxjkjlfzekkrlsqagqbokkmopvsbehmbgxnsivrmfddsvwvbowraifpybhzhqniymwqvtjjampsjlpkbnecwlrumavtskeqwcjtvsdvfupoffbishkosdkhffcxrgooizhdrmywgqnnvqwnxiepmoyrwbrnhhdqglvxxwwgmjtjzpyyycpmtofrpweydkqjvmhukfcwowhpnfnnkczdqhuhdaghkwshkbeuggswczlshyfrdhuvoqdtslcdhvmuyqofxhbbintpnbgskcoyuttngwqptcjrvywshavpprhcsnxesnemgxzotrdowngrlelseijtzqytgxxykbmsbogwfjlbytujxhdeqpzrhzlnwknioaapoilvswzqpvcpd";
            string[] arrtimeLimit = new string[] { "fnffbmcxphnjlmidinwqramhazc", "jfekmtptudwtufgdqtludbsvsuh", "buiddpjpbcexyjvseayhqtvmwwg", "tqadsxlxabhqejwdigyamyavruq", "dsmmofmrxjcwmfdemnapviovuov", "qfrilmvxacjrbvvxhbblponejyu", "guldkqxvdjsajumcvhxsqytdpjs", "wuqqaldgxwfszokazeobbxyunzp", "sozkmtdfwoienejggzsgyzwbatz", "wiamarnmdirigftkvxvbduvlsuh", "vvcxrkxfmqsbwdsgjazwvxrycmt", "qgozclczcmykbrccnhutrwtprsg", "pwulmdbofmqgqegafxfhkmeefaz", "usrdjpxwcaxqhlincmwestlzeyd", "kfgjcycfjrcgvfdnmrvctkyzetl", "fxlljqrsupwzyqegnjmbxdybwkz", "votiustfcmwwglvgllksavgbsjc", "ufovcdqlxlgcpsalznifatruuma", "xgjbcqqytvqgsbmwsfyeelmllic", "geogduqhxgxxbspjmtwkldirjve", "ugowoumxxipehxdgzxwklpgxclf", "uuayojomziawhjloqjrzkrvucmb", "aoohijkizvymuimrqxeeqnxpxqm", "svyvxojexuqhungiiowbgufmkvk", "zsanmvlhsztzynrihfqrpgosnix", "zuulvnoizlvjgwehvryfbfvsubi", "mqqmskgdhvtpxfkglhksedsjgyg", "zujhjcavumdzuztlwywejdqdjvv", "ucvadqlmznmwhbzhvaciyaeeljw" }; // [107] should be the answer
            int len = arrtimeLimit.Aggregate(0, (current, next) => next.Length + current);
            int textLen = textTimeLimit.Length;

            SubstringSearch sss = new SubstringSearch();
            var answer = sss.FindSubstring( textTimeLimit, arrtimeLimit);
            Console.WriteLine($"{String.Join(",",answer.ToArray())}");
        }

        private string _s;
        public IList<int> FindSubstring(string s, string[] words)
        {
            _s = s;
            WordPerms(words);
            // WordPermsHeapAlgorithm(words, words.Length, words.Length);
            var answers = SearchWords(_wordPerms.ToArray(), s);
            List<int> answerList = new List<int>();
            foreach (var answer in answers)
            {
                answerList.Add(answer.Item1);
            }
            return answerList.Distinct().ToList();
        }


        private List<string> _wordPerms = new List<string>();
        private void WordPerms(string[] words)
        {
            _wordPerms.Clear();
            bool[] visited = new bool[words.Length];
            Stack<string> targetWord = new Stack<string>();
            Array.Fill(visited, false);
            WordpermsRecursive(words, visited, targetWord, 0);


        }

        List<string> _notThere = new List<string>();
        
        private void WordpermsRecursive(string[] words, bool[] visited, Stack<string> targetWord, int depth)
        {
            string currenConcat = String.Join("", targetWord.Reverse().ToArray());
            // Console.WriteLine($"{currenConcat}");

            if (depth == words.Length)
            {
                _wordPerms.Add(currenConcat);
                return;
            }

            if (targetWord.Count() > 1)
            {
                var res2 = SearchWords(new string[] { currenConcat }, _s);
                if (res2.Count() == 0)
                {
                    _notThere.Add(currenConcat);
                    return;
                }
            }


            for (int i = 0; i < words.Length; i++)
            {
                if (visited[i]) { continue; }
                visited[i] = true;
                targetWord.Push(words[i]);
                WordpermsRecursive(words, visited, targetWord, depth + 1);
                targetWord.Pop();
                visited[i] = false;
            }

        }

        private void WordPermsHeapAlgorithm(string[] words, int size, int n)
        {
            // if size becomes 1 then prints the obtained
            // permutation
            if (size == 1)
                _wordPerms.Add(String.Join("", words.Take(n)));

            for (int i = 0; i < size; i++)
            {
                WordPermsHeapAlgorithm(words, size - 1, n);

                // if size is odd, swap 0th i.e (first) and
                // (size-1)th i.e (last) element
                if (size % 2 == 1)
                {
                    string temp = words[0];
                    words[0] = words[size - 1];
                    words[size - 1] = temp;
                }

                // If size is even, swap ith and
                // (size-1)th i.e (last) element
                else
                {
                    string temp = words[i];
                    words[i] = words[size - 1];
                    words[size - 1] = temp;
                }
            }
        }

        StateNode _root = new StateNode(0);
        static int MAXC = 26; // max number of characters in alphabet
        char[] alphs = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        Dictionary<string, List<Tuple<int, int>>> _searchCache = new Dictionary<string, List<Tuple<int, int>>>();

        public List<Tuple<int, int>> SearchWords(String[] arr, String text)
        {
            string key = String.Join("-", arr) + text;

            if(_searchCache.ContainsKey(key))
            {
                return _searchCache[key];
            }

            List<Tuple<int, int>> wordsMatching = new List<Tuple<int, int>>();
            CreateTrie(arr);


            //search for matches
            var currentState = _root;
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];
                currentState = FindNextState(currentState, ch);
                foreach (var word in currentState.words)
                {
                    wordsMatching.Add(new Tuple<int, int>(i - word.Length + 1, i));
                }

            }
            _searchCache[key] =  wordsMatching;
            return wordsMatching;
        }

        private StateNode FindNextState(StateNode currentState, char ch)
        {
            var answer = currentState;
            int c = ch - 'a';
            while (answer.transitions[c] == null) // if no goto function exists go to failure function .. goes on till we reach root state
            {
                answer = answer.failure;
            }
            return answer.transitions[c]; // could be root node or another node

        }
        private void CreateTrie(string[] arr)
        {
            _root = new StateNode(0);
            int states = 1;
            foreach (string word in arr)
            {
                StateNode current = _root;

                for (int i = 0; i < word.Length; i++)
                {
                    int ch = word[i] - 'a'; // gives 0-25 index of a-z.. so 'c' will be 2
                    if (current.transitions[ch] == null || current.transitions[ch].state == 0)
                    {
                        current.transitions[ch] = new StateNode(states++);
                    }
                    current = current.transitions[ch];

                }
                current.words.Add(word);
            }

            BuildFailureLinks();
        }

        private void BuildFailureLinks()
        {
            // for root state failure links already set in constructor - all go to root state itself if there is not transform for specific character
            //do BFS search to set failure links of nodes level by level
            Queue<StateNode> q = new Queue<StateNode>();
            for (int ch = 0; ch < MAXC; ch++)
            {
                if (_root.transitions[ch].state != 0) // has valid transition from root node
                {
                    q.Enqueue(_root.transitions[ch]);
                    _root.transitions[ch].failure = _root; // first level nodes failure go to root node
                }
            }

            while (q.Count() > 0)
            {
                StateNode currentNode = q.Dequeue();
                for (int ch = 0; ch < MAXC; ch++)
                {
                    if (currentNode.transitions[ch] != null && currentNode.transitions[ch].state != 0)
                    {  // has valid transition from current node- need to get failure function for the child node
                        var failureNode = currentNode.failure;
                        while (failureNode.transitions[ch] == null)
                        {
                            failureNode = failureNode.failure;
                        }
                        currentNode.transitions[ch].failure = failureNode.transitions[ch]; // failurenode could be root node or one of other nodes in climb which have valid transition state
                        q.Enqueue(currentNode.transitions[ch]);
                    }
                }
            }
        }
    }
}

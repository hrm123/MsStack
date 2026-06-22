using AlgoDemos.ints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlgoDemos.String
{
    /// <summary>
    /// fail test case  
    /// </summary>
    public class WordBreakSolution
    {
        Dictionary<int, List<string>> startDict = new();
        Dictionary<int, List<int>> wordEndingsDict = new();
        string _s;
        int _n0based;
        IList<string> _wordDict;


        public bool WordBreak(string s, IList<string> wordDict)
        {
            _s = s;
            _wordDict = wordDict.OrderByDescending(w => w.Length).ToList();
            ReduceString();
            _n0based = _s.Length - 1;
            startDict.Clear();
            wordEndingsDict.Clear();
            //wordDict.SelectMany(w => w.Trim()).ToHashSet();
            //special cases - if all words are longer than s, then return false
            if (!_wordDict.Any(x => x.Length == 1))
            {
                if (_wordDict.All(w => w.Length > s.Length))
                {
                    return false;
                }
                //if all words are even length and s is odd length, then return false   
                if (_wordDict.All(w => w.Length % 2 == 0) && _s.Length % 2 == 1)
                {
                    return false;
                }

                // if all words are odd length and s is even length, then return false
                if (_wordDict.All(w => w.Length % 2 == 1) && _s.Length % 2 == 0)
                {
                    return false;
                }
            }

            foreach (string word in _wordDict)
            {
                if (_s == word)
                {
                    return true;
                }
            }

            HashSet<char> searchChars = wordDict.SelectMany(w => w).ToHashSet();
            HashSet<char> allowedChars = _s.ToHashSet();
            if (searchChars.Count < allowedChars.Count)
            {
                return false;
            }

            int firstStart = _s.Length;
            // order words by length so that we follow greedy approach where we first try the longest possible match, and then try shorter matches.
            // wordDict = wordDict.OrderBy(w => w.Length).ToList();
            foreach (var wrd in _wordDict)
            {
                string pattern = @$"(?=({wrd}))";
                MatchCollection matches = Regex.Matches(_s, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    int ms = match.Index;
                    // Console.WriteLine($"Found '{wrd}' at index {ms}");
                    firstStart = ms < firstStart ? ms : firstStart;

                    if (startDict.ContainsKey(ms))
                    {
                        startDict[ms].Add(wrd);
                    }
                    else
                    {
                        startDict[ms] = new List<string> { wrd };
                    }
                    int wordEndingIndex = ms + wrd.Length - 1;
                    wordEndingsDict[wordEndingIndex] =
                        wordEndingsDict.ContainsKey(wordEndingIndex) ? wordEndingsDict[wordEndingIndex].Append(ms).ToList() : new List<int> { ms };


                }
            }

            int end = firstStart;
            if (end != 0)
            {
                return false;
            }


            wordEndingsDict.Where(kvp => kvp.Key == _n0based && kvp.Value.Contains(0)).ToList().ForEach(kvp =>
            {
                Console.WriteLine($"Found full word '{s}' at index {kvp.Key} with start index {kvp.Value.FirstOrDefault()}");
            });

            var tmp= wordEndingsDict.Where(kvp => kvp.Value.Count > 1).ToList();

            int status = MergeIntervals();
            return status==_n0based || ( startDict[0].Count >= 1 && startDict[0].Any(item => item==s));

        }
       

        public void ReduceString()
        {
            // If any word in wordDict repetas 2 times consequively, then we can reduce the string by removing the repeated word.
            // For example, if wordDict contains "ab" and "cd", and s is "ababcd", then we can reduce s to "abcd" by removing the repeated "ab".
            // This is because if we can form "abcd" from wordDict, then we can also form "ababcd" by adding "ab" at the beginning.
            foreach(var word in _wordDict)
            {
                string pattern = @$"({word})\1+";
                MatchCollection matches = Regex.Matches(_s, pattern, RegexOptions.IgnoreCase);
                while (matches.Count > 1)
                {
                    foreach (Match match in matches)
                    {
                        string repeatedWord = match.Groups[1].Value;
                        int repeatCount = match.Length / repeatedWord.Length;
                        if (repeatCount > 1)
                        {
                            _s = _s.Replace(match.Value, repeatedWord);
                            Console.WriteLine($"Reduced string to '{_s}' by removing repeated '{repeatedWord}'");
                        }
                    }

                    matches = Regex.Matches(_s, pattern, RegexOptions.IgnoreCase);
                }
            }
        }

        /// <summary>
        /// Merges ranges on either side of given node and clears the given node. 
        /// Returns the index of the merged node, or -1 if no merge was possible, 
        /// or (length of string -1) if the end of the string was reached.
        /// </summary>
        /// <returns></returns>
        private int MergeOneNodeWithSibling()
        {
            int endIndexToMerge = -1;
            List<string> wordsTomerge = new List<string>();
            for(int y=0; y<wordEndingsDict.Count; y++)
            {
                var kvp = wordEndingsDict.ElementAt(y);
                var endIndex = kvp.Key;
                if (wordEndingsDict[kvp.Key].Count>0) // this end index has one or more words that end here
                {
                    if(endIndex == _n0based)
                    {
                        if(kvp.Value.Count > 0 && kvp.Value.Contains(0))
                        {
                            //the end index has reference to start index => we have found soution
                            return _n0based;
                        }
                        else
                        {
                            continue; //since this is lst index we cannot merge anything
                        }
                    }
                    if (startDict.ContainsKey(endIndex+1) && startDict[endIndex+1].Count > 0)
                    {
                        // there are one or more words that start at this end index, so we can merge the words that end here with the words that start here.
                        endIndexToMerge = endIndex;
                        break;
                    }

                }
            }
            if(endIndexToMerge!=-1)
            {
                Console.WriteLine($"Merging words at index={endIndexToMerge}");
                // merge the words that end at endIndexToMerge with the words that start at endIndexToMerge
                List<string> newWords = new List<string>();
                int[] startIndices = wordEndingsDict[endIndexToMerge].ToArray();
                foreach (int startIndex in startIndices)
                {
                    if(_s.Length<= endIndexToMerge - startIndex + 1)
                    {
                        continue;
                    }
                    string prefix = _s.Substring(startIndex, endIndexToMerge - startIndex + 1);
                    string[] suffixes = startDict[endIndexToMerge + 1].ToArray();
                    foreach (string suffix in suffixes)
                    {
                        string newWord = prefix + suffix;
                        if (newWord == _s)
                        {
                            // the full word has been created
                            return _n0based;
                        }
                        if(startIndex + newWord.Length - 1 > _n0based)
                        {
                            continue;
                        }
                        newWords.Add(newWord);
                        int newWordEndingIndex = startIndex + newWord.Length - 1;
                        var newEnding = wordEndingsDict[newWordEndingIndex]; // add current startIndex to new endIndex of merged word
                        if (newEnding == null)
                        {
                            wordEndingsDict[newWordEndingIndex] = new List<int>();
                            newEnding = wordEndingsDict[newWordEndingIndex];
                        }
                        
                        newEnding.Add(startIndex); // add the current startIndex to ending at startIndex + newWord.Length
                        if (startDict[endIndexToMerge + 1].Count == 0)
                        {
                            newEnding.Remove(newWordEndingIndex - suffix.Length + 1); // remove the previous word start index
                            startDict[endIndexToMerge + 1].Remove(suffix);
                        }
                    }
                    if (suffixes.Length > 0 && prefix.Length>0)
                    {
                        startDict[startIndex].Remove(prefix);
                        startDict[startIndex].AddRange(newWords);
                        wordEndingsDict[endIndexToMerge].Remove(startIndex); // remove the current startIndex from ending at endIndexToMerge
                        Console.WriteLine($"Merged {prefix} with {string.Join(", ", suffixes)} to create {string.Join(", ", newWords)}");
                    }
                    
                }
                return endIndexToMerge;
            }
            return -1;

        }
        private int MergeIntervals()
        {
            var status = MergeOneNodeWithSibling();
            while (status != -1 && status != _n0based)
            {
                if(status == _n0based)
                {
                    return status;
                }
                status = MergeOneNodeWithSibling();
            }
            return status;

        }

        public static void Demo()
        {
            WordBreakSolution wbs = new();
            string[] wordDict;
            string s;

            /*
            
            s = "fajbeokiakfmlacbinjdnjdmmfha";
            wordDict = ["be", "ellaekgjhibcomc", "ahaklkan", "jcm", "lchidklmcone", "ljmdgagaen", "giioojldjkfnno", "el", "eibjaffacjll", "hn", "hbjakhjneml", "foi", "nbhf", "aigf", "cfdlnc", "fa", "amakgofedhkghgl", "ddhmhdhioh", "ijoddeabbiei", "giamcgco", "nholghlfbbendi", "emlc", "m", "elgibme", "behkignjenmh", "lodkkjgioe", "doe", "hgflgakna", "macghogdidmdm", "ec", "kncigolkog", "ljooio", "lch", "gkaclkbjn", "ofiaglfoffl", "alhfbb", "cfmdbgo", "cfcnajknk", "jfh", "almbgdjnbbbgmhb", "dmlnnohf", "olojeejfafc", "ndgcmgoe", "cmkdjilfeo", "bengdd", "enfg", "dbngiggni", "anmkljn", "njdnjdmmfha", "ndimmddfmhe", "hmkdjkhhiilnf", "ehd", "jfdl", "dlgki", "bhoflnomibkki", "lg", "fjojjnnkdfenhol", "lefhhl", "nimdl", "gejgomblmim", "ahbnlmlfmejjj", "glhacaojnf", "mfjdhnhdkm", "do", "fnh", "mnmjdk", "hfjgdlnnb", "hniolfhkhbie", "ldgodonogcab", "mabjnnohnijhn", "aceojlkmdg", "aedfljg", "cehk", "jag", "oniegflnhje", "jo", "maokc", "jkbndc", "djbn", "dajkdblojkf", "dmen", "kcdjdocinenc", "cgindbm", "h", "odaof", "cmogcbgj", "anjahlgbbmba", "haoe", "ggacnminj", "ilcfjoedhe", "klookammgofl", "onnmenn", "mbdneaioo", "jc", "dekgil", "bjdfibfd", "hfbnlgmlllcb", "afebehf", "obekljbnh", "eoaedhjk", "nobamccd", "mdieojoknf", "komcglmakkaa", "jcliimcc", "jmmgbmha", "gdogjnn", "ednembco", "dgno", "jiaheeabifahfmo", "djkcgnakkh", "kdkiglgf", "eb", "fmdnlhj", "eicohdciejc", "jgofmankkf", "o", "nnmomkmkmiaoga", "njchkccln", "ndamha", "eleanmojdi", "ebkl", "jcageehlelcg", "acfddofjihgmn", "iklaomfhjm", "io", "igmob", "lfnhnlnigbbignk", "anihfojmedf", "nj", "oilcabalhb", "adjfbkfjch", "lbfb", "mgfnngfccb", "jhmhggm", "dnllc", "c", "ljim", "jmikd", "mdfimdgac", "fhbclgo", "edclcdia", "nelmfjejff", "i", "cmcbbckdnjcoo", "cddocce", "hc", "keh", "keofhnhemd", "biln", "mjcnbjmkikon", "fekbdnkolahh", "hkfmjbj", "mjoj", "jn", "ilof", "ifhfk", "aofmg", "nofljgmmmf", "hcdifeiclbchlf", "imlijgdg", "ocdiiemcmbkglm", "nhoekmlkjfoa", "kibffkbleedda", "kdhdjekccbkc", "bcbflcag", "jekmkdimnnjjoo", "mmgfljchalbem", "kchk", "oi", "ncf", "jembgfa", "l", "kfkeianmmmdacl", "ecjkkfggj", "jdgcfnhfjonkhig", "jhagiokii", "nifm", "bbjjlj", "adajlokomibfg", "ojk", "lockdel", "bh", "hoojolglchck", "conko", "eadi", "kfigoijnfimolen", "g", "dbnj", "cojkbmo", "hh", "mcdbh", "ngdmgioen", "ehjagfohnolkho", "dgfgdlc", "aoglneoh", "gbc", "ijjckddeicld", "imekih", "liiaecniil", "hahejbhgiclb", "fnmojm", "ablihjhggiahhno", "colloaakco", "jhobddaanbhmlg", "cbfajfhkoh", "cim", "laghknigabn", "dcbnbkegkjam", "gem", "ljjim", "icclogji", "omidhe", "f", "giiaclfcjkagl", "ndcjldekjnkekm", "aiikdccohcj", "mkbmb", "oomhhafobic", "bkacdjfgbggn", "ahghdoahbi", "hedm", "eeoj", "bdgdlfollegej", "eg", "dfeb", "dkffkid", "hcne", "gjkohnaaabn", "jfeododjgdhlfbf", "clfkmconnkfb", "abnbkcni", "hk", "ghnmhjm", "oibjibmkaibdefa", "hjambim", "oe", "aao", "jil", "fmhomflfen", "hlidcklnmb", "hiaonkhd", "bibbmkandf", "hke", "bmfcionm", "inhcnlkbkkmjicn", "jckjedhgoghi", "chmik", "mnjldknhaec", "hocbccbg", "ljadj", "lciikgnlj", "ifjjhkbhifione", "foikblanoco", "ode", "mjc", "fhklofh", "mmoklkkog", "hocbojmhffeajo", "ccmmd", "bkkh", "nhhgcflniebkme", "lfohikenfbjacli", "cmehijnihijgng", "caa", "bmk", "emofof", "jjagiogohfab", "ibh", "eoacdlnodalkjbl", "cbbjbbnjom", "iljiomeloehen", "gignlngclmh", "b", "ll", "dokgngnocde", "cienegffibgieba", "agbachloidg", "mlelnafokd", "nmcmka", "akeogjbjcnf", "nebdic", "a", "efc", "ljdk", "jhcag", "bkbikbjgae", "mcjlgjeo", "lo", "dbiofobl", "cehebiljff", "eeagngm", "ondahcjiel", "coblanndhlhoggj", "jaobmjml", "jfejjinofek", "hhnna", "gdhcn", "acelcomgkgohm", "njkkjkkln", "jmc", "hkekoho", "boefec", "cioibfgjmhb", "ebggdbeimn", "emhg", "cfghkhii", "d", "k", "khoddedia", "nhje", "eebkfml", "bohhd", "kg", "n", "ilgemokdehcbaif", "cldicda", "einij", "akmabcgfn", "fmkmcn", "bnlfbagkke", "oakbgjejmcncj", "iehdfadgoik", "kkcfo", "jmjkmfcacjjnd", "ndokhh", "hjfeelhckkjjmj", "dnomohejbodkb", "jcmblncjadno", "oiofcodobiml", "ddmillkncjfdfj", "aihenmkdnhdhkhf", "bfdbakeilfdojnc", "jjhbkbne", "aigabk", "cae", "oednojjb", "gdoe", "jokjceohkmbm", "offkanbahigo", "kfomigbfddjli", "dkkjobgkcejei", "mdilld", "bofkika", "kkinig", "cljcflbghjmhmke", "kmbjlgdcbdjn", "bkgbmoahda", "kmnajjdemggnfg", "mgjndldil", "iemb", "kehaokgjg", "icign", "oijmaolehmoo", "amhgldifmgekhe", "diacnollhi", "lnjhdaafadl", "bdfiackhogoje", "ebjlfa", "deabkgfhnead", "gadcob", "haa", "dbhnbhjcmmab", "bbmjainilbbej", "dc", "bgjgafnjjflne", "ehholgnn", "fmhccbnc", "mdnfl", "feeejdgc", "mfhlobdadooh", "ojna", "gkgjnijdbgo", "ghngnhn", "nhnjaiaadiedgg", "nk", "hmd", "nmbmijaffogl", "onkcgbgmago", "gfli", "ofjlec", "nlfnbkkdc", "hakilani", "bofjdjkhllb", "ocjncleljnecfc", "gdonnkodmkejhf", "fiflchanfllgnf", "kaejakoibgln", "hmdlfioacgaci", "honmfbcog", "mlacbi", "gf", "ejbbemoeha", "acfjegee", "lllflaocnnkeadi", "mdgoebfgacecmbg", "faejgln", "kmlmhffgcmekm", "akcjmgdg", "kmhhh", "fdohjehacdln", "e", "ojba", "ohadmod", "eaenkdiaokl", "dii", "cgfjaklblafeifo", "imoeflkcgbbem", "nbjkmb", "jjgm", "hofgelg", "cnihecmdigdgflg", "fnmikkeldjgb", "onlhgonldjaedh", "fmkdn", "kfbcbleen", "oejioibnmab", "cg", "meadghbocjnj", "hmmdnkegfeieijn", "ijgenomhndlje", "maccdcgfjig", "iabemie", "mlfg", "mdblmdaechmeaml", "dhlafgjo", "eabbiila", "kf", "oehggehfmijlfmg", "klljaejidfhbon", "akmbgmignoag", "jgbkngmigdfm", "kjeelnbn", "ajaa", "mlcjoiaahoiga", "oalnielba", "ffmobgkc", "kmhoknfffdmo", "nagjiffnjhh", "dlehllomjok", "agaejefhdbk", "nnegoijfdj", "ndl", "dhfginocgi", "nflmglgh", "bcd", "gbgjijemmdio", "jk", "gidgjbmb", "hi", "lmgoah", "fdebefcech", "ach", "bahaoj", "ccmmblgibgjahi", "moid", "jhilgedidndm", "ldiakemnj", "bbnibccm", "jkbneoaheaajnm", "clkgmbjlgdnl", "lobbdldifnnijh", "dnmih", "jglia", "didicmghfe", "dlhbcfclf", "akbmioocoihkfh", "foofdldm", "imenimfcame", "ifekbmgnbdkc", "jjlkaabdollola", "gie", "hbaj", "noomfnfccmgaa", "dcjffeg", "nb", "obdb", "lolgjflimkib", "eaiigminlakkb", "cia", "hkf", "jknfklaio", "igklbiomo", "jfjgh", "ekgnkfnhjcch", "kmonfcclieehlik", "oggkmccklnmj", "bedhobcl", "egmnhajcnhcdgb", "imfdhekamfel", "bmmkhfdbm", "gnjfbcjlecfn", "llmkgclm", "gafinbnhfe", "mlbfedkoeeddfao", "kklcdmglleb", "ckekmeiea", "mi", "kfejn", "lm", "mk", "abkoajocfdili", "jidac", "jonhkanccl", "lllodjgnmm", "abfeaodlmjkngol", "cdncnh", "lkcb", "abhilnmmhijab", "hiljkfakojjld", "mbboobkaolkljo", "jhkblobaofgoh", "ncm", "mgbdhmcicomf", "oag", "akmjdd", "abkenodnhj", "mljf", "afb", "afejkobmiffeee", "oollnkilabmb", "gfaocokmcmjlmb", "cokmecdd", "bo", "endocdnmjiek", "bcf", "lhllbagiel", "bhihgofhj", "ffce", "neio", "ofbfiiab", "kjdo", "lgfggnamceeo", "kofledoinamcj", "femhndomndoakoa", "fmodaigcka", "omakggcalhn", "hhhogmcbjnhelkk", "mgah", "jghjjfmk", "ecolelfmcb", "eajjkdncafhhgab", "obno", "fifigfeok", "laafjimienff", "beckbbmhmofb", "nafhihmgnikd", "cbcfnhlkne", "kao", "nlkfhbm", "fmh", "ohfek", "oj", "hifgcgi", "adhkn", "lffgmodeafnn", "ngchmhdbmhmhh", "mcffimhnlffab", "blhmkdhbnhbb", "kkb", "lgkine", "hgfbdbfffanhik", "joebhbh", "img", "kglcddmloo", "hoflgfao", "bdhgdekb", "mggflahnoo", "cmnol", "imnmmgimmedf", "mcjmoofomiia", "mlakhbjfnbmgena", "ilhmcnkkeg", "domhbmkcd", "fco", "fdio", "cmkoagblnd", "kmihfigmceiiicm", "afgbadbgbaon", "menahlemehifooe", "jacokdiiokaic", "limj", "fkedaoomokjbkdi", "jkncd", "jblmcmfnegnk", "jjicjhjhbg", "gbfcead", "jf", "aifnkmnao", "effmhlhchngknl", "odhjeib", "ohcgmgb", "bgbd", "am", "kkjfbdlh", "hgbjakkokjgooel", "jbeokiakf", "flaoba", "cifcdnanmk", "mice", "ihhofdai", "ldnfmeiemhf", "kefbbohhgineacj", "bi", "njfie", "ociodahlomoekkf", "andhoindeca", "ajnndjocjeg", "bmijkmjbbkgbbh", "feanh", "bjemcefkfcaenal", "edfdenghinm", "moal", "ndbjdmijh", "enccnhmoifa", "dbckadjibam", "gd", "oglj", "aldjelhbemle", "cmbkofkcoe", "ihciacibeh", "lcojkclhmibgoif", "jfmjncnolfj", "gfcmcabhjki", "aggfmakaanjb", "mhbelld", "hon", "nkfoikcddehcah", "kggbigknacmohb", "jbkgndofcmaaohh", "gkjano", "afhhhh", "mjng", "jilckm", "dekkedjehmenbm", "clfm", "acmhbkdadgena", "oenokachg", "lhiea", "dceiag", "eebgj", "oolifidh", "dj", "cdfn", "eghdglgiok", "jdhegkefhbdhkm", "mhgngafea", "akabbcjkdnbc", "gcbn", "kimdgahf", "oc"];
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");
           

            wordDict = ["a", "b"];
            s = "ab";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            wordDict = ["cats", "dog", "sand", "and", "cat"];
            s = "catsandog";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            wordDict = ["aaaa", "aa", "a"];
            s = "aaaaaaaa";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            
            wordDict = ["leet", "code"];
            s = "leetcode";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");
             
             
            


            wordDict = ["cc", "ac"];
            s = "ccaccc";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            wordDict = ["a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            

            
            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            wordDict = ["aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa", "ba"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            */

            // fails this.. have to see why merge is not happening correctly
            s = "acaaaaabbbdbcccdcdaadcdccacbcccabbbbcdaaaaaadb";
            wordDict = ["abbcbda", "cbdaaa", "b", "dadaaad", "dccbbbc", "dccadd", "ccbdbc", "bbca", "bacbcdd", "a", "bacb", "cbc", "adc", "c", "cbdbcad", "cdbab", "db", "abbcdbd", "bcb", "bbdab", "aa", "bcadb", "bacbcb", "ca", "dbdabdb", "ccd", "acbb", "bdc", "acbccd", "d", "cccdcda", "dcbd", "cbccacd", "ac", "cca", "aaddc", "dccac", "ccdc", "bbbbcda", "ba", "adbcadb", "dca", "abd", "bdbb", "ddadbad", "badb", "ab", "aaaaa", "acba", "abbb"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

        }

    }
}

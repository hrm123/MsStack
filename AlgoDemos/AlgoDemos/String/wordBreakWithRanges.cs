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
    public class WordBreakWithRanges
    {

        string _s;
        int _n0based;
        IList<string> _wordDict;
        Dictionary<int, HashSet<string>> _rangeStartDict = new();
        Dictionary<int, HashSet<string>> _rangeEndDict = new();
        Dictionary<string, string> _rangeNameToWordDict = new();

        /// <summary>
        /// range start => word match starts at [0] or word match starts at next node after another range (word match) ends
        /// range ends => currently running range ends at a node
        /// all ranges (word matches) will be given unique name
        /// Dictionary stores ranges that start at an index and ranges that end at an index
        /// running ranges is a list of range names
        /// There is no complete word break if at any node of [0,s.length-1] running ranges count=0
        /// </summary>
        /// <param name="s"></param>
        /// <param name="wordDict"></param>
        /// <returns></returns>
        public bool WordBreak(string s, IList<string> wordDict)
        {
            _s = s;
            _wordDict = wordDict.OrderByDescending(w => w.Length).ToList();
            ReduceString();
            _n0based = _s.Length - 1;
            _rangeStartDict.Clear();
            _rangeEndDict.Clear();
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
            int rangeNumber = 0;
            

            // order words by length so that we follow greedy approach where we first try the longest possible match, and then try shorter matches.
            wordDict = wordDict.OrderBy(w => w.Length).ToList();
            foreach (var wrd in _wordDict)
            {
                string pattern = @$"(?=({wrd}))";
                MatchCollection matches = Regex.Matches(_s, pattern, RegexOptions.IgnoreCase);
                foreach (Match match in matches)
                {
                    rangeNumber++;
                    string rangeName = $"r{rangeNumber}";
                    int ms = match.Index;
                    // Console.WriteLine($"Found '{wrd}' at index {ms}");
                    firstStart = ms < firstStart ? ms : firstStart;
                    if (!_rangeStartDict.ContainsKey(ms))
                    {
                        _rangeStartDict[ms] = new();
                    }
                    int endIndex = ms + wrd.Length - 1;
                    if (!_rangeEndDict.ContainsKey(endIndex))
                    {
                        _rangeEndDict[endIndex] = new();
                    }
                    _rangeStartDict[ms].Add(rangeName);
                    _rangeEndDict[endIndex].Add(rangeName);
                    _rangeNameToWordDict[rangeName] = wrd;
                }
            }

            int end = firstStart;
            if (firstStart != 0)
            {
                return false;
            }

            // printRangesDebug();

            return WalkThroughRanges();

        }

        void printRangesDebug()
        {
            foreach(var kvp in _rangeNameToWordDict)
            {
                Console.WriteLine($"({kvp.Key},{kvp.Value})");
            }
        }

        void printRunningRangesDebug(HashSet<string> runningRanges, HashSet<string> prevEndRange, int y)
        {
            Console.WriteLine($"iteration {y}");
            foreach (var prevRange in prevEndRange) { 
                foreach (var rng in runningRanges)
                {
                    Console.WriteLine($"({prevRange}-{rng})");
                }
            }
        }

        //runningranges has list of  contiguoug ranges from the start to current index 
        public bool WalkThroughRanges()
        {
            HashSet<string> runningRanges = new();
            runningRanges.UnionWith(_rangeStartDict[0]);
            int rangesEndingAtPreviousIndex = 0;
            if (_rangeEndDict.ContainsKey(0))
            {
                rangesEndingAtPreviousIndex = _rangeEndDict[0].Count(); // for one letter matches at [0]
                runningRanges.ExceptWith(_rangeEndDict[0]);
            }

            for (int y= 1;y<= _n0based;y++)
            {
                if(runningRanges.Count == 0 && rangesEndingAtPreviousIndex == 0)
                {
                    return false; // string cannot be formed from words in the dictionary
                }
                if (rangesEndingAtPreviousIndex > 0 ) 
                {
                    // if any running range ended at previous node, and n ranges start from this node, then n ranges need to be added to running range 
                    if (_rangeStartDict.ContainsKey(y))
                    {
                        runningRanges.UnionWith(_rangeStartDict[y]);
                    }
                }
                if (_rangeEndDict.ContainsKey(y))
                {
                    var rangesEndingHere = runningRanges.Intersect(_rangeEndDict[y]).ToList(); // any running ranges ending at this node get removed from running ranges set
                    runningRanges.ExceptWith(_rangeEndDict[y]);
                    rangesEndingAtPreviousIndex = rangesEndingHere.Count();
                    printRunningRangesDebug(runningRanges, _rangeEndDict[y], y);
                }
                else
                {
                    rangesEndingAtPreviousIndex = 0;
                }
            }
            if (runningRanges.Count == 0 && rangesEndingAtPreviousIndex == 0)
            {
                return false; // string cannot be formed from words in the dictionary
            }
            return true;
        }


        public void ReduceString()
        {
            foreach (var word in _wordDict)
            {
                string pattern = @$"({word})(\s*\1)+";
                _s = Regex.Replace(_s,pattern, "$1", RegexOptions.IgnoreCase);
            }
        }
        public void ReduceStringOld()
        {
            // If any word in wordDict repetas 2 times consequively, then we can reduce the string by removing the repeated word.
            // For example, if wordDict contains "ab" and "cd", and s is "ababcd", then we can reduce s to "abcd" by removing the repeated "ab".
            // This is because if we can form "abcd" from wordDict, then we can also form "ababcd" by adding "ab" at the beginning.
            foreach(var word in _wordDict)
            {
                string pattern = @$"({word})\1++";
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

        public static void Demo()
        {
            WordBreakWithRanges wbs = new();
            string[] wordDict;
            string s;




            s = "acaaaaabbbdbcccdcdaadcdccacbcccabbbbcdaaaaaadb";
            wordDict = ["abbcbda", "cbdaaa", "b", "dadaaad", "dccbbbc", "dccadd", "ccbdbc", "bbca", "bacbcdd", "a", "bacb", "cbc", "adc", "c", "cbdbcad", "cdbab", "db", "abbcdbd", "bcb", "bbdab", "aa", "bcadb", "bacbcb", "ca", "dbdabdb", "ccd", "acbb", "bdc", "acbccd", "d", "cccdcda", "dcbd", "cbccacd", "ac", "cca", "aaddc", "dccac", "ccdc", "bbbbcda", "ba", "adbcadb", "dca", "abd", "bdbb", "ddadbad", "badb", "ab", "aaaaa", "acba", "abbb"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            wordDict = ["a", "aa", "ba"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabaabaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            wordDict = ["aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa", "ba"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");



            wordDict = ["leet", "code"];
            s = "leetcode";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");



            wordDict = ["cats", "dog", "sand", "and", "cat"];
            s = "catsandog";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            s = "fajbeokiakfmlacbinjdnjdmmfha";
            wordDict = ["be", "ellaekgjhibcomc", "ahaklkan", "jcm", "lchidklmcone", "ljmdgagaen", "giioojldjkfnno", "el", "eibjaffacjll", "hn", "hbjakhjneml", "foi", "nbhf", "aigf", "cfdlnc", "fa", "amakgofedhkghgl", "ddhmhdhioh", "ijoddeabbiei", "giamcgco", "nholghlfbbendi", "emlc", "m", "elgibme", "behkignjenmh", "lodkkjgioe", "doe", "hgflgakna", "macghogdidmdm", "ec", "kncigolkog", "ljooio", "lch", "gkaclkbjn", "ofiaglfoffl", "alhfbb", "cfmdbgo", "cfcnajknk", "jfh", "almbgdjnbbbgmhb", "dmlnnohf", "olojeejfafc", "ndgcmgoe", "cmkdjilfeo", "bengdd", "enfg", "dbngiggni", "anmkljn", "njdnjdmmfha", "ndimmddfmhe", "hmkdjkhhiilnf", "ehd", "jfdl", "dlgki", "bhoflnomibkki", "lg", "fjojjnnkdfenhol", "lefhhl", "nimdl", "gejgomblmim", "ahbnlmlfmejjj", "glhacaojnf", "mfjdhnhdkm", "do", "fnh", "mnmjdk", "hfjgdlnnb", "hniolfhkhbie", "ldgodonogcab", "mabjnnohnijhn", "aceojlkmdg", "aedfljg", "cehk", "jag", "oniegflnhje", "jo", "maokc", "jkbndc", "djbn", "dajkdblojkf", "dmen", "kcdjdocinenc", "cgindbm", "h", "odaof", "cmogcbgj", "anjahlgbbmba", "haoe", "ggacnminj", "ilcfjoedhe", "klookammgofl", "onnmenn", "mbdneaioo", "jc", "dekgil", "bjdfibfd", "hfbnlgmlllcb", "afebehf", "obekljbnh", "eoaedhjk", "nobamccd", "mdieojoknf", "komcglmakkaa", "jcliimcc", "jmmgbmha", "gdogjnn", "ednembco", "dgno", "jiaheeabifahfmo", "djkcgnakkh", "kdkiglgf", "eb", "fmdnlhj", "eicohdciejc", "jgofmankkf", "o", "nnmomkmkmiaoga", "njchkccln", "ndamha", "eleanmojdi", "ebkl", "jcageehlelcg", "acfddofjihgmn", "iklaomfhjm", "io", "igmob", "lfnhnlnigbbignk", "anihfojmedf", "nj", "oilcabalhb", "adjfbkfjch", "lbfb", "mgfnngfccb", "jhmhggm", "dnllc", "c", "ljim", "jmikd", "mdfimdgac", "fhbclgo", "edclcdia", "nelmfjejff", "i", "cmcbbckdnjcoo", "cddocce", "hc", "keh", "keofhnhemd", "biln", "mjcnbjmkikon", "fekbdnkolahh", "hkfmjbj", "mjoj", "jn", "ilof", "ifhfk", "aofmg", "nofljgmmmf", "hcdifeiclbchlf", "imlijgdg", "ocdiiemcmbkglm", "nhoekmlkjfoa", "kibffkbleedda", "kdhdjekccbkc", "bcbflcag", "jekmkdimnnjjoo", "mmgfljchalbem", "kchk", "oi", "ncf", "jembgfa", "l", "kfkeianmmmdacl", "ecjkkfggj", "jdgcfnhfjonkhig", "jhagiokii", "nifm", "bbjjlj", "adajlokomibfg", "ojk", "lockdel", "bh", "hoojolglchck", "conko", "eadi", "kfigoijnfimolen", "g", "dbnj", "cojkbmo", "hh", "mcdbh", "ngdmgioen", "ehjagfohnolkho", "dgfgdlc", "aoglneoh", "gbc", "ijjckddeicld", "imekih", "liiaecniil", "hahejbhgiclb", "fnmojm", "ablihjhggiahhno", "colloaakco", "jhobddaanbhmlg", "cbfajfhkoh", "cim", "laghknigabn", "dcbnbkegkjam", "gem", "ljjim", "icclogji", "omidhe", "f", "giiaclfcjkagl", "ndcjldekjnkekm", "aiikdccohcj", "mkbmb", "oomhhafobic", "bkacdjfgbggn", "ahghdoahbi", "hedm", "eeoj", "bdgdlfollegej", "eg", "dfeb", "dkffkid", "hcne", "gjkohnaaabn", "jfeododjgdhlfbf", "clfkmconnkfb", "abnbkcni", "hk", "ghnmhjm", "oibjibmkaibdefa", "hjambim", "oe", "aao", "jil", "fmhomflfen", "hlidcklnmb", "hiaonkhd", "bibbmkandf", "hke", "bmfcionm", "inhcnlkbkkmjicn", "jckjedhgoghi", "chmik", "mnjldknhaec", "hocbccbg", "ljadj", "lciikgnlj", "ifjjhkbhifione", "foikblanoco", "ode", "mjc", "fhklofh", "mmoklkkog", "hocbojmhffeajo", "ccmmd", "bkkh", "nhhgcflniebkme", "lfohikenfbjacli", "cmehijnihijgng", "caa", "bmk", "emofof", "jjagiogohfab", "ibh", "eoacdlnodalkjbl", "cbbjbbnjom", "iljiomeloehen", "gignlngclmh", "b", "ll", "dokgngnocde", "cienegffibgieba", "agbachloidg", "mlelnafokd", "nmcmka", "akeogjbjcnf", "nebdic", "a", "efc", "ljdk", "jhcag", "bkbikbjgae", "mcjlgjeo", "lo", "dbiofobl", "cehebiljff", "eeagngm", "ondahcjiel", "coblanndhlhoggj", "jaobmjml", "jfejjinofek", "hhnna", "gdhcn", "acelcomgkgohm", "njkkjkkln", "jmc", "hkekoho", "boefec", "cioibfgjmhb", "ebggdbeimn", "emhg", "cfghkhii", "d", "k", "khoddedia", "nhje", "eebkfml", "bohhd", "kg", "n", "ilgemokdehcbaif", "cldicda", "einij", "akmabcgfn", "fmkmcn", "bnlfbagkke", "oakbgjejmcncj", "iehdfadgoik", "kkcfo", "jmjkmfcacjjnd", "ndokhh", "hjfeelhckkjjmj", "dnomohejbodkb", "jcmblncjadno", "oiofcodobiml", "ddmillkncjfdfj", "aihenmkdnhdhkhf", "bfdbakeilfdojnc", "jjhbkbne", "aigabk", "cae", "oednojjb", "gdoe", "jokjceohkmbm", "offkanbahigo", "kfomigbfddjli", "dkkjobgkcejei", "mdilld", "bofkika", "kkinig", "cljcflbghjmhmke", "kmbjlgdcbdjn", "bkgbmoahda", "kmnajjdemggnfg", "mgjndldil", "iemb", "kehaokgjg", "icign", "oijmaolehmoo", "amhgldifmgekhe", "diacnollhi", "lnjhdaafadl", "bdfiackhogoje", "ebjlfa", "deabkgfhnead", "gadcob", "haa", "dbhnbhjcmmab", "bbmjainilbbej", "dc", "bgjgafnjjflne", "ehholgnn", "fmhccbnc", "mdnfl", "feeejdgc", "mfhlobdadooh", "ojna", "gkgjnijdbgo", "ghngnhn", "nhnjaiaadiedgg", "nk", "hmd", "nmbmijaffogl", "onkcgbgmago", "gfli", "ofjlec", "nlfnbkkdc", "hakilani", "bofjdjkhllb", "ocjncleljnecfc", "gdonnkodmkejhf", "fiflchanfllgnf", "kaejakoibgln", "hmdlfioacgaci", "honmfbcog", "mlacbi", "gf", "ejbbemoeha", "acfjegee", "lllflaocnnkeadi", "mdgoebfgacecmbg", "faejgln", "kmlmhffgcmekm", "akcjmgdg", "kmhhh", "fdohjehacdln", "e", "ojba", "ohadmod", "eaenkdiaokl", "dii", "cgfjaklblafeifo", "imoeflkcgbbem", "nbjkmb", "jjgm", "hofgelg", "cnihecmdigdgflg", "fnmikkeldjgb", "onlhgonldjaedh", "fmkdn", "kfbcbleen", "oejioibnmab", "cg", "meadghbocjnj", "hmmdnkegfeieijn", "ijgenomhndlje", "maccdcgfjig", "iabemie", "mlfg", "mdblmdaechmeaml", "dhlafgjo", "eabbiila", "kf", "oehggehfmijlfmg", "klljaejidfhbon", "akmbgmignoag", "jgbkngmigdfm", "kjeelnbn", "ajaa", "mlcjoiaahoiga", "oalnielba", "ffmobgkc", "kmhoknfffdmo", "nagjiffnjhh", "dlehllomjok", "agaejefhdbk", "nnegoijfdj", "ndl", "dhfginocgi", "nflmglgh", "bcd", "gbgjijemmdio", "jk", "gidgjbmb", "hi", "lmgoah", "fdebefcech", "ach", "bahaoj", "ccmmblgibgjahi", "moid", "jhilgedidndm", "ldiakemnj", "bbnibccm", "jkbneoaheaajnm", "clkgmbjlgdnl", "lobbdldifnnijh", "dnmih", "jglia", "didicmghfe", "dlhbcfclf", "akbmioocoihkfh", "foofdldm", "imenimfcame", "ifekbmgnbdkc", "jjlkaabdollola", "gie", "hbaj", "noomfnfccmgaa", "dcjffeg", "nb", "obdb", "lolgjflimkib", "eaiigminlakkb", "cia", "hkf", "jknfklaio", "igklbiomo", "jfjgh", "ekgnkfnhjcch", "kmonfcclieehlik", "oggkmccklnmj", "bedhobcl", "egmnhajcnhcdgb", "imfdhekamfel", "bmmkhfdbm", "gnjfbcjlecfn", "llmkgclm", "gafinbnhfe", "mlbfedkoeeddfao", "kklcdmglleb", "ckekmeiea", "mi", "kfejn", "lm", "mk", "abkoajocfdili", "jidac", "jonhkanccl", "lllodjgnmm", "abfeaodlmjkngol", "cdncnh", "lkcb", "abhilnmmhijab", "hiljkfakojjld", "mbboobkaolkljo", "jhkblobaofgoh", "ncm", "mgbdhmcicomf", "oag", "akmjdd", "abkenodnhj", "mljf", "afb", "afejkobmiffeee", "oollnkilabmb", "gfaocokmcmjlmb", "cokmecdd", "bo", "endocdnmjiek", "bcf", "lhllbagiel", "bhihgofhj", "ffce", "neio", "ofbfiiab", "kjdo", "lgfggnamceeo", "kofledoinamcj", "femhndomndoakoa", "fmodaigcka", "omakggcalhn", "hhhogmcbjnhelkk", "mgah", "jghjjfmk", "ecolelfmcb", "eajjkdncafhhgab", "obno", "fifigfeok", "laafjimienff", "beckbbmhmofb", "nafhihmgnikd", "cbcfnhlkne", "kao", "nlkfhbm", "fmh", "ohfek", "oj", "hifgcgi", "adhkn", "lffgmodeafnn", "ngchmhdbmhmhh", "mcffimhnlffab", "blhmkdhbnhbb", "kkb", "lgkine", "hgfbdbfffanhik", "joebhbh", "img", "kglcddmloo", "hoflgfao", "bdhgdekb", "mggflahnoo", "cmnol", "imnmmgimmedf", "mcjmoofomiia", "mlakhbjfnbmgena", "ilhmcnkkeg", "domhbmkcd", "fco", "fdio", "cmkoagblnd", "kmihfigmceiiicm", "afgbadbgbaon", "menahlemehifooe", "jacokdiiokaic", "limj", "fkedaoomokjbkdi", "jkncd", "jblmcmfnegnk", "jjicjhjhbg", "gbfcead", "jf", "aifnkmnao", "effmhlhchngknl", "odhjeib", "ohcgmgb", "bgbd", "am", "kkjfbdlh", "hgbjakkokjgooel", "jbeokiakf", "flaoba", "cifcdnanmk", "mice", "ihhofdai", "ldnfmeiemhf", "kefbbohhgineacj", "bi", "njfie", "ociodahlomoekkf", "andhoindeca", "ajnndjocjeg", "bmijkmjbbkgbbh", "feanh", "bjemcefkfcaenal", "edfdenghinm", "moal", "ndbjdmijh", "enccnhmoifa", "dbckadjibam", "gd", "oglj", "aldjelhbemle", "cmbkofkcoe", "ihciacibeh", "lcojkclhmibgoif", "jfmjncnolfj", "gfcmcabhjki", "aggfmakaanjb", "mhbelld", "hon", "nkfoikcddehcah", "kggbigknacmohb", "jbkgndofcmaaohh", "gkjano", "afhhhh", "mjng", "jilckm", "dekkedjehmenbm", "clfm", "acmhbkdadgena", "oenokachg", "lhiea", "dceiag", "eebgj", "oolifidh", "dj", "cdfn", "eghdglgiok", "jdhegkefhbdhkm", "mhgngafea", "akabbcjkdnbc", "gcbn", "kimdgahf", "oc"];
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");
           

            wordDict = ["a", "b"];
            s = "ab";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            

            wordDict = ["aaaa", "aa", "a"];
            s = "aaaaaaaa";
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

        }

    }
}

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
    /// iterate over string chars. At [0] store all words starting from there as prefixes at index 0.
    /// at ith index .. all the possible prefixes til (i-1) have been stored in Dictionary<int,List<string>>
    /// See if there are nay words starting at ith index. If yes add all of them to pefixes at dictionary for [(i-1)+ len(word starting at ith index] and
    /// store them at [i] in dictionary. You can remove all prefixes till teh index [i-1] from the dictionary. check if dictionaty has any string at [len(input string-1).
    /// If yes exit algo with True. else If you reach end of input string they exit with False. ~O(n) run time when combined with Reduce().
    /// 
    /// </summary>
    public class WordBreakWithPrefixes
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
            prefixDict.Clear();

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
                    _rangeStartDict[ms].Add(wrd);
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

        Dictionary<int, HashSet<string>> prefixDict = new();

        //runningranges has list of  contiguoug ranges from the start to current index 
        public bool WalkThroughRanges()
        {
            foreach (var item in _rangeStartDict[0])
            {
                int l = item.Length;
                if (!prefixDict.ContainsKey(l))
                {
                    prefixDict[l] = new();
                }
                prefixDict[l].Add(item);
            }
            for (int y= 1;y<= _n0based;y++)
            {
                if (prefixDict.ContainsKey(_n0based+1)  && prefixDict[_n0based + 1].Count > 0)
                {
                    return true; // we could form a prefix that is ful length of word
                }
                if(prefixDict.ContainsKey(y) && prefixDict[y].Count > 0 && _rangeStartDict.ContainsKey(y) &&
                    _rangeStartDict[y].Count>0)
                {
                    //add new prefixDict with length of y-1 + len(word starting from y)
                    foreach(var item in prefixDict[y])
                    {
                        foreach(var addItem in _rangeStartDict[y])
                        {
                            var newPrefix = item + addItem;
                            if (! prefixDict.ContainsKey(newPrefix.Length))
                            {
                                prefixDict[newPrefix.Length] = new();
                            }
                            prefixDict[newPrefix.Length].Add(newPrefix);
                        }
                    }
                    // remove prefixDict[y-1]
                    prefixDict[y].Clear();
                }
            }
            return (prefixDict.ContainsKey(_n0based + 1) && prefixDict[_n0based + 1].Count > 0);
        }


        public void ReduceString()
        {
            foreach (var word in _wordDict)
            {
                string pattern = @$"({word})(\s*\1)+";
                _s = Regex.Replace(_s,pattern, "$1", RegexOptions.IgnoreCase);
            }
            // Console.WriteLine($"reduced string = {_s}");
        }

        public static void Demo()
        {
            WordBreakWithPrefixes wbs = new();
            string[] wordDict;
            string s;

            // only one failing testcase
            wordDict = ["kfomka", "hecagbngambii", "anobmnikj", "c", "nnkmfelneemfgcl", "ah", "bgomgohl", "lcbjbg", "ebjfoiddndih", "hjknoamjbfhckb", "eioldlijmmla", "nbekmcnakif", "fgahmihodolmhbi", "gnjfe", "hk", "b", "jbfgm", "ecojceoaejkkoed", "cemodhmbcmgl", "j", "gdcnjj", "kolaijoicbc", "liibjjcini", "lmbenj", "eklingemgdjncaa", "m", "hkh", "fblb", "fk", "nnfkfanaga", "eldjml", "iejn", "gbmjfdooeeko", "jafogijka", "ngnfggojmhclkjd", "bfagnfclg", "imkeobcdidiifbm", "ogeo", "gicjog", "cjnibenelm", "ogoloc", "edciifkaff", "kbeeg", "nebn", "jdd", "aeojhclmdn", "dilbhl", "dkk", "bgmck", "ohgkefkadonafg", "labem", "fheoglj", "gkcanacfjfhogjc", "eglkcddd", "lelelihakeh", "hhjijfiodfi", "enehbibnhfjd", "gkm", "ggj", "ag", "hhhjogk", "lllicdhihn", "goakjjnk", "lhbn", "fhheedadamlnedh", "bin", "cl", "ggjljjjf", "fdcdaobhlhgj", "nijlf", "i", "gaemagobjfc", "dg", "g", "jhlelodgeekj", "hcimohlni", "fdoiohikhacgb", "k", "doiaigclm", "bdfaoncbhfkdbjd", "f", "jaikbciac", "cjgadmfoodmba", "molokllh", "gfkngeebnggo", "lahd", "n", "ehfngoc", "lejfcee", "kofhmoh", "cgda", "de", "kljnicikjeh", "edomdbibhif", "jehdkgmmofihdi", "hifcjkloebel", "gcghgbemjege", "kobhhefbbb", "aaikgaolhllhlm", "akg", "kmmikgkhnn", "dnamfhaf", "mjhj", "ifadcgmgjaa", "acnjehgkflgkd", "bjj", "maihjn", "ojakklhl", "ign", "jhd", "kndkhbebgh", "amljjfeahcdlfdg", "fnboolobch", "gcclgcoaojc", "kfokbbkllmcd", "fec", "dljma", "noa", "cfjie", "fohhemkka", "bfaldajf", "nbk", "kmbnjoalnhki", "ccieabbnlhbjmj", "nmacelialookal", "hdlefnbmgklo", "bfbblofk", "doohocnadd", "klmed", "e", "hkkcmbljlojkghm", "jjiadlgf", "ogadjhambjikce", "bglghjndlk", "gackokkbhj", "oofohdogb", "leiolllnjj", "edekdnibja", "gjhglilocif", "ccfnfjalchc", "gl", "ihee", "cfgccdmecem", "mdmcdgjelhgk", "laboglchdhbk", "ajmiim", "cebhalkngloae", "hgohednmkahdi", "ddiecjnkmgbbei", "ajaengmcdlbk", "kgg", "ndchkjdn", "heklaamafiomea", "ehg", "imelcifnhkae", "hcgadilb", "elndjcodnhcc", "nkjd", "gjnfkogkjeobo", "eolega", "lm", "jddfkfbbbhia", "cddmfeckheeo", "bfnmaalmjdb", "fbcg", "ko", "mojfj", "kk", "bbljjnnikdhg", "l", "calbc", "mkekn", "ejlhdk", "hkebdiebecf", "emhelbbda", "mlba", "ckjmih", "odfacclfl", "lgfjjbgookmnoe", "begnkogf", "gakojeblk", "bfflcmdko", "cfdclljcg", "ho", "fo", "acmi", "oemknmffgcio", "mlkhk", "kfhkndmdojhidg", "ckfcibmnikn", "dgoecamdliaeeoa", "ocealkbbec", "kbmmihb", "ncikad", "hi", "nccjbnldneijc", "hgiccigeehmdl", "dlfmjhmioa", "kmff", "gfhkd", "okiamg", "ekdbamm", "fc", "neg", "cfmo", "ccgahikbbl", "khhoc", "elbg", "cbghbacjbfm", "jkagbmfgemjfg", "ijceidhhajmja", "imibemhdg", "ja", "idkfd", "ndogdkjjkf", "fhic", "ooajkki", "fdnjhh", "ba", "jdlnidngkfffbmi", "jddjfnnjoidcnm", "kghljjikbacd", "idllbbn", "d", "mgkajbnjedeiee", "fbllleanknmoomb", "lom", "kofjmmjm", "mcdlbglonin", "gcnboanh", "fggii", "fdkbmic", "bbiln", "cdjcjhonjgiagkb", "kooenbeoongcle", "cecnlfbaanckdkj", "fejlmog", "fanekdneoaammb", "maojbcegdamn", "bcmanmjdeabdo", "amloj", "adgoej", "jh", "fhf", "cogdljlgek", "o", "joeiajlioggj", "oncal", "lbgg", "elainnbffk", "hbdi", "femcanllndoh", "ke", "hmib", "nagfahhljh", "ibifdlfeechcbal", "knec", "oegfcghlgalcnno", "abiefmjldmln", "mlfglgni", "jkofhjeb", "ifjbneblfldjel", "nahhcimkjhjgb", "cdgkbn", "nnklfbeecgedie", "gmllmjbodhgllc", "hogollongjo", "fmoinacebll", "fkngbganmh", "jgdblmhlmfij", "fkkdjknahamcfb", "aieakdokibj", "hddlcdiailhd", "iajhmg", "jenocgo", "embdib", "dghbmljjogka", "bahcggjgmlf", "fb", "jldkcfom", "mfi", "kdkke", "odhbl", "jin", "kcjmkggcmnami", "kofig", "bid", "ohnohi", "fcbojdgoaoa", "dj", "ifkbmbod", "dhdedohlghk", "nmkeakohicfdjf", "ahbifnnoaldgbj", "egldeibiinoac", "iehfhjjjmil", "bmeimi", "ombngooicknel", "lfdkngobmik", "ifjcjkfnmgjcnmi", "fmf", "aoeaa", "an", "ffgddcjblehhggo", "hijfdcchdilcl", "hacbaamkhblnkk", "najefebghcbkjfl", "hcnnlogjfmmjcma", "njgcogemlnohl", "ihejh", "ej", "ofn", "ggcklj", "omah", "hg", "obk", "giig", "cklna", "lihaiollfnem", "ionlnlhjckf", "cfdlijnmgjoebl", "dloehimen", "acggkacahfhkdne", "iecd", "gn", "odgbnalk", "ahfhcd", "dghlag", "bchfe", "dldblmnbifnmlo", "cffhbijal", "dbddifnojfibha", "mhh", "cjjol", "fed", "bhcnf", "ciiibbedklnnk", "ikniooicmm", "ejf", "ammeennkcdgbjco", "jmhmd", "cek", "bjbhcmda", "kfjmhbf", "chjmmnea", "ifccifn", "naedmco", "iohchafbega", "kjejfhbco", "anlhhhhg"];
            s = "fohhemkkaecojceoaejkkoedkofhmohkcjmkggcmnami";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");



            wordDict = ["cats", "dog", "sand", "and", "cat"];
            s = "catsandog";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

            s = "ab";
            wordDict = ["a", "b"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            s = "acaaaaabbbdbcccdcdaadcdccacbcccabbbbcdaaaaaadb";
            wordDict = ["abbcbda", "cbdaaa", "b", "dadaaad", "dccbbbc", "dccadd", "ccbdbc", "bbca", "bacbcdd", "a", "bacb", "cbc", "adc", "c", "cbdbcad", "cdbab", "db", "abbcdbd", "bcb", "bbdab", "aa", "bcadb", "bacbcb", "ca", "dbdabdb", "ccd", "acbb", "bdc", "acbccd", "d", "cccdcda", "dcbd", "cbccacd", "ac", "cca", "aaddc", "dccac", "ccdc", "bbbbcda", "ba", "adbcadb", "dca", "abd", "bdbb", "ddadbad", "badb", "ab", "aaaaa", "acba", "abbb"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");

            wordDict = ["cc", "ac"];
            s = "ccaccc";
            Console.WriteLine($"Problem : s = \"{s}\", wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be true");


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

            
             
            


            
            s = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaab";
            wordDict = ["a", "aa", "aaa", "aaaa", "aaaaa", "aaaaaa", "aaaaaaa", "aaaaaaaa", "aaaaaaaaa", "aaaaaaaaaa"];
            Console.WriteLine($"Problem : s = \"{s}\", of length {s.Length}, wordDict = [{string.Join(", ", wordDict)}]");
            Console.WriteLine($"answer={wbs.WordBreak(s, wordDict)} should be false");

        }

    }
}

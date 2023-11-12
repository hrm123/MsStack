using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoDemos.codecamp.matrix
{
    
    

    public class TrieNode
    {
        public TrieNode[] array;
        public string value;
        public TrieNode()
        {
            array = new TrieNode[26];
        }
    }

    public class Trie
    {
        public TrieNode root;
        public Trie()
        {
            root = new TrieNode();
        }
        public void add(string word)
        {
            TrieNode temp = root;
            for (int i = 0; i < word.Length; i++)
            {
                char ch = word[i];
                int index = ch - 'a';
                if (temp.array[index] == null)
                {
                    temp.array[index] = new TrieNode();
                    temp = temp.array[index];
                }
                else
                {
                    temp = temp.array[index];
                }
            }
            temp.value = word;
        }
    }

    /// <summary>
    /// 212. Word Search II - 1934ms beats 24% of C# users. 128 MB - beats 16% of C# users
    /// </summary>  
    public class WordsInBoard
    {
        Trie _trie = null;
        HashSet<string> _result = null;
        char[][] _board = null;
        int _X, _Y;
        public IList<string> FindWords(char[][] board, string[] words)
        {
            _trie = new Trie();
            _result = new HashSet<string>();
            _board = board;
            _X = board.Length;
            _Y = board[0].Length;
            foreach (string word in words)
            {
                _trie.add(word);
            }
            for (int x = 0; x < _X; x++)
            {
                for (int y = 0; y < _Y; y++)
                {
                    var curr = _trie.root.array[_board[x][y] - 'a'];
                    if (curr != null)
                    {
                        RecursiveFindWords(x, y, curr, new HashSet<string>());
                    }
                }
            }
            return _result.ToList();
        }

        void RecursiveFindWords(int x, int y, TrieNode curr, HashSet<string> visited)
        {
            if (curr.value != null)
            {
                _result.Add(curr.value);
            }
            visited.Add(x + "#" + y);
            TrieNode temp = null;
            //left
            if (x > 0 && (temp = curr.array[_board[x - 1][y] - 'a']) != null && !visited.Contains((x - 1) + "#" + y))
            {
                RecursiveFindWords(x - 1, y, temp, visited);
            }

            //top
            if (y > 0 && (temp = curr.array[_board[x][y - 1] - 'a']) != null && !visited.Contains(x + "#" + (y - 1)))
            {
                RecursiveFindWords(x, y - 1, temp, visited);
            }

            //right
            if (x < _X - 1 && (temp = curr.array[_board[x + 1][y] - 'a']) != null && !visited.Contains((x + 1) + "#" + y))
            {
                RecursiveFindWords(x + 1, y, temp, visited);
            }

            //bottom
            if (y < _Y - 1 && (temp = curr.array[_board[x][y + 1] - 'a']) != null && !visited.Contains(x + "#" + (y + 1)))
            {
                RecursiveFindWords(x, y + 1, temp, visited);
            }
            visited.Remove(x + "#" + y);
        }

    }
}

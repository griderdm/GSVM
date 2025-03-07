using System.Text.RegularExpressions;

namespace CPP
{
    public class CPreprocessor
    {
        public string SystemDirectory { get; set; }

        Dictionary<string, string> defines = new Dictionary<string, string>();


        public string[] Preprocess(string[] input)
        {
            foreach (string s in input)
            {
                if (s.Trim().StartsWith('#'))
                {
                    ParseDirective(s);
                }
            }
        }

        void ParseDirective(string line)
        {

        }

        string[] Tokenize(string line)
        {
            string l = line.Trim();
            if (!l.StartsWith('#')) return new string[0];

            Match include = Regex.Match(l, @"^#include <(.*)>$");
            Match define = Regex.Match(l, @"^#define (.*) (.*)$");
        }
    }
}

namespace CPP
{
    public class CPreprocessor
    {
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
    }
}

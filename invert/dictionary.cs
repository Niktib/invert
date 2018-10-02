using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace invert
{
    class dictionary
    {
        int docID = 0;
        int lineNum = 0;
        /* Dictionary class
        * Takes in the documents in an array and outputs the dictionary.txt file with word frequency
        */
        public dictionary(string[] lines)
        {
            bool flag = false;
            int documentsCovered = 0, value;
            string[] lineArray;
            List<posting> p = new List<posting>();
            SortedDictionary<string, int> d = new SortedDictionary<string, int>();
            SortedDictionary<string, List<posting>> post = new SortedDictionary<string, List<posting>>();

            for (int i = 0; i < lines.Count(); i++)
            {

                if (lines[i].Substring(0, 2) == ".I") { documentsCovered++; }

                flag = flagCheck(lines[i], flag);
                if (!flag)
                {
                    lineArray = dataCleaner(lines[i].ToLower());
                    for (int j = 0; j < lineArray.Count(); j++)
                    {
                        if (d.TryGetValue(lineArray[j], out value) == true)
                        {
                            d[lineArray[j]] = ++value;
                            post.TryGetValue(lineArray[j], out p);
                            p.Add(new posting());
                            post[lineArray[j]] = p;
                        }
                        else
                        {
                            d.Add(lineArray[j], 1);

                        }
                    }
                }
            }
            printOut(d);
        }
        /*
         * 
         */
        private void printOut(SortedDictionary<string, int> d)
        {
            using (StreamWriter file = new StreamWriter("dictionary.txt"))
                foreach (var entry in d)
                    file.WriteLine(entry.Key + " " + entry.Value);
        }
        /* Method dataCleaner
         * Takes in a sentence and returns an array of only Alphanumeric characters and '-'
         */
        private string[] dataCleaner(string sentence)
        {
            string[] matchesCollection;
            Regex r = new Regex("^[A-Za-z0-9-]*");
            if (r.IsMatch(sentence))
            {
                matchesCollection = new string[r.Matches(sentence).Count];
                for (int i = 0; i < matchesCollection.Length; i++)
                    matchesCollection[i] = r.Matches(sentence)[i].ToString();
            }
            else
            {
                matchesCollection = new string[0];
            }
            return matchesCollection;
        }
        /* Method flagCheck
        * Gets the current line and the current flag status.
        * Helps to know the junk data under certain flags that isn't needed atm (.X, .N, .B)
        */
        private bool flagCheck(string headerFlag, bool flag)
        {
            string header = headerFlag.Substring(0, 2);
            if (header == ".I") docID++;
            if (header == ".K" || header == ".W" || header == ".A" ) { return false; }
            if (header == ".I" || header == ".B" || header == ".X" || header == ".N" || flag == true) { return true; }
            return false;
        }
    }
}

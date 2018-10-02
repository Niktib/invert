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
        /* Dictionary class
        * Takes in the documents in an array and outputs the dictionary.txt file with word frequency
        */
        public dictionary(string[] lines)
        {
            int docID = 0, value;
            string[] lineArray;
            string textBetweenFlags;
            List<posting> p = new List<posting>();
            SortedDictionary<string, int> d = new SortedDictionary<string, int>();
            SortedDictionary<string, List<posting>> post = new SortedDictionary<string, List<posting>>();

            for (int i = 0; i < lines.Count(); i++)
            {
                textBetweenFlags = "";
                while (!flagCheck(lines[i]))
                {
                    textBetweenFlags += lines[i];
                    i++;
                    if (i == lines.Count()) break;
                }
                //Increments the 
                if (i != lines.Count() && lines[i].Substring(0, 2) == ".I") { docID++; }
                if (textBetweenFlags != "")
                { 
                    lineArray = dataCleaner(textBetweenFlags.ToLower());
                    for (int j = 0; j < lineArray.Count(); j++)
                    {
                        if (d.TryGetValue(lineArray[j], out value) == true)
                        {
                            d[lineArray[j]] = ++value;
                            post.TryGetValue(lineArray[j], out p);
                            if (p.Last().docID == docID)
                            {
                                p.Last().addingTerm(j);
                            }
                            else
                            {
                                p.Add(new posting(docID, j + 1));
                            }
                            post[lineArray[j]] = p;
                        }
                        else
                        {
                            d.Add(lineArray[j], 1);
                            p.Add(new posting(docID, j + 1));
                            post.Add(lineArray[j], p);
                        }
                        p = new List<posting>(); //look into this later
                    }
                }

            }
            printOutDictionary(d);
            printOutPostings(post);
        }
        private void printOutPostings(SortedDictionary<string, List<posting>> post)
        {
            List<posting> p = new List<posting>();
            using (StreamWriter file = new StreamWriter("postings.txt"))
            {
                foreach (var entry in post)
                {
                    post.TryGetValue(entry.Key, out p);
                    foreach (var entry2 in p)
                    {
                        file.Write(entry2.printOut());
                    }
                    file.WriteLine();
                }
            }
        }
        /*
         * 
         */
        private void printOutDictionary(SortedDictionary<string, int> d)
        {
            using (StreamWriter file = new StreamWriter("dictionary.txt"))
                foreach (var entry in d)
                    file.WriteLine(entry.Key + " " + entry.Value);
        }
        private bool flagCheck(string sentence)
        {
            Regex r = new Regex("^.[A-Z]");
            return r.IsMatch(sentence);
        }
        /* Method dataCleaner
         * Takes in a sentence and returns an array of only Alphanumeric characters and '-'
         */
        private string[] dataCleaner(string sentence)
        {
            string[] matchesCollection;
            Regex r = new Regex("[A-Za-z-]*"); //"^[A-Za-z0-9-]*"
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
            return matchesCollection = matchesCollection.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
    }
}

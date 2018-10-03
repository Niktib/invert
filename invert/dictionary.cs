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
        * 
        */
        public dictionary(string[] lines, bool stopWords, bool stemming)
        {
            int docID = 0;
            string[] lineArray;
            string textBetweenFlags;
            PorterStemmer ps = new PorterStemmer();
            List<posting> p = new List<posting>();
            List<string> stopWordList = new List<string>();
            SortedDictionary<string, int> d = new SortedDictionary<string, int>();
            SortedDictionary<string, List<posting>> post = new SortedDictionary<string, List<posting>>();

            if (stopWords) { stopWordList = populateStopWords(); }

            string currentFlag = "";
            for (int i = 0; i < lines.Count(); i++)
            {
                textBetweenFlags = "";
                while (!flagCheck(lines[i]))
                {
                    textBetweenFlags = textBetweenFlags + " " + lines[i];
                    i++;
                    if (i == lines.Count()) break;
                }
                if (currentFlag != ".W") { textBetweenFlags = ""; }
                if (i != lines.Count() && lines[i].Substring(0, 2) == ".I") { docID++; }
                if (i != lines.Count()){ currentFlag = lines[i].Substring(0, 2); }

                if (textBetweenFlags != "")
                {
                    lineArray = dataCleaner(textBetweenFlags.ToLower().Trim());
                    for (int j = 0; j < lineArray.Count(); j++)
                    {
                        string key = lineArray[j];
                        if (!stopWords || (stopWords && !stopWordList.Contains(key)))
                        {
                            if (stemming) { key = ps.StemWord(key); }
                            if (!post.ContainsKey(key))
                            {
                                post[key] = new List<posting>();
                                d[key] = 0;
                            }
                            d[key]++;
                            p = post[key];
                            posting entry = p.LastOrDefault();
                            if (entry == null || entry.docID != docID)
                            {
                                p.Add(new posting(docID, j + 1));
                            }
                            else
                            {
                                entry.addingTerm(j + 1);
                            }
                        }
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
        private string[] dataCleaner(string sentence)
        {
            string[] matchesCollection;
            Regex r = new Regex("[A-Za-z-]+"); //"^[A-Za-z0-9-]*"
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
            return matchesCollection;//= matchesCollection.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        }
        private List<string> populateStopWords()
        {
            string[] commonWordsArray = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\common_words");
            List<string> ls = new List<string>(commonWordsArray);

            return ls;
        }
    }
}

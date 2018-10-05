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

        private List<string> stopWordList;
        private List<titlesAndAbstracts> titlesAndAbstractsList;
        private SortedDictionary<string, int> dictionaryMap;
        private SortedDictionary<string, List<posting>> postMap;
        
        public dictionary(string[] lines, bool stopWords, bool stemming)
        {
            int docID = 0;
            stopWordList = new List<string>();
            titlesAndAbstractsList = new List<titlesAndAbstracts>();
            dictionaryMap = new SortedDictionary<string, int>();
            postMap = new SortedDictionary<string, List<posting>>();

            if (stopWords) { stopWordList = populateStopWords(); }
            string currentFlag = "";
            string textBetweenFlags = "";
            string currentIDTitle = "";

            for (int i = 0; i < lines.Count(); i++)
            {

                if (flagCheck(lines[i])) { currentFlag = lines[i].Substring(0, 2); }
                switch (currentFlag)
                {
                    case ".I":
                        if (currentIDTitle != "" && textBetweenFlags != "")
                        {
                            titlesAndAbstractsList.Add(new titlesAndAbstracts(docID, currentIDTitle, textBetweenFlags));
                            organizeTheLists(docID, textBetweenFlags, stopWords, stemming);
                        }
                        else if (currentIDTitle != "")
                        {
                            titlesAndAbstractsList.Add(new titlesAndAbstracts(docID, currentIDTitle, "No Abstract Provided"));
                        }
                        docID++;
                        textBetweenFlags = "";
                        currentIDTitle = "";
                        break;
                    case ".T":
                        currentIDTitle = currentIDTitle + " " + lines[i];
                        break;
                    case ".W":
                        i++;
                        while (!flagCheck(lines[i]))
                        {
                            textBetweenFlags = textBetweenFlags + " " + lines[i];
                            i++;
                            if (i == lines.Count() || flagCheck(lines[i + 1])) break;
                        }
                        currentFlag = lines[i].Substring(0, 2);
                        break;
                    default:
                        break;
                }
            }
            if (currentIDTitle != "" && textBetweenFlags != "")
            {
                titlesAndAbstractsList.Add(new titlesAndAbstracts(docID, currentIDTitle, textBetweenFlags));
                organizeTheLists(docID, textBetweenFlags, stopWords, stemming);
            }
            else if (currentIDTitle != "")  { titlesAndAbstractsList.Add(new titlesAndAbstracts(docID, currentIDTitle, "No Abstract Provided")); }

            printOutDictionary(dictionaryMap);
            printOutPostings(postMap);
            printOutTitlesAndAbstracts(titlesAndAbstractsList);
        }

        private void organizeTheLists(int docID, string textBetweenFlags, bool stopWords, bool stemming)
        {
            List<posting> postingsList = new List<posting>();
            PorterStemmer ps = new PorterStemmer();
            string[] lineArray = dataCleaner(textBetweenFlags.ToLower().Trim());
            for (int j = 0; j < lineArray.Count(); j++)
            {
                string key = lineArray[j];
                if (!stopWords || (stopWords && !stopWordList.Contains(key)))
                {
                    if (stemming) { key = ps.StemWord(key); }
                    if (!postMap.ContainsKey(key))
                    {
                        postMap[key] = new List<posting>();
                        dictionaryMap[key] = 0;
                    }
                    dictionaryMap[key]++;
                    postingsList = postMap[key];
                    posting entry = postingsList.LastOrDefault();
                    if (entry == null || entry.docID != docID)
                    {
                        postingsList.Add(new posting(docID, j + 1));
                    }
                    else
                    {
                        entry.addingTerm(j + 1);
                    }
                }
            }
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
        private void printOutTitlesAndAbstracts(List<titlesAndAbstracts> t)
        {
            using (StreamWriter file = new StreamWriter("titlesAndAbstracts.txt"))
                foreach (var entry in t)
                    file.WriteLine(entry.printOut());
        }
        private bool flagCheck(string sentence)
        {
            Regex r = new Regex("^\\.[A-Z]$|^\\.[I]");
            return r.IsMatch(sentence);
        }
        private string[] dataCleaner(string sentence)
        {
            string[] matchesCollection;
            Regex r = new Regex("[A-Za-z0-9-]+");
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
        private List<string> populateStopWords()
        {
            string[] commonWordsArray = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\common_words");
            List<string> ls = new List<string>(commonWordsArray);

            return ls;
        }
    }
}

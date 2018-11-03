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
        
        private List<titlesAndAbstracts> titlesAndAbstractsList;
        private SortedDictionary<string, int> dictionaryMap;
        private SortedDictionary<string, List<posting>> postMap;
        
        public dictionary(string[] lines, bool stopWords, bool stemming)
        {
            int docID = 0;
            titlesAndAbstractsList = new List<titlesAndAbstracts>();
            dictionaryMap = new SortedDictionary<string, int>();
            postMap = new SortedDictionary<string, List<posting>>();

            string currentFlag = "";
            string abstractText = "";
            string titleText = "";
            string authors = "";

            for (int i = 0; i < lines.Count(); i++)
            {                
                if (flagCheck(lines[i])) { currentFlag = lines[i].Substring(0, 2);}
                switch (currentFlag)
                {
                    case ".I":
                        docID++;
                        abstractText = "";
                        titleText = "";
                        authors = "";
                        break;
                    case ".T":
                        titleText = titleText + " " + lines[i];
                        break;
                    case ".W":
                        abstractText = abstractText + " " + lines[i];
                        break;
                    case ".A":
                        authors = authors + "\t" + lines[i];
                        break;
                    case ".X":
                        if (abstractText.Length > 3) { abstractText = abstractText.Substring(3); }
                        if (authors.Length > 3) { authors = authors.Substring(3); }
                        titleText = titleText.Substring(3);
                        organizeTheLists(docID, titleText + " " + abstractText, stopWords, stemming);

                        if (abstractText == "") { abstractText = "No Abstract Provided"; }
                        if (authors == "") { authors = "No Authors Provided"; }
                        titlesAndAbstractsList.Add(new titlesAndAbstracts(docID, titleText, abstractText, authors));
                        currentFlag = "";
                        break;
                    default:
                        break;
                }
            }

            printOutDictionary(dictionaryMap);
            printOutPostings(postMap);
            printOutTitlesAndAbstracts(titlesAndAbstractsList);
        }

        //Puts together Postings and Dictionary
        private void organizeTheLists(int docID, string abstractText, bool stopWords, bool stemming)
        {
            List<posting> postingsList = new List<posting>();
            PorterStemmer ps = new PorterStemmer();
            StopWords sw = new StopWords();
            string[] lineArray = dataCleaner(abstractText.ToLower().Trim());
            for (int j = 0; j < lineArray.Count(); j++)
            {
                string key = lineArray[j];
                
                if (!stopWords || (stopWords && !sw.StopMatching(key)))
                {
                    if (stemming) { key = ps.StemWord(key); }
                    if (!postMap.ContainsKey(key))
                    {
                        postMap[key] = new List<posting>();
                        dictionaryMap[key] = 0;
                    }
                    postingsList = postMap[key];
                    posting entry = postingsList.LastOrDefault();
                    if (entry == null || entry.docID != docID)
                    {
                        postingsList.Add(new posting(docID, j + 1));
                        dictionaryMap[key]++;
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
    }
}

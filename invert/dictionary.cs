using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invert
{
    class dictionary
    {
        public dictionary(string[] lines)
        {
            bool flag = false;
            SortedDictionary<string, int> d = new SortedDictionary<string, int>();
            int documentsCovered = 0, value;
            string[] lineArray;

            for (int i = 0; i < lines.Count(); i++)
            {
               
                if (lines[i].Substring(0, 2) == ".I") { documentsCovered++; }

                flag = flagCheck(lines[i], flag);
                if (!flag)
                {
                    lineArray = lines[i].ToLower().Split(' ');
                    for (int j = 0; j < lineArray.Count(); j++)
                    {
                        if (d.TryGetValue(lineArray[j], out value) == true)
                        {
                            d[lineArray[j]] = ++value;
                        }
                        else { d.Add(lineArray[j], 1); }
                    }
                }

            }
            using (StreamWriter file = new StreamWriter("dictionary.txt"))
                foreach (var entry in d)
                    file.WriteLine("[{0} {1}]", entry.Key, entry.Value);

        }
        private string dataCleaner(string word)
        {

            return "";
        }
        private bool flagCheck(string headerFlag, bool flag)
        {
            string header = headerFlag.Substring(0, 2);
            if (header == ".K" || header == ".W" || header == ".A" ) { return false; }
            if (header == ".I" || header == ".B" || header == ".X" || header == ".N" || flag == true) { return true; }
            return false;
        }
    }
}

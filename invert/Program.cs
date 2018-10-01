using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invert
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileLocation = AppDomain.CurrentDomain.BaseDirectory + "\\cacm.all";
            string[] lines = File.ReadAllLines(fileLocation).ToArray();
            dictionarySetup(lines);
        }
        static private void dictionarySetup(string[] lines)
        {
            bool bFlag = false, xFlag = false, nFlag = false, iFlag = false;
            string headerFlag;
            SortedDictionary<string, int> d = new SortedDictionary<string, int>();
            int documentsCovered = 0, value;
            string[] lineArray;

            for (int i = 0; i < lines.Count(); i++)
            {
                if (i==10)
                {
                    i = i;
                }
                headerFlag = lines[i].Substring(0, 2);
                if (headerFlag == ".I") { documentsCovered++; iFlag = true; }
                else if (headerFlag == ".B" && lines[i].Length == 2) { bFlag = true; }
                else if (headerFlag == ".X" && lines[i].Length == 2) { xFlag = true; }
                else if (headerFlag == ".N" && lines[i].Length == 2) { nFlag = true; }
                else if (lines[i].Substring(0, 1) == "." && lines[i].Length == 2)
                {
                    bFlag = false; xFlag = false; nFlag = false; iFlag = false;
                }

                if (!bFlag && !xFlag && !nFlag && !iFlag)
                {
                    lineArray = lines[i].ToLower().Split(' ');
                    for (int j = 0; j < lineArray.Count(); j++)
                    {
                        if(d.TryGetValue(lineArray[j], out value) == true)
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
    }

}

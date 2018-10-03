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
            if (File.Exists(fileLocation))
            {
                string[] lines = File.ReadAllLines(fileLocation).ToArray();
                bool stopWords = false, stemming = false;
                foreach (var item in args)
                {
                    if (item.ToString() == "-stop") stopWords = true;
                    if (item.ToString() == "-stem") stemming = true;
                }
                dictionary d = new dictionary(lines, stopWords, stemming);
                Console.WriteLine("Done");
            }
            else
            {
                Console.WriteLine("File cacm.all not found in executable starting folder.");
                Console.ReadLine();
            }
        }
    }

}

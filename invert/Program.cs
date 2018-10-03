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
            bool stopWords = false, stemming = false ;
            foreach (var item in args)
            {
                if (item.ToString() == "-stop") stopWords = true;
                if (item.ToString() == "-stem") stemming = true;
            }
            dictionary d = new dictionary(lines, stopWords, stemming);
        }
    }

}

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
            dictionary d = new dictionary(lines, true, true);
        }
    }

}

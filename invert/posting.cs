using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invert
{
    class posting
    {
        public int docID { get; set; }
        private int termFreq { get; set; }

        public posting(int idocID, int iposition)
        {
            docID = idocID;
            termFreq = 1;
        }
        public void addingTerm(int iposition)
        {
            termFreq++;
        }
        public string visualPrintOut()
        {
            return "[" + docID + '\t' + termFreq + "]";
        }
        public string printOut()
        {
            return docID + "\t" + termFreq + "|";
        }
    }
}

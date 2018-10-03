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
        private List<int> positions = new List<int>();

        public posting(int idocID, int iposition)
        {
            docID = idocID;
            termFreq = 1;
            positions.Add(iposition);
        }
        public void addingTerm(int iposition)
        {
            termFreq++;
            positions.Add(iposition);
        }
        public string visualPrintOut()
        {
            return "[" + docID + '\t' + termFreq + '\t' + "{" + string.Join(",", positions) + "}" + "]";
        }
        public string printOut()
        {
            return docID + "\t" + termFreq + "\t" + string.Join(",", positions) + "|";
        }
    }
}

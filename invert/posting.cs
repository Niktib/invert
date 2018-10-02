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
        public int termFreq { get; set; }
        public List<int> positions { get; set; }

        public posting(int idocID)
        {
            
        }
        private string printOut()
        {
            return "[" + docID + '\t' + termFreq + '\t' + "{" + string.Join(",", positions) + "}" + "]";
        }
    }
}

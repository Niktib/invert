using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace invert
{
    class titlesAndAbstracts
    {
        public int docID { get; set; }
        private string title { get; set; }
        private string abstractText { get; set; }

        public titlesAndAbstracts(int idocID, string _title, string _abstractText)
        {
            docID = idocID;
            title = _title.Substring(3).Trim().Replace("\t", "");
            abstractText = _abstractText;
        }
        public string visualPrintOut()
        {
            return "[" + docID + '\t' + title + '\t' +  abstractText + "]";
        }
        public string printOut()
        {
            return docID + "\t" + title + "\t" + abstractText;
        }
    }
}

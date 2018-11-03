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
        private string authorNames { get; set; }

        public titlesAndAbstracts(int idocID, string _title, string _abstractText, string _authors)
        {
            docID = idocID;
            title = _title.Trim().Replace("\t", "");
            abstractText = _abstractText.Trim().Replace("  ", " ");
            authorNames = _authors;
        }
        public string visualPrintOut()
        {
            return "[" + docID + '\t' + title + '\t' +  abstractText + "\t" + authorNames + "]";
        }
        public string printOut()
        {
            return docID + "\t" + title + "\t" + abstractText + "\t" + authorNames;
        }
    }
}

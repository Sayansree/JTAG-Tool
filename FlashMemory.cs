using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DLPComposer.ControlProgram.FlashProgrammer;

namespace DLPComposer.ControlProgram.FlashProgrammer
{
    public class FlashMemory
    {
        public string Mfg { get; set; }
        public string MfgID { get; set; }
        public string LMfgID { get; set; }
        public string Device { get; set; }
        public string DevID { get; set; }
        public string LDevID { get; set; }
        public int Mb { get; set; }
        public int Alg { get; set; }
        public string Size { get; set; }
        public int noofsec { get; set; }
        public string[] Sector_Addresses { get; set; }
    };

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class ElectronicBoard
    {
        public string DEVID { get; set; }
        public string DEVICENAME { get; set; }
        public int[] DATA { get; set; }
        public int[] DATAINP {  set; get; }
        public int[] DATACTRL { get; set; }
        public int[] ADDR { get; set; }
        public int[] ADDRCTRL { get; set; }
        public int RESET { get; set; }
        public int CSZ { get; set; }
        public int CSZCTRL { get; set; }
        public int RESETCTRL { get; set; }
        public int WEZ { get; set; }
        public int WEZCTRL { get; set; }
        public int OEZ { get; set; }
        public int OEZCTRL { get; set; }
        public int BSLEN { get; set; }
        public int IRLEN { get; set; }
        public byte SAFE { get; set; }
        public string EXTEST { get; set; }
        public int FLASHID { get; set; }
        public string[] OPTPINSNAME { get; set; }
        public int[] OPTPINS { get; set; }
        public int[] OPTPINSINP { get; set; }
        public int[] OPTPINSCTRL { get; set; }
        public int[] OPTPINSVAL { get; set; }
        public bool REVERSE { get; set; }
    };

}

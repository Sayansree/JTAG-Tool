using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public class PIN
    {
        public int ctr;
        public int output;
        public int input;
        public int val;
        public PIN(int ctr, int output, int input, int val)
        {
            this.ctr = ctr;
            this.output = output;
            this.input = input;
            this.val = val;
        }
    };
    public class Device
    {
        private JTAG DeviceChain;
        int index;
        const int INPUT = -1;
        const int OUTPUT = 1;
        const int BIDIR = 0;

        const byte Z = 2;
        const byte HIGH = 1;
        const byte LOW = 0;
        //-----------device_data
        byte SAFE;
        int BSLen;
        int EXTEST;
        public PIN CE,OE,WE,RST;//chip enable, output enable, write enable
        public PIN[] ADDR;
        public PIN[] DATA;
        bool reverseCell;
        //--------------------
        byte[] BoundaryCellsRead;
        byte[] BoundaryCellsWrite;

        public Dictionary<string,PIN> util;
        public Device(JTAG DeviceChain,int index,ElectronicBoard brdData) 
        {
            this.DeviceChain = DeviceChain;
            this.index = index;
            WE=CE=OE=RST = null;
            util=new Dictionary<string, PIN> ();
            parse_info(brdData);
            BoundaryCellsRead = new byte[BSLen];
            BoundaryCellsWrite = new byte[BSLen];
            resetBS();
        }
        private void parse_info(ElectronicBoard brdData)
        {
            BSLen = brdData.BSLEN;      
            EXTEST = Convert.ToInt32( brdData.EXTEST,2); 
            SAFE = brdData.SAFE;
            reverseCell = brdData.REVERSE; 

            //optional pins
            for( int i=0;i < brdData.OPTPINSNAME.Length; i++)
            {
               util.Add(brdData.OPTPINSNAME[i], new PIN(brdData.OPTPINSCTRL[i], brdData.OPTPINS[i], brdData.OPTPINSINP[i], brdData.OPTPINSVAL[i]));
               // Console.WriteLine(brdData.OPTPINSVAL[i]);
            }
            //flash pins
            ADDR = new PIN[brdData.ADDR.Length];
            DATA = new PIN[brdData.DATA.Length];
            for(int i = 0; i < brdData.ADDR.Length; i++)
            {
                ADDR[i] = new PIN(brdData.ADDRCTRL[i], brdData.ADDR[i], -1,OUTPUT);
                util.Add(("A" + i), ADDR[i]);
            }
            for(int i = 0;i < brdData.DATA.Length; i++)
            {
                DATA[i]= new PIN(brdData.DATACTRL[i], brdData.DATA[i], brdData.DATAINP[i],BIDIR);
                util.Add(("D" + i),DATA[i]);
            }
            if (brdData.OEZ != -1)
            {
                OE = new PIN(brdData.OEZCTRL, brdData.OEZ, -1, OUTPUT);
                util.Add("OE", OE);
            }
            if(brdData.WEZ != -1)
            {
                WE = new PIN(brdData.WEZCTRL, brdData.WEZ,-1, OUTPUT);
                util.Add("WE", WE);
            }
            if(brdData.CSZ != -1)
            {
                CE= new PIN(brdData.CSZCTRL, brdData.CSZ,-1, OUTPUT);
                util.Add("CE", CE);
            }
            if (brdData.RESET != -1)
            {
                RST = new PIN(brdData.RESETCTRL, brdData.RESET,-1, OUTPUT);
                util.Add("RST", RST);
            }
        }
        private int R(int x)
        {
            return (reverseCell)? BSLen - x - 1: x;
        }

        //----------------cell modifiers------------
        public void setPinWrite(PIN p,byte val)
        {
            if (val == Z)
            {
                if (p.ctr != -1) BoundaryCellsWrite[R(p.ctr)] = SAFE;
            }
            else if (p.output != -1) ///add feature to disable write in case of input pin
            {
                if (p.ctr != -1) BoundaryCellsWrite[R(p.ctr)] = (SAFE == HIGH) ? LOW : HIGH;
                BoundaryCellsWrite[R(p.output)] = val;
            }
        }
        public byte getPinRead(PIN p)
        {
            if(p.input != -1)
                return BoundaryCellsRead[R(p.input)];
            return Z;
        }
        public byte readCell(int n)
        {
            return BoundaryCellsWrite[R(n)];
        }


        public void resetBS()
        {
            for (int i = 0; i < BSLen; i++)
            {
                BoundaryCellsRead[i] = SAFE;
                BoundaryCellsWrite[i] = SAFE;
            }
        }
        private void setAddress(UInt32 addr)
        {
            UInt32 mask = 1;
            for (int i = 0; i < ADDR.Length; i++, mask = mask << 1)
            {
                setPinWrite(ADDR[i], ((addr & mask) > 0 ) ? HIGH : LOW);
            }
        }
        private void setData(UInt32 data)
        {
            UInt32 mask = 1;
            for (int i = 0; i < DATA.Length; i++, mask = mask << 1)
            {
                setPinWrite(DATA[i], ((data & mask)>0) ? HIGH : LOW);
            }
        }
        private UInt32 getData()
        {
            UInt32 data = 0; 
            UInt32 mask = 1;
            for (int i = 0; i < DATA.Length; i++, mask = mask << 1)
            {
                data+=(getPinRead(DATA[i])>0) ? mask : LOW;
            }
            return data;

        }


        //-----------------------functions----------
        public void flashWrite(UInt32 addr, UInt32 data)
        {
           // addr=addr >> 1; //word size 2 bytes
            setAddress(addr);
            setData(data);

            //BoundaryCellsWrite[R(WE.output)]=LOW;
            setPinWrite(WE,LOW);
            WriteBoundaryCells();

            //BoundaryCellsWrite[R(WE.output)]=HIGH;
            setPinWrite(WE, HIGH);
            WriteBoundaryCells();
        }
        public UInt32 flashRead(UInt32 addr)
        {
           // addr = addr >> 1; //word size 2 bytes
            setAddress(addr);
            for(int i = 0; i < DATA.Length; i++)
            {
                setPinWrite(DATA[i], Z);
            }

            //BoundaryCellsWrite[R(OE.output)] = LOW;
            setPinWrite(OE, LOW);
            WriteBoundaryCells();
            
            //BoundaryCellsWrite[R(OE.output)] = HIGH;
            setPinWrite(OE, HIGH);
            ReadBoundaryCells();
            return getData();
        }
        public void Select()
        {
            DeviceChain.writeIRbyIndex(index, EXTEST);
            resetBS();
            if (util.ContainsKey("SELECT"))
            {
                setPinWrite(util["SELECT"], HIGH);
                DeviceChain.writeDRbyIndex(index, BoundaryCellsWrite);
            }
        }
        public void SelectMultiple(List<int>index)
        {
            DeviceChain.writeIRbyMultipleIndex(index, EXTEST);
            resetBS();
            if (util.ContainsKey("SELECT"))
            {
                setPinWrite(util["SELECT"], HIGH);
                DeviceChain.writeDRbyMultipleIndex(index, BoundaryCellsWrite);
            }
        }
        public void DeSelect()
        {
            resetBS();
            DeviceChain.writeDRbyIndex(index, BoundaryCellsWrite);
        }
        public void ReadBoundaryCells()
        {
            DeviceChain.readDRbyIndex(index, BoundaryCellsWrite, BoundaryCellsRead);
        }
        public void WriteBoundaryCells()
        {
            DeviceChain.writeDRbyIndex(index, BoundaryCellsWrite);
        }
        public void ReadandRestoreBoundaryCells()
        {
            ReadBoundaryCells();
            Array.Copy(BoundaryCellsRead, BoundaryCellsWrite, BSLen);
            WriteBoundaryCells();
        }
    }
}

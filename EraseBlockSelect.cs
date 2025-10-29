using DLPComposer.ControlProgram.FlashProgrammer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class EraseBlockSelect : Form
    {
        public EraseBlockSelect(Form3 parent, Flash flash)//deviceid ,flash 
        {
            this.parent = parent;
            this.flash = flash;
            InitializeComponent();
            inittable();
        }

        private void inittable()
        {
            for (int i = 0; i < flash.sectors.Count; i++)
            {
                Flash.Sector sec = flash.sectors[i];
                string text = String.Format("0x{0:X7}-0x{1:X7} ({2:d}KBytes)", sec.startAddr,sec.endAddr,sec.size>>9);
                checkedListBox_sector.Items.Add(text);
            }
        }

        private void checkedListBox_sector_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            //checkedListBox_sector.Items.Clear();
            //inittable();
        }

        private void ErasedButtonClick(object sender, EventArgs e)
        {

            List<Flash.Sector> sectorToErase = new List<Flash.Sector>();
            for (int i = 0; i < checkedListBox_sector.Items.Count; i++)
            {
                if(checkedListBox_sector.GetItemChecked(i))
                    sectorToErase.Add(flash.sectors[i]);
            }
            parent.EraseSectors(sectorToErase);
            this.Close();

        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

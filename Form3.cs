using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Security.Cryptography;
using System.Windows.Documents;
using static WindowsFormsApp1.Flash;
using System.Diagnostics;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3(Form1 parent, Device device)
        {
            InitializeComponent();
            this.parent = parent;
            this.device = device;
            this.flash = new Flash(device);
            parent.Hide();
            init();
        }
        private void init()
        {

        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.Show();
        }
        private void resetProgress()
        {
            label1.Enabled = false;
            label2.Enabled = false; 
            label3.Enabled = false;
            label4.Enabled = false;
            label5.Enabled = false;
            label6.Enabled = false;
            label7.Text = "";

            progressBar1.Enabled = false;
            progressBar2.Enabled = false;
            progressBar3.Enabled = false;

        }
        private void  LockControls(bool v)
        {
            button1.Enabled = !v;
            button2.Enabled=!v;
            button3.Enabled=!v;
            button4.Enabled=!v;
            button5.Enabled=!v;
            button6.Enabled=v;

        }
        private void CancelButtonClick(object sender, EventArgs e)
        {
            stopOperation = true;
        }
        public void EraseSectors(List<Sector> sectorsToErase)
        {
            resetProgress();
            label1.Enabled = true;
            label4.Enabled = true;
            label4.Text = "0%";
            progressBar1.Enabled = true;
            progressBar1.Value = 0;
            label7.Text = "Erasing...";

            LockControls(true);
            System.Windows.Forms.Application.DoEvents();

            //List<Sector> sectorsToErase = new List<Sector>() { new Sector(0x00000, 0x10000), new Sector(0x20000, 0x10000) };

            int SectorCount;
            UInt32 SizeToErase = 0, SizeEaresed = 0;
            //uint MID=0, DID=0;
            // flash.GetFlashMfgAndDevID(ref MID, ref DID);
            // flash.GetFlashInfoCFI();
            // uint count = flash.m_FlashInfo.NumEraseBlocks;
            if (sectorsToErase.Count > 0)
            {
                for (SectorCount = 0; SectorCount < sectorsToErase.Count; SectorCount++)
                    SizeToErase += sectorsToErase[SectorCount].size;

                for (SectorCount = 0; SectorCount < sectorsToErase.Count; SectorCount++)
                {
                    Sector current_sector = sectorsToErase[SectorCount];
                    Console.WriteLine(String.Format("Erasing sector : 0x{0:X8}-0x{1:X8} ({2:d}KB)", current_sector.startAddr, current_sector.endAddr, current_sector.size >> 10));

                    flash.FlashSectorErase(current_sector.startAddr);
                    SizeEaresed += current_sector.size;

                    progressBar1.Value = (int)(SizeEaresed * 100 / SizeToErase);
                    label4.Text = progressBar2.Value + "% Complete";
                    System.Windows.Forms.Application.DoEvents();
                    if (stopOperation) break;

                }
            }
            LockControls(false);
            if (!stopOperation)
            {
                label4.Text = "100% Complete";
                label7.Text = "Erase Completed!";
            }
        }

        private void EraseButtonClick(object sender, EventArgs e)//erase
        {
            //object of eraseblockselect
            eraseUI = new EraseBlockSelect(this, flash);
            eraseUI.Show();
        }

        private void WriteButtonClick(object sender, EventArgs e)//write
        {
            resetProgress();
            label2.Enabled = true;
            label5.Enabled = true;
            label5.Text = "0%";
            progressBar2.Enabled = true;
            progressBar2.Value = 0;
            label7.Text = "Writing...";
            if (parallelVerify)
            {
                label3.Enabled = true;
                label6.Enabled = true;
                label6.Text = "0%";
                progressBar1.Enabled = true;
                progressBar1.Value = 0;
                label7.Text = "Writing and Verifying...";
            }
            UInt32 Addr = 0, count, word;
            int  i, Length;
            UInt32 BytesWrote = 0;

            if (file2Load == "")
            {
                System.Windows.MessageBox.Show("No File selected to download");
                stopOperation = true;
                return;
            }
            byte[] fileBytes;
            try
            {
                fileBytes = File.ReadAllBytes(file2Load);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No such File found");
                stopOperation = true;
                return;
            }
            Length = fileBytes.Length;
            Addr = Convert.ToUInt32(StartAddressTextBox.Text, 16);
            //Addr &= 0xFFFFFFFE;
            //UpdateData(false);
            //double _microSecPerTick =1000000D / System.Diagnostics.Stopwatch.Frequency;
            //double limit = 250;
            //limit /= _microSecPerTick;
            LockControls(true);
            stopOperation = false;

            int sec = 0;
            while (BytesWrote < Length)
            {
                if (Length - BytesWrote > 512)
                    count = 512;
                else
                    count =  (UInt32)Length - BytesWrote;

                if (count >= 8)
                {
                    while (flash.sectors[sec].startAddr > Addr) sec++;
                    bool f=flash.FlashWriteByteArr(Addr, flash.sectors[sec].startAddr, fileBytes, (int)BytesWrote, (int)count);
                    if (parallelVerify&& !f)
                    {
                        System.Windows.MessageBox.Show("data mismatch at address : " + Addr +"-"+ (Addr+count/2));
                        LockControls(false);
                        return;
                    }
                    Addr += count/2;
                }
                else
                {
                    for (i = 0; i < count; i += 2)
                    {
                        word = (UInt32)(fileBytes[i+ BytesWrote + 1] << 8) | fileBytes[BytesWrote + i];

                        if (word != 0xFFFF)
                        {
                            flash.FlashWriteWord(Addr, word);
                        }

                        Addr += 1;
                    }
                }
                BytesWrote += count;
                progressBar2.Value = (int)(BytesWrote * 100 / Length);
                label5.Text = progressBar2.Value + "% Complete";
                if (parallelVerify)
                {
                    progressBar3.Value = (int)(BytesWrote * 100 / Length);
                    label6.Text = progressBar3.Value + "% Complete";
                }
                System.Windows.Forms.Application.DoEvents();
                if (stopOperation) break;
            }
            LockControls(false);
            if (!stopOperation)
            {
                label5.Text = "100% Complete";
                label7.Text = "Write Completed!";
                if (parallelVerify)
                {
                    label6.Text = "100% Complete";
                    label7.Text = "Verify Completed";
                }
            }

        }
        private void delayMicroSeconds(double limit)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (stopwatch.ElapsedTicks >= limit)
                    return;
            }
        }
        private void VerifyBurronClick(object sender, EventArgs e)//verify
        {
            resetProgress();
            label3.Enabled = true;
            label6.Enabled = true;
            label6.Text = "0%";
            progressBar1.Enabled = true;
            progressBar1.Value = 0;
            label7.Text = "Verifying...";
            LockControls(true);

            UInt32 Addr = 0, count, word, wordRead;
            int i, Length;
            UInt32 BytesVerified = 0;

            if (file2Load == "")
            {
                System.Windows.MessageBox.Show("No File selected to download");
                stopOperation = true;
                return;
            }
            byte[] fileBytes;
            try
            {
                fileBytes = File.ReadAllBytes(file2Load);
            }catch (Exception ex)
            {
                System.Windows.MessageBox.Show("No such File found");
                stopOperation = true;
                return;
            }
            Length = fileBytes.Length;
            Addr = Convert.ToUInt32(StartAddressTextBox.Text, 16);

            LockControls(true);
            stopOperation = false;

            while (BytesVerified < Length)
            {
                if (Length - BytesVerified > 512)
                    count = 512;
                else
                    count = (UInt32) Length - BytesVerified;

                for (i = 0; i < count; i += 2)
                {
                    uint fileptr = (uint) (BytesVerified + i) ;
                    word = (UInt32)(fileBytes[fileptr + 1] << 8) | fileBytes[fileptr];

                    wordRead = flash.FlashReadWord(Addr+ fileptr/2);

                    if (word != wordRead)
                    { 
                        System.Windows.MessageBox.Show("data mismatch at address : "+ Addr+ fileptr/2);
                        LockControls(false);
                        return;
                    }
                    //Addr += 1;
                }
                BytesVerified += count;
                progressBar3.Value = (int)(BytesVerified * 100 / Length);
                label6.Text = progressBar3.Value + "% Complete";
                System.Windows.Forms.Application.DoEvents();
                if (stopOperation) break;
            }
            LockControls(false);
            if (!stopOperation)
            {
                label6.Text = "100% Complete";
                label7.Text = "Verify Completed!";
            }
        }
        private void ReadButtonClick(object sender, EventArgs e)//read
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Binary Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bin files (*.bin)|*.bin",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;

                byte[] fileBytes;
                uint size = 10;// Convert.ToUInt32(textBox_size.Text, 16);
                //int size = Convert.ToInt32((.Text).ToString,16);
                fileBytes = new byte[size];

                //device.flashWrite(0x50055, 0x98);

                for (int i = 0; i < 500; i+=2)
                {
                    uint word =device.flashRead(0x0000 + Convert.ToUInt32(i/2));
                    fileBytes[i] =(byte)( word & 0xFF);
                    fileBytes[i+1] =(byte)( (word >> 8) & 0xFF);

                    Console.WriteLine(String.Format("0x{0:X4}",word));
                }

                try
                {
                    File.WriteAllBytes(fileName, fileBytes);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in process: {0}", ex);
                }
            }
        }
        private void ProgramButtonClick(object sender, EventArgs e)//program
        {
            
            resetProgress();
            label1.Enabled = true;
            label2.Enabled = true;
            label3.Enabled = true;

            label4.Enabled = true;
            label4.Text = "0%";
            label5.Enabled = true;
            label5.Text = "0%";
            label6.Enabled = true;
            label6.Text = "0%";
            progressBar1.Enabled = true;
            progressBar1.Value = 0;
            progressBar2.Enabled = true;
            progressBar2.Value = 0;
            progressBar3.Enabled = true;
            progressBar3.Value = 0;
            label7.Text = "Erasing...";
            stopOperation = false;

            List<Sector> sectorsToErase = new List<Sector>();
            uint eraseSize = 0;
            for(int i = 0; i < flash.sectors.Count; i++)
            {
                eraseSize += flash.sectors[i].size;
                sectorsToErase.Add(flash.sectors[i]);
                if (eraseSize >= fileSize/2) break;
            }
            EraseSectors(sectorsToErase);
            WriteButtonClick(sender, e);
            if(!parallelVerify)
            VerifyBurronClick(sender, e);

            
        }

        private void FileBrowse(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Binary Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bin files (*.bin)|*.bin",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file2Load = openFileDialog1.FileName;
                Console.WriteLine(file2Load);
                textBox1.Text= file2Load;
                fileSize = (new FileInfo(file2Load)).Length;
                FileSizeBox.Text=(fileSize/1024).ToString();
            }
                
        }

        private bool isHex(string s)
        {
            char[] allowedChars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            foreach (char c in s.ToUpper())
            {

                if (!allowedChars.Contains(c) || !allowedChars.Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
        private void AddrBox_Validating(object sender, CancelEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (!isHex(t.Text))
            {
                System.Windows.Forms.MessageBox.Show("'" + t.Text + "' is not Not a hexadecimal Number");
                e.Cancel = true;
                t.Text = "0000";
            }
        }
    }
}

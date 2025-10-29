using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Microsoft.Win32;
using FTD2XX_NET;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ElectronicBoardData brddata = new ElectronicBoardData("BoardFile.json");
            electronicBoards = brddata.GetAll();
            buildDict();

        }

        private void buildDict()
        {
            deviceMap.Clear();
            for(int i=0;i< electronicBoards.Count;i++)
            {
                deviceMap.Add(electronicBoards[i].DEVID,
                    new deviceInfo(electronicBoards[i].DEVICENAME, electronicBoards[i].IRLEN,i));
            }
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    //label1.Text = "pinging google...";
        //    Process process = new Process();
        //    ProcessStartInfo startInfo = new ProcessStartInfo
        //    {
        //        WindowStyle = ProcessWindowStyle.Hidden,
        //        RedirectStandardInput = true,
        //        RedirectStandardOutput = true,
        //        CreateNoWindow = true,
        //        UseShellExecute = false,
        //        FileName = "CMD.exe",
        //        Arguments = "/c ping google.com -n 3"
        //    };
        //    process.StartInfo = startInfo;
        //    process.Start();
        //    string output = process.StandardOutput.ReadToEnd();
        //    //label1.Text = output;
        //    process.WaitForExit();
        //    using (ManagementClass i_Entity = new ManagementClass("Win32_PnPEntity"))
        //    {
        //        foreach (ManagementObject i_Inst in i_Entity.GetInstances())
        //        {
        //            Object o_Guid = i_Inst.GetPropertyValue("ClassGuid");
        //            if (o_Guid == null || o_Guid.ToString().ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
        //                continue; // Skip all devices except device class "PORTS"

        //            String s_Caption = i_Inst.GetPropertyValue("Caption").ToString();
        //            String s_Manufact = i_Inst.GetPropertyValue("Manufacturer").ToString();
        //            String s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
        //            String s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";
        //            String s_PortName = Registry.GetValue(s_RegPath, "PortName", "").ToString();

        //            int s32_Pos = s_Caption.IndexOf(" (COM");
        //            if (s32_Pos > 0) // remove COM port from description
        //                s_Caption = s_Caption.Substring(0, s32_Pos);

        //            Console.WriteLine("Port Name:    " + s_PortName);
        //            Console.WriteLine("Description:  " + s_Caption);
        //            Console.WriteLine("Manufacturer: " + s_Manufact);
        //            Console.WriteLine("Device ID:    " + s_DeviceID);
        //            Console.WriteLine("-----------------------------------");
        //        }
        //    }

        //}

        //private void label1_Click(object sender, EventArgs e)
        //{

        //}

        //private void listclick(object sender, EventArgs e)
        //{


        //    string[] ports = SerialPort.GetPortNames();

        //    //listBox1.Items.Clear();
        //    //foreach (string port in ports)
        //    //{
        //    //    //Console.WriteLine(port);
        //    //    listBox1.Items.Add(port);
        //    //}
        //    //listBox1.EndUpdate();
        //}



        //private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string[] ports = SerialPort.GetPortNames();
        //    bool com=false, lpt0=false, lpt1 = false;
        //    foreach (string port in ports)
        //    {
        //        if (port.Substring(0, 3) == "COM")
        //            com = true;
        //        else if (port == "LPT0")
        //            lpt0 = true;
        //        else if (port == "LPT1")
        //            lpt1 = true;
        //    }
        //    string curItem = listBox2.SelectedItem.ToString();
        //    if (curItem == "USB" && !com)
        //    {
        //        MessageBox.Show("no USB Detected"); return;
        //    }
        //    if (curItem == "LPT0" && !lpt0)
        //    {
        //        MessageBox.Show("no LPT0 Detected"); return;
        //    }
        //    if (curItem == "LPT1" && !lpt1)
        //    {
        //        MessageBox.Show("no LPT1 Detected"); return;
        //    }
        //}


        private void usb_scan(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Text = "Select a Device";
            devices = jTAG.getDeviceList();
            if (devices.Count == 0)
            {
                comboBox1.Text = "No device Found";
            }
            else
            comboBox1.Items.AddRange(devices.ToArray());

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //int n = 7;
            List<String> list = new List<String>();
            for(int i=0; i < deviceCount; i++)
            {
                list.Add(String.Format("{0:x8}",IDList[i]));
            }
            drawChain(e, list);   
            
        }
        private void drawChain(PaintEventArgs e,List<String> lst)
        {
            int rectWidth = 100;
            int rectHeight = 60;
            int rectSpaceX = 40;
            int rectSpaceY = 40;
            int canvasWidth = panel1.Size.Width;
            int canvasHeight = panel1.Size.Height;
            int Xmargin = 20;
            int Ymargin = 20;
            int legend_space = 20;

            int MaxBlockInRow = (canvasWidth + rectSpaceX - 2 * Xmargin) / (rectWidth + rectSpaceX);
            int maxBlockCol = (int)Math.Ceiling((double)lst.Count / MaxBlockInRow);
            //Console.WriteLine(maxBlockCol);
            int new_Height =maxBlockCol * (rectSpaceY + rectHeight) + Ymargin * 2- rectSpaceY +legend_space;
            Size new_size = new Size(canvasWidth, new_Height);
            internal_resize = true;
            panel1.MaximumSize = new_size;
            panel1.Size = new_size;
            internal_resize = false;

            int f_size = 8;
            Font drawFont = new Font("Arial", f_size);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            StringFormat drawFormat = new StringFormat();
            drawFormat.LineAlignment = StringAlignment.Center;
            drawFormat.Alignment = StringAlignment.Center;
            drawFormat.FormatFlags = 0;

            SolidBrush validBrush = new SolidBrush(Color.FromArgb(128, 0, 150, 0));
            SolidBrush invalidBrush = new SolidBrush(Color.FromArgb(128, 150, 0, 0));
            SolidBrush selectedBrush = new SolidBrush(Color.FromArgb(128, 170, 170, 40));
            Graphics g = e.Graphics;
            int row = 0;
            int n = 0;

            int a, b, c;
            a = canvasWidth / 2-115;
            b = canvasWidth / 2+15;
            c=canvasWidth / 2+145;
            e.Graphics.FillRectangle(invalidBrush, new Rectangle(a-80, new_Height- legend_space, 30, 15));
            e.Graphics.FillRectangle(validBrush, new Rectangle(b-73, new_Height - legend_space, 30, 15));
            e.Graphics.FillRectangle(selectedBrush, new Rectangle(c-73, new_Height - legend_space, 30, 15));
            g.DrawString("Unidentified Device", drawFont, drawBrush, a, new_Height - legend_space + 7.5f, drawFormat);
            g.DrawString("Identified Device" , drawFont, drawBrush, b, new_Height - legend_space + 7.5f, drawFormat);
            g.DrawString("Selected Device", drawFont, drawBrush, c, new_Height - legend_space + 7.5f, drawFormat);
            
            selectable = true;
            rectList = new List<Rectangle>();

            while (n<lst.Count)
            {
                for (int col = 0; col < MaxBlockInRow && n < lst.Count; n++, col++)
                {
                    int X = Xmargin + col * (rectSpaceX + rectWidth);
                    int Y = Ymargin + row * (rectSpaceY + rectHeight);
                    Rectangle deviceBox = new Rectangle(X, Y, rectWidth, rectHeight);
                    rectList.Add(deviceBox);
                    g.DrawRectangle(Pens.Gray, deviceBox);
                    g.DrawRectangle(Pens.Gray, new Rectangle(X, Y, 20, 15));
                    g.DrawRectangle(Pens.Gray, new Rectangle(X+20, Y, rectWidth-20, 15));

                    String str = lst[n];

                    int X_text = X+rectWidth/2;
                    int Y_text = Y+rectHeight/2;
                    g.DrawString("D"+(n+1).ToString(), drawFont, drawBrush, X+10, Y+7.5f, drawFormat);
                    g.DrawString("ID: " +str, drawFont, drawBrush, X_text+10, Y+7.5f, drawFormat);

                    if (SelectedIndex == n)
                    {
                        e.Graphics.FillRectangle(selectedBrush, deviceBox);
                        g.DrawString(deviceMap[str].name, drawFont, drawBrush, X_text, Y_text - 7.5f, drawFormat);
                        g.DrawString("IR Length: " + deviceMap[str].IRlen, drawFont, drawBrush, X_text, Y_text + 6.5f, drawFormat);
                        //g.DrawString(deviceMap[str].brdFile, drawFont, drawBrush, X_text, Y_text + 20.5f, drawFormat);
                    }
                    else if(deviceMap.ContainsKey(str))
                    {
                        e.Graphics.FillRectangle(validBrush, deviceBox);
                        g.DrawString(deviceMap[str].name, drawFont, drawBrush, X_text , Y_text-7.5f, drawFormat);
                        g.DrawString("IR Length: "+deviceMap[str].IRlen, drawFont, drawBrush, X_text, Y_text +6.5f, drawFormat);
                       // g.DrawString(deviceMap[str].brdFile, drawFont, drawBrush, X_text, Y_text +20.5f, drawFormat);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(invalidBrush, deviceBox);
                        g.DrawString("UNIDENTIFIED", drawFont, drawBrush, X_text , Y_text-7.5f, drawFormat);
                        g.DrawString("Please Add Board", drawFont, drawBrush, X_text, Y_text + 7.5f, drawFormat);
                        g.DrawString("File for this ID!", drawFont, drawBrush, X_text, Y_text + 20f, drawFormat);
                        selectable= false;
                    }

                    if (n < lst.Count-1)
                    {
                        if (col < MaxBlockInRow - 1)
                            arrow(e, X + rectWidth, Y + rectHeight / 2, 1, 0, 40);
                        else
                            arrow(e, X + rectWidth / 2, Y + rectHeight, 1, 1, 40);
                    }
                }
                row++;
                for (int col = MaxBlockInRow - 1; col >= 0 && n <lst.Count; n++, col--)
                {
                    int X = Xmargin + col * (rectSpaceX + rectWidth);
                    int Y = Ymargin + row * (rectSpaceY + rectHeight);
                    Rectangle deviceBox = new Rectangle(X, Y, rectWidth, rectHeight);
                    rectList.Add(deviceBox);
                    g.DrawRectangle(Pens.Gray, deviceBox);
                    g.DrawRectangle(Pens.Gray, new Rectangle(X, Y, 20, 15));
                    g.DrawRectangle(Pens.Gray, new Rectangle(X + 20, Y, rectWidth - 20, 15));

                    String str = lst[n];

                    int X_text = X + rectWidth / 2;
                    int Y_text = Y + rectHeight / 2;
                    g.DrawString("D" + (n+1).ToString(), drawFont, drawBrush, X + 10, Y + 7.5f, drawFormat);
                    g.DrawString("ID: " + str, drawFont, drawBrush, X_text + 10, Y + 7.5f, drawFormat);

                    if (SelectedIndex == n)
                    {
                        e.Graphics.FillRectangle(selectedBrush, deviceBox);
                        g.DrawString(deviceMap[str].name, drawFont, drawBrush, X_text, Y_text - 7.5f, drawFormat);
                        g.DrawString("IR Length: " + deviceMap[str].IRlen, drawFont, drawBrush, X_text, Y_text + 6.5f, drawFormat);
                       // g.DrawString(deviceMap[str].brdFile, drawFont, drawBrush, X_text, Y_text + 20.5f, drawFormat);
                    }
                    else if (deviceMap.ContainsKey(str))
                    {
                        e.Graphics.FillRectangle(validBrush, deviceBox);
                        g.DrawString(deviceMap[str].name, drawFont, drawBrush, X_text, Y_text - 7.5f, drawFormat);
                        g.DrawString("IR Length: " + deviceMap[str].IRlen, drawFont, drawBrush, X_text, Y_text + 6.5f, drawFormat);
                       // g.DrawString(deviceMap[str].brdFile, drawFont, drawBrush, X_text, Y_text + 20.5f, drawFormat);
                    }
                    else
                    {
                        e.Graphics.FillRectangle(invalidBrush, deviceBox);
                        g.DrawString("UNIDENTIFIED", drawFont, drawBrush, X_text, Y_text - 7.5f, drawFormat);
                        g.DrawString("Please Add Board", drawFont, drawBrush, X_text, Y_text + 7.5f, drawFormat);
                        g.DrawString("File for this ID!", drawFont, drawBrush, X_text, Y_text + 20f, drawFormat);
                        selectable= false;
                    }

                    if (n < lst.Count - 1)
                    {
                        if (col > 0)
                            arrow(e, X, Y + rectHeight / 2, 0, 1, 40);
                        else
                            arrow(e, X + rectWidth / 2, Y + rectHeight, 1, 1, 40);
                    }
                }
                row++;
            }
            if(selectable == true)
            {
                List<int>irLen= new List<int>();
                for(int i = 0; i < lst.Count;i++)
                {
                    irLen.Add(deviceMap[lst[i]].IRlen);
                }
                jTAG.initChainConfig(irLen);
            }
        } 
        private void arrow(PaintEventArgs pea,int curr_X,int curr_Y,int Dir_X,int Dir_Y, int arr_length = 50)
        {
           
            Pen pen = new Pen(Brushes.Black, 2);

            PointF pt1 = new PointF(curr_X, curr_Y); //start point

            if (Dir_X == 1 && Dir_Y == 0)
            { // ---->
                PointF pt2 = new PointF(curr_X + arr_length, curr_Y); //end point
                PointF pt3 = new PointF(curr_X + arr_length - 10, curr_Y + 5);
                PointF pt4 = new PointF(curr_X + arr_length - 10, curr_Y - 5);
                pea.Graphics.DrawLine(pen, pt1, pt2);
                pea.Graphics.DrawLine(pen, pt2, pt3);
                pea.Graphics.DrawLine(pen, pt2, pt4);

            }
            else if (Dir_X == 0 && Dir_Y == 1)
            { // <----
                PointF pt2 = new PointF(curr_X - arr_length, curr_Y); //end point
                PointF pt3 = new PointF(curr_X - arr_length + 10, curr_Y + 5);
                PointF pt4 = new PointF(curr_X - arr_length + 10, curr_Y - 5);
                pea.Graphics.DrawLine(pen, pt1, pt2);
                pea.Graphics.DrawLine(pen, pt2, pt3);
                pea.Graphics.DrawLine(pen, pt2, pt4);
            }
            else
            { // |
              // \/
                PointF pt2 = new PointF(curr_X, curr_Y + arr_length); //end point
                PointF pt3 = new PointF(curr_X - 5, curr_Y + arr_length - 10);
                PointF pt4 = new PointF(curr_X + 5, curr_Y + arr_length - 10);
                pea.Graphics.DrawLine(pen, pt1, pt2);
                pea.Graphics.DrawLine(pen, pt2, pt3);
                pea.Graphics.DrawLine(pen, pt2, pt4);
            }

        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (!internal_resize)
            {
                panel1.Invalidate();
                panel1.Update();
                panel1.Refresh();
            }
            groupBox2.Location = new Point(groupBox2.Location.X, panel1.Size.Height + panel1.Location.Y + 20);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
            Size new_size = new Size( this.Size.Width - 40, panel1.Size.Height);
            
            panel1.MaximumSize = new_size;
            panel1.Size = new_size;

        }

        private void button1_Click(object sender, EventArgs e)//connect button
        {

            if (this.connected)
            {
                if (SelectedIndex != -1)
                    deviceList[SelectedIndex].DeSelect();
                if (jTAG.Close())
                {
                    label3.Text = "Disconnected from " + comboBox1.SelectedItem.ToString();
                    this.connected = false;
                    button1.Text = "Connect";
                    comboBox1.Enabled = true;
                    
                    SelectedIndex = -1;
                    groupBox2.Visible= false;
                    deviceCount = 0;
                    panel1.Invalidate();
                    panel1.Update();
                    panel1.Refresh();
                }
                else
                    label3.Text = "Error occoured while disconnecting!";

            }
            else if (comboBox1.Items.Count == 0)
            {
                label3.Text = "No device Selected!";
            } else{
                if (jTAG.OpenBySerial(devices[comboBox1.SelectedIndex].SerialNumber))
                {
                    label3.Text = "Connected to " + comboBox1.SelectedItem.ToString();
                    this.connected = true;
                    button1.Text = "Disconnect";
                    comboBox1.Enabled = false;
                    IDList=jTAG.DetectChainLength();
                    if (IDList == null)
                        deviceCount = 0;
                    else
                        deviceCount = IDList.Count();
                    panel1.Invalidate();
                    panel1.Update();
                    panel1.Refresh();
                    deviceList = new List<Device>();
                    if (selectable)
                    {
                        for (int i = 0; i < deviceCount; i++)
                        {
                            int brd = deviceMap[String.Format("{0:x8}", IDList[i])].brdFile;
                            deviceList.Add(new Device(
                                jTAG, i,
                                electronicBoards[brd]
                                ));
                        }
                    }
                }
                else
                    label3.Text = "Error occoured while connecting!";
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.jTAG = new JTAG();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!selectable)
            {
                MessageBox.Show("To select a device board files of all the\n devices in chain must be present.\n Please add the missing board files!");
                return;
            }
            for(int i=0;i<deviceCount;i++)
            {
                if (rectList[i].Contains(e.Location))
                {
                    groupBox2.Visible = true;
                    if (SelectedIndex != i)
                    {
                        int prevIndex = SelectedIndex;
                        SelectedIndex = i;
                        string id = String.Format("{0:x8}", IDList[i]);
                        label7.Text = id;
                        label8.Text = deviceMap[id].name;
                        label11.Text = deviceMap[id].IRlen.ToString();
                        label10.Text = deviceMap[id].brdFile.ToString();
                        label12.Text = String.Format("Selected Device No {0:d2}", SelectedIndex+1);
                        panel1.Invalidate();
                        panel1.Update();
                        panel1.Refresh();
                        //int ircode = 0b100110;//extest
                        //jTAG.writeIRbyIndex(i, ircode);
                        if(prevIndex!=-1)
                            deviceList[prevIndex].DeSelect();
                        deviceList[i].Select();

                    }
                   
                }
            }
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flashPannel = new Form3(this, deviceList[SelectedIndex]);
            flashPannel.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            debugPannel = new Form2(this, deviceList[SelectedIndex]);
            debugPannel.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (connected)
            {
                if (SelectedIndex != -1)
                    deviceList[SelectedIndex].DeSelect();
                jTAG.Close();
            }
        }
    }
}


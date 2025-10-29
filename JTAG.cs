using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FTD2XX_NET;



namespace WindowsFormsApp1
{
    public class JTAG
    {
        private const int READ_TIMEOUT = 0; //2000
        private const int WRITE_TIMEOUT = 2000;
        private const int MAX_JTAG_DEVICES = 32;
        private const int MAX_IR_SIZE = 32;
        private const int CLOCK_DIVISOR = 0x0001; //clock devisor
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x05DB)*2) (MHz) =  20Khz
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x012B)*2) (MHz) = 100Khz
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x001D)*2) (MHz) =   1Mhz
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x0002)*2) (MHz) =  10Mhz	
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x0001)*2) (MHz) =  15Mhz	
                                                  // Value of clock divisor, SCL Frequency = 60/((1+0x0000)*2) (MHz) =  30Mhz	

        private FTDI myFtdiDevice;
        private Byte[] outputBuffer=new Byte[4096];//4kB Buffer
        private Byte[] inputBuffer=new Byte[4096];//4kB Buffer
        private uint bytesToWrite=0;
        private uint bytesToRead=0;
        private uint bytesSent;
        private uint bytesRead;
        private List<int> irLen;
        private int chainLength;
        private int irLength;
        FTDI.FT_STATUS ftStatus;
        public JTAG() {
            myFtdiDevice = new FTDI();
            ftStatus = FTDI.FT_STATUS.FT_OK;
            irLen = null;
            chainLength = 0;
        }

        public List<ComboboxDeviceItem> getDeviceList()
        {
            UInt32 ftdiDeviceCount = 0;
            List<ComboboxDeviceItem> list=  new List<ComboboxDeviceItem>();

            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {   
                Console.WriteLine("Number of FTDI devices: " + ftdiDeviceCount.ToString());
                Console.WriteLine("");
            }
            else
            {
                MessageBox.Show("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                Console.WriteLine("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                return list;
            }

            if (ftdiDeviceCount == 0)
                return list;

            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);
            

            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                for (UInt32 i = 0; i < ftdiDeviceCount; i++)
                {
                    Console.WriteLine("Device Index: " + i.ToString());
                    Console.WriteLine("Flags: " + String.Format("{0:x}", ftdiDeviceList[i].Flags));
                    Console.WriteLine("Type: " + ftdiDeviceList[i].Type.ToString());
                    Console.WriteLine("ID: " + String.Format("{0:x}", ftdiDeviceList[i].ID));
                    Console.WriteLine("Location ID: " + String.Format("{0:x}", ftdiDeviceList[i].LocId));
                    Console.WriteLine("Serial Number: " + ftdiDeviceList[i].SerialNumber.ToString());
                    Console.WriteLine("Description: " + ftdiDeviceList[i].Description.ToString());
                    Console.WriteLine("");

                    ComboboxDeviceItem item = new ComboboxDeviceItem();
                    item.Type = ftdiDeviceList[i].Type.ToString();
                    item.DeviceID = String.Format("{0:x}", ftdiDeviceList[i].ID);
                    item.SerialNumber = ftdiDeviceList[i].SerialNumber.ToString();
                    item.Description = ftdiDeviceList[i].Description.ToString();
                    list.Add(item);

                }
            }
            return list;
        }

        private bool testBadCommand()
        {
            outputBuffer[0] = 0xAA;
            ftStatus = myFtdiDevice.Write(outputBuffer,0x01,ref bytesSent);
            do
            {
                myFtdiDevice.GetRxBytesAvailable(ref bytesToRead);
            } while (ftStatus == FTDI.FT_STATUS.FT_OK && bytesToRead==0);
            ftStatus =myFtdiDevice.Read(inputBuffer,bytesToRead,ref bytesRead);
            if (ftStatus==FTDI.FT_STATUS.FT_OK && inputBuffer[0] == 0xFA && inputBuffer[1] == outputBuffer[0])
                return true;
            else 
                return false;
        }
        private void EnableLoopBack()
        {  //tdi->tdo(loopback)    //tdi->devices->tdo(normally)
            outputBuffer[0] = 0x84;
            ftStatus |= myFtdiDevice.Write(outputBuffer, 1, ref bytesSent);
        }
        private void DisableLoopBack() {
            outputBuffer[0] = 0x85;
            ftStatus |= myFtdiDevice.Write(outputBuffer, 1, ref bytesSent);
        }
        public void AddFlushBuffer()
        {
            outputBuffer[bytesToWrite++] = 0x87;
        }
        public void Purge()
        {
            myFtdiDevice.Purge(FTDI.FT_PURGE.FT_PURGE_TX | FTDI.FT_PURGE.FT_PURGE_RX);
        }

        private bool testDeviceInLoop()
        {
            byte[] data = { 1,0,0,0,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,1,0,0,1,0,1,1,0,1,1};
            byte[] readData = new byte[data.Length];

            bytesToWrite = 0;
            EnterIdleMode();
            EnableLoopBack();
            ReadDR(data, ref readData,data.Length);
            DisableLoopBack();

            Console.WriteLine("\nTesting Device in Loop\nSent Bits ...");
            for (int i = 0; i < data.Length; i++)
                Console.Write(data[i] + " ");
            Console.WriteLine("\nReceived Bits...");
            bool match = true;
            for (int i = 0; i < readData.Length; i++)
            {
                Console.Write(readData[i] + " ");
                if (readData[i] != data[i])
                    match = false;

            }
            Console.WriteLine();
            if (match&& ftStatus==FTDI.FT_STATUS.FT_OK)
                return true;
            else
                return false;
  
        }
        private void EnterIdleMode()
        {
            //H,H,H,H,H,L-> 1001 1111 -> 0x9F //0001 1111
            outputBuffer[bytesToWrite++] = 0x4B;//TMS Write -VE
            outputBuffer[bytesToWrite++] = 0x05;//6bit long
            outputBuffer[bytesToWrite++] = 0x9F;   //1F
        }
        public bool IdleMode()
        {
            bytesToWrite = 0;
            EnterIdleMode();
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            return ftStatus==FTDI.FT_STATUS.FT_OK;
        }
        private void EnterResetMode()
        {
            //H,H,H,H,H-> 1001 1111 -> 0x9F 
            outputBuffer[bytesToWrite++] = 0x4B;//TMS Write -VE
            outputBuffer[bytesToWrite++] = 0x04;//5bit long
            outputBuffer[bytesToWrite++] = 0x9F;  //FF
        }
        private bool ResetMode()
        {
            bytesToWrite = 0;
            EnterResetMode();
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            return ftStatus == FTDI.FT_STATUS.FT_OK;
        }
        private void EnterShiftIRMode()
        {
            //L,H,H,L,L -> 0000 0110 ->0x06 (ENTER shift IR Mode)
            outputBuffer[bytesToWrite++] = 0x4B;//TMS write -VE
            outputBuffer[bytesToWrite++] = 0x04;//5bit long
            outputBuffer[bytesToWrite++] = 0x06;
            //0000 1100
        }
        private void EnterShiftDRMode()
        {
            //L,H,L,L -> 0000 0010 ->0x02 (ENTER shift DR Mode)
            outputBuffer[bytesToWrite++] = 0x4B;//TMS write -VE
            outputBuffer[bytesToWrite++] = 0x03;//4bit long
            outputBuffer[bytesToWrite++] = 0x02;

        }
        private void ExitWriteMode(byte lastBit)
        {
            //H,H,L -> b000 0011 ->0x03 (ENTER Run-Test/Idle Mode) , b=last bit
            outputBuffer[bytesToWrite++] = 0x4B;//TMS write -VE
            outputBuffer[bytesToWrite++] = 0x02;//4bit long
            outputBuffer[bytesToWrite++] = (byte)(0x03 | (lastBit<<7));
            //AddFlushBuffer();

        }
        private void ExitReadMode(byte lastBit)
        {
            //H,H,L -> b000 0011 ->0x03 (ENTER Run-Test/Idle Mode) , b=last bit

            outputBuffer[bytesToWrite++] = 0x6F;//DIN -ve DOUT+VE
            outputBuffer[bytesToWrite++] = 0x00;//1bit long
            outputBuffer[bytesToWrite++] = (byte)(0x03 | (lastBit << 7));
            outputBuffer[bytesToWrite++] = 0x4B;//DOUT-VE
            outputBuffer[bytesToWrite++] = 0x01;//1bit long
            outputBuffer[bytesToWrite++] = 0x01;

        }

        //------------------------------------------------------------------------------ read write transfer functions--------------------
        //-----------------read func-----------------------
        private void EnterReadBytes(byte[] data, int len)
        {
            len--;
            outputBuffer[bytesToWrite++] = 0x3D;//LSB write Bytes -VE
            outputBuffer[bytesToWrite++] = (byte)(len & 0xFF);   //LengthL
            outputBuffer[bytesToWrite++] = (byte)((len >> 8) & 0xFF);//LengthH
            for (int i = 0; i <= len; i++)
                outputBuffer[bytesToWrite++] = data[i];
        }
        private void EnterReadBits(byte[] data, int len, int st = 0)
        {
            len--;
            outputBuffer[bytesToWrite++] = 0x3F;//LSB r/w bits -VE
            outputBuffer[bytesToWrite++] = (byte)(len & 0xFF);   //Length
            outputBuffer[bytesToWrite] = 0;//data init
            for (int i = 0; i <= len; i++)
                outputBuffer[bytesToWrite] |= (byte)(data[st + i] << i);
            bytesToWrite++;
        }

        private void EnterReadBitStream(byte[] data, int len)
        {
            len--;//last bit clocked with TMS
            if (len < 0) return;
            int lenBytes = len / 8;
            int lenBits = len % 8;
            
            byte[] dataByte=new byte[lenBytes];
            for (int i = 0; i < lenBytes; i++)
            {
                dataByte[i] = 0;
                for (int j = 0; j < 8; j++)
                    dataByte[i] |= (byte)(data[i*8 + j] << j);
            }
            if(lenBytes>0)
                EnterReadBytes(dataByte, lenBytes);
            if(lenBits>0)
                EnterReadBits(data, lenBits, 8 * lenBytes);
            ExitReadMode(data[len]);


        }
        public bool ReadDR(byte[] data,ref byte[] readData, int len)
        {
            bytesToWrite = 0;ftStatus = FTDI.FT_STATUS.FT_OK;
            EnterShiftDRMode();
            EnterReadBitStream(data, len);
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            len--;
            int lenBytes = len / 8;
            int lenBits = len % 8;
            int expectedBytes = lenBytes + ((lenBits > 0) ? 2 : 1);
            do
            {
                ftStatus |= myFtdiDevice.GetRxBytesAvailable(ref bytesToRead);
                //sConsole.WriteLine(bytesToRead);
            } while (ftStatus == FTDI.FT_STATUS.FT_OK && bytesToRead < expectedBytes);
            //Console.WriteLine(bytesToRead);
            if (bytesToRead > 0)
            {
                ftStatus |= myFtdiDevice.Read(inputBuffer, bytesToRead, ref bytesRead);
                if (ftStatus == FTDI.FT_STATUS.FT_OK)
                {
                    //readbytes
                    for (int i = 0; i < lenBytes; i++)
                        for (int j = 0; j < 8; j++)
                            readData[8 * i + j] = (byte)(inputBuffer[i] >> j & 0x1);
                    //read bits
                    for (int i = 0; i <= lenBits; i++)
                        readData[len - i] = (byte)(inputBuffer[bytesToRead - 1] >> (7 - i) & 0x1);
                    return true;
                }
            }
            return false;
        }
        public bool ReadIR(byte[] data, ref byte [] readData,int len)
        {
            bytesToWrite = 0;
            EnterShiftIRMode();
            EnterReadBitStream(data, len);
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            len--;
            int lenBytes = len / 8;
            int lenBits = len % 8;
            int expectedBytes = lenBytes +( (lenBits > 0) ? 2 : 1);
            do
            {
                ftStatus |= myFtdiDevice.GetRxBytesAvailable(ref bytesToRead);
            } while (ftStatus == FTDI.FT_STATUS.FT_OK && bytesToRead < expectedBytes) ;
            //Console.WriteLine(bytesToRead);
            if (bytesToRead > 0 )
            {
                ftStatus = myFtdiDevice.Read(inputBuffer, bytesToRead, ref bytesRead);
                if(ftStatus == FTDI.FT_STATUS.FT_OK)
                {
                    for (int i = 0; i < lenBytes; i++)
                        for (int j = 0; j < 8; j++)
                            readData[8 * i + j] = (byte)(inputBuffer[i] >> j & 0x1);
                    for (int i = 0; i <=lenBits; i++)
                        readData[len - i] = (byte)(inputBuffer[bytesToRead - 1] >> (7 -i) & 0x1);


                return true;
                }

            }
            return false;
        }
        //--------------write  func---------------------
        private void EnterWriteBytes(byte[] data,int len)
        {
            len--;
            outputBuffer[bytesToWrite++] = 0x19;//LSB write Bytes -VE
            outputBuffer[bytesToWrite++] = (byte)(len & 0xFF);   //LengthL
            outputBuffer[bytesToWrite++] = (byte)((len>>8)&0xFF);//LengthH
            for (int i = 0; i <= len; i++)
            {
                outputBuffer[bytesToWrite++] =data[i];
            }
        }
        private void EnterWriteBits(byte[] data, int len,int st=0)
        {
            len--;
            outputBuffer[bytesToWrite++] = 0x1B;//LSB write Bytes -VE
            outputBuffer[bytesToWrite++] = (byte)(len & 0xFF);   //LengthL
            outputBuffer[bytesToWrite] =0;//data init
            for(int i = 0; i <= len; i++)
            {
                outputBuffer[bytesToWrite] |=  (byte)(data[st + i] << i);
            }
            bytesToWrite++;
        }
        private void EnterWriteBitStream(byte[] data, int len)
        {
            len--;//last bit clocked with TMS
            if (len < 0) return;
            int lenBytes = len / 8;
            int lenBits = len % 8;
            byte[] dataByte = new byte[lenBytes];
            for (int i = 0; i < lenBytes; i++)
            {
                dataByte[i] = 0;
                for (int j = 0; j < 8; j++)
                    dataByte[i] |= (byte)(data[i * 8 + j] << j);
            }
            if (lenBytes > 0)
                EnterWriteBytes(dataByte, lenBytes);
            if (lenBits > 0)
                EnterWriteBits(data, lenBits, 8 * lenBytes);
            ExitWriteMode(data[len]);
         
        }
        public bool WriteIR(byte[] data,int len)
        {

            bytesToWrite = 0; ftStatus = FTDI.FT_STATUS.FT_OK;
            EnterShiftIRMode();
            EnterWriteBitStream(data, len);
            ftStatus = myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            return ftStatus == FTDI.FT_STATUS.FT_OK;
        }
        public bool WriteDR(byte[] data, int len)
        {
            bytesToWrite = 0; ftStatus = FTDI.FT_STATUS.FT_OK;
            EnterShiftDRMode();
            EnterWriteBitStream(data, len);
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);
            return ftStatus == FTDI.FT_STATUS.FT_OK;
        }
        //------------high level func------------------
        public List<UInt32> DetectChainLength()
        { 
            const int maxbuff = MAX_JTAG_DEVICES * MAX_IR_SIZE;
            byte[] data = new byte[maxbuff];
            byte[] read = new byte[maxbuff];
            for (int i = 0; i < maxbuff; i++) data[i] = 1;
            IdleMode();
            WriteIR(data, maxbuff);
            for (int i = 0; i < MAX_JTAG_DEVICES; i++)
            {
                data[i] = 0;
            }
            ReadDR(data, ref read, maxbuff);
            int cnt = 0;
            for (int i=0;i<= MAX_JTAG_DEVICES; i++)
            {
                if (read[i + MAX_JTAG_DEVICES] == 0) cnt++;
                else break;
            }
            if (cnt > MAX_JTAG_DEVICES)
            {
                Console.WriteLine("Error JTAG LOOP OPEN !");
                return null;
            }
            List<UInt32> DID = new List<UInt32>();
            for (int i = MAX_JTAG_DEVICES+cnt; i <maxbuff; i++)
            {
                if (read[i] != 1)
                {
                    Console.WriteLine(" Error while reading BYPASS REGISTERS !");
                    return null;
                }
            }
            Console.WriteLine("Devices in Chain:" + cnt);
            IdleMode();
            ReadDR(data, ref read, MAX_IR_SIZE * cnt);
            for (int i = 0; i < cnt; i++)
            {
                DID.Add(0);
                for (int j = 0; j < MAX_IR_SIZE; j++)
                    DID[i] |= (UInt32)(read[i * MAX_IR_SIZE + j]) << j;
                Console.WriteLine(String.Format("Device {1:d2} , {0:x8}", DID[i], cnt - i - 1));
            }
            DID.Reverse();
            if (ftStatus== FTDI.FT_STATUS.FT_OK)
                return DID;
            return null;
        }
        public void initChainConfig(List<int> irLen)
        {
            this.irLen = new List<int>(irLen);
            chainLength = irLen.Count;
            irLength = 0;
            for (int i = 0; i < chainLength; i++)
            {
                irLength += irLen[i];
            }
        }
        public bool writeIRbyIndex(int index, int IRCode)
        {
            if (irLen == null || irLen.Count == 0) return false;

            byte[] dataIR = new byte[irLength];
            int IR_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (i == index)
                {
                    for (int j = 0; j < irLen[i]; j++)
                    {
                        dataIR[IR_ptr++] = (byte)(IRCode >> (irLen[i] - 1 - j) & 1);//for selected device use ir code

                    }
                }
                else
                {
                    for (int j = 0; j < irLen[i]; j++)
                    {
                        dataIR[IR_ptr++] = 1;//for ir of other to be in bypass
                    }
                }

            }
            bool st = true;
            Array.Reverse(dataIR);
            st = st && IdleMode();
            st = st && WriteIR(dataIR, dataIR.Length);
            return st;
        }
        public bool writeDRbyIndex(int index, byte[] data)
        {
            if (irLen == null || irLen.Count == 0) return false;

            byte[] dataDR = new byte[chainLength - 1 + data.Length];
            int DR_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (i == index)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        dataDR[DR_ptr++] = data[j];//for selected device data
                    }
                }
                else
                    dataDR[DR_ptr++] = 0;//any value for bypas register
            }
            Array.Reverse(dataDR);
            return  WriteDR(dataDR, dataDR.Length);
        }
        public bool readDRbyIndex(int index,byte []data,byte[] result)
        {
            if (irLen == null || irLen.Count == 0) return false;
            byte[] dataDR = new byte[chainLength - 1 + data.Length];
            int DR_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (i == index)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        dataDR[DR_ptr++] = data[j];//for selected device data
                    }
                }
                else
                    dataDR[DR_ptr++] = 0;//any value for bypas register

            }
            Array.Reverse(dataDR);
            bool st =ReadDR(dataDR, ref dataDR, dataDR.Length);
            Array.Reverse(dataDR);
            int result_ptr = 0;
            DR_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (i == index)
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        result[result_ptr++] = dataDR[DR_ptr++];//for selected device data
                    }
                    break;
                }
                else
                    DR_ptr++;
            }
            return st;
        }
        public bool writeIRDRpair(int index, int irCode, byte[] data)
        {
            bool st = true;
            st = st && writeIRbyIndex(index, irCode);
            st = st && writeDRbyIndex(index, data);
            return st;
        }
        public bool writeIRbyMultipleIndex(List<int> indexs, int IRCode)
        {
            if (irLen == null || irLen.Count == 0) return false;
            //indexs.Sort();
            byte[] dataIR = new byte[irLength];
            int IR_ptr = 0, ind_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (ind_ptr < indexs.Count && i == indexs[ind_ptr])
                {
                    for (int j = 0; j < irLen[i]; j++)
                    {
                        dataIR[IR_ptr++] = (byte)(IRCode >> (irLen[i] - 1 - j) & 1);//for selected device use ir code

                    }
                    ind_ptr++;
                }
                else
                {
                    for (int j = 0; j < irLen[i]; j++)
                    {
                        dataIR[IR_ptr++] = 1;//for ir of other to be in bypass
                    }
                }

            }
            bool st = true;
            Array.Reverse(dataIR);
            st = st && IdleMode();
            st = st && WriteIR(dataIR, dataIR.Length);
            return st;
        }
        public bool writeDRbyMultipleIndex(List<int> indexs, byte[] data)
        {
            if (irLen == null || irLen.Count == 0) return false;

            int selected = indexs.Count;
            byte[] dataDR = new byte[chainLength - selected + selected * data.Length];
            int DR_ptr = 0, ind_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (ind_ptr < indexs.Count && i == indexs[ind_ptr])
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        dataDR[DR_ptr++] = data[j];//for selected device data
                    }
                    ind_ptr++;
                }
                else
                    dataDR[DR_ptr++] = 0;//any value for bypas register
            }
            Array.Reverse(dataDR);
            return WriteDR(dataDR, dataDR.Length);
        }
        public bool readDRbyMultipleIndex(List<int> indexs, byte[] data, List<byte[]> result)
        {
            if (irLen == null || irLen.Count == 0) return false;
            int selected = indexs.Count;
            byte[] dataDR = new byte[chainLength - selected + selected * data.Length];
            int DR_ptr = 0, ind_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (ind_ptr < indexs.Count && i == indexs[ind_ptr])
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        dataDR[DR_ptr++] = data[j];//for selected device data
                    }
                    ind_ptr++;
                }
                else
                    dataDR[DR_ptr++] = 0;//any value for bypas register

            }
            Array.Reverse(dataDR);
            bool st = ReadDR(dataDR, ref dataDR, dataDR.Length);
            Array.Reverse(dataDR);
            int result_ptr = 0;
            DR_ptr = 0; ind_ptr = 0;
            for (int i = 0; i < chainLength; i++)
            {
                if (ind_ptr < indexs.Count && i == indexs[ind_ptr])
                {
                    for (int j = 0; j < data.Length; j++)
                    {
                        result[ind_ptr][result_ptr++] = dataDR[DR_ptr++];//for selected device data
                    }
                    result_ptr = 0;
                    ind_ptr++;
                }
                else
                    DR_ptr++;
            }
            return st;
        }
        //public bool readBoundaryCells(int index, int irCode, byte[] data,byte[] result)
        //{
        //    if (irLen == null || irLen.Count == 0) return false;

        //    byte[] dataIR = new byte[irLength];
        //    byte[] dataDR = new byte[chainLength - 1 + data.Length];
        //    byte[] read = new byte[1100];
        //    int IR_ptr = 0;
        //    int DR_ptr = 0;
        //    for (int i = 0; i < chainLength; i++)
        //    {
        //        if (i == index)
        //        {
        //            for (int j = 0; j < irLen[i]; j++)
        //            {
        //                dataIR[IR_ptr++] = (byte)(irCode >> (irLen[i] - 1 - j) & 1);//for selected device use ir code

        //            }
        //            for (int j = 0; j < data.Length; j++)
        //            {
        //                dataDR[DR_ptr++] = data[j];//for selected device data
        //            }
        //        }
        //        else
        //        {
        //            for (int j = 0; j < irLen[i]; j++)
        //            {
        //                dataIR[IR_ptr++] = 1;//for ir of other to be in bypass
        //            }
        //            dataDR[DR_ptr++] = 0;//any value for bypas register
        //        }

        //    }
        //    bool st = true;
        //    Array.Reverse(dataDR);
        //    Array.Reverse(dataIR);
        //    st = st && IdleMode();
        //    st = st && WriteIR(dataIR, dataIR.Length);
        //    st = st && ReadDR(dataDR,ref dataDR, dataDR.Length);
        //    Array.Reverse(dataDR);
        //    int result_ptr = 0;
        //    DR_ptr = 0;
        //    for (int i = 0; i < chainLength; i++)
        //    {
        //        if (i == index)
        //        {
        //            for (int j = 0; j < data.Length; j++)
        //            {
        //                result[result_ptr++] =dataDR[DR_ptr++];//for selected device data
        //            }
        //            break;
        //        }else 
        //            DR_ptr++;
        //    }
        //    return st;
        //}
        private bool ConfigureMPSSE()
        {
            ftStatus =myFtdiDevice.ResetDevice();
            //ftStatus |= myFtdiDevice.Purge(FTDI.FT_PURGE.FT_PURGE_TX|FTDI.FT_PURGE.FT_PURGE_RX);
            ftStatus |= myFtdiDevice.SetCharacters(0, false, 0, false);
            ftStatus |= myFtdiDevice.SetTimeouts(READ_TIMEOUT, WRITE_TIMEOUT);
            ftStatus |= myFtdiDevice.SetLatency(16);//between 2-255 ms
            ftStatus |= myFtdiDevice.SetBitMode(0x0B, 0x00);
            ftStatus |= myFtdiDevice.SetBitMode(0x00, 0x02);

            if(ftStatus == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("Entered MPSSE Mode!");
            else
            {

                Console.WriteLine("Error in initialising MPSSE (error " + ftStatus.ToString() + ")");
                Close();
                return false;
            }
            if (testBadCommand())
                Console.WriteLine("MPSSE Bad Command Test Passed!");
            else
            {
                Console.WriteLine("Error in syncronizing MPSSE");
                Close();
                return false;
            }

            bytesToWrite = 0;
            // Set TCK frequency 
            // TCK = 60MHz /((1 + [(1 +0xValueH*256) OR 0xValueL])*2)

            outputBuffer[bytesToWrite++] = 0x8A; //use 60Mhz Master Clock (0x8b)
            outputBuffer[bytesToWrite++] = 0x97; //disable adaptive clockig (useful for arm)  (0x96 turn on)
            outputBuffer[bytesToWrite++] = 0x8D; //disable 3 phase data clocking (0x8c turn on)
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);

            bytesToWrite = 0;
            outputBuffer[bytesToWrite++] = 0x80; //low byte MPSSE 
            outputBuffer[bytesToWrite++] = 0xE8; //0001 1000  TRST,TMS ->HIGH,  TDI,TCLK,TDO ->LOW
                                                 //(clk =0 initially ==> data at write @ falling edge)
            outputBuffer[bytesToWrite++] = 0xEB; //0000 1011  TRST,TMS,TDI,TCLK ->Output, TDO ->Input
            ftStatus|= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);

            bytesToWrite = 0;
            outputBuffer[bytesToWrite++] = 0x82; //high byte MPSSE
            outputBuffer[bytesToWrite++] = 0x20; //gpio not required
            outputBuffer[bytesToWrite++] = 0x30; //gpio not required
            ftStatus |= myFtdiDevice.Write(outputBuffer, bytesToWrite, ref bytesSent);

            bytesToWrite = 0;
            outputBuffer[bytesToWrite++] = 0x86;//clk divisor
            outputBuffer[bytesToWrite++] = (byte)(CLOCK_DIVISOR & 0xFF);
            outputBuffer[bytesToWrite++] = (byte)(CLOCK_DIVISOR>> 8 & 0xFF);
            ftStatus= myFtdiDevice.Write(outputBuffer, bytesToWrite,ref bytesSent);
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
                Console.WriteLine("MPSSE Configuration Done!");
            else
            {

                Console.WriteLine("Error in Configuring MPSSE clock (error " + ftStatus.ToString() + ")");
                Close();
                return false;
            }
            if (testDeviceInLoop())
                Console.WriteLine("Test JTAG Device in Loop Test Passed!");
            else
            {
                Console.WriteLine("Error Occoured While Testing JTAG in Loop!");
                Close();
                return false;
            }
            return true;

        }
        public bool OpenBySerial(string sno)
        {
            ftStatus =myFtdiDevice.OpenBySerialNumber(sno);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Console.WriteLine("Failed to open device (error " + ftStatus.ToString() + ")");
                return false;
            }
            Console.WriteLine("\n\nConnected to SerialNo:" + sno + "!");
            return ConfigureMPSSE();

        }
        public bool OpenByDescription(string sno)
        {
            ftStatus = myFtdiDevice.OpenByDescription(sno);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Console.WriteLine("Failed to open device (error " + ftStatus.ToString() + ")");
                return false;
            }
            Console.WriteLine("Connected to Description:" + sno + "!");
            return ConfigureMPSSE();
        }
        public bool OpenByIndex(uint index)
        {
            ftStatus = myFtdiDevice.OpenByIndex(index);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Console.WriteLine("Failed to open device (error " + ftStatus.ToString() + ")");
                return false;
            }
            Console.WriteLine("Connected to Device Index:" + index + "!");
            return ConfigureMPSSE();
        }
        public bool Close()
        {
            irLen = null;
            chainLength = 0;
            ftStatus = myFtdiDevice.Close();
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                Console.WriteLine("Failed to close device (error " + ftStatus.ToString() + ")");
                return false;
            }
            Console.WriteLine("Closed Connection!");
            return true;
        }
    }
}

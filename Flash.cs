using DLPComposer.ControlProgram.FlashProgrammer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class Flash
    {
        // Flash memory class definitions
        const int FLASH_CLASS_A = 0x0; // Class A definition
        const int FLASH_CLASS_B = 0x1; // Class B definition
        const int FLASH_CLASS_C = 0x2; // Class C definition
        const int FLASH_CLASS_RESERVED = 0x3; // Reserved class definition

        // Flash memory manufacturer definitions
        const int FLASH_AMD = 0x01; // AMD manufacturer definition
        const int FLASH_FUJITSU = 0x04; // Fujitsu manufacturer definition
        const int FLASH_STMICRO = 0x20; // STMicro manufacturer definition
        const int FLASH_NUMONYX = 0x20; // Numonyx manufacturer definition
        const int FLASH_TOSHIBA = 0x98; // Toshiba manufacturer definition
        const int FLASH_HYUNDAI = 0xAD; // Hyundai manufacturer definition
        const int FLASH_INTEL = 0x89; // Intel manufacturer definition
        const int FLASH_MICRON = 0x89; // Micron manufacturer definition
        const int FLASH_MICRON2 = 0x2C; // Micron2 manufacturer definition
        const int FLASH_SHARP = 0xB0; // Sharp manufacturer definition
        const int FLASH_ATMEL = 0x1F; // Atmel manufacturer definition
        const int FLASH_ATMEL2 = 0x161F; // Atmel2 manufacturer definition
        const int FLASH_MACRONIX = 0xC2; // Macronix manufacturer definition
        const int FLASH_SAMSUNG = 0xEC; // Samsung manufacturer definition
        const int FLASH_EON = 0x1C; // EON manufacturer definition

        // Other constants
        const int MAX_ERASE_BLOCKS = 7; // Maximum number of erase blocks
        const int ERASE_BLOCK_OVERFLOW = -1; // Erase block overflow value
        const int FLASH_MULTIWORD_BUFFER_SIZE = 64; // Multiword buffer size
        const int FLASH_EON_CONFIGURATION_CODE = 0x7F; // EON configuration code
        const int FLASH_EXTENDED_DEVICE_ID_CODE = 0x227E; // Extended device ID code

        // Dictionary mapping manufacturers to classes
        Dictionary<UInt32, UInt32> FL_FlashType = new Dictionary<UInt32, UInt32>()
        {
            {FLASH_AMD,       FLASH_CLASS_A },
            {FLASH_FUJITSU,   FLASH_CLASS_A },
            {FLASH_STMICRO,   FLASH_CLASS_A },
            {FLASH_TOSHIBA,   FLASH_CLASS_A },
            {FLASH_HYUNDAI,   FLASH_CLASS_A },
            {FLASH_MACRONIX,  FLASH_CLASS_A },
            {FLASH_SAMSUNG,   FLASH_CLASS_A },
            {FLASH_EON,       FLASH_CLASS_A },
            {FLASH_INTEL,     FLASH_CLASS_A },
            //{FLASH_MICRON,    FLASH_CLASS_A }, // This entry is commented out
            {FLASH_SHARP,     FLASH_CLASS_B },
            {FLASH_ATMEL,     FLASH_CLASS_C },
            {FLASH_ATMEL2,    FLASH_CLASS_C },
        };
        // This class saves information about a flash memory device
        public class FlashDeviceInfo
        {
            // Public fields that store device info
            public UInt32 DeviceID; // Device ID value
            public UInt32 ManufactureID; // Manufacturer ID value
            public UInt32 FlashALG; // Flash algorithm value
            public UInt32 DeviceSize; // Device size value
            public UInt32 NumEraseBlocks; // Number of erase blocks
            public UInt32[] EraseBlock; // Array of erase blocks

            // Constructor that initializes EraseBlock array to MAX_ERASE_BLOCKS length
            public FlashDeviceInfo()
            {
                EraseBlock = new UInt32[MAX_ERASE_BLOCKS];
            }
        }

        public class Sector
        {
            public UInt32 startAddr;
            public UInt32 endAddr;
            public UInt32 size;
            public Sector(uint startAddr, uint size)
            {
                this.startAddr = startAddr;
                this.endAddr = startAddr+size-1;
                this.size = size;
            }
        }
        public List<Sector> sectors;
        // Global variables for a flash memory
        private Device device; // Device object
        UInt32 FL_FlashProgramType = 0; // Flash program type
        UInt32 FL_pa, FL_p5; // Other flash-related variables
        public UInt32 FL_gdevid = 0; // Global device ID
        public UInt32 FL_gmanuid = 0; // Global manufacturer ID
        public FlashDeviceInfo m_FlashInfo = new FlashDeviceInfo(); // Object that stores device info

        // Constructor that selects a device and sets its pins
        public Flash(Device device)
        {
            this.device = device;
            device.Select();
            device.setPinWrite(device.CE, 0);
            device.setPinWrite(device.RST, 1);
            device.setPinWrite(device.WE, 1);
            device.setPinWrite(device.OE, 1);
            device.WriteBoundaryCells();
            sectors = new List<Sector>();
            GetFlashMfgAndDevID();
            if(!GetFlashInfoCFI())
                getFlashInfoFile();
           // testFlash();
        }
        private void getFlashInfoFile()
        {
            Console.WriteLine("Fetching FlashParameters.json");
            sectors.Add(new Sector(0x0000, 0x10000));
            sectors.Add(new Sector(0x10000, 0x10000));
            sectors.Add(new Sector(0x20000, 0x10000));
            //FlashMemoryData fmd = new FlashMemoryData("FlashParameters.json");

            //string[] sectors = fmd.GetAll()[0].Sector_Addresses;
            //for (int i = 0; i < sectors.Length - 1; i++)
            //{
            //    long x = Convert.ToInt64(sectors[i + 1], 16);
            //    long y = Convert.ToInt64(sectors[i], 16);
            //    string end = String.Format("0x{0:X7}", x - 1);
            //    string st = String.Format("0x{0:X7}", y);
            //    checkedListBox_sector.Items.Add(st + "-" + end);
            //}
        }
        public void testFlash()
        {
            uint manuid, devid, exdevid1, exdevid2;
            manuid = device.flashRead(0x0000);
            Console.WriteLine(String.Format("before autoselect at 0x0000 : 0x{0:X4}", manuid));
            //id entry
            device.flashWrite(0x0555, 0xAA);
            device.flashWrite(0x02AA, 0x55);
            device.flashWrite(0x0555, 0x90);
            //read
            manuid=device.flashRead(0x0000); //should be 1
            devid=device.flashRead(0x0001);//should be 227E
            exdevid1= device.flashRead(0x000E);//should be 2248
            exdevid2 = device.flashRead(0x000F);//should be 2210
            uint d1=device.flashRead(0x10);
            uint d2 = device.flashRead(0x11);
            uint d3 = device.flashRead(0x12);
            Console.WriteLine("" + d1 + "," + d2 + "," + d3);
            Console.WriteLine(
                String.Format("manufacturer ID : 0x{0:X4}\ndevice ID : 0x{1:X4}\n" +
                "extended device ID 1 : 0x{2:X4}\n extended device ID 2 : 0x{3:X4}",manuid,devid,exdevid1,exdevid2));
           //exit
           device.flashWrite(0x0000, 0xF0);

            manuid = device.flashRead(0x50000);
            Console.WriteLine(String.Format("before cfi at 0x0000 : 0x{0:X4}", manuid));

            //cfi entry
            device.flashWrite(0x0055, 0x98);
            //read
            manuid = device.flashRead(0x0000); //should be 1
            devid = device.flashRead(0x0001);//should be 227E
            exdevid1 = device.flashRead(0x000E);//should be 2248
            exdevid2 = device.flashRead(0x000F);//should be 2210
            Console.WriteLine(
                String.Format("manufacturer ID : 0x{0:X4}\ndevice ID : 0x{1:X4}\n" +
                "extended device ID 1 : 0x{2:X4}\n extended device ID 2 : 0x{3:X4}", manuid, devid, exdevid1, exdevid2));
            //exit
            device.flashWrite(0x0000, 0xF0);

            manuid = device.flashRead(0x0000);
            Console.WriteLine(String.Format("after cfi at 0x0000 : 0x{0:X4}", manuid));

        }
        // Method that resets the flash by exiting ASO mode
        public void FlashReset()
        {
            // device.flashWrite(0x0AAA, 0xFF); // Write to flash memory
            // device.flashWrite(0x0AAA, 0xF0); // Write to flash memory
            // device.flashWrite(0xAAAA, 0xF0); // Write to flash memory 
            device.flashWrite(0x0000, 0xF0);
        }
        // Method that reads information from the flash memory itself
        public void GetFlashMfgAndDevID()
        {
            device.flashWrite(0x0055, 0x98);
            FL_gmanuid = device.flashRead(0x0000); //should be 1
            FL_gdevid = device.flashRead(0x0001);//should be 227E
            device.flashWrite(0x0000, 0xF0);

            // Declare variables for manufacturer and device IDs
            //UInt32 manuid1, manuid2, manuid3, devid1, devid2;

            //// Initialize some variables
            //manuid2 = 0;
            //devid1 = devid2 = 0;

            //// Reset flash memory
            //FlashReset();

            //// Read manufacturer ID value at address 0x00
            //manuid1 = device.flashRead(0x00);

            //device.flashWrite(0x0AAA, 0x90);

            //// Check if condition 1: Class B
            //if (manuid1 != device.flashRead(0x00))
            //{
            //    // Read additional values for Class B
            //    manuid1 = device.flashRead(0x000);
            //    devid1 = device.flashRead(0x002);
            //    manuid3 = device.flashRead(0x200);

            //    // Check for EON Configuration Code
            //    if (manuid1 == FLASH_EON_CONFIGURATION_CODE)
            //    {
            //        if (manuid3 == FLASH_EON) manuid1 = manuid3;
            //    }

            //    // Set Flash Program Type to Class B
            //    FL_FlashProgramType = FLASH_CLASS_B;
            //}
            //// Check if condition 2: Flash A or C
            //else
            //{
            //    // Reset flash memory
            //    FlashReset();

            //    // Write values to exit ASO and enter autoselect mode
            //    device.flashWrite(0xAAA, 0xAA);
            //    device.flashWrite(0x555, 0x55);
            //    device.flashWrite(0xAAA, 0x90);

            //    // Check for Flash A
            //    if (manuid1 != device.flashRead(0x00))
            //    {
            //        // Read additional values for Flash A
            //        manuid1 = device.flashRead(0x000);
            //        devid1 = device.flashRead(0x002);
            //        manuid3 = device.flashRead(0x200);

            //        // Check for EON Configuration Code
            //        if (manuid1 == FLASH_EON_CONFIGURATION_CODE)
            //        {
            //            if (manuid3 == FLASH_EON) manuid1 = manuid3;
            //        }

            //        // Set Flash Program Type to Class A
            //        FL_FlashProgramType = FLASH_CLASS_A;
            //    }
            //    // Check if condition 3: Flash C
            //    else
            //    {
            //        // Reset flash memory 
            //        FlashReset();

            //        // Write values to exit ASO and enter autoselect mode
            //        device.flashWrite(0xAAAA, 0xAA);
            //        device.flashWrite(0x5555, 0x55);
            //        device.flashWrite(0xAAAA, 0x90);

            //        // Read additional values for Flash C
            //        manuid2 = device.flashRead(0x000);
            //        devid2 = device.flashRead(0x002);
            //        manuid3 = device.flashRead(0x200);

            //        // Check for EON Configuration Code
            //        if (manuid2 == FLASH_EON_CONFIGURATION_CODE)
            //        {
            //            if (manuid3 == FLASH_EON) manuid2 = manuid3;
            //        }

            //        // Set Flash Program Type to Class C
            //        FL_FlashProgramType = FLASH_CLASS_C;
            //    }
            //}

            //// Output the Flash Program Type and ID values to console
            //Console.WriteLine("Hiiiii  " + FL_FlashProgramType.ToString() + " manuid1 " + manuid1 + " devid1 " + devid1 + " manuid3 " + manuid3);

            //// Reset flash memory
            //FlashReset();

            //// Set global manufacturer and device ID variables
            //SetFlashGlobals(manuid1, manuid2, devid1, devid2);
            //Dev_Id = FL_gdevid;
            //Man_Id = FL_gmanuid;

            //// Output the Flash Program Type and ID values to console
            //Console.WriteLine("Hiiiii  " + FL_FlashProgramType.ToString() + " manid " + Man_Id + " Dev_id " + Dev_Id);
        }

        public void FlashSectorErase(UInt32 Address)
        {
            //UInt32 EraseTimeout = 0;

            //// CONDITION: Check if flash program type is class B
            //if (FL_FlashProgramType == FLASH_CLASS_B)
            //{
            //    // CONDITION: Check if flash manufacturer ID is Intel and device ID matches certain values
            //    if ((FL_gmanuid == FLASH_INTEL) && ((FL_gdevid == 0x88C5) || (FL_gdevid == 0x88C3) || (FL_gdevid == 0x88C1)))
            //    {
            //        // Send specific commands to the flash device for erasing the sector
            //        device.flashWrite(Address, 0x60);
            //        device.flashWrite(Address, 0xD0);
            //        // Poll the flash device to check if the erase operation is complete
            //        PollFlashComplete(Address);
            //    }

            //    // CONDITION: Check if flash manufacturer ID is Numonyx and device ID matches certain values
            //    if ((FL_gmanuid == FLASH_NUMONYX) && ((FL_gdevid == 0x88CF) || (FL_gdevid == 0x88BB) || (FL_gdevid == 0x8849)))
            //    {
            //        // Send specific commands to the flash device for erasing the sector
            //        device.flashWrite(Address, 0x60);
            //        device.flashWrite(Address, 0xD0);
            //        // Poll the flash device to check if the erase operation is complete
            //        PollFlashComplete(Address);
            //    }

            //    // Send commands to the flash device for erasing the sector
            //    device.flashWrite(Address, 0x20);
            //    device.flashWrite(Address, 0xd0);
            //}
            //// CONDITION: Flash program type is not class B
            //else
            //{
            //    // Send commands to the flash device for erasing the sector
            //    device.flashWrite(FL_pa, 0xAA);
            //    device.flashWrite(FL_p5, 0x55);
            //    device.flashWrite(FL_pa, 0x80);
            //    device.flashWrite(FL_pa, 0xAA);
            //    device.flashWrite(FL_p5, 0x55);
            //    device.flashWrite(Address, 0x30);
            //}

            //// Wait for the flash device to complete the erase operation
            //while ((device.flashRead(Address) != 0xFFFF) && (EraseTimeout++ < 0x500000)) ;
            //// Poll the flash device to check if the erase operation is complete
            //PollFlashComplete(Address);

            device.flashWrite(0x555, 0xAA);
            device.flashWrite(0x2AA, 0x55);
            device.flashWrite(0x555, 0x80);
            device.flashWrite(0x555, 0xAA);
            device.flashWrite(0x2AA, 0x55);
            device.flashWrite(Address, 0x30);

            while ((device.flashRead(Address) != 0xFFFF)) ;
            //// Poll the flash device to check if the erase operation is complete
            PollFlashComplete(Address);
        }

        /* ----------------------------------------------------------
         * Function: FlashWriteByteArr
         * 
         * ----------------------------------------------------------*/
        public bool FlashWriteByteArr(UInt32 Address, UInt32 SAddress, byte[] bData, int start, int Len)
        {
            // CONDITION: Check if flash program type is class B
            //if (FL_FlashProgramType == FLASH_CLASS_B)
            //{
            //    // Read two bytes from the byte array and combine them to form a 16-bit data
            //    data = (uint)(bData[start + i + 1] << 8) | bData[start + i];
            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(Address + i, 0x40);
            //    device.flashWrite(Address + i, data);

            //    // CONDITION: Loop through the data array and write to flash if the data is not 0xFFFF
            //    for (i = 2; i < Len - 2; i += 2)
            //    {
            //        // Read two bytes from the byte array and combine them to form a 16-bit data
            //        data = (uint)(bData[start + i + 1] << 8) | bData[start + i];
            //        // CONDITION: Check if the data is not 0xFFFF
            //        if (data != 0xFFFF)
            //        {
            //            // Send commands to the flash device for writing the data to the specified address
            //            device.flashWrite(Address + i, 0x40);
            //            device.flashWrite(Address + i, data);
            //        }
            //    }

            //    // Read the last two bytes from the byte array and combine them to form a 16-bit data
            //    data = (uint)(bData[start + i + 1] << 8) | bData[start + i];
            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(Address, 0x40);
            //    device.flashWrite(Address, data);
            //}
            //// CONDITION: Flash program type is not class B
            //else
            //{
            //    // Read two bytes from the byte array and combine them to form a 16-bit data
            //    data = (uint)(bData[start + i + 1] << 8) | bData[start + i];
            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(FL_pa, 0xAA);
            //    device.flashWrite(FL_p5, 0x55);
            //    device.flashWrite(FL_pa, 0xA0);
            //    device.flashWrite(Address, data);

            //    // CONDITION: Loop through the data array and write to flash if the data is not 0xFFFF
            //    for (i = 2; i < Len - 2; i += 2)
            //    {
            //        // Read two bytes from the byte array and combine them to form a 16-bit data
            //        data = (uint)(bData[i + 1] << 8) | bData[i];

            //        // CONDITION: Check if the data is not 0xFFFF
            //        if (data != 0xFFFF)
            //        {
            //            // Send commands to the flash device for writing the data to the specified address
            //            device.flashWrite(FL_pa, 0xAA);
            //            device.flashWrite(FL_p5, 0x55);
            //            device.flashWrite(FL_pa, 0xA0);
            //            device.flashWrite(Address + i, data);
            //        }
            //    }

            //    // Read the last two bytes from the byte array and combine them to form a 16-bit data
            //    data = (uint)(bData[i + 1] << 8) | bData[i];
            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(FL_pa, 0xAA);
            //    device.flashWrite(FL_p5, 0x55);
            //    device.flashWrite(FL_pa, 0xA0);
            //    device.flashWrite(Address + i, data);
            //}
            UInt32 i = 0, data=0;
            device.flashWrite(0x555,0xAA);
            device.flashWrite(0x2AA,0x55);
            device.flashWrite(SAddress,0x25);
            device.flashWrite(SAddress, (uint)(Len/2 - 1));
            for (i = 0; i < Len ; i += 2)
            {
                // Read two bytes from the byte array and combine them to form a 16-bit data
                data = (uint)(bData[start + i + 1] << 8) | bData[start+ i];
                device.flashWrite(Address + i/2, data);
            }
            device.flashWrite(SAddress, 0x29);
            uint lastAddr = (uint)( Address + Len/2 - 1);
            uint status = device.flashRead(lastAddr);
            while ((status& 0x80) != (data & 0x80))
            {
                if ((status & 0x20) == 0x20 || (status & 0x02) == 0x02)
                {
                    if ((status & 0x80) != (data & 0x80))
                        return true;
                    else return false;
                }
                status = device.flashRead(lastAddr);
            }
            return true;
        }

        /* ----------------------------------------------------------
         * Function: FlashWriteWord
         * 
         * Description: Writes a word of data to the specified address in the flash memory.
         * ----------------------------------------------------------*/
        public void FlashWriteWord(UInt32 Address, UInt32 Data)
        {
            //UInt32 tempData;
            //UInt32 WriteTimeout = 0;

            // CONDITION: Check if flash program type is class B
            //if (FL_FlashProgramType == FLASH_CLASS_B)
            //{
            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(Address, 0x40);
            //    device.flashWrite(Address, Data);
            //}
            //// CONDITION: Flash program type is not class B
            //else
            //{

            //    // Send commands to the flash device for writing the data to the specified address
            //    device.flashWrite(FL_pa, 0xAA);
            //    device.flashWrite(FL_p5, 0x55);
            //    device.flashWrite(FL_pa, 0xA0);
            //    device.flashWrite(Address, Data);
            //}
            device.flashWrite(0x555, 0xAA);
            device.flashWrite(0x2AA, 0x55);
            device.flashWrite(0x555, 0xA0);
            device.flashWrite(Address, Data);
            while ((device.flashRead(Address) != Data)) ;
        }

        /* ----------------------------------------------------------
         * Function: FlashReadWord
         * 
         * Description: Reads a word of data from the specified address in the flash memory.
         * ----------------------------------------------------------*/
        public UInt32 FlashReadWord(UInt32 Address)
        {
            // Read and return the data from the specified address in the flash device
            return device.flashRead(Address);
        }

        /* ----------------------------------------------------------
         * Function: ProgramType
         * 
         * Description: Sets the flash program type.
         * ----------------------------------------------------------*/
        public void ProgramType(byte Type)
        {
            // Set the flash program type
            FL_FlashProgramType = Type;

            // CONDITION: Check if flash program type is class A
            if (FL_FlashProgramType == FLASH_CLASS_A)
            {
                FL_pa = 0xaaa;
                FL_p5 = 0x555;
            }
            // CONDITION: Flash program type is not class A
            else
            {
                FL_pa = 0xaaaa;
                FL_p5 = 0x5555;
            }

            // Update the flash algorithm information
            m_FlashInfo.FlashALG = FL_FlashProgramType;
        }

        /* ----------------------------------------------------------
         * Function: GetFlashInfoCFI
         * 
         * Description: Retrieves flash device information using CFI (Common Flash Interface).

         The `GetFlashInfoCFI` function retrieves detailed information about the flash memory by performing a CFI (Common Flash Interface) query. 
         It returns a boolean value indicating whether the query was successful.

        The function begins by declaring several variables to store the flash information, including `D1`, `D2`, `D3`, `AddressOffset`, `PEQAddress`, `NumSectors`, and `i`.

        Next, it calls the `GetFlashMfgAndDevID` function to obtain the flash manufacturer ID and device ID and stores them in the corresponding fields of the `m_FlashInfo` structure.

        The `NumEraseBlocks` field of `m_FlashInfo` is initialized to 0, and the `FlashALG` field is set to the value of `FL_FlashProgramType`.

        The function then sends a CFI query by writing specific values to memory addresses 0xAA and reads the response from addresses 0x20, 0x22, and 0x24, storing them in `D1`, `D2`, and `D3`, respectively.

        If the response matches 'Q', 'R', and 'Y', indicating a valid CFI response, the function proceeds to retrieve additional flash information. 
        It reads the device size, number of erase blocks, and PEQ address from specific memory locations and assigns them to the corresponding fields of `m_FlashInfo`. 
        It also initializes `AddressOffset` to 0x5A and `NumSectors` to 0.

        A check is performed to ensure that the number of erase blocks does not exceed the maximum limit defined by `MAX_ERASE_BLOCKS`. 
        If it exceeds the limit, the `NumEraseBlocks` field is set to 0, the flash is reset using the `FlashReset` function, and the function returns false.

        Otherwise, the function enters a loop to retrieve information about each erase block. 
        It reads eight consecutive memory locations starting from `AddressOffset`, combines the values to form a 32-bit erase block information, and stores it in the corresponding element of the `EraseBlock` array in `m_FlashInfo`. 
        It then increments `AddressOffset` by 8 and updates `NumSectors` by adding the number of sectors in the current erase block.

        If the initial CFI query does not yield a valid response, the function sends another CFI query by writing to address 0xAAA and performs the same process as described above.

        If no valid response is obtained from either CFI query, the function resets the flash using `FlashReset` and returns false.

        Finally, the function calls `FlashReset` again to ensure the flash is in a reset state and returns true to indicate a successful retrieval of flash information.

        In summary, the `GetFlashInfoCFI` function retrieves detailed flash information using CFI queries, populates the `m_FlashInfo` structure with the obtained data, and returns a boolean value indicating the success of the operation.
         * ----------------------------------------------------------*/
        public bool GetFlashInfoCFI()
        {
            //device.flashWrite(0x0055, 0x98);
            device.flashWrite(0x0555, 0xAA);
            device.flashWrite(0x02AA, 0x55);
            device.flashWrite(0x0555, 0x90);
            uint d1, d2, d3;
            d1 = device.flashRead(0x10);
            d2 = device.flashRead(0x11);
            d3 = device.flashRead(0x12);
           
            if (d1 == 'Q' && d2 == 'R' && d3 == 'Y')
            {
                Console.WriteLine("CFI Entries found in Flash");
                uint Blocks = device.flashRead(0x2C);
                Console.WriteLine("Blocks :" + Blocks);
                uint Addr = 0;
                for (uint i = 0; i < Blocks; i++)
                {
                    uint sectorsNum = (device.flashRead(0x2E + i) << 8 | device.flashRead(0x2D + i)) + 1;
                    uint sectorSize = device.flashRead(0x30 + i) << 16 | device.flashRead(0x2F + i) << 8;
                    Console.WriteLine(String.Format("Blocks[{0:d}] : Sectors: {1:d} , SectorSize: {2:d}", i, sectorsNum, sectorSize));
                    for(int j=0; j < sectorsNum; j++)
                    {
                        sectors.Add(new Sector(Addr, sectorSize/2));
                        Addr += sectorSize/2;
                    }
                }
                FlashReset();
                return true;
            }
            Console.WriteLine("Filed to find CFI in Flash");
            FlashReset();
            return false;



            //UInt32 D1, D2, D3, AddressOffset, PEQAddress;
            //UInt32 NumSectors;
            //UInt32 i;

            //// Get the flash manufacturer ID and device ID
            //GetFlashMfgAndDevID();

            //m_FlashInfo.NumEraseBlocks = 0;
            //m_FlashInfo.FlashALG = FL_FlashProgramType;

            //// Send the CFI query on 0x0AA (most of the flash parts respond to it)
            //device.flashWrite(0xAA, 0x98);
            //D1 = device.flashRead(0x20);
            //D2 = device.flashRead(0x22);
            //D3 = device.flashRead(0x24);

            //// CONDITION: Check if the response matches 'Q', 'R', and 'Y'
            //if (('Q' == D1) && ('R' == D2) && ('Y' == D3))
            //{
            //    // Get the flash device size, number of erase blocks, and PEQ address
            //    m_FlashInfo.DeviceSize = device.flashRead(0x4E);
            //    m_FlashInfo.NumEraseBlocks = device.flashRead(0x58);//2C
            //    PEQAddress = device.flashRead(0x2A << 1);

            //    AddressOffset = 0x5A;
            //    NumSectors = 0;

            //    // CONDITION: Check if the number of erase blocks exceeds the maximum limit
            //    if (m_FlashInfo.NumEraseBlocks > MAX_ERASE_BLOCKS)
            //    {
            //        m_FlashInfo.NumEraseBlocks = 0;
            //        FlashReset();
            //        return false;
            //    }

            //    // Loop through the erase blocks and retrieve their information
            //    for (i = 0; i < m_FlashInfo.NumEraseBlocks; i++)
            //    {
            //        m_FlashInfo.EraseBlock[i] =
            //                         (UInt32)device.flashRead(AddressOffset) | ((UInt32)device.flashRead(AddressOffset + 2) << 8) |
            //                        ((UInt32)device.flashRead(AddressOffset + 4) << 16) | ((UInt32)device.flashRead(AddressOffset + 6) << 24);

            //        AddressOffset += 8;
            //        NumSectors += ((m_FlashInfo.EraseBlock[i] & 0xFFFF) + 1);
            //    }
            //}
            //// CONDITION: If no response matches, send the CFI query on 0xAAA (some devices respond to it)
            //else
            //{
            //    device.flashWrite(0xAAA, 0x98);
            //    D1 = device.flashRead(0x20);
            //    D2 = device.flashRead(0x22);
            //    D3 = device.flashRead(0x24);

            //    // CONDITION: Check if the response matches 'Q', 'R', and 'Y'
            //    if (('Q' == D1) && ('R' == D2) && ('Y' == D3))
            //    {
            //        // Get the flash device size, number of erase blocks, and PEQ address
            //        m_FlashInfo.DeviceSize = device.flashRead(0x4E);
            //        m_FlashInfo.NumEraseBlocks = device.flashRead(0x58);
            //        PEQAddress = device.flashRead(0x2A << 1);

            //        AddressOffset = 0x5A;
            //        NumSectors = 0;

            //        // CONDITION: Check if the number of erase blocks exceeds the maximum limit
            //        if (m_FlashInfo.NumEraseBlocks > MAX_ERASE_BLOCKS)
            //        {
            //            m_FlashInfo.NumEraseBlocks = 0;
            //            FlashReset();
            //            return false;
            //        }

            //        // Loop through the erase blocks and retrieve their information
            //        for (i = 0; i < m_FlashInfo.NumEraseBlocks; i++)
            //        {
            //            m_FlashInfo.EraseBlock[i] =
            //                             (UInt32)device.flashRead(AddressOffset) | ((UInt32)device.flashRead(AddressOffset + 2) << 8) |
            //                            ((UInt32)device.flashRead(AddressOffset + 4) << 16) | ((UInt32)device.flashRead(AddressOffset + 6) << 24);

            //            AddressOffset += 8;
            //            NumSectors += ((m_FlashInfo.EraseBlock[i] & 0xFFFF) + 1);
            //        }
            //    }
            //    // CONDITION: If no response matches, reset the flash and return false
            //    else
            //    {
            //        FlashReset();
            //        return false;
            //    }
            //}

            //FlashReset();
            //return true;

        }


        /* ----------------------------------------------------------
    * Function: PollFlashComplete
    * 
    * Description: Polls the flash memory until a write operation is completed at the specified address.

    The `PollFlashComplete` function is responsible for polling the flash memory to check if a write operation is completed. 
    The behavior of the function depends on the flash program type, specifically whether it is classified as class B or not.

    When the flash program type is class B, the function enters a loop and continuously polls the flash memory until the write operation at the specified address is completed. 
    It does so by repeatedly reading the value at the address and checking the most significant bit (bit 7) until it becomes set to 1, indicating completion. 
    Once the write operation is completed, the function proceeds to write a value of 0xFF to the same address to finalize the operation.

    On the other hand, when the flash program type is not class B, the function takes a different approach. 
    It first reads the value at the specified address and stores it in `test_value`. 
    Then, it performs another read and stores the result in `test_value1`. 
    The function then enters a loop where it compares the two values, `test_value` and `test_value1`, to check if they are different. 
    If they are different, it means that the write operation is still ongoing, and the loop continues. 
    If they are the same, it indicates that the write operation has completed, and the loop exits.

    In summary, the `PollFlashComplete` function allows for monitoring the completion of write operations in the flash memory. 
    It handles the different behavior based on the flash program type and ensures that the write operation is successfully completed before proceeding. 
    The function takes the address to poll in the flash memory as a parameter.
    * ----------------------------------------------------------*/
        public void PollFlashComplete(UInt32 Address)
        {
            //UInt32 test_value, test_value1;

            //// CONDITION: Check if flash program type is class B
            //if (FL_FlashProgramType == FLASH_CLASS_B)
            //{
            //    // Poll the flash memory until the write operation is completed
            //    while ((device.flashRead(Address) & 0x80) == 0) ;

            //    // Write a value to the specified address to complete the operation
            //    device.flashWrite(Address, 0xFF);
            //}
            //// CONDITION: Flash program type is not class B
            //else
            //{
            //    test_value = (device.flashRead(Address) & 0x40);
            //    test_value1 = (device.flashRead(Address) & 0x40);

            //    // CONDITION: Poll the flash memory until the write operation is completed
            //    while (test_value != test_value1)
            //    {
            //        test_value = test_value1;
            //        test_value1 = (device.flashRead(Address) & 0x40);
            //    }
            //}
            while((device.flashRead(Address)&0x40) == 0);
        }

        /* ----------------------------------------------------------
         * Function: SetFlashGlobals
         * 
         * Description: Saves the manufacturer ID and device ID in global variables.
         *              The values are obtained from the GetFlashMfgAndDevID function.

         The SetFlashGlobals function is responsible for saving the manufacturer ID and device ID in global variables. 
         These values are obtained from the GetFlashMfgAndDevID function. 
         The function takes four parameters: manuid1, manuid2, devid1, and devid2, representing the first and second manufacturer IDs and the first and second device IDs, respectively.

        Within the function, the FL_FlashProgramType variable is initialized to 0. 
        The function then checks if manuid1 exists in the FL_FlashType dictionary. 
        If it does, FL_FlashProgramType is set to the corresponding value from the dictionary, and FL_gmanuid and FL_gdevid are assigned the values of manuid1 and devid1, respectively.

        Similarly, the function checks if manuid2 exists in the FL_FlashType dictionary. 
        If it does, FL_FlashProgramType is set to the corresponding value. Additionally, if manuid2 is equal to FLASH_TOSHIBA, FL_FlashProgramType is set to FLASH_CLASS_C. 
        The variables FL_gmanuid and FL_gdevid are assigned the values of manuid2 and devid2, respectively.

        There is a special code section that handles Numonyx parts with an STMicro manufacturer ID but should be treated as Intel device types. 
        If manuid1 is equal to FLASH_STMICRO, the function checks the device IDs for specific Numonyx parts (0x88CF, 0x88BB, and 0x8849). 
        If any of these device IDs match devid1, FL_gmanuid is set to FLASH_NUMONYX and FL_FlashProgramType is set to FLASH_CLASS_B.

        If the global manufacturer ID (FL_gmanuid) is still not set, the function sets FL_FlashProgramType to FLASH_CLASS_A, and FL_gmanuid and FL_gdevid are assigned the values of manuid1 and devid1, respectively.

        Finally, the function checks if FL_FlashProgramType is still not set. 
        If it is 0, indicating that no manufacturer ID matches, FL_FlashProgramType is set to FLASH_CLASS_A.

        Based on the value of FL_FlashProgramType, the function sets the values of FL_pa and FL_p5 accordingly. 
        If FL_FlashProgramType is FLASH_CLASS_A, FL_pa is set to 0xaaa and FL_p5 is set to 0x555. Otherwise, FL_pa is set to 0xaaaa and FL_p5 is set to 0x5555.
         * ----------------------------------------------------------*/
        public void SetFlashGlobals(UInt32 manuid1, UInt32 manuid2, UInt32 devid1, UInt32 devid2)
        {
            FL_FlashProgramType = 0;

            // CONDITION: Check if the manufacturer ID exists in the dictionary
            if (FL_FlashType.ContainsKey(manuid1))
            {
                FL_FlashProgramType = FL_FlashType[manuid1];
                FL_gmanuid = manuid1;
                FL_gdevid = devid1;
            }

            // CONDITION: Check if the manufacturer ID exists in the dictionary
            if (FL_FlashType.ContainsKey(manuid2))
            {
                FL_FlashProgramType = FL_FlashType[manuid2];

                // CONDITION: Check if the manufacturer ID is FLASH_TOSHIBA
                if (manuid2 == FLASH_TOSHIBA)
                {
                    FL_FlashProgramType = FLASH_CLASS_C;
                }

                FL_gmanuid = manuid2;
                FL_gdevid = devid2;
            }

            // Special code to handle Numonyx parts with an STMicro mfg ID but should be handled as Intel device types
            if (manuid1 == FLASH_STMICRO)
            {
                // Check the device IDs for specific Numonyx parts
                if ((devid1 == 0x88CF) || (devid1 == 0x88BB) || (devid1 == 0x8849))
                {
                    FL_gmanuid = FLASH_NUMONYX;
                    FL_FlashProgramType = FLASH_CLASS_B;
                }
            }

            // CONDITION: Check if the global manufacturer ID is still not set
            if (FL_gmanuid == 0)
            {
                FL_FlashProgramType = FLASH_CLASS_A;
                FL_gmanuid = manuid1;
                FL_gdevid = devid1;
            }

            FL_gmanuid = manuid1;
            FL_gdevid = devid1;

            // CONDITION: Check if the flash program type is still not set
            if (FL_FlashProgramType == 0)
            {
                FL_FlashProgramType = FLASH_CLASS_A;
            }

            // CONDITION: Check the flash program type and set the corresponding values
            if (FL_FlashProgramType == FLASH_CLASS_A)
            {
                FL_pa = 0xaaa;
                FL_p5 = 0x555;
            }
            else
            {
                FL_pa = 0xaaaa;
                FL_p5 = 0x5555;
            }
        }

    }
}

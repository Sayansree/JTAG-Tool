using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using Microsoft.Win32;
using System.Drawing;

namespace WindowsFormsApp1
{
    public class ComboboxDeviceItem
    {
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string DeviceID { get; set; }

        public override string ToString()
        {
            return Type + " (" + Description  + ")";
        }
    }
    //internal class util
    //{
    //    public static List<ComboboxDeviceItem> port_scan()

    //    {
    //        List<ComboboxDeviceItem> devices = new List<ComboboxDeviceItem>();
    //        using (ManagementClass i_Entity = new ManagementClass("Win32_PnPEntity"))
    //        {
    //            foreach (ManagementObject i_Inst in i_Entity.GetInstances())
    //            {
    //                Object o_Guid = i_Inst.GetPropertyValue("ClassGuid");
    //                if (o_Guid == null || o_Guid.ToString().ToUpper() != "{4D36E978-E325-11CE-BFC1-08002BE10318}")
    //                    continue; // Skip all devices except device class "PORTS"

    //                String s_Caption = i_Inst.GetPropertyValue("Caption").ToString();
    //                String s_Manufact = i_Inst.GetPropertyValue("Manufacturer").ToString();
    //                String s_DeviceID = i_Inst.GetPropertyValue("PnpDeviceID").ToString();
    //                String s_RegPath = "HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Enum\\" + s_DeviceID + "\\Device Parameters";
    //                String s_PortName = Registry.GetValue(s_RegPath, "PortName", "").ToString();

    //                int s32_Pos = s_Caption.IndexOf(" (COM");
    //                if (s32_Pos > 0) // remove COM port from description
    //                    s_Caption = s_Caption.Substring(0, s32_Pos);

    //                Console.WriteLine("Port Name:    " + s_PortName);
    //                Console.WriteLine("Description:  " + s_Caption);
    //                Console.WriteLine("Manufacturer: " + s_Manufact);
    //                Console.WriteLine("Device ID:    " + s_DeviceID);
    //                Console.WriteLine("-----------------------------------");
    //                ComboboxDeviceItem item = new ComboboxDeviceItem();
    //                //item.Port = s_PortName;
    //                //item.Description = s_Caption;
    //                //item.Manufacturer = s_Manufact;
    //                //item.DeviceID = s_DeviceID;
    //                devices.Add(item);
    //            }
              
    //        }
    //        return devices;


    //    }
    //}
}

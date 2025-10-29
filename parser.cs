using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public class parser
    {
        public static BSDLObject readBDSL(string fileName)
        {
            try
            {
                BSDLObject bobj;
                using (StreamReader r = new StreamReader(fileName))
                {
                    string json = r.ReadToEnd();
                    bobj = JsonConvert.DeserializeObject<BSDLObject>(json);
                    Console.WriteLine("name " + bobj.component_name);
                    Console.WriteLine("irlen " + bobj.instruction_register_description.instruction_length);
                    Console.WriteLine("bslen " + bobj.boundary_scan_register_description.fixed_boundary_stmts.boundary_length);
                    Console.WriteLine("IDCODE " + bobj.optional_register_description.idcode_register.ToString());
                }
                return bobj;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }
            return null;
        }
        public static BSDLObject transform()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\Downloads",
                Title = "Browse BSDL Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "bin",
                Filter = "bsdl files (*.bsdl)|*.bsdl",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = false
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                string tempVar = Path.GetTempPath() + "\\ParsedBSDL.json";
                bsdlParser(fileName, tempVar);
                Console.WriteLine(tempVar);
                try
                {
                    BSDLObject bobj;
                    using (StreamReader r = new StreamReader(tempVar))
                    {
                        string json = r.ReadToEnd();
                        bobj = JsonConvert.DeserializeObject<BSDLObject>(json);
                        string filepath = "Controllers\\" + bobj.component_name + ".json";             ///
                        if(!Directory.Exists(System.IO.Path.GetDirectoryName(filepath)))
                             Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filepath));
                        File.WriteAllText(filepath, json);
                        Console.WriteLine("name " + bobj.component_name);                 ///
                        Console.WriteLine("irlen " + bobj.instruction_register_description.instruction_length);
                        Console.WriteLine("bslen " + bobj.boundary_scan_register_description.fixed_boundary_stmts.boundary_length);
                        Console.WriteLine("IDCODE " + bobj.optional_register_description.idcode_register.ToString());
                    }
                    File.Delete(tempVar);
                    return bobj;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception caught in process: {0}", ex);
                }
            }
            return null;
        }
        public static void bsdlParser(string src, string dest)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/k bsdl2json.exe \"" + src + "\" > \"" + dest + "\"  && exit";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception caught in process: {0}", ex);
            }
        }
    }
}

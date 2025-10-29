using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;
using DLPComposer.ControlProgram.FlashProgrammer;

 namespace DLPComposer.ControlProgram.FlashProgrammer
{
    public class FlashMemoryData
    {
        private readonly string _filePath;

        public FlashMemoryData(string filePath)
        {
            _filePath = filePath;
        }

        public List<FlashMemory> GetAll()
        {
            List<FlashMemory> memory;
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                memory = JsonConvert.DeserializeObject<List<FlashMemory>>(json);
            }
            return memory;
        }
    }

}

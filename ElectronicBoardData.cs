using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//using System.Text.Json.Serialization;
using System.Threading.Tasks;

 namespace WindowsFormsApp1
{
    public class ElectronicBoardData
    {
        private readonly string _filePath;

        public ElectronicBoardData(string filePath)
        {
            _filePath = filePath;
        }

        public List<ElectronicBoard> GetAll()
        {
            List<ElectronicBoard> boards;
            using (StreamReader r = new StreamReader(_filePath))
            {
                string json = r.ReadToEnd();
                boards = JsonConvert.DeserializeObject<List<ElectronicBoard>>(json);
            }
            return boards;
        }

        //public void Add(ElectronicBoard board)
        //{
        //    List<ElectronicBoard> boards = GetAll();
        //    boards.Add(board);
        //    SaveAll(boards);
        //}
        //public void Update(ElectronicBoard board)
        //{
        //    List<ElectronicBoard> boards = GetAll();
        //    ElectronicBoard existingBoard = boards.FirstOrDefault(b => b.DEVID == board.DEVID);
        //    if (existingBoard != null)
        //    {
        //        existingBoard.DEVID = board.DEVID;
        //        existingBoard.DEVICENAME = board.DEVICENAME;
        //        existingBoard.DATA = board.DATA;
        //        existingBoard.DATACTRL = board.DATACTRL;
        //        existingBoard.ADDR = board.ADDR;
        //        existingBoard.ADDRCTRL = board.ADDRCTRL;
        //        existingBoard.RESET = board.RESET;
        //        existingBoard.CSZ = board.CSZ;
        //        existingBoard.CSZCTRL = board.CSZCTRL;
        //        existingBoard.RESETCTRL = board.RESETCTRL;
        //        existingBoard.WEZ = board.WEZ;
        //        existingBoard.WEZCTRL = board.WEZCTRL;
        //        existingBoard.OEZ = board.OEZ;
        //        existingBoard.OEZCTRL = board.OEZCTRL;
        //        existingBoard.BSLEN = board.BSLEN;
        //        existingBoard.IRLEN = board.IRLEN;
        //        existingBoard.SAFE = board.SAFE;
        //        existingBoard.EXTEST = board.EXTEST;
        //        existingBoard.OPTPINSNAME = board.OPTPINSNAME;
        //        existingBoard.OPTPINS = board.OPTPINS;
        //        existingBoard.OPTPINSCTRL = board.OPTPINSCTRL;
        //        existingBoard.OPTPINSVAL = board.OPTPINSVAL;
                
        //        SaveAll(boards);
        //    }
        //}

        //public void Delete(string id)
        //{
        //    List<ElectronicBoard> boards = GetAll();
        //    ElectronicBoard existingBoard = boards.FirstOrDefault(b => b.DEVID == id);
        //    if (existingBoard != null)
        //    {
        //        boards.Remove(existingBoard);
        //        SaveAll(boards);
        //    }
        //}

        //private void SaveAll(List<ElectronicBoard> boards)
        //{
        //    string json = JsonConvert.SerializeObject(boards);
        //    using (StreamWriter w = new StreamWriter(_filePath))
        //    {
        //        w.Write(json);
        //    }
        //}
    }

}

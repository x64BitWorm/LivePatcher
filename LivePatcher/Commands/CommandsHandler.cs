using System;
using System.Linq;

namespace LivePatcher.Commands
{
    public class CommandsHandler
    {
        private PatchManager _patcher;
        private VariablesMemory _memory;

        public CommandsHandler(PatchManager patcher, VariablesMemory memory)
        {
            _patcher = patcher;
            _memory = memory;
        }

        [Command("#", "[*] comment (does nothing)")]
        public void CommentCommand(string[] comments)
        {
            // skip comments
        }

        [Command("load", "[executable path] loads executable in memory")]
        public void LoadCommand(string executable)
        {
            _patcher.LoadFile(executable);
        }

        [Command("start", "[] starts loaded executable")]
        public void StartCommand()
        {
            _patcher.RunProcess();
        }

        [Command("delay", "[milliseconds] delays script for N milliseconds")]
        public void DelayCommand(string milliseconds)
        {
            System.Threading.Thread.Sleep(int.Parse(milliseconds));
        }

        [Command("write", "[addr|var,type,val|var] writes data (3) interpreted as type (2) into memory (1)")]
        public void WriteCommand(string writeTo, string type, string data)
        {
            long addr = _memory.Get(writeTo);
            long size = Utils.TypeSizeFromName(type);
            if (size == -1)
            {
                size = data.Length / 2;
            }
            byte[] bytes = new byte[size];
            if (data.StartsWith('_'))
            {
                byte[] bts = Utils.BytesFromHex(Utils.ToHex(_memory.Get(data))).Reverse().ToArray();
                Array.Copy(bts, bytes, Math.Min(bytes.Length, bts.Length));
            }
            else
            {
                var bts = Utils.BytesFromHex(data);
                Array.Copy(bts, bytes, Math.Min(bytes.Length, bts.Length));
            }
            _patcher.WriteMemory(addr, bytes);
        }

        [Command("read", "[addr|var,type,var] reads data from (1) interpreted as type (2) into variable (3)")]
        public void ReadCommand(string readFrom, string type, string readTo)
        {
            var addr = _memory.Get(readFrom);
            var size = Utils.TypeSizeFromName(type);
            var bytes = _patcher.ReadMemory(addr, size).Reverse().ToArray();
            _memory.Set(readTo, Utils.FromHex(bytes));
        }

        [Command("allocate", "[size,var] allocate (1) size block in process memory and returns address into (2)")]
        public void AllocateCommand(string size, string variable)
        {
            _memory.Set(variable, _patcher.Allocate(int.Parse(size)));
        }

        [Command("thread", "[label] create and start thread starting from (1) address")]
        public void ThreadCommand(string variable)
        {
            _patcher.StartThread(_memory.Get(variable));
        }

        [Command("dll", "[dllname,var] gets dll address into memory and writes it into (2)")]
        public void DllCommand(string dllname, string variable)
        {
            _memory.Set(variable, _patcher.DllOffset(dllname));
        }

        [Command("set", "[var,op,arg1,arg2] performs operation (add,sub,mul,div,value,jump) over arg1 and arg2 (result goes into (1))")]
        public void SetCommand2(string variable, string operation, string arg1, string arg2)
        {
            long res = 0;
            switch (operation)
            {
                case "add": res = _memory.Get(arg1) + _memory.Get(arg2); break;
                case "sub": res = _memory.Get(arg1) - _memory.Get(arg2); break;
                case "mul": res = _memory.Get(arg1) * _memory.Get(arg2); break;
                case "div": res = _memory.Get(arg1) / _memory.Get(arg2); break;
                case "jump": res = (uint)((int)_memory.Get(arg2) - (int)_memory.Get(arg1) - 5); break;
            }
            _memory.Set(variable, res);
        }

        [Command("set", "[var,op,arg1] performs operation (value) over arg1 (result goes into (1))")]
        public void SetCommand1(string variable, string operation, string arg1)
        {
            long res = 0;
            switch (operation)
            {
                case "value": res = _memory.Get(arg1); break;
            }
            _memory.Set(variable, res);
        }

        [Command("print", "[text,variable] prints variable (2) value after text (1)")]
        public void PrintCommand(string text, string variable)
        {
            Console.WriteLine($"{text} {_memory.Get(variable)}");
        }

        [Command("input", "[variable] inputs into variable (1)")]
        public void InputCommand(string variable)
        {
            _memory.Set(variable, long.Parse(Console.ReadLine()));
        }
    }
}

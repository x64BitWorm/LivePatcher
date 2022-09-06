using System;
using System.Collections.Generic;

namespace LivePatcher
{
    public class VariablesMemory
    {
        private Dictionary<string, long> _variables = new();

        public void Set(string variable, long value)
        {
            _variables[variable] = value;
        }

        public long Get(string input)
        {
            if (input.StartsWith("_"))
            {
                return _variables[input];
            }
            if (input.StartsWith("0x"))
            {
                return Convert.ToInt64(input.Substring(2), 16);
            }
            else
            {
                return long.Parse(input);
            }
        }
    }
}

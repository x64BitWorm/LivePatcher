namespace LivePatcher
{
    public static class Utils
    {
        public static long FromHex(string address)
        {
            const string hex = "0123456789abcdef";
            long result = 0;
            foreach(var c in address)
            {
                result <<= 4;
                result |= (uint)hex.IndexOf(char.ToLower(c));
            }
            return result;
        }

        public static long FromHex(byte[] data)
        {
            long result = 0;
            for (byte i = 0; i < 8; i++)
            {
                result |= (long)data[i] << (i << 3);
            }
            return result;
        }

        public static string ToHex(long address)
        {
            string result = "";
            const string hex = "0123456789abcdef";
            while(address > 0)
            {
                result = hex[(int)(address & 15)] + result;
                address >>= 4;
            }
            return result;
        }

        public static byte[] BytesFromHex(string data)
        {
            if((data.Length & 1) == 1)
            {
                data = '0' + data;
            }
            const string hex = "0123456789abcdef";
            byte[] result = new byte[data.Length >> 1];
            for (var i = 0; i < data.Length; i += 2)
            {
                result[i >> 1] = (byte)(hex.IndexOf(char.ToLower(data[i])) << 4 | hex.IndexOf(char.ToLower(data[i + 1])));
            }
            return result;
        }

        public static int TypeSizeFromName(string name)
        {
            switch (name)
            {
                case "char": return 1;
                case "word": return 2;
                case "dword": return 4;
                case "qword": return 8;
                case "bytes": return -1;
                default: return 0;
            }
        }
    }
}

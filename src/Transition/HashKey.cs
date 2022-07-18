using Microsoft.VisualBasic.CompilerServices;
using System;
namespace Transition
{
    public class HashKey
    {
        public byte Key { get; set; }
        public HashKey()
        {
            this.Key = 0;
        }
        public HashKey(int Key)
        {
            this.Key = Convert.ToByte(Key);
        }
        public HashKey(byte Key)
        {
            this.Key = Key;
        }
        public HashKey(string Key)
        {
            this.Key = ByteType.FromString("&H" + Key);
        }
        public override string ToString()
        {
            return string.Format("{0:X2}", this.Key);
        }
    }
}

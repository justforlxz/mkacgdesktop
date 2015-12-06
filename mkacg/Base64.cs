using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mkacg
{
    class Base64
    {
        public int Length;
        public string mp3Convert2Binary (byte[] bytes)
        {
            string result = Convert.ToBase64String(bytes);


            return result;
        }

        public byte[] loadWavSound (string filePath)
        {
            FileStream file = new FileStream(filePath , FileMode.Open);
            BinaryReader binaryReader = new BinaryReader(file);
            Length = Convert.ToInt32(binaryReader.BaseStream.Length);
            byte[] bytes = binaryReader.ReadBytes(Length);
            return bytes;
        }
    }
}

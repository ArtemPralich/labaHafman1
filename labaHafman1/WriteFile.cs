using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace labaHafman1
{
    public class WriteFile
    {
        public static void WriterBit(string str, string path)
        {
            BitArray ba = new BitArray(str.Length + 16 - str.Length % 8);
            for (int i = 0; i < str.Length + 16 - str.Length % 8; i++)
            {
                if (i < str.Length) ba[i] = str[i] == '1';
                if (i >= str.Length)
                {
                    ba[i] = !(str[str.Length - 1] == '1');
                }
            }
            byte[] bytes = new byte[str.Length / 8 + 2];
            ba.CopyTo(bytes, 0);

            Stream s = new FileStream(@"./../../../" + path, FileMode.Create);
            s.Write(bytes, 0, bytes.Length);
            s.Flush();
            s.Close();
        }
        public static void Writer(string str, string path)
        {

            StreamWriter s = new StreamWriter(@"./../../../" + path, false, System.Text.Encoding.Default);
            s.Write(str);
            s.Flush();
            s.Close();
        }
    }
}

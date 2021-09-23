using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace labaHafman1
{
    public static class ReadFile
    {
        public static string Read(string path)
        {
            using(StreamReader fileReader = new StreamReader("./../../../" + path))
            {
                return fileReader.ReadToEnd();
            }
        }
        public static string Reader(string path)
        {
            string finalString = "";

            FileStream file = new FileStream(@"./../../../" + path, FileMode.Open, FileAccess.Read);
            byte[] buf = new byte[10];
            int r = -1;
            byte b;
            while (r != 0)
            {
                r = file.Read(buf, 0, buf.Length);
                for (int j = 0; j < r; j++)
                {
                    b = buf[j];
                    for (int i = 0; i < 8; i++)
                        finalString += $"{(b >> i) & 1}";
                    //Console.Write((b >> i) & 1);
                    //Console.Write((j + 1) % 4 == 0 ? '\n' : ' ');
                }
            }
            char[] buffStr = finalString.ToCharArray();
            for (int i = finalString.Length - 1; i > 0; i--)
            {
                if (buffStr[i] != buffStr[finalString.Length - 1]) return new string(buffStr.Take(i + 1).ToArray());
            }
            file.Close();
            return "";

        }
    }
}

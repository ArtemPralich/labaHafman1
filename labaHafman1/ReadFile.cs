using System;
using System.Collections.Generic;
using System.IO;
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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace labaHafman1
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<char, string> shifrSymbols = new Dictionary<char, string>();
            Dictionary<char, int> symbolRate = new Dictionary<char, int>();
            string contentFile = ReadFile.Read("text.txt");
            char[] chars = contentFile.ToCharArray();

            for (int i = 0, n = chars.Length; i < n; i++)
            {
                if (symbolRate.ContainsKey(chars[i])) symbolRate[chars[i]]++;
                else symbolRate.Add(chars[i], 1);
            }
            List<Node> nodes = createNodesFirstLine(symbolRate);
            Node finalNodes = createTree(nodes)[0];
            createCodeSymbol(shifrSymbols, finalNodes, "", false);
            outputTree(finalNodes, 0);
            foreach( var a in shifrSymbols)
            {
                Console.WriteLine(a.Key + "-" + a.Value);
            }
            Console.WriteLine("Текст который нужно сжать");
            Console.WriteLine(contentFile);
            Console.WriteLine("Сжатый текст");
            string str = compression(chars, shifrSymbols);
            Console.WriteLine(str);
            Console.WriteLine("Расжатый текст");
            string destr = "";
            
            destr = decompression(str.ToCharArray(), finalNodes); 
            Console.WriteLine(destr);

            Console.ReadKey();
            Writer(str);
            Reader();
        }

        public static void Writer(string str)
        {
            //Console.WriteLine(str.Length%8);
            //if (str.Length % 8 != 0)
            //{
            //    int value = !(str[str.Length - 1] == '1') ? 1 : 0;
            //    for (int i = 0; i < 8 - (str.Length+1 % 8) + 8; i++)
            //    {
            //        str += $"{value}";
            //    }
            //}

            Console.WriteLine(str.Length);
            Console.WriteLine(str);
            
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
            //BitArray ba1 = new BitArray(8 - str.Length % 8 + 8);

            //for (int i = 0; i < 8 - str.Length % 8 + 8; i++)
            //{
            //    //Console.WriteLine(1);
            //    ba1[i] = !ba[str.Length - 1];
            //}
            //ba1.CopyTo(bytes, 0);
            Stream s = new FileStream(@"./../../../finalText.txt", FileMode.Create);
            s.Write(bytes, 0, bytes.Length);
            s.Flush();
            s.Close();
        }
        public static void Reader()
        {

            FileStream file = new FileStream(@"./../../../finalText.txt", FileMode.Open, FileAccess.Read);
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
                        Console.Write((b >> i) & 1);
                    //Console.Write((j + 1) % 4 == 0 ? '\n' : ' ');
                }
            }
            file.Close();
        }

        public static string decompression(char[] chars, Node node)
        {
            Node bufNode = node;
            string shifrStr = "";
            string byte1 = "";
            foreach (var ch in chars)
            {
                byte1 += ch;
                if (ch == '0' && bufNode.left != null) bufNode = bufNode.left;
                if (ch == '1' && bufNode.right != null) bufNode = bufNode.right;
                if (bufNode.left == null && bufNode.right == null)
                {

                    shifrStr += bufNode.symbol;
                    bufNode = node;
                }
            }
            Console.WriteLine(byte1);
            return shifrStr;
        }

        public static string compression(char[] chars, Dictionary<char, string> shifrSymbols)
        {
            string shifrStr = "";
            foreach(var ch in chars)
            {
                shifrStr = shifrStr + shifrSymbols[ch];
            }
            return shifrStr;
        }

        public static void createCodeSymbol(Dictionary<char, string> shifrSymbols, Node node, string code, bool indi)
        {
            if(indi) code = code + node.bin;

            if (node.left != null)
            {
                createCodeSymbol(shifrSymbols, node.left, code, true);
            }
            if (node.right != null)
            {
                createCodeSymbol(shifrSymbols, node.right, code, true);
            }
            if (node.right == null && node.left == null)
            {
                shifrSymbols.Add(node.symbol, code);
            }
        }

        public static void outputTree(Node node, int countTabs)
        {
            if (node.left != null)
            {
                outputTree(node.left, countTabs + 1);
            }
            for (int i = 0; i < countTabs; i++) Console.Write("      ");
            Console.WriteLine($"{node.weigth} - {node.symbol} - {node.bin}");
            if (node.right != null)
            {
                outputTree(node.right, countTabs + 1);
            }
        }

        public static List<Node> createNodesFirstLine(Dictionary<char, int> symbolRate)
        {
            List<Node> nodes = new List<Node>();
            foreach (var item in symbolRate)
            {
                nodes.Add(new Node() { symbol = item.Key, weigth = item.Value, left = null, right = null });
            }
            return nodes;
        }
        public static List<Node> createTree(List<Node> nodes)
        {
            if (nodes.Count == 1) return nodes;
            Node node;
            Node minnode1 = null;
            Node minnode2 = null;
            foreach (var item in nodes)
            {
                if(minnode1 == null || item.weigth <= minnode1.weigth)
                {
                    minnode2 = minnode1;
                    minnode1 = item;
                }
                else if(minnode2 == null && minnode1.weigth != item.weigth)
                {
                    minnode2 = item;
                }
            }
            node = new Node() { weigth = minnode1.weigth + minnode2.weigth, left = minnode1, right = minnode2 };
            minnode1.bin = 0;
            minnode2.bin = 1;
            nodes.Remove(minnode2);
            nodes.Remove(minnode1);
            nodes.Add(node);
            createTree(nodes);
            return nodes;
        }
    }
}

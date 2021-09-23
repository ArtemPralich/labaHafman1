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
            Dictionary<char, int> symbols = new Dictionary<char, int>();

            string contentFile = ReadFile.Read("text.txt");

            char[] chars = contentFile.ToCharArray();
            for (int i = 0, n = chars.Length; i < n; i++)
            {
                if (symbols.ContainsKey(chars[i])) symbols[chars[i]]++;
                else symbols.Add(chars[i], 1);
            }

            List<Node> nodes = createNodesFirstLine(symbols);
            Node finalNodes = TreeActions.createTree(nodes)[0];

            Dictionary<char, string> shifrSymbols = new Dictionary<char, string>();
            createCodeSymbol(shifrSymbols, finalNodes, "", false);

            //TreeActions.outputTree(finalNodes, 0);
            
            foreach( var a in shifrSymbols)
            {
                if( a.Key == 10)
                {
                    Console.WriteLine("\\n" + "-" + a.Value);
                    continue;
                }
                if (a.Key == 13)
                {
                    Console.WriteLine("\\r" + "-" + a.Value);
                    continue;
                }
                Console.WriteLine(a.Key + "-" + a.Value);
            }

            string compresStr = compression(chars, shifrSymbols);

            string destr = "";
            destr = decompression(compresStr.ToCharArray(), finalNodes);

            WriteFile.WriterBit(compresStr, "final.txt");
            string final = ReadFile.Reader("final.txt");
            //Console.WriteLine("Строка:" + final);

            destr = decompression(final.ToCharArray(), finalNodes);
            WriteFile.Writer(destr, "finalText.txt");

            //Console.WriteLine(destr);
            //Console.WriteLine(destr);
            //Console.WriteLine(str);
            //Console.WriteLine("Расжатый текст");
            //Console.WriteLine("Текст который нужно сжать");
            //Console.WriteLine(contentFile);
            //Console.WriteLine("Сжатый текст");
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
        public static List<Node> createNodesFirstLine(Dictionary<char, int> symbolRate)
        {
            List<Node> nodes = new List<Node>();
            foreach (var item in symbolRate)
            {
                nodes.Add(new Node() { symbol = item.Key, weigth = item.Value, left = null, right = null });
            }
            return nodes;
        }
        
    }
}

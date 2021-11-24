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

            ///sfdsdfds
            ///
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

            string compresStr = compression(chars, shifrSymbols);

            string destr = "";
            destr = decompression(compresStr.ToCharArray(), finalNodes);

            WriteFile.WriterBit(compresStr, "final.txt");
            string final = ReadFile.Reader("final.txt");
            //Console.WriteLine("Строка:" + final);

            destr = decompression(final.ToCharArray(), finalNodes);
            WriteFile.Writer(destr, "finalText.txt");

            int weight = outputDictionary(shifrSymbols, symbols);
            if (checkCraftMackmillan(shifrSymbols)) Console.WriteLine("Неравенство выполняется");
            else Console.WriteLine("Неравенство не выполняется");
            Console.WriteLine("Энтропия: " + calcEntropy(shifrSymbols, symbols, weight));
            Console.WriteLine("Средняя длина кодового слова: " + avgWord(shifrSymbols));
            //Console.WriteLine(destr);
            //Console.WriteLine(destr);
            //Console.WriteLine(str);
            //Console.WriteLine("Расжатый текст");
            //Console.WriteLine("Текст который нужно сжать");
            //Console.WriteLine(contentFile);
            //Console.WriteLine("Сжатый текст");
        }

        public static double calcEntropy(Dictionary<char, string> shifrSymbol, Dictionary<char, int> symbols, int weight)
        {
            double sum = 0;
            foreach(var symbol in symbols)
            {
                double p = (double)symbol.Value / weight;
                sum += p * (double)Math.Log(p);
            }
            return -sum;
        }
        public static double avgWord(Dictionary<char, string> shifrSymbol)
        {
            double sum = 0; 
            
            foreach(var symbol in shifrSymbol)
            {
                sum += symbol.Value.Length;
            }
            return sum/shifrSymbol.Count;
        }
        public static bool checkCraftMackmillan(Dictionary<char, string> shifrSymbols)
        {
            double sum = 0;
            foreach(var symbol in shifrSymbols){
                sum += Math.Pow(2, -symbol.Value.Length);
            }
            return sum <= 1.0d;
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
            //Console.WriteLine(byte1);
            return shifrStr;
        }
        public static int outputDictionary(Dictionary<char, string> shifrSymbols, Dictionary<char, int> symbols)
        {
            int count = 0;
            foreach(var a in symbols)
            {
                count += a.Value;
            }
            Console.WriteLine("{0,5}   |{1,15}| {2,15}| {3, 6}", "symb",  "%    ", "code   ", "length");
            Console.WriteLine("------------------------------------------------");
            foreach (var a in shifrSymbols)
            {
                if (a.Key == 10)
                {
                    Console.WriteLine("{0,5}   |{1,15}| {2,15}| {3, 5}", "\\n", Math.Round(((decimal)symbols[a.Key] / count), 5), a.Value, a.Value.Length);
                    //Console.WriteLine("\\n" + "-" + a.Value);
                    continue;
                }
                if (a.Key == 13)
                {
                    Console.WriteLine("{0,5}   |{1,15}| {2,15}| {3, 5}", "\\r", Math.Round(((decimal)symbols[a.Key] / count),5), a.Value, a.Value.Length);
                    //Console.WriteLine("\\r" + "-" + a.Value);
                    continue;
                }
                Console.WriteLine("{0,5}   |{1,15}| {2,15}| {3, 5}", a.Key, Math.Round(((decimal)symbols[a.Key] / count), 5), a.Value, a.Value.Length);
                //Console.WriteLine(a.Key + "-" + a.Value);
            }
            return count;
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

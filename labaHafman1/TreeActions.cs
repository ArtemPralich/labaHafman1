using System;
using System.Collections.Generic;
using System.Text;

namespace labaHafman1
{
    public static class TreeActions
    {
        public static List<Node> createTree(List<Node> nodes)
        {
            if (nodes.Count == 1) return nodes;
            Node node;
            Node minnode1 = null;
            Node minnode2 = null;
            foreach (var item in nodes)
            {
                if (minnode1 == null || item.weigth <= minnode1.weigth)
                {
                    minnode2 = minnode1;
                    minnode1 = item;
                }
                else if (minnode2 == null && minnode1.weigth != item.weigth)
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
    }
}

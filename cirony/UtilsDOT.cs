using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class UtilsDOT
    {
        public static void GenerateASTImage(ParseTreeNode ast, string fileName)
        {
            string dotScript = "graph ast{";
            int nodeID = 0;
            GenerateDotEdges(ast, ref dotScript, ref nodeID);
            dotScript += "}";

            string dotCommand = $"dot {fileName}.dot -o {fileName}.png -Tpng";

            File.WriteAllText($"{fileName}.dot", dotScript);
            Utils.ExecuteCommand(dotCommand);
        }

        private static void GenerateDotEdges(ParseTreeNode ast, ref string dotScript, ref int NodeId)
        {
            if (ast == null)
                return;

            int NodoPadreID = NodeId;
            dotScript += GetNodeScript(ast, NodoPadreID);

            foreach (ParseTreeNode child in ast.ChildNodes)
            {
                if (true)//(!child.IsPunctuationOrEmptyTransient())
                {
                    NodeId++;
                    dotScript += GetNodeScript(child, NodeId);

                    dotScript += $"{NodoPadreID} -- {NodeId};\n";

                    GenerateDotEdges(child, ref dotScript, ref NodeId);
                }
            }
        }

        private static string GetNodeScript(ParseTreeNode node, int NodeId)
        {
            string script = "";
            script += $"{NodeId} [label=\"{node.Term}";
            string tokenText = node.FindTokenAndGetText();
            if (!string.IsNullOrEmpty(tokenText) && node.ChildNodes.Count == 0)
                script += $" [{tokenText}]";
            script += "\"];";
            return script;
        }
    }
}

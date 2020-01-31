using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class Compiler
    {
        SymbolsTable SymbolsTable;
        Grammar grammar;

        public Compiler(Grammar grammar)
        {
            this.grammar = grammar;
            SymbolsTable = new SymbolsTable();
        }

        public Compiler(Grammar grammar, SymbolsTable symbolsTable)
        {
            this.grammar = grammar;
            SymbolsTable = symbolsTable;
        }


        public bool IsValidFile(string path)
        {
            string sourceCode = File.ReadAllText(path);
            return Parse(sourceCode) != null;
        }

        public bool IsValid(string sourceCode)
        {
            return Parse(sourceCode) != null;
        }

        public ParseTreeNode Parse(string sourceCode)
        {
            LanguageData language = new LanguageData(grammar);

            Parser parser = new Parser(language);

            ParseTree parseTree = parser.Parse(sourceCode);

            ParseTreeNode root = parseTree.Root;

            return root;

        }

        public void CompileAndGenerateASTImage(string sourceCode, string imageFileName)
        {
            ParseTreeNode ast = Parse(sourceCode);
            UtilsDOT.GenerateASTImage(ast, "ast");
        }

        public void CompileAndFillSymbolsTable(string sourceCode)
        {            
            ParseTreeNode ast = Parse(sourceCode);
            SymbolsTable.Fill(ast, "");
        }

        public string GetSymbolsTableAsString()
        {
            return SymbolsTable.ToString();
        }

        public string CompileAndGetIntermediateCode(string sourceCode)
        {
            ParseTreeNode ast = Parse(sourceCode);
            SymbolsTable.Fill(ast, "");
            IntermediateCodeGenerator codGen = new IntermediateCodeGenerator(SymbolsTable);
            CodeGenerationResult result = codGen.GetCode(ast, "");
            return result.Code;
        }
    }
}

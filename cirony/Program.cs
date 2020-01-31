using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFileName = "test.txt";
            string sourceCode = File.ReadAllText(sourceFileName);
            CGramar grammar = new CGramar();
            Compiler compiler = new Compiler(grammar);
            compiler.CompileAndGenerateASTImage(sourceCode, "ast");
            Console.WriteLine(compiler.IsValid(sourceCode).ToString());
            PrintLine();
            compiler.CompileAndFillSymbolsTable(sourceCode);
            Console.WriteLine(compiler.GetSymbolsTableAsString());
            PrintLine();
            Console.WriteLine(compiler.CompileAndGetIntermediateCode(sourceCode));
            Console.ReadKey();
        }

        static void PrintLine()
        {
            Console.WriteLine("------------------------------------------");
        }
    }
}

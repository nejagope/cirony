using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class SymbolsTable: List<Symbol>
    {

        new public string ToString()
        {
            string s = "";
            s += "Id  ------------- Type ---------- Size ------------ Context --------- Position --- FunctionID\n";
            foreach (Symbol symbol in this)
            {
                s += $"{symbol.Id} -- {symbol.Type} -- {symbol.Size} -- {symbol.Context} -- {symbol.Position} -- {symbol.FunctionId}\n";
            }
            return s;
        }

        public SymbolsTable() : base()
        {            
        }

        public Symbol FindFunction(string FunctionID)
        {
            return Find(sym => sym.Type.Equals("FUNC") && sym.FunctionId.Equals(FunctionID));
        }
    
        public Symbol Find(string Id, string Type, int Size, string Context)
        {
            return Find(new Symbol(Id, Type, Size, Context));
        }

        public Symbol Find(Symbol symbol)
        {            
            return 
                Find(sym => 
                   sym.Id == symbol.Id
                && sym.Type == symbol.Type                 
                && symbol.Context.StartsWith(sym.Context)
                && (symbol.Type == "FUNC" ? sym.Size == symbol.Size : true)
                );            
        }

        public void Fill(ParseTreeNode ast, string context)
        {
            if (ast == null)
                return;

            Symbol newSymbol, symbolFound = null;
            int newSymbolSize = 0;
            string newSymbolId = null;
            string dataType = "";

            switch (ast.Term.Name)
            {
                case "PROGRAM":
                case "DECLS":
                case "DECL":
                case "SENTS":
                case "SENT":
                case "PARAMS":
                    break;
                case "FUNC":                    
                    dataType = ast.ChildNodes[0].FindTokenAndGetText();
                    newSymbolId = ast.ChildNodes[1].FindTokenAndGetText();
                    string funcContext = GetFunctionID(ast, context);
                    newSymbol = new Symbol(newSymbolId, "FUNC", newSymbolSize, context, dataType, ast, funcContext);
                    symbolFound = Find(newSymbol);
                    if (symbolFound == null)
                    {
                        Add(newSymbol);
                    }
                    else
                    {
                        //error symbol already exists
                    }
                    //el offset tendrá valor de 0 si la función tiene parámetros o -1 si no los tiene
                    int offset = ast.ChildNodes.Count - 4;
                    
                    if (offset == 0)
                    {
                        //obtener el tamaño del símbolo
                        ParseTreeNodeList paramNodes = ASTNodeListElements(ast.ChildNodes[2 + offset]);
                        foreach(ParseTreeNode paramNode in paramNodes)
                        {
                            newSymbol.Size += GetTypeSize(paramNode.ChildNodes[0].Term.Name);
                        }
                        //PARAMS
                        Fill(ast.ChildNodes[2], funcContext);
                    }

                    //int returnSize = GetTypeSize(dataType);
                    //newSymbol.Size += returnSize;//return size
                    newSymbol.Size += 2; //this + return
                    
                    //SENTS
                    Fill(ast.ChildNodes[3 + offset], funcContext);
                    return;
                case "PARAM":
                    dataType = ast.FindTokenAndGetText();
                    newSymbolId = ast.ChildNodes[1].FindTokenAndGetText();
                    newSymbol = new Symbol(newSymbolId, "PARAM", GetTypeSize(dataType), context, dataType);
                    symbolFound = Find(newSymbol);
                    if (symbolFound == null)
                    {
                        Add(newSymbol);
                        newSymbol.Position = GetSymbolPosition(newSymbol);
                    }
                    else
                    {
                        //error symbol already exists
                    }
                    return;
            }

            foreach (ParseTreeNode child in ast.ChildNodes)
            {
                Fill(child, context);
            }
        }

        public int GetSymbolPosition(Symbol symbol)
        {
            int position = 0;
            foreach (Symbol s in this)
            {
                if (s == symbol)
                    return position + 2; //return + this

                if (s.Context.StartsWith(symbol.Context)){
                    position++;
                }
            }
            return position + 2;//return + this
        }

        public ParseTreeNodeList ASTNodeListElements(ParseTreeNode ast)
        {
            ParseTreeNodeList nodes = new ParseTreeNodeList();
            switch (ast.Term.Name)
            {
                case "PARAMS":
                    if (ast.ChildNodes.Count == 2)
                    {
                        nodes.AddRange(ASTNodeListElements(ast.ChildNodes[0]));
                        nodes.Add(ast.ChildNodes[1]);
                    }
                    else
                    {
                        nodes.Add(ast.ChildNodes[0]);
                    }
                    break;
            }
            return nodes;
        }

        public int GetTypeSize(string type)
        {
            return 1;
        }

        public string GetFunctionID(ParseTreeNode astFunction, string context)
        {
            string dataType = astFunction.ChildNodes[0].FindTokenAndGetText();
            string newSymbolId = astFunction.ChildNodes[1].FindTokenAndGetText();
            string functionId = $"{context}_func-{newSymbolId}-{dataType}";
            if (astFunction.ChildNodes.Count == 4)
            {
                ParseTreeNodeList paramsNode = ASTNodeListElements(astFunction.ChildNodes[2]);
                foreach(var param in paramsNode)
                {
                    functionId += "-" + param.ChildNodes[0].FindTokenAndGetText();
                }
            }
            return functionId;
        }
    }    
}

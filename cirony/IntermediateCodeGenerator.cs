using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class IntermediateCodeGenerator
    {
        SymbolsTable st;
        int _t, _l;

        public IntermediateCodeGenerator(SymbolsTable st)
        {
            this.st = st;
            _t = 0;
            _l = 0;
        }

        public CodeGenerationResult GetCode(ParseTreeNode ast, string context)
        {
            if (ast == null)
                return null;

            CodeGenerationResult result = null, result1 = null, result2 = null;
            string t = "";
            string code = "";
            Symbol symbol;
            
            switch (ast.Term.Name)
            {
                case "FUNC":
                    //symbol = st.Find(ast.ChildNodes[1].FindTokenAndGetText(), "FUNC", 0, "");
                    symbol = st.FindFunction(st.GetFunctionID(ast, context));
                    code = $"function {symbol.FunctionId}{{\n";
                    int sentsNodeId = symbol.Node.ChildNodes.Count - 1;
                    code += GetCode(symbol.Node.ChildNodes[sentsNodeId], symbol.FunctionId).Code;                    
                    code += "}";                    
                    return new CodeGenerationResult(code, "");

                case "E":
                    switch (ast.ChildNodes.Count){
                        case 1:
                        
                            result1 = GetCode(ast.ChildNodes[0], context);
                            t = GetT();
                            code = result1.Code;
                            code += $"{t} = {result1.Result};\n";
                            return new CodeGenerationResult(code, t);
                        
                        case 3:
                            result1 = GetCode(ast.ChildNodes[0], context);
                            result2 = GetCode(ast.ChildNodes[2], context);
                            t = GetT();
                            code = $"{t} = {result1.Result} {ast.ChildNodes[1].FindTokenAndGetText()} {result2.Result};\n";
                            return new CodeGenerationResult(result1.Code + result2.Code + code, t);
                    }
                    break;
                case "=":
                    result1 = GetCode(ast.ChildNodes[0], context);
                    result2 = GetCode(ast.ChildNodes[1], context);
                    code = result1.Code;
                    code += result2.Code;
                    code += $"{result1.Result} = {result2.Result};\n";
                    return new CodeGenerationResult(code, "");

                case "num":
                    return new CodeGenerationResult("", ast.FindTokenAndGetText());

                case "id":
                    symbol = st.Find(ast.FindTokenAndGetText(), "PARAM", 0, context);
                    t = GetT();
                    code = $"{t} = p + {symbol.Position};\n";                           
                    result = new CodeGenerationResult(code, $"stack[{t}]");                    
                    return result;

                default:
                    code = "";
                    foreach (ParseTreeNode child in ast.ChildNodes) {
                        code += GetCode(child, context).Code;
                    }
                    return new CodeGenerationResult(code, "");
            }
            return result;
        }

        private string GetT()
        {
            return $"t{_t++}";
        }

        private string GetL()
        {
            return $"l{_l++}";
        }

    }    
}

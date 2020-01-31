using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class CGramar : Grammar
    {
        public CGramar(): base(false)
        {
            // 1. Terminals
            var number = new NumberLiteral("num");
            //Let's allow big integers (with unlimited number of digits):            
            number.DefaultIntTypes = new TypeCode[] { TypeCode.Int32, TypeCode.Int64, NumberLiteral.TypeCodeBigInt };
            var id = new IdentifierTerminal("id");
            var comment = new CommentTerminal("comment", "//", "\n", "\r");
            //comment must to be added to NonGrammarTerminals list; it is not used directly in grammar rules,
            // so we add it to this list to let Scanner know that it is also a valid terminal. 
            base.NonGrammarTerminals.Add(comment);

            var type_int = ToTerm("int");            

            // 2. Non-terminals
            var E = new NonTerminal("E");            
            var PROGRAM = new NonTerminal("PROGRAM");
            var DECLS = new NonTerminal("DCLS");
            var DECL = new NonTerminal("DECL");
            var FUNC = new NonTerminal("FUNC");
            var TYPE = new NonTerminal("TYPE");
            var PARAMS = new NonTerminal("PARAMS");
            var PARAM_LIST = new NonTerminal("PARAM_LIST");
            var PARAM = new NonTerminal("PARAM");
            var BLOCK = new NonTerminal("BLOCK");
            var SENTS = new NonTerminal("SENTS");
            var SENT = new NonTerminal("SENT");
            var ASSIGNMENT = new NonTerminal("=");
            var RETURN = new NonTerminal("RET");
            var CALL = new NonTerminal("CALL");            
            var ARGS = new NonTerminal("ARGS");

            PROGRAM.Rule = DECLS; 

            DECLS.Rule = DECLS + DECL 
                        | DECL;

            DECL.Rule = FUNC;

            FUNC.Rule = TYPE + id + "(" + PARAMS + ")" + "{" + SENTS + "}"
                      | TYPE + id + "()" + "{" + SENTS + "}";
            
            TYPE.Rule = type_int;

            PARAMS.Rule = PARAMS + "," + PARAM 
                            | PARAM;
            

            PARAM.Rule = TYPE + id;

            SENTS.Rule = SENTS + SENT
                       | SENT;

            SENT.Rule = ASSIGNMENT + ";"
                      | CALL + ";"
                      | RETURN + ";";

            ASSIGNMENT.Rule = id + "=" + E;
            RETURN.Rule = "return" + E
                        | "return";

            CALL.Rule = id + "(" + ARGS + ")"
                        | id + "()";

            ARGS.Rule = ARGS + "," + E
                        | E;

            E.Rule = 
                    CALL
                    | E + "+" + E
                    | E + "*" + E
                    | E + "^" + E
                    | number
                    | id;

            BLOCK.Rule = "{" + SENTS + "}" 
                        | SENT;

            this.Root = PROGRAM;


            // 4. Operators precedence
            RegisterOperators(1, "+", "-");
            RegisterOperators(2, "*", "/");
            RegisterOperators(3, Associativity.Right, "^");

            // 5. Punctuation and transient terms            
            MarkPunctuation("(", ")", "{", "}", ",", ";", "=");
            RegisterBracePair("(", ")");
            RegisterBracePair("{", "}");
            
            //MarkTransient(PARAM_LIST);            
        }
    }
}

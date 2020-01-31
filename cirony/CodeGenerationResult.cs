using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class CodeGenerationResult
    {
        public string Code { get; set; }
        public string Result { get; set; }        
        

        public CodeGenerationResult(string Code, string tResult)
        {
            this.Code = Code;
            this.Result = tResult;            
        }

        public CodeGenerationResult(string Code, int t)
        {
            this.Code = Code;            
            this.Result = $"t{t}";            
        }
    }
}

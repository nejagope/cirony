using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cirony
{
    class Symbol
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public string Context { get; set; }
        public string DataType { get; set; }
        public ParseTreeNode Node { get; set; }
        public string FunctionId { get; set; }
        public int Position { get; set; }

        public Symbol(string Id, string Type, int Size, string Context)
        {
            this.Id = Id;
            this.Type = Type;
            this.Size = Size;
            this.Context = Context;
        }

        public Symbol(string Id, string Type, int Size, string Context, string DataType)
        {
            this.Id = Id;
            this.Type = Type;
            this.Size = Size;
            this.Context = Context;
            this.DataType = DataType;
        }

        public Symbol(string Id, string Type, int Size, string Context, string DataType, ParseTreeNode Node)
        {
            this.Id = Id;
            this.Type = Type;
            this.Size = Size;
            this.Context = Context;
            this.DataType = DataType;
            this.Node = Node;
        }

        public Symbol(string Id, string Type, int Size, string Context, string DataType, ParseTreeNode Node, string FunctionId)
        {
            this.Id = Id;
            this.Type = Type;
            this.Size = Size;
            this.Context = Context;
            this.DataType = DataType;
            this.Node = Node;
            this.FunctionId = FunctionId;
        }
    }
}

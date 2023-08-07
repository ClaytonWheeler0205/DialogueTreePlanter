using System.Collections.Generic;

using DialogueTreePlanter.Elements;

namespace DialogueTreePlanter.Data.Error
{

    public class NodeErrorData
    {
        public ErrorData ErrorData { get; set; }
        public List<DialogueNodeBase> Nodes { get; set; }

        public NodeErrorData() 
        { 
            ErrorData = new ErrorData();
            Nodes = new List<DialogueNodeBase>();
        }
    }
}
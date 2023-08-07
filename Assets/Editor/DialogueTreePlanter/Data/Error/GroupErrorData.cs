using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DialogueTreePlanter.Elements;

namespace DialogueTreePlanter.Data.Error
{
    public class GroupErrorData
    {
        public ErrorData ErrorData { get; set; }
        public List<DialogueTreeGroup> Groups { get; set; }

        public GroupErrorData()
        {
            ErrorData = new ErrorData();
            Groups = new List<DialogueTreeGroup>();
        }
    }
}
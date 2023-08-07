using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTreePlanter.Enumerations;
using UnityEditor.Experimental.GraphView;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Windows;

namespace DialogueTreePlanter.Elements
{
    public class DialogueNodeSingleChoice : DialogueNodeBase
    {

        public override void Initialize(DialogueTreeGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            DialogueNodeType = DialogueType.SingleChoice;
            Choices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();
            foreach(string choice in Choices)
            {
                Port choicePort = this.CreatePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}
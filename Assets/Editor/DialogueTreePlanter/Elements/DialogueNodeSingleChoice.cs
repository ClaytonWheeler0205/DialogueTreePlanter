using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTreePlanter.Enumerations;
using UnityEditor.Experimental.GraphView;
using DialogueTreePlanter.Utilities;

namespace DialogueTreePlanter.Elements
{
    public class DialogueNodeSingleChoice : DialogueNodeBase
    {

        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
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
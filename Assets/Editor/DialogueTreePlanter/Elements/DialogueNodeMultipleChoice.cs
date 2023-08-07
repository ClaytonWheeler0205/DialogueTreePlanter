using UnityEngine;
using DialogueTreePlanter.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Windows;

namespace DialogueTreePlanter.Elements
{
    public class DialogueNodeMultipleChoice : DialogueNodeBase
    {
        public override void Initialize(DialogueTreeGraphView graphView, Vector2 position)
        {
            base.Initialize(graphView, position);
            DialogueNodeType = DialogueType.MultipleChoice;
            Choices.Add("New choice");
        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = ElementUtility.CreateButton("Add Choice", () => 
            {
                Port choicePort = CreateChoicePort("New Choice");
                Choices.Add("New Choice");
                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("dtp-node__button");

            mainContainer.Insert(2, addChoiceButton);

            foreach (string choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }

        #region Elements Creation
        private Port CreateChoicePort(string choice)
        {
            Port choicePort = this.CreatePort();

            Button deleteChoiceButton = ElementUtility.CreateButton("X");

            deleteChoiceButton.AddToClassList("dtp-node__button");

            TextField choiceTextField = ElementUtility.CreateTextField(choice);

            choiceTextField.AddClasses(
                "dtp-node__textfield",
                "dtp-node__choice-textfield",
                "dtp-node__textfield__hidden");

            choicePort.Add(choiceTextField);
            choicePort.Add(deleteChoiceButton);
            return choicePort;
        }
        #endregion
    }
}
using UnityEngine;
using DialogueTreePlanter.Enumerations;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace DialogueTreePlanter.Elements
{
    public class DialogueNodeMultipleChoice : DialogueNodeBase
    {
        public override void Initialize(Vector2 position)
        {
            base.Initialize(position);
            DialogueNodeType = DialogueType.MultipleChoice;
            Choices.Add("New choice");
        }

        public override void Draw()
        {
            base.Draw();

            Button addChoiceButton = new Button()
            {
                text = "Add Choice"
            };

            addChoiceButton.AddToClassList("dtp-node__button");

            mainContainer.Insert(2, addChoiceButton);

            foreach (string choice in Choices)
            {
                Port choicePort = InstantiatePort(Orientation.Horizontal, Direction.Output, Port.Capacity.Single, typeof(bool));
                choicePort.portName = "";

                Button deleteChoiceButton = new Button()
                {
                    text = "X"
                };

                deleteChoiceButton.AddToClassList("dtp-node__button");

                TextField choiceTextField = new TextField()
                {
                    value = choice
                };

                choiceTextField.AddToClassList("dtp-node__textfield");
                choiceTextField.AddToClassList("dtp-node__choice-textfield");
                choiceTextField.AddToClassList("dtp-node__textfield__hidden");

                choicePort.Add(choiceTextField);
                choicePort.Add(deleteChoiceButton);
                outputContainer.Add(choicePort);
            }
            RefreshExpandedState();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DialogueTreePlanter.Actors;
using DialogueTreePlanter.Enumerations;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DialogueTreePlanter.Utilities;

namespace DialogueTreePlanter.Elements
{
    // Dev note: Make this an abstract class at a later time
    public abstract class DialogueNodeBase : Node
    {
        [SerializeField]
        private string _dialogueName;
        [SerializeField]
        private SO_Actor _dialogueActor;
        [SerializeField]
        protected List<string> Choices;
        [SerializeField]
        private string _dialogueText;
        protected DialogueType DialogueNodeType;

        public virtual void Initialize(Vector2 position)
        {
            _dialogueName = "DialogueName";
            _dialogueActor = null;
            Choices = new List<string>();
            _dialogueText = "Dialogue Text.";

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("dtp-node__main-container");
            extensionContainer.AddToClassList("dtp-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title container
            TextField dialogueNameTextField = ElementUtility.CreateTextField(_dialogueName);

            dialogueNameTextField.AddClasses(
                "dtp-node__textfield",
                "dtp-node__filename-textfield",
                "dtp-node__textfield__hidden");

            titleContainer.Insert(0, dialogueNameTextField);

            // Actor Container
            ObjectField dialogueActorField = new ObjectField()
            {
                objectType = typeof(SO_Actor),
                value = _dialogueActor
            };
            mainContainer.Insert(1, dialogueActorField);

            // Input port
            Port inputPort = this.CreatePort("Dialogue Connection", direction: Direction.Input, capacity: Port.Capacity.Multi);
            inputContainer.Add(inputPort);

            // Dialogue Content
            VisualElement customDataContainer = new VisualElement();
            customDataContainer.AddToClassList("dtp-node__custom-data-container");

            Foldout textFoldout = ElementUtility.CreateFoldout("Dialogue Contents");

            TextField dialogueContentTextField = ElementUtility.CreateTextArea(_dialogueText);

            dialogueContentTextField.AddClasses(
                "dtp-node__textfield",
                "dtp-node__quote-textfield");

            textFoldout.Add(dialogueContentTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DialogueTreePlanter.Actors;
using DialogueTreePlanter.Enumerations;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Windows;

namespace DialogueTreePlanter.Elements
{
    // Dev note: Make this an abstract class at a later time
    public abstract class DialogueNodeBase : Node
    {
        [SerializeField]
        private string _dialogueName;
        public string DialogueName => _dialogueName;
        [SerializeField]
        private SO_Actor _dialogueActor;
        [SerializeField]
        protected List<string> Choices;
        [SerializeField]
        private string _dialogueText;
        protected DialogueType DialogueNodeType;
        public DialogueTreeGroup Group { get; set; }

        private StyleColor _defaultBackgroundColor;
        private DialogueTreeGraphView _graphView;

        public virtual void Initialize(DialogueTreeGraphView graphView, Vector2 position)
        {
            _dialogueName = "DialogueName";
            _dialogueActor = null;
            Choices = new List<string>();
            _dialogueText = "Dialogue Text.";
            _graphView = graphView;

            SetPosition(new Rect(position, Vector2.zero));

            mainContainer.AddToClassList("dtp-node__main-container");
            extensionContainer.AddToClassList("dtp-node__extension-container");
            _defaultBackgroundColor = mainContainer.style.backgroundColor;
        }

        #region Overrided Methods
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Input Ports", actionEvent => DisconnectPorts(inputContainer));
            evt.menu.AppendAction("Disconnect Output Ports", actionEvent => DisconnectPorts(outputContainer));
            base.BuildContextualMenu(evt);
        }
        #endregion

        public virtual void Draw()
        {
            // Title container
            TextField dialogueNameTextField = ElementUtility.CreateTextField(_dialogueName, callback =>
            {
                if (Group == null)
                {
                    _graphView.RemoveUngroupedNode(this);
                    _dialogueName = callback.newValue;
                    _graphView.AddUngroupedNode(this);
                    return;
                }
                DialogueTreeGroup currentGroup = Group;
                _graphView.RemoveGroupedNode(this, Group);
                _dialogueName = callback.newValue;
                _graphView.AddGroupedNode(this, currentGroup);
            });

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

        #region Utility Methods
        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach(Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                _graphView.DeleteElements(port.connections);
            }
        }

        public void SetErrorStyle(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetStyle()
        {
            mainContainer.style.backgroundColor = _defaultBackgroundColor;
        }
        #endregion
    }
}
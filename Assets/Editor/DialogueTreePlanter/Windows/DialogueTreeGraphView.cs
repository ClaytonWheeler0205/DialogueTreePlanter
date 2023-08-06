using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DialogueTreePlanter.Elements;
using UnityEngine;
using DialogueTreePlanter.Enumerations;

namespace DialogueTreePlanter.Windows
{
    public class DialogueTreeGraphView : GraphView
    {
        public DialogueTreeGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();
        }

        // Dev note: Please move the create node functionality into a factory class
        private DialogueNodeBase CreateNode(DialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DialogueTreePlanter.Elements.DialogueNode{dialogueType}");
            DialogueNodeBase node = Activator.CreateInstance( nodeType ) as DialogueNodeBase;
            node.Initialize(position);
            node.Draw();
            return node;
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultipleChoice));
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType dialogueType)
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.mousePosition))));

            return menuManipulator;
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet graphStyleSheet = EditorGUIUtility.Load("DialogueTreePlanter/DialogueTreeGraphViewStyles.uss") as StyleSheet;
            StyleSheet nodeStyleSheet = EditorGUIUtility.Load("DialogueTreePlanter/DialogueTreeNodeStyles.uss") as StyleSheet;
            styleSheets.Add(graphStyleSheet);
            styleSheets.Add(nodeStyleSheet);
        }
    }
}
using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using DialogueTreePlanter.Elements;
using DialogueTreePlanter.Enumerations;
using DialogueTreePlanter.Utilities;

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

        #region Overrided Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiablePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if(startPort.node == port.node)
                {
                    return;
                }

                if(startPort.direction == port.direction)
                {
                    return;
                }

                compatiablePorts.Add(port);
            });
            return compatiablePorts;
        }
        #endregion

        #region Manipulators
        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", DialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", DialogueType.MultipleChoice));
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(string actionTitle, DialogueType dialogueType)
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, actionEvent.eventInfo.mousePosition))));

            return menuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Dialogue Group", actionEvent.eventInfo.mousePosition))));

            return menuManipulator;
        }
        #endregion

        #region Elements Creation
        // Dev note: Please move the create node functionality into a factory class
        private DialogueNodeBase CreateNode(DialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DialogueTreePlanter.Elements.DialogueNode{dialogueType}");
            DialogueNodeBase node = Activator.CreateInstance(nodeType) as DialogueNodeBase;
            node.Initialize(position);
            node.Draw();
            return node;
        }

        private Group CreateGroup(string title, Vector2 mousePosition)
        {
            Group group = new Group()
            {
                title = title
            };
            group.SetPosition(new Rect(mousePosition, Vector2.zero));
            return group;
        }
        #endregion

        #region Elements Addition
        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "DialogueTreePlanter/DialogueTreeGraphViewStyles.uss",
                "DialogueTreePlanter/DialogueTreeNodeStyles.uss");
        }
        #endregion
    }
}
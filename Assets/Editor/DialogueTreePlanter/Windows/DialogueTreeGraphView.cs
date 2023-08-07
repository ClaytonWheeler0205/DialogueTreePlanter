using System;
using System.Collections.Generic;

using UnityEditor;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;

using DialogueTreePlanter.Elements;
using DialogueTreePlanter.Enumerations;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Data.Error;

namespace DialogueTreePlanter.Windows
{
    public class DialogueTreeGraphView : GraphView
    {
        private DialogueTreeWindow _editorWindow;
        private DialogueTreeSearchWindow _searchWindow;

        private SerializableDictionary<string, NodeErrorData> _ungroupedNodes;
        private SerializableDictionary<string, GroupErrorData> _groups;
        private SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>> _groupedNodes;
        public DialogueTreeGraphView(DialogueTreeWindow editorWindow)
        {
            _editorWindow = editorWindow;

            _ungroupedNodes = new SerializableDictionary<string, NodeErrorData>();
            _groups = new SerializableDictionary<string, GroupErrorData>();
            _groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, NodeErrorData>>();

            AddManipulators();
            AddSearchWindow();
            AddGridBackground();
            AddStyles();

            OnElementsDeleted();
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
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
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.mousePosition)))));

            return menuManipulator;
        }

        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator menuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => AddElement(CreateGroup("Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.mousePosition)))));

            return menuManipulator;
        }
        #endregion

        #region Elements Creation
        // Dev note: Please move the create node functionality into a factory class
        public DialogueNodeBase CreateNode(DialogueType dialogueType, Vector2 position)
        {
            Type nodeType = Type.GetType($"DialogueTreePlanter.Elements.DialogueNode{dialogueType}");
            DialogueNodeBase node = Activator.CreateInstance(nodeType) as DialogueNodeBase;
            node.Initialize(this, position);
            node.Draw();

            AddUngroupedNode(node);

            return node;
        }

        public DialogueTreeGroup CreateGroup(string title, Vector2 mousePosition)
        {
            DialogueTreeGroup group = new DialogueTreeGroup(title, mousePosition);
            AddGroup(group);
            return group;
        }

        #endregion

        #region Callbacks
        private void OnElementsDeleted()
        {
            Type groupType = typeof(DialogueTreeGroup);
            deleteSelection = (operationName, askUser) =>
            {
                List<DialogueTreeGroup> groupsToDelete = new List<DialogueTreeGroup>();
                List<DialogueNodeBase> nodesToDelete = new List<DialogueNodeBase>();
                foreach(GraphElement element in selection)
                {
                    if(element is DialogueNodeBase node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if(element.GetType() != groupType)
                    {
                        continue;
                    }

                    DialogueTreeGroup group = element as DialogueTreeGroup;
                    RemoveGroup(group);
                    groupsToDelete.Add(group);
                }

                foreach (DialogueTreeGroup group in groupsToDelete)
                {
                    RemoveElement(group);
                }
                foreach (DialogueNodeBase node in nodesToDelete)
                {
                    if(node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }
                    RemoveUngroupedNode(node);
                    RemoveElement(node);
                }
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach(GraphElement element in elements)
                {
                    if(!(element is DialogueNodeBase))
                    {
                        continue;
                    }

                    DialogueTreeGroup nodeGroup = group as DialogueTreeGroup;
                    DialogueNodeBase node = element as DialogueNodeBase;

                    RemoveUngroupedNode(node);
                    AddGroupedNode(node, nodeGroup);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is DialogueNodeBase))
                    {
                        continue;
                    }

                    DialogueNodeBase node = element as DialogueNodeBase;

                    // Remove node from its group
                    RemoveGroupedNode(node, group);
                    AddUngroupedNode(node);
                }
            };
        }

        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                DialogueTreeGroup treeGroup = group as DialogueTreeGroup;

                RemoveGroup(treeGroup);
                treeGroup.OldTitle = newTitle;
                AddGroup(treeGroup);
            };
        }
        #endregion

        #region Repeated Elements
        public void AddUngroupedNode(DialogueNodeBase node)
        {
            string nodeName = node.DialogueName;

            if(!_ungroupedNodes.ContainsKey(nodeName)) 
            {
                NodeErrorData nodeErrorData = new NodeErrorData();
                nodeErrorData.Nodes.Add(node);
                _ungroupedNodes.Add(nodeName, nodeErrorData);
                return;
            }

            List<DialogueNodeBase> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Add(node);

            Color errorColor = _ungroupedNodes[nodeName].ErrorData.Color;

            node.SetErrorStyle(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ungroupedNodesList[0].SetErrorStyle(errorColor);
            }
        }

        public void RemoveUngroupedNode(DialogueNodeBase node)
        {
            string nodeName = node.DialogueName;
            List<DialogueNodeBase> ungroupedNodesList = _ungroupedNodes[nodeName].Nodes;

            ungroupedNodesList.Remove(node);
            node.ResetStyle();

            if (ungroupedNodesList.Count == 1)
            {
                ungroupedNodesList[0].ResetStyle();
            }
            else if (ungroupedNodesList.Count == 0)
            {
                _ungroupedNodes.Remove(nodeName);
            }
        }

        private void AddGroup(DialogueTreeGroup group)
        {
            string groupName = group.title;

            if (!_groups.ContainsKey(groupName))
            {
                GroupErrorData groupErrorData = new GroupErrorData();
                groupErrorData.Groups.Add(group);
                _groups.Add(groupName, groupErrorData);
                return;
            }

            List<DialogueTreeGroup> groupsList = _groups[groupName].Groups;

            groupsList.Add(group);

            Color errorColor = _groups[groupName].ErrorData.Color;
            group.SetErrorStyle(errorColor);

            if(groupsList.Count == 2) 
            {
                groupsList[0].SetErrorStyle(errorColor);
            }
        }

        private void RemoveGroup(DialogueTreeGroup group)
        {
            string oldGroupName = group.OldTitle;
            List<DialogueTreeGroup> groupsList = _groups[oldGroupName].Groups;

            groupsList.Remove(group);
            group.ResetStyle();

            if (groupsList.Count == 1)
            {
                groupsList[0].ResetStyle();
            }
            else if (groupsList.Count == 0)
            {
                _groups.Remove(oldGroupName);
            }
        }

        public void AddGroupedNode(DialogueNodeBase node, DialogueTreeGroup group)
        {
            string nodeName = node.DialogueName;

            node.Group = group;

            if (!_groupedNodes.ContainsKey(group))
            {
                _groupedNodes.Add(group, new SerializableDictionary<string, NodeErrorData>());
            }

            if (!_groupedNodes[group].ContainsKey(nodeName))
            {
                NodeErrorData errorData = new NodeErrorData();
                errorData.Nodes.Add(node);

                _groupedNodes[group].Add(nodeName, errorData);

                return;
            }

            List<DialogueNodeBase> groupedNodesList = _groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Add(node);
            Color errorColor = _groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorStyle(errorColor);

            if(groupedNodesList.Count == 2)
            {
                groupedNodesList[0].SetErrorStyle(errorColor);
            }

        }

        public void RemoveGroupedNode(DialogueNodeBase node, Group group)
        {
            string nodeName = node.DialogueName;
            node.Group = null;

            List<DialogueNodeBase> groupedNodesList = _groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);

            node.ResetStyle();

            if (groupedNodesList.Count == 1)
            {
                groupedNodesList[0].ResetStyle();
            }
            else if (groupedNodesList.Count == 0)
            {
                _groupedNodes[group].Remove(nodeName);

                if (_groupedNodes[group].Count == 0)
                {
                    _groupedNodes.Remove(group);
                }
            }
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

        private void AddSearchWindow()
        {
            if(_searchWindow == null)
            {
                _searchWindow = ScriptableObject.CreateInstance<DialogueTreeSearchWindow>();
                _searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
        }
        #endregion

        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;
            if (isSearchWindow)
            {
                worldMousePosition -= _editorWindow.position.position;
            }
            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);
            return localMousePosition;
        }
        #endregion
    }
}
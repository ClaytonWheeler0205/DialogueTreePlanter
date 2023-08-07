using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

using DialogueTreePlanter.Enumerations;
using DialogueTreePlanter.Elements;

namespace DialogueTreePlanter.Windows
{
    public class DialogueTreeSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private DialogueTreeGraphView _graphView;
        private Texture2D _indentationIcon;
        public void Initialize(DialogueTreeGraphView graphView)
        {
            _graphView = graphView;
            _indentationIcon = new Texture2D(1, 1);
            _indentationIcon.SetPixel(1, 1, Color.clear);
            _indentationIcon.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> entries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", _indentationIcon))
                {
                    level = 2,
                    userData = DialogueType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", _indentationIcon))
                {
                    level = 2,
                    userData = DialogueType.MultipleChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", _indentationIcon))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return entries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = _graphView.GetLocalMousePosition(context.screenMousePosition, true);
            switch(SearchTreeEntry.userData)
            {
                case DialogueType.SingleChoice:
                    DialogueNodeSingleChoice singleChoiceNode = _graphView.CreateNode(DialogueType.SingleChoice, localMousePosition) as DialogueNodeSingleChoice;
                    _graphView.AddElement(singleChoiceNode);
                    return true;
                case DialogueType.MultipleChoice:
                    DialogueNodeMultipleChoice multipleChoiceNode = _graphView.CreateNode(DialogueType.MultipleChoice, localMousePosition) as DialogueNodeMultipleChoice;
                    _graphView.AddElement(multipleChoiceNode);
                    return true;
                case Group _:
                    DialogueTreeGroup group = _graphView.CreateGroup("DialogueGroup", context.screenMousePosition);
                    _graphView.AddElement(group);
                    return true;
                default:
                    return false;
            }
        }
    }
}
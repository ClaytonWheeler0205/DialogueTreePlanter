using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

using DialogueTreePlanter.Utilities;

namespace DialogueTreePlanter.Windows
{
    public class DialogueTreeWindow : EditorWindow
    {
        [MenuItem("Window/Dialogue Tree Planter/Dialogue Tree Graph")]
        public static void ShowMyEditor()
        {
            EditorWindow window = GetWindow<DialogueTreeWindow>();
            window.titleContent = new GUIContent("DIalogue Tree Graph");
        }

        public void CreateGUI()
        {
            AddGraphView();
            AddStyles();
        }

        #region Elements Addition
        private void AddGraphView()
        {
            DialogueTreeGraphView graphView = new DialogueTreeGraphView(this);
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("DialogueTreePlanter/DialogueTreeVariables.uss");
        }
        #endregion
    }
}
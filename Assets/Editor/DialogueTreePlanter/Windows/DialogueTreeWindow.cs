using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

        private void AddGraphView()
        {
            DialogueTreeGraphView graphView = new DialogueTreeGraphView();
            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load("DialogueTreePlanter/DialogueTreeVariables.uss") as StyleSheet;
            rootVisualElement.styleSheets.Add(styleSheet);
        }
    }
}
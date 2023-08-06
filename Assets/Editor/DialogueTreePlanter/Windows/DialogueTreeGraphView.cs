using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using DialogueTreePlanter.Elements;

namespace DialogueTreePlanter.Windows
{
    public class DialogueTreeGraphView : GraphView
    {
        public DialogueTreeGraphView()
        {
            AddManipulators();
            AddGridBackground();
            AddStyles();

            CreateNode();
        }

        private void CreateNode()
        {
            DialogueNodeBase node = new DialogueNodeBase();
            Add(node);
        }

        private void AddManipulators()
        {
            this.AddManipulator(new ContentDragger());
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddStyles()
        {
            StyleSheet styleSheet = EditorGUIUtility.Load("DialogueTreePlanter/DialogueTreeGraphViewStyles.uss") as StyleSheet;
            styleSheets.Add(styleSheet);
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Actors;

namespace DialogueTreePlanter.Windows
{

    public class CastingCallWindow : EditorWindow
    {
        private VisualElement leftPane;
        private VisualElement rightPane;

        private static ActorFactory _actorFactory;
        private static ActorGatherer _actorGatherer;

        private List<SO_Actor> _actors;

        [MenuItem("Window/Dialogue Tree Planter/Casting Call")]
        public static void ShowMyEditor()
        {
            EditorWindow window = GetWindow<CastingCallWindow>();
            window.titleContent = new GUIContent("Casting Call");
            _actorFactory = new ConcreteActorFactory();
            _actorGatherer = new ActorGathererImpl();
        }

        public void CreateGUI()
        {
            _actors = _actorGatherer.GetActorsList();
        }

        private Button CreateNewActorButton()
        {
            Button newActorButton = new Button() { text = "New Actor"};
            newActorButton.clicked += () =>
            {
                _actorFactory.CreateActorAsset();
            };
            return newActorButton;
        }
    }
}
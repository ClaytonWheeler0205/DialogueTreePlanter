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
        TwoPaneSplitView splitView;
        private VisualElement rightPane;
        private ListView actorsListView;

        private static ActorFactory _actorFactory;
        private static ActorGatherer _actorGatherer;

        private List<SO_Actor> _actors;

        [MenuItem("Window/Dialogue Tree Planter/Casting Call")]
        public static void ShowMyEditor()
        {
            EditorWindow window = GetWindow<CastingCallWindow>();
            window.titleContent = new GUIContent("Casting Call");
        }

        private void Awake()
        {
            _actorFactory = new ConcreteActorFactory();
            _actorGatherer = new ActorGathererImpl();
            _actors = _actorGatherer.GetActorsList();
        }

        public void CreateGUI()
        {
            CreateDualPanels();
            leftPane.Add(CreateNewActorButton());
            CreateListView();
        }

        private Button CreateNewActorButton()
        {
            Button newActorButton = new Button() { text = "New Actor"};
            newActorButton.clicked += () =>
            {
                _actorFactory.CreateActorAsset();
                _actors = _actorGatherer.GetActorsList();
                actorsListView.itemsSource = _actors;
                actorsListView.RefreshItems();
            };
            return newActorButton;
        }

        private void CreateDualPanels()
        {
            splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            leftPane = new VisualElement();
            splitView.Add(leftPane);

            rightPane = new VisualElement();
            splitView.Add(rightPane);
        }

        private void CreateListView()
        {
            actorsListView = new ListView();
            leftPane.Add(actorsListView);

            actorsListView.makeItem = () => new Label();
            actorsListView.bindItem = (item, index) => { (item as Label).text = _actors[index].name; };
            actorsListView.itemsSource = _actors;
            actorsListView.onSelectionChange += OnActorSelectionChange;
        }

        private void OnActorSelectionChange(IEnumerable<object> actors)
        {
            rightPane.Clear();

            SO_Actor actor = actors.First() as SO_Actor;
            if (actor == null) { return; }

            // WIP work on actor editor in right pane of window
            Debug.Log(actor.name);
        }
    }
}
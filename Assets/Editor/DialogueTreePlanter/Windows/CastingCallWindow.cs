using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using DialogueTreePlanter.Utilities;
using DialogueTreePlanter.Actors;
using UnityEditor.UIElements;

namespace DialogueTreePlanter.Windows
{

    public class CastingCallWindow : EditorWindow
    {
        private VisualElement leftPane;
        TwoPaneSplitView splitView;
        private VisualElement rightPane;
        private ListView actorsListView;

        private ObjectField actorIconBinding;
        private TextField actorNameBinding;
        private ObjectField actorVoiceBinding;

        private ActorFactory _actorFactory;
        private ActorGatherer _actorGatherer;
        private ActorDestroyer _actorDestroyer;


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
            _actorDestroyer = new ActorDestroyerImpl();
        }

        public void CreateGUI()
        {
            CreateDualPanels();
            CreateNewActorButton();
            CreateActorEditorFields();
            CreateListView();
            
        }

        private void CreateNewActorButton()
        {
            Button newActorButton = new Button() { text = "New Actor"};
            newActorButton.clicked += () =>
            {
                _actorFactory.CreateActorAsset();
                _actors = _actorGatherer.GetActorsList();
                actorsListView.itemsSource = _actors;
                actorsListView.RefreshItems();
            };
            leftPane.Add(newActorButton);
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

        private void CreateActorEditorFields()
        {
            actorNameBinding = new TextField("Actor Name");
            actorIconBinding = new ObjectField("Actor Icon");
            actorIconBinding.objectType = typeof(Sprite);
            actorVoiceBinding = new ObjectField("Actor Voice");
            actorVoiceBinding.objectType = typeof(AudioSource);
        }

        private void OnActorSelectionChange(IEnumerable<object> actors)
        {
            ClearActorEditor();

            SO_Actor actor = actors.First() as SO_Actor;
            if (actor == null) { return; }

            SerializedObject so = new SerializedObject(actor);

            SerializedProperty propertyActorIcon = so.FindProperty("_actorIcon");
            SerializedProperty propertyActorName = so.FindProperty("_actorName");
            SerializedProperty propertyActorVoice = so.FindProperty("_actorVoice");

            actorIconBinding.BindProperty(propertyActorIcon);
            actorNameBinding.BindProperty(propertyActorName);
            actorVoiceBinding.BindProperty(propertyActorVoice);

            rightPane.Add(actorIconBinding);
            rightPane.Add(actorNameBinding);
            rightPane.Add(actorVoiceBinding);
            CreateDeleteActorButton(actor);
        }

        private void ClearActorEditor()
        {
            rightPane.Clear();
            actorIconBinding.Unbind();
            actorNameBinding.Unbind();
            actorVoiceBinding.Unbind();
        }

        private void CreateDeleteActorButton(SO_Actor actorToDelete)
        {
            Button deleteActorButton = new Button() { text = "Delete" };
            deleteActorButton.style.color = Color.red;
            deleteActorButton.clicked += () =>
            {
                ClearActorEditor();
                _actorDestroyer.DestroyActor(actorToDelete);
                _actors = _actorGatherer.GetActorsList();
                actorsListView.itemsSource = _actors;
                actorsListView.RefreshItems();
            };
            rightPane.Add(deleteActorButton);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using DialogueTreePlanter.Actors;
using UnityEngine.UIElements;

namespace DialogueTreePlanter.Windows
{

    public class ActorRenameWindow : EditorWindow
    {
        private SO_Actor _actorToRename = null;

        private TextField _assetNameTextField;

        public delegate void OnActorRename();
        public OnActorRename onActorRename;

        public static EditorWindow ShowWindow()
        {
            EditorWindow window = GetWindow<ActorRenameWindow>(true);
            window.titleContent = new GUIContent("Rename Actor Asset");
            return window;
        }

        public void CreateGUI()
        {
            _assetNameTextField = new TextField("New Name");
            _assetNameTextField.value = "";
            rootVisualElement.Add(_assetNameTextField);
            Button renameActorButton = new Button() { text = "Rename" };
            renameActorButton.clicked += () =>
            {
                if(_assetNameTextField.value.Length <= 0)
                {
                    Debug.LogWarning("New name is empty. Please give the actor asset a name.");
                    return;
                }
                // check to see if the file name already exists
                string existingFileName = AssetDatabase.AssetPathToGUID("Assets/_Game/DialogueTreePlanter/Actors/" + _assetNameTextField.value + ".asset", 
                    AssetPathToGUIDOptions.OnlyExistingAssets);
                if(existingFileName != "")
                {
                    Debug.LogWarning("File name is taken! Please give the actor asset a new name");
                    return;
                }
                Debug.Log("Renaming " + _actorToRename.name + " to " + _assetNameTextField.value);
                string assetPath = AssetDatabase.GetAssetPath(_actorToRename.GetInstanceID());
                AssetDatabase.RenameAsset(assetPath, _assetNameTextField.value);
                AssetDatabase.SaveAssets();
                onActorRename?.Invoke();
                Close();
            };
            rootVisualElement.Add(renameActorButton);
        }

        public void SetActor(SO_Actor actor)
        {
            _actorToRename = actor;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using DialogueTreePlanter.Actors;
using DialogueTreePlanter.Enumerations;

namespace DialogueTreePlanter.Elements
{
    public class DialogueNodeBase : Node
    {
        [SerializeField]
        private string _dialogueName;
        [SerializeField]
        private SO_Actor _dialogueActor;
        [SerializeField]
        private List<string> _choices;
        [SerializeField]
        private string _dialogueText;
        private DialogueType _dialogueType;

        public void Initialize()
        {
            _dialogueName = "DialogueName";
            _dialogueActor = null;
            _choices = new List<string>();
            _dialogueText = "Dialogue Text.";
        }

        public void Draw()
        {

        }
    }
}
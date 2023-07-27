using UnityEngine;

namespace DialogueTreePlanter.Actors
{

    public class SO_Actor : ScriptableObject
    {
        [SerializeField]
        private string _actorName;
        public string ActorName => _actorName;
        [SerializeField]
        private Sprite _actorIcon;
        public Sprite ActorIcon => _actorIcon;
        [SerializeField]
        private AudioSource _actorVoice;
        public AudioSource ActorVoice => _actorVoice;
    }
}
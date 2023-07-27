using DialogueTreePlanter.Actors;
using System.Collections.Generic;
using UnityEditor;

namespace DialogueTreePlanter.Utilities
{

    public class ActorGathererImpl : ActorGatherer
    {
        public override List<SO_Actor> GetActorsList()
        {
            List<SO_Actor> actors = new List<SO_Actor>();
            foreach(string asset in actorAssetFiles)
            {
                actors.Add(AssetDatabase.LoadAssetAtPath<SO_Actor>(actorsPath + asset));
            }
            return actors;
        }
    }
}
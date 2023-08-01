using DialogueTreePlanter.Actors;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;

namespace DialogueTreePlanter.Utilities
{

    public class ActorGathererImpl : ActorGatherer
    {
        public override List<SO_Actor> GetActorsList()
        {
            List<SO_Actor> actors = new List<SO_Actor>();
            GetActorAssets();
            foreach(string asset in actorAssetFiles)
            {
                actors.Add(AssetDatabase.LoadAssetAtPath<SO_Actor>(asset));
            }
            return actors;
        }
    }
}
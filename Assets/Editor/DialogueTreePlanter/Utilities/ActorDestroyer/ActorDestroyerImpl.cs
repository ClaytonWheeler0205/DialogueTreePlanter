using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueTreePlanter.Actors;
using UnityEditor;

namespace DialogueTreePlanter.Utilities
{
    public class ActorDestroyerImpl : ActorDestroyer
    {
        public override bool DestroyActor(SO_Actor actorToDestroy)
        {
            if (actorToDestroy != null)
            {
                if (FindPathFromAsset(actorToDestroy))
                {
                    return AssetDatabase.DeleteAsset(assetPath);
                }
            }
            return false;
        }
    }
}
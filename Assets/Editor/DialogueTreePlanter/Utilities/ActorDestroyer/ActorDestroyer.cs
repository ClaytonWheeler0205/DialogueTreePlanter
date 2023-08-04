using DialogueTreePlanter.Actors;
using UnityEditor;

namespace DialogueTreePlanter.Utilities
{

    public abstract class ActorDestroyer
    {
        protected string assetPath = null;
        public abstract bool DestroyActor(SO_Actor actorToDestroy);

        protected bool FindPathFromAsset(SO_Actor asset)
        {
            assetPath = AssetDatabase.GetAssetPath(asset);
            if(assetPath == null || assetPath == "")
            {
                return false;
            }
            return true;
        }
    }
}
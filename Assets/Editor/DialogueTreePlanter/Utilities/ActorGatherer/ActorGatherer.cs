using System.IO;
using System.Collections.Generic;
using DialogueTreePlanter.Actors;

namespace DialogueTreePlanter.Utilities
{

    public abstract class ActorGatherer
    {
        protected const string actorsPath = "Assets/_Game/DialogueTreePlanter/Actors/";
        protected const string fileExtension = ".asset";
        protected string[] actorAssetFiles;

        public abstract List<SO_Actor> GetActorsList();

        protected void GetActorAssets()
        {
            actorAssetFiles = Directory.GetFiles(actorsPath, "*" + fileExtension);
        }
    }
}
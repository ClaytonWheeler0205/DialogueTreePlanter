using DialogueTreePlanter.Actors;
using System.IO;

namespace DialogueTreePlanter.Utilities
{

    public abstract class ActorFactory
    {
        protected const string path = "Assets/_Game/DialogueTreePlanter/Actors/";
        protected string assetName = "NewActor";
        protected const string fileExtension = ".asset";

        public abstract SO_Actor CreateActorAsset();

        protected void SetFileName()
        {
            // Search for all the asset files in the actors directory. This should only contain actor SO's and thus hold the number of actors currently made
            int numActors = Directory.GetFiles(path, "*" + fileExtension).Length;
            assetName = "NewActor" + numActors.ToString();
        }
    }
}
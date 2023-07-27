using DialogueTreePlanter.Actors;
using DialogueTreePlanter.Utilities;
using UnityEditor;
using UnityEngine;

public class ConcreteActorFactory : ActorFactory
{
    public override SO_Actor CreateActorAsset()
    {
        SO_Actor newActor = ScriptableObject.CreateInstance<SO_Actor>();
        SetFileName();
        AssetDatabase.CreateAsset(newActor, path + assetName + fileExtension);
        Debug.Log("New actor SO created!");
        return newActor;
    }
}

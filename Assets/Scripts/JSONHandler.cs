using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/* 
 * Likely  to be deleted  later, mainly a test for JSON synchnoization, although
 * it might be used later if it can carry over cleanly enough.
 * */
public class JSONHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //serialize an object to JSON
        ActorData actorData = new ActorData();
        actorData.order = 1;
        actorData.action = "stall";
        actorData.value = 2.0f;

        string json = JsonUtility.ToJson(actorData);
        Debug.Log(json);

        File.WriteAllText(Application.dataPath + "/saveFile.json", json);

        //serialize FROM this JSON to the object we need

        string readjson = File.ReadAllText(Application.dataPath + "/saveFile.json");
        ActorData loadedActorData = JsonUtility.FromJson<ActorData>(readjson);
        //Debug.Log(loadedActorData.order);
        //Debug.Log(loadedActorData.action);
        //Debug.Log(loadedActorData.value);
    }

    private class ActorData
    {
        public int order;
        public string action;
        public float value;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataHandler : MonoBehaviour
{
    //purely for testing purposes, to be completely overhauled to suit this
    //game's mechanics later    
    //static readonly instead of const because const is declared at runtime
    private string SAVE_FOLDER;

    // Start is called before the first frame update
    void Awake()
    {
        SAVE_FOLDER = Application.dataPath + "/Saves/";

        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }
    }

    private void Save()
    {
        SaveObject saveObject = new SaveObject
        {
            score = 10,
            playerPosition = Vector3.back,
        };
        //serialize
        string json = JsonUtility.ToJson(saveObject);
        //save to json. Just run this at point in your code where saving makes sense, like a button press
        File.WriteAllText(SAVE_FOLDER + "datasave.txt", json);
    }

    private void Load()
    {
        //steps for getting save files, will be important for choosing one from a set
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles();
        FileInfo testFile = saveFiles[0];

        //de-serialize
        //SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(json);
        if (File.Exists(testFile.FullName))
        {
            string loadString = File.ReadAllText(testFile.FullName);
            SaveObject loadedSaveObject = JsonUtility.FromJson<SaveObject>(loadString);
            Debug.Log(loadedSaveObject.score);
            Debug.Log(loadedSaveObject.playerPosition);
        }
    }


    private class SaveObject
    {
        public int score;
        public Vector3 playerPosition;
    }
}

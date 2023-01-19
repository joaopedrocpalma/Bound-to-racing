using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class Manage_settings : MonoBehaviour
{
    public InputField keyW, keyS, keyA, keyD, keySpace, keyPower, KeyR;

    private FileStream settings;

    private string keyWText = KeyCode.W.ToString();
    private string keySText = KeyCode.S.ToString();
    private string KeyAText = KeyCode.A.ToString();
    private string KeyDText = KeyCode.D.ToString();    
    private string keyRText = KeyCode.R.ToString();
    private string keySpaceText = KeyCode.Space.ToString();


    public void SaveFile()
    {
        string path = Application.persistentDataPath + "/save.dat";

        if (File.Exists(path)) {
            settings = File.OpenWrite(path);
        }
        else
        {
            File.Create(path);
        }

        GameData data = new GameData(keyW.text, keyS.text, keyA.text, keyD.text, keySpace.text, keyPower.text, KeyR.text);

        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(settings, data);

        settings.Close();
    }


    public void LoadFile()
    {
        string path = Application.persistentDataPath + "/save.dat";

        if (File.Exists(path))
        {
            settings = File.OpenRead(path);
        }
        else
        {
            Debug.LogError("Data not found");
            return;
        }

        BinaryFormatter bf = new BinaryFormatter();
        GameData data = (GameData)bf.Deserialize(settings);
        settings.Close();

        //Debug.Log(data.keyWText);
    }


    [System.Serializable]
    class GameData
    {
        private string keyWText, keySText, KeyAText, KeyDText, keySpaceText, keyPowerText, keyRText;

        public GameData(string keyWT, string keyST, string KeyAT, string KeyDT, string keySpaceT, string keyPT, string keyRT)
        {
            keyWText = keyWT;
            keySText = keyST;
            KeyAText = KeyAT;
            KeyDText = KeyDT;
            keySpaceText = keySpaceT;
            keyPowerText = keyPT;
            keyRText = keyRT;
        }
    }
}



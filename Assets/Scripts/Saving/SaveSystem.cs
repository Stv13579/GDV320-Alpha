using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveFirstLoad()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData.pnu";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            bool sData = true;
            formatter.Serialize(stream, sData);
        }
    }

    public static bool LoadStartedState()
    {
        string path = Application.persistentDataPath + "/SaveData.pnu";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            bool sData;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                if (stream.Length == 0)
                {
                    sData = false;
                }
                else
                {
                    sData = (bool)formatter.Deserialize(stream);
                }
            }
            return sData;
        }
        else
        {
            Debug.Log("Save file not found " + path);
            return false;
        }
    }

    public static void SaveNPCData(NPCData dataToSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/SaveData" + dataToSave.name + ".pnu";
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            NPCSaveData sData = new NPCSaveData(dataToSave);
            formatter.Serialize(stream, sData);
        }
    }

    //Returns save data which can be applied to the npc 
    public static NPCSaveData LoadNPCData(string npcFileName)
    {
        string path = Application.persistentDataPath + "/SaveData" + npcFileName + ".pnu";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            NPCSaveData sData;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                if(stream.Length == 0)
                {
                    sData = null;
                }
                else
                {
                    sData = (NPCSaveData)formatter.Deserialize(stream);
                }
            }
            return sData;
        }
        else
        {
            Debug.Log("Save file not found " + path);
            return null;
        }
    }
}

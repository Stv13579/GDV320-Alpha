using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

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
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                sData = (NPCSaveData)formatter.Deserialize(stream);
            }
            return sData;
        }
        else
        {
            Debug.LogError("Save file not found " + path);
            return null;
        }
    }
}

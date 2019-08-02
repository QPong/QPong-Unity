using UnityEngine;
using System.IO;
using System.Collections;

public class Storage : MonoBehaviour {

    private string playerData = "player.json";


    // Static singleton Instance
    private static Storage instance;

    // Static singleton instance
    public static Storage Instance
    {
        get { return instance ?? (instance = new GameObject("Storage").AddComponent<Storage>()); }
    }

    public Player LoadPlayerData() {
        Player player = new Player();
        string filePath = Path.Combine(Application.persistentDataPath, playerData);
        string data;

        if (filePath.Contains("://"))
        {
            WWW file = new WWW(filePath);
            while (!file.isDone) { }
            data = file.text;
        }
        else
        {
            if (File.Exists(filePath))
            {
                data = File.ReadAllText(filePath);
            }
            else
            {
                return player;
            }
        }
        player = JsonUtility.FromJson<Player>(data);
        return player;
    }

    public void SavePlayerData(Player player) {
        string filePath = Path.Combine(Application.persistentDataPath, playerData);
        string data = JsonUtility.ToJson(player);

        if (filePath.Contains("://")) {
            WWW www = new WWW(filePath);
            File.WriteAllText(filePath, www.text);
        } else {
            File.WriteAllText(filePath, data);
        }
    }
}

using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private string SavePath => Application.persistentDataPath + "/save.json";

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveLevel(string levelName)
    {
        SaveData data = new SaveData();
        data.currentLevel = levelName;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("Saved NEXT level (door): " + levelName);
    }

    public void SaveGame()
    {
        string currentLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        SaveData data = new SaveData();
        data.currentLevel = currentLevel;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);

        Debug.Log("Saved CURRENT level (editor): " + currentLevel);
    }

    public void ReloadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found!");
            return;
        }

        string json = File.ReadAllText(SavePath);
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Loading Level: " + data.currentLevel);

        UnityEngine.SceneManagement.SceneManager.LoadScene(data.currentLevel);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    public void Load()
    {
        SceneManager.LoadScene(levelName);
    }
}
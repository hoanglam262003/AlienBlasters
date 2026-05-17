using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    public void Load()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
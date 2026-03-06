using UnityEngine;

public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameUI;

    private bool isPaused;

    private void Update()
    {
        if (GameInput.Instance.IsPausePressed())
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pauseGameUI.SetActive(true);
        isPaused = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        pauseGameUI.SetActive(false);
        isPaused = false;
    }
}
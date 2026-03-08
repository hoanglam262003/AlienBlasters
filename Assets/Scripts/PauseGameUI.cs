using UnityEngine;
using UnityEngine.UI;

public class PauseGameUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button resumeButton;

    private bool isPaused;

    private void Awake()
    {
        settingsButton.onClick.AddListener(OpenSettings);
        resumeButton.onClick.AddListener(ResumeGame);
    }

    private void Update()
    {
        if (settingsUI.gameObject.activeSelf)
            return;
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

    public void OpenSettings()
    {
        pauseGameUI.SetActive(false);
        settingsUI.Show();
    }

    public void ShowPauseMenu()
    {
        pauseGameUI.SetActive(true);
    }
}
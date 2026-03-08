using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private GameObject pauseGameUI;

    private void Awake()
    {
        backButton.onClick.AddListener(Back);
    }

    private void Update()
    {
        if (GameInput.Instance.IsPausePressed())
        {
            Back();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Back()
    {
        gameObject.SetActive(false);
        pauseGameUI.SetActive(true);
    }
}
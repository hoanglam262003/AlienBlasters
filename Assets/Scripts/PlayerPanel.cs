using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;
    [SerializeField]
    private Image[] hearts; 

    private Player player;
    private void Start()
    {
        BindPlayer();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindPlayer();
    }

    private void BindPlayer()
    {
        if (player != null)
        {
            player.OnCoinsChanged -= UpdateScore;
            player.OnHealthChanged -= UpdateHearts;
        }

        player = PlayerRegistry.Instance.GetPlayer();

        if (player == null)
            return;

        player.OnCoinsChanged += UpdateScore;
        player.OnHealthChanged += UpdateHearts;

        UpdateScore(player.coinsCollected);
        UpdateHearts(player.health);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnCoinsChanged -= UpdateScore;
            player.OnHealthChanged -= UpdateHearts;
        }
    }

    private void UpdateScore(int coins)
    {
        score.SetText(coins.ToString());
    }

    private void UpdateHearts(int health)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }
}

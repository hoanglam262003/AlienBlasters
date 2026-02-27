using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text score;

    private Player player;
    private void Start()
    {
        player = PlayerRegistry.Instance.GetPlayer();

        if (player == null)
        {
            return;
        }

        player.OnCoinsChanged += UpdateScore;
        UpdateScore(player.coinsCollected);
    }

    private void OnDestroy()
    {
        if (player != null)
            player.OnCoinsChanged -= UpdateScore;
    }

    private void UpdateScore(int coins)
    {
        score.SetText(coins.ToString());
    }
}

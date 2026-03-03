using UnityEngine;

public class PlayerRegistry : MonoBehaviour
{
    public static PlayerRegistry Instance { get; private set; }

    private Player currentPlayer;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        GameObject obj = new GameObject("PlayerRegistry");
        obj.AddComponent<PlayerRegistry>();
    }

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

    public void Register(Player player)
    {
        currentPlayer = player;
    }

    public Player GetPlayer()
    {
        return currentPlayer;
    }

    public void Clear()
    {
        currentPlayer = null;
    }
}
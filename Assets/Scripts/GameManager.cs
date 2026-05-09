using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool IsCinematicPlaying { get; private set; }
    public void ToggleCinematic(bool cinematicPlaying)
    {
        IsCinematicPlaying = cinematicPlaying;
    }
}

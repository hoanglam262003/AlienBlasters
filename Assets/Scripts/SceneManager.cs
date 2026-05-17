using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static bool IsCinematicPlaying { get; private set; }
    public void ToggleCinematic(bool cinematicPlaying)
    {
        IsCinematicPlaying = cinematicPlaying;
    }
}

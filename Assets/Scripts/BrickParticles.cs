using UnityEngine;

public class BrickParticles : MonoBehaviour
{
    private void Start()
    {
        var particleSystem = GetComponent<ParticleSystem>();
        Destroy(gameObject, particleSystem.main.duration);
    }
}

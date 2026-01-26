using Unity.Cinemachine;
using UnityEngine;

public class CameraFallCatchUp : MonoBehaviour
{
    [SerializeField] private CinemachinePositionComposer composer;
    [SerializeField] private Rigidbody2D playerRb;

    [Header("Tuning")]
    [SerializeField] private float fallVelocityThreshold = -5f;
    [SerializeField] private float normalDeadZoneY = 0.2f;
    [SerializeField] private float normalDampingY = 3f;
    [SerializeField] private float fallDampingY = 0f;

    private void LateUpdate()
    {
        var deadZone = composer.Composition.DeadZone;
        if (playerRb.linearVelocity.y < fallVelocityThreshold)
        {
            deadZone.Size.y = 0.2f;
            composer.Damping.y = fallDampingY;
        }
        else
        {
            deadZone.Size.y = normalDeadZoneY;
            composer.Damping.y = normalDampingY;
        }
        composer.Composition.DeadZone = deadZone;
    }
}

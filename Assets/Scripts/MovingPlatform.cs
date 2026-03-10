using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Vector3 positionStart;

    [SerializeField] private Vector3 positionEnd;
    [SerializeField] private float speed = 0.5f;

    private void Start()
    {
        positionStart = transform.localPosition;
    }

    private void Update()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.localPosition = Vector3.Lerp(positionStart, positionEnd, t);
    }
}
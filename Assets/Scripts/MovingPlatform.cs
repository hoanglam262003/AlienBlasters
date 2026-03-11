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

    private void LateUpdate()
    {
        float t = Mathf.PingPong(Time.time * speed, 1f);
        transform.localPosition = Vector3.Lerp(positionStart, positionEnd, t);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    player.transform.SetParent(transform);
                    player.SetOnMovingPlatform(true);
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null && collision.transform.parent == transform && gameObject.activeInHierarchy)
        {
            player.transform.SetParent(null);
            player.SetOnMovingPlatform(false);
        }
    }
}
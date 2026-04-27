using Assets.Scripts.Interfaces;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction = Vector2.right;
    [SerializeField]
    private float distance = 10f;
    [SerializeField]
    private SpriteRenderer laserBurst;
    [SerializeField]
    private LineRenderer lineRenderer;

    private bool isOn;

    private void Awake()
    {
        SetLaserState(false);
    }

    private void OnValidate()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, transform.position);
        var endPoint = (Vector2)transform.position + direction.normalized * distance;
        var hit = Physics2D.Raycast(transform.position, direction, distance);
        if (hit.collider)
        {
            endPoint = hit.point;
        }
        lineRenderer.SetPosition(1, endPoint);
        laserBurst.transform.position = endPoint;
    }

    private void Update()
    {
        if (isOn == false)
        {
            laserBurst.enabled = false;
            return;
        }
        var endPoint = (Vector2)transform.position + direction.normalized * distance;
        var hit = Physics2D.Raycast(transform.position, direction, distance);
        if (hit.collider) 
        {
            endPoint = hit.point;
            laserBurst.transform.position = endPoint;
            laserBurst.enabled = true;
            laserBurst.transform.localScale = Vector3.one * (0.5f + Mathf.PingPong(Time.time, 1f));
            var takeLaserDamage = hit.collider.GetComponent<ITakeLaserDamage>();
            if (takeLaserDamage != null)
            {
                takeLaserDamage.TakeLaserDamage();
            }
        }
        else
        {
            laserBurst.enabled = false;
        }
        lineRenderer.SetPosition(1, endPoint);
    }

    public void SetLaserState(bool state)
    {
        isOn = state;
        lineRenderer.enabled = state;
    }
}

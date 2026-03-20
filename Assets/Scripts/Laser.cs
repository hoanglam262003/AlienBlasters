using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction = Vector2.right;
    [SerializeField]
    private float distance = 10f;
    private LineRenderer lineRenderer;
    private bool isOn;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLaserState(false);
    }

    private void Update()
    {
        if (isOn == false) return;

        var endPoint = (Vector2)transform.position + direction.normalized * distance;
        var hit = Physics2D.Raycast(transform.position, direction, distance);
        if (hit.collider) 
        {
            endPoint = hit.point;
        }
        lineRenderer.SetPosition(1, endPoint);
    }

    public void SetLaserState(bool state)
    {
        isOn = state;
        lineRenderer.enabled = state;
    }
}

using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private bool isOn;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        SetLaserState(false);
    }

    public void SetLaserState(bool state)
    {
        isOn = state;
        lineRenderer.enabled = state;
    }
}

using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class KeyReviewController : MonoBehaviour
{
    [SerializeField] private Transform focusPoint;
    [SerializeField] private Transform[] keys;
    [SerializeField] private float moveDuration = 1f;
    [SerializeField] private float reviewTimePerKey = 5f;
    [SerializeField] private CinemachineCamera keyCamera;

    private bool isReviewing = false;

    public void StartReview()
    {
        Debug.Log("StartReview CALLED");
        if (isReviewing) return;
        isReviewing = true;
        keyCamera.Priority = 1;
        StopAllCoroutines();
        StartCoroutine(ReviewSequence());
    }

    private IEnumerator ReviewSequence()
    {
        Debug.Log("Total keys: " + keys.Length);
        foreach (var key in keys)
        {
            if (key == null)
            {
                Debug.LogError("Key is NULL!");
                continue;
            }
            Debug.Log("Moving to: " + key.name + " | Pos: " + key.position);
            yield return MoveTo(key.position);
            float stayTime = reviewTimePerKey - moveDuration;
            if (stayTime > 0)
                yield return new WaitForSecondsRealtime(stayTime);
        }
        keyCamera.Priority = -1;
        isReviewing = false;
    }

    private IEnumerator MoveTo(Vector3 target)
    {
        Vector3 start = focusPoint.position;
        float time = 0;

        while (time < moveDuration)
        {
            focusPoint.position = Vector3.Lerp(start, target, time / moveDuration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        focusPoint.position = target;
    }
}

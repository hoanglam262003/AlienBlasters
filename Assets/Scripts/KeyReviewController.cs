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
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip reviewKeySfx;

    private bool isReviewing = false;

    public void StartReview()
    {
        if (isReviewing) return;
        isReviewing = true;
        sceneManager.ToggleCinematic(true);
        keyCamera.Priority = 1;
        StopAllCoroutines();
        StartCoroutine(ReviewSequence());
    }

    private IEnumerator ReviewSequence()
    {
        foreach (var key in keys)
        {
            if (key == null)
            {
                continue;
            }
            yield return MoveTo(key.position);
            if (audioSource != null && reviewKeySfx != null)
            {
                audioSource.PlayOneShot(reviewKeySfx);
            }
            float stayTime = reviewTimePerKey - moveDuration;
            if (stayTime > 0)
                yield return new WaitForSecondsRealtime(stayTime);
        }
        keyCamera.Priority = -1;
        sceneManager.ToggleCinematic(false);
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

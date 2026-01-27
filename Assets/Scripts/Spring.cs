using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite sprung;
    private SpriteRenderer spriteRenderer;
    private Coroutine resetCoroutine;
    private AudioSource audioSource;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        defaultSprite = spriteRenderer.sprite;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            spriteRenderer.sprite = sprung;
            audioSource.Play();
        }
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() == null) return;
        resetCoroutine = StartCoroutine(ResetSpriteAfterDelay());
    }

    private IEnumerator ResetSpriteAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        spriteRenderer.sprite = defaultSprite;
        resetCoroutine = null;
    }
}

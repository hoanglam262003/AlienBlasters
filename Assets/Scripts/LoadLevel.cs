using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [SerializeField]
    private string levelName;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Player>() != null)
        {
            GameManager.Instance.SaveLevel(levelName);
            UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
        }
    }
}

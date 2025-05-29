using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;

public class TransitionManager : MonoBehaviour {
    [SerializeField] 
    public GameObject player;
    public Image fadeImage;

    public string nextRoom;

    public float xMax;
    public float xMin;

    public float yMax;
    public float yMin;

    private const float fadeDuration = 0.3f;

    public void Update() {
        if (player == null) return;

        Vector2 playerPosition = player.transform.position;

        if (playerPosition.x < xMax && playerPosition.x > xMin &&
            playerPosition.y < yMax && playerPosition.y > yMin)
            StartCoroutine(TransitionToNextRoom());
    }

    private IEnumerator TransitionToNextRoom() {
        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 0;

        fadeImage.color = color;

        float elapsed = 0f;

        while (elapsed < fadeDuration) {
            elapsed += Time.deltaTime;

            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene(nextRoom);
    }

    private void debugUtil(Vector2 playerPosition) {
        // Requires Update method..
        Debug.Log("Player x position: " + playerPosition.x);
        Debug.Log("Player y position: " + playerPosition.y);

        Debug.Log("X range: " + xMin + " -> " + xMax);
        Debug.Log("X range: " + yMin + " -> " + yMax);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.Collections;

public class TransitionManager : MonoBehaviour {
    [SerializeField] 
    public GameObject player;
    public Image fadeImage;

    public string nextRoom;
    public string currRoom;

    public Region activationRegion;

    private const float fadeDuration = 0.3f;

    public void Start() {
        if (GameManager.instance.wentPrevious) {
            GameManager.instance.wentPrevious = false;

            player.transform.position = GameManager.instance.previousPositions[currRoom];
        }
    }

    public void Update() {
        if (player == null || GameManager.instance.isBattlePlaying || GameManager.instance.isDialoguePlaying) return;

        Vector2 playerPosition = player.transform.position;

        if (activationRegion.Contains(player.transform.position.x, player.transform.position.y)) {
            StartCoroutine(TransitionToNextRoom());
        }

        GameManager.instance.previousPositions[currRoom] = playerPosition;
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
    }
}
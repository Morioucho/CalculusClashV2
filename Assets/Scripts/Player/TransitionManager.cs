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

    private const float KFadeDuration = 0.3f;

    private void Start() {
        if (!GameManager.instance.wentPrevious) {
            return;
        }

        GameManager.instance.wentPrevious = false;

        player.transform.position = GameManager.instance.previousPositions[currRoom];
    }

    private void Update() {
        if (player == null || GameManager.instance.isBattlePlaying || GameManager.instance.isDialoguePlaying) return;

        Vector2 playerPosition = player.transform.position;

        if (activationRegion.Contains(player.transform.position.x, player.transform.position.y)) {
            StartCoroutine(TransitionToNextRoom());
        }

        GameManager.instance.previousPositions[currRoom] = playerPosition;
    }

    private IEnumerator TransitionToNextRoom() {
        fadeImage.gameObject.SetActive(true);

        var color = fadeImage.color;
        color.a = 0;

        fadeImage.color = color;

        var elapsed = 0f;

        while (elapsed < KFadeDuration) {
            elapsed += Time.deltaTime;

            var alpha = Mathf.Clamp01(elapsed / KFadeDuration);

            color.a = alpha;
            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene(nextRoom);
    }
}
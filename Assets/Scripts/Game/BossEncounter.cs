using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossEncounter : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    public GameObject transitionPanel;
    public GameObject bossObject;

    public Region activationRegion;

    public string bossId;
    public string currentRoom;

    void Update() {
        if (!GameManager.instance.isBattleHandled) {
            player.transform.position = new Vector2(
                GameManager.instance.roomPositions[currentRoom].X,
                GameManager.instance.roomPositions[currentRoom].Y
            );

            GameManager.instance.isBattleHandled = true;
        }

        if (!activationRegion.Contains(player.transform.position.x, player.transform.position.y)
            || GameManager.instance.isDialoguePlaying || GameManager.instance.isBattlePlaying)
            return;

        if(GameManager.instance.randomAccess.TryAdd(bossId, "completed")) {
            TriggerEncounter();
        } else {
            bossObject.SetActive(false);
        }
    }

    private void TriggerEncounter() {
        var updatedPosition = new Position(player.transform.position.x, player.transform.position.y);
        GameManager.instance.roomPositions[currentRoom] = updatedPosition;

        GameManager.instance.encounterEnemyId = bossId;
        GameManager.instance.previousScene = currentRoom;

        GameManager.instance.isBattlePlaying = true;
        StartCoroutine(FadeAndLoadFight());
    }

    private IEnumerator FadeAndLoadFight() {
        AudioClip alertClip = Resources.Load<AudioClip>("SFX/alert");

        if (alertClip != null) {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.clip = alertClip;
            audioSource.Play();

            yield return new WaitForSeconds(alertClip.length);

            Destroy(audioSource);
        }

        if (transitionPanel != null) {
            var panelImage = transitionPanel.GetComponent<Image>();
            if (panelImage != null) {
                transitionPanel.SetActive(true);

                Color color = panelImage.color;
                float duration = 0.3f;
                float elapsed = 0f;

                color.a = 0f;
                panelImage.color = color;

                while (elapsed < duration) {
                    elapsed += Time.deltaTime;
                    color.a = Mathf.Clamp01(elapsed / duration);
                    panelImage.color = color;
                    yield return null;
                }

                color.a = 1f;
                panelImage.color = color;
            }
        }

        SceneManager.LoadScene("Fight");
    }
}
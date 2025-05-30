using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Encounter : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    public GameObject transitionPanel;
    public Region activationRegion;

    public string[] enemyIds;
    public string currentRoom;
    public float encounterPercentagePerSecond;

    private float timer = 0f;

    void Start() {
        int seed = System.DateTime.Now.Ticks.GetHashCode();
        Random.InitState(seed);
    }

    void Update() {
        if (!GameManager.instance.battleHandled) {
            player.transform.position = new Vector2(
                    GameManager.instance.roomPositions[currentRoom].x,
                    GameManager.instance.roomPositions[currentRoom].y
                );

            GameManager.instance.battleHandled = true;
        }

        if (!activationRegion.Contains(player.transform.position.x, player.transform.position.y)
            || GameManager.instance.isDialoguePlaying || GameManager.instance.isBattlePlaying)
            return;

        timer += Time.deltaTime;

        if (timer >= 1f) {
            timer = 0f;

            int roll = Random.Range(1, 101);

            if (roll < encounterPercentagePerSecond) {
                TriggerEncounter();
            }
        }
    }

    private void TriggerEncounter() {
        GameManager.instance.encounterEnemyID = enemyIds[Random.Range(0, enemyIds.Length)];
        GameManager.instance.previousScene = currentRoom;

        GameManager.instance.roomPositions[currentRoom].x = player.transform.position.x;
        GameManager.instance.roomPositions[currentRoom].y = player.transform.position.y;

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
            Image panelImage = transitionPanel.GetComponent<Image>();
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

        GameManager.instance.battleHandled = false;
        SceneManager.LoadScene("Fight");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class Encounter : MonoBehaviour {
    [SerializeField]
    public GameObject player;
    public string[] enemyIds;
    public string currentRoom;
    public float encounterPercentagePerSecond;

    private float timer = 0f;

    void Awake() {
        if (GameManager.instance.roomPositions[currentRoom].x != 0
            && GameManager.instance.roomPositions[currentRoom].y != 0)
        {
            player.transform.position = new Vector3(
                GameManager.instance.roomPositions[currentRoom].x,
                GameManager.instance.roomPositions[currentRoom].y,

                player.transform.position.z
            );
        }
    }

    void Update() {
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

        SceneManager.LoadScene("Fight");
    }
}
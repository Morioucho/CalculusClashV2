using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {
    public Image enemyImage;
    public Text dialogueText;

    public int playerHP = 100;

    private EnemyData currEnemy;
    private int currEnemyHP;
    private int dialogueIndex = 0;

    void Start() {
        string enemyId = GameManager.GetInstance().encounterEnemyID;
        currEnemy = EnemyLoader.LoadEnemy(enemyId);

        if (currEnemy != null) {
            currEnemyHP = currEnemy.health;
            Sprite sprite = Resources.Load<Sprite>("EnemySprites/" + Path.GetFileNameWithoutExtension(currEnemy.sprite));
            enemyImage.sprite = sprite;

            NextDialogue();
        }
    }

    public void NextDialogue() {
        if (dialogueIndex < currEnemy.dialogue.Length) {
            dialogueText.text = currEnemy.dialogue[dialogueIndex++];
        } else {
            StartCombat();
        }
    }

    void StartCombat() {
        Debug.Log("Combat begins with: " + currEnemy.name);
        currEnemyHP -= 10;

        if (currEnemyHP <= 0) {
            EndCombat();
        } else {
            playerHP -= currEnemy.attack;
            if (playerHP <= 0) {
                Debug.Log("Player Defeated");
                // Handle Game Over
            } else {
                NextDialogue();
            }
        }
    }
    void EndCombat() {
        Debug.Log("Enemy Defeated");
        SceneManager.LoadScene(GameManager.GetInstance().previousScene);
    }
}
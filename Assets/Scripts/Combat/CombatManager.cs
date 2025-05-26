using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using System.IO;
using System.Collections;

public class CombatManager : MonoBehaviour {
    [SerializeField]
    public Image enemyImage;

    [SerializeField]
    public TextMeshProUGUI dialogueText;

    public int playerLives = 3;

    private EnemyData currEnemy;
    private AudioSource musicSource;

    private int currEnemyHP;
    private int dialogueIndex = 0;
    private bool inCombat = false;

    void Start() {
        // Load the enemy.
        // string enemyId = GameManager.GetInstance().encounterEnemyID;
        currEnemy = EnemyLoader.LoadEnemy("dummy");

        // Load the sprite.
        if (currEnemy != null) {
            currEnemyHP = currEnemy.health;
            Sprite sprite = Resources.Load<Sprite>("EnemySprites/" + Path.GetFileNameWithoutExtension(currEnemy.sprite));
            enemyImage.sprite = sprite;
        }

        // Load the audio.
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = currEnemy.musicVolume;

        AudioClip bossTheme = Resources.Load<AudioClip>("Audio/EnemyMusic/" + currEnemy.id);

        if (bossTheme != null) {
            musicSource.clip = bossTheme;
            musicSource.Play();
        } else {
            Debug.LogError("Boss music not found!");
        }

        // Start the dialogue.
        NextDialogue();
    }

    public void NextDialogue() {
        dialogueIndex = 0;
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue() {
        while (dialogueIndex < currEnemy.dialogue.Length) {
            dialogueText.text = currEnemy.dialogue[dialogueIndex].text;
            float waitTime = currEnemy.dialogue[dialogueIndex].duration;

            dialogueIndex++;
            yield return new WaitForSeconds(waitTime);
        }

        StartCombat();
    }

    void StartCombat() {
        while (inCombat) {
            currEnemyHP -= 10;

            if (currEnemyHP <= 0) {
                EndCombat();
                inCombat = false;
            } else {
                playerLives -= 1;

                if (playerLives <= 0) {
                    Debug.Log("Player Defeated");
                    // Handle Game Over
                } else {
                    NextDialogue();
                }
            }
        }

        Debug.Log("Combat begins with: " + currEnemy.name);
    }
    void EndCombat() {
        Debug.Log("Enemy Defeated");
        SceneManager.LoadScene(GameManager.GetInstance().previousScene);
    }
}
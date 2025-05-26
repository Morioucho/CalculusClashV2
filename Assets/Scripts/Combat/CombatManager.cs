using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CombatManager : MonoBehaviour {
    [SerializeField]
    public Image enemyImage;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI playerLivesText;

    public List<GameObject> buttons;

    public int playerLives = 3;

    private int buttonIndex = 0;

    private EnemyData currEnemy;
    private AudioSource musicSource;

    private int currEnemyHP;
    private int dialogueIndex = 0;
    private bool inCombat = false;

    private List<Sprite> selectedSprites;
    private List<Sprite> unselectedSprites;


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

        // Load button sprites
        string[] spriteNames = { "Solve", "Help", "Item", "Skip" };
        selectedSprites = new List<Sprite>();
        unselectedSprites = new List<Sprite>();

        for (int i = 0; i < spriteNames.Length; ++i) {
            Sprite selected = Resources.Load<Sprite>("UI/Combat/" + spriteNames[i] + "Selected");
            Sprite unselected = Resources.Load<Sprite>("UI/Combat/" + spriteNames[i]);

            if (selected == null || unselected == null) {
                Debug.LogError("Missing sprite: " + spriteNames[i]);
            } else {
                selectedSprites.Add(selected);
                unselectedSprites.Add(unselected);
            }
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

    public void Update() {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if(this.buttonIndex < 3) {
                this.buttonIndex++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if(this.buttonIndex > 0) {
                this.buttonIndex--;
            }
        }

        for (int i = 0; i < buttons.Count; ++i) {
            Image img = buttons[i].GetComponent<Image>();
            img.sprite = (i == buttonIndex) ? selectedSprites[i] : unselectedSprites[i];
        }
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
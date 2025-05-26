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
    public Image questionImage;

    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI playerLivesText;

    public GameObject textbox;
    public GameObject questionBox;
    public GameObject questionPanel;

    public List<GameObject> buttons;
    public List<GameObject> questionButtons;

    public int playerLives = 3;

    private int buttonIndex = 0;
    private int questionButtonIndex = 0;

    private EnemyData currEnemy;
    private AudioSource musicSource;

    private int currEnemyHP;
    private int dialogueIndex = 0;

    private bool isButtonEnabled = false;
    private bool isQuestionEnabled = false;

    private List<Sprite> selectedSprites;
    private List<Sprite> unselectedSprites;

    private List<Sprite> questionSelectedSprites;
    private List<Sprite> questionUnselectedSprites;

    private bool optionLocked = false;

    private enum CombatState { Dialogue, PlayerChoice, Question, EnemyTurn, End }
    private CombatState state = CombatState.Dialogue;

    private QuestionData currentQuestion;
    private int correctAnswerIndex;

    void Start() {
        currEnemy = EnemyLoader.LoadEnemy("dummy");

        if (currEnemy != null) {
            currEnemyHP = currEnemy.health;
            Sprite sprite = Resources.Load<Sprite>("EnemySprites/" + Path.GetFileNameWithoutExtension(currEnemy.sprite));
            enemyImage.sprite = sprite;
        }

        // Hide the question panel by default
        if (questionPanel != null)
            questionPanel.SetActive(false);

        // Load button sprites
        string[] spriteNames = { "Solve", "Help", "Item", "Skip" };
        selectedSprites = new List<Sprite>();
        unselectedSprites = new List<Sprite>();

        for (int i = 0; i < spriteNames.Length; ++i) {
            Sprite selected = Resources.Load<Sprite>("UI/Combat/" + spriteNames[i] + "Selected");
            Sprite unselected = Resources.Load<Sprite>("UI/Combat/" + spriteNames[i]);
            selectedSprites.Add(selected);
            unselectedSprites.Add(unselected);
        }

        // Load question sprites
        string[] questionSpriteNames = { "A", "B", "C", "D" };
        questionSelectedSprites = new List<Sprite>();
        questionUnselectedSprites = new List<Sprite>();

        for (int i = 0; i < questionSpriteNames.Length; ++i) {
            Sprite selected = Resources.Load<Sprite>("UI/Combat/" + questionSpriteNames[i] + "Selected");
            Sprite unselected = Resources.Load<Sprite>("UI/Combat/" + questionSpriteNames[i]);
            questionSelectedSprites.Add(selected);
            questionUnselectedSprites.Add(unselected);
        }

        // Load the audio.
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = currEnemy.musicVolume;

        AudioClip bossTheme = Resources.Load<AudioClip>("Audio/EnemyMusic/" + currEnemy.id);

        if (bossTheme != null) {
            musicSource.clip = bossTheme;
            musicSource.Play();
        }

        // Start the dialogue.
        NextDialogue();
    }

    void Update() {
        switch (state) {
            case CombatState.PlayerChoice:
                HandlePlayerChoiceInput();
                break;
            case CombatState.Question:
                HandleQuestionInput();
                break;
        }
    }

    public void NextDialogue() {
        dialogueIndex = 0;
        state = CombatState.Dialogue;
        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue() {
        textbox.SetActive(true);
        questionBox.SetActive(false);
        if (questionPanel != null)
            questionPanel.SetActive(false);
        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);

        while (dialogueIndex < currEnemy.dialogue.Length) {
            dialogueText.text = currEnemy.dialogue[dialogueIndex].text;
            float waitTime = currEnemy.dialogue[dialogueIndex].duration;
            dialogueIndex++;
            yield return new WaitForSeconds(waitTime);
        }
        state = CombatState.PlayerChoice;

        EnablePlayerChoice();
    }

    void EnablePlayerChoice() {
        isButtonEnabled = true;
        buttonIndex = 0;

        UpdateButtonSprites();

        textbox.SetActive(true);
        questionBox.SetActive(false);
        if (questionPanel != null)
            questionPanel.SetActive(false);
        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);
    }

    void HandlePlayerChoiceInput() {
        if (!isButtonEnabled) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if (buttonIndex < buttons.Count - 1) buttonIndex++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if (buttonIndex > 0) buttonIndex--;
        }
        UpdateButtonSprites();

        if (Input.GetKeyDown(KeyCode.Return)) {
            switch (buttonIndex) {
                case 0: // Solve
                    StartQuestion();
                    break;
                case 1: // Help
                    break;
                case 2: // Item
                    break;
                case 3: // Skip
                    break;
            }
        }
    }

    void UpdateButtonSprites() {
        for (int i = 0; i < buttons.Count; ++i) {
            Image img = buttons[i].GetComponent<Image>();
            img.sprite = (i == buttonIndex) ? selectedSprites[i] : unselectedSprites[i];
        }
    }

    void StartQuestion() {
        isButtonEnabled = false;
        isQuestionEnabled = true;
        state = CombatState.Question;

        Sprite questionSprite;
        currentQuestion = QuestionLoader.GetRandomQuestion(currEnemy.unit, out questionSprite);
        correctAnswerIndex = currentQuestion.correct;

        textbox.SetActive(false);
        questionBox.SetActive(true);
        questionImage.sprite = questionSprite;

        questionImage.preserveAspect = true;
        if (questionPanel != null && questionImage != null) {
            RectTransform panelRect = questionPanel.GetComponent<RectTransform>();
            RectTransform imageRect = questionImage.rectTransform;
            if (panelRect != null && imageRect != null) {
                imageRect.sizeDelta = panelRect.rect.size;
            }
        }

        if (questionPanel != null)
            questionPanel.SetActive(true);
        if (enemyImage != null)
            enemyImage.gameObject.SetActive(false);

        questionButtonIndex = 0;
        UpdateQuestionButtonSprites();
    }

    void HandleQuestionInput() {
        if (!isQuestionEnabled) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if (questionButtonIndex < questionButtons.Count - 1) questionButtonIndex++;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if (questionButtonIndex > 0) questionButtonIndex--;
        }
        UpdateQuestionButtonSprites();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
            isQuestionEnabled = false;

            bool correct = (questionButtonIndex == correctAnswerIndex);

            if (correct) {
                currEnemyHP -= 10;
                if (currEnemyHP <= 0) {
                    if (questionPanel != null)
                        questionPanel.SetActive(false);

                    if (enemyImage != null)
                        enemyImage.gameObject.SetActive(true);

                    EndCombat();
                    return;
                }
            } else {
                playerLives -= 1;
                playerLivesText.text = playerLives.ToString();

                if (playerLives <= 0) {
                    if (questionPanel != null)
                        questionPanel.SetActive(false);

                    if (enemyImage != null)
                        enemyImage.gameObject.SetActive(true);

                    GameOver();
                    return;
                }
            }


            if (questionPanel != null)
                questionPanel.SetActive(false);

            if (enemyImage != null)
                enemyImage.gameObject.SetActive(true);

            state = CombatState.PlayerChoice;
            EnablePlayerChoice();
        }
    }

    void UpdateQuestionButtonSprites() {
        for (int i = 0; i < questionButtons.Count; ++i) {
            Image img = questionButtons[i].GetComponent<Image>();
            img.sprite = (i == questionButtonIndex) ? questionSelectedSprites[i] : questionUnselectedSprites[i];
        }
    }

    void EnemyAttack() {
        playerLives -= 1;
        playerLivesText.text = "Lives: " + playerLives;

        if (playerLives <= 0) {
            GameOver();
            return;
        }

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }

    void EndCombat() {
        state = CombatState.End;
        Debug.Log("Enemy Defeated");
        SceneManager.LoadScene(GameManager.GetInstance().previousScene);
    }

    void GameOver() {
        state = CombatState.End;
        Debug.Log("Player Defeated");
    }
}

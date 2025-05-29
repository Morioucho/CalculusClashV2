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
    public TextMeshProUGUI damageText;

    public GameObject textbox;
    public GameObject questionBox;
    public GameObject questionPanel;
    public GameObject itemPanel;
    public GameObject starText;

    public List<GameObject> buttons;
    public List<GameObject> questionButtons;
    public List<TextMeshProUGUI> itemOptions;

    public int playerLives = 3;

    private int buttonIndex = 0;
    private int itemIndex = 0;
    private int questionButtonIndex = 0;
    private int dialogueIndex = 0;

    private EnemyData currEnemy;

    private AudioSource musicSource;
    private AudioSource sfxSource;
    private AudioSource buttonSource;

    private int currEnemyHp;

    private bool isButtonEnabled = false;
    private bool isQuestionEnabled = false;

    private bool skipTypewriter = false;
    private bool waitingForEnter = false;

    private Dictionary<string, bool> actionLocks = new Dictionary<string, bool>();

    private List<Sprite> menuSelectedSprites;
    private List<Sprite> menuUnselectedSprites;

    private List<Sprite> questionSelectedSprites;
    private List<Sprite> questionUnselectedSprites;

    private ItemList items;
    private readonly List<ItemData> displayableItems = new List<ItemData>();

    private readonly Color defaultColor = Color.white;
    private readonly Color selectedColor = new Color((123f / 255f), (163f / 255f), (217f / 255f));

    private readonly float typewriterSpeed = 0.08f;

    private enum CombatState { Dialogue, PlayerChoice, Question, EnemyTurn, ShowTip, ShowItems, End }
    private CombatState state = CombatState.Dialogue;

    private QuestionData currentQuestion;
    private int correctAnswerIndex;

    public void Start() {
        // Todo: Implement proper enemy loading via GameManager.
        currEnemy = EnemyLoader.LoadEnemy(GameManager.instance.encounterEnemyID);

        if (currEnemy != null) {
            currEnemyHp = currEnemy.health;

            var sprite = Resources.Load<Sprite>("EnemySprites/" + Path.GetFileNameWithoutExtension(currEnemy.sprite));

            enemyImage.sprite = sprite;
            enemyImage.preserveAspect = true;

            var imageRect = enemyImage.rectTransform;
            var targetHeight = imageRect.rect.height;

            if (sprite != null) {
                var aspect = sprite.rect.width / sprite.rect.height;
                imageRect.sizeDelta = new Vector2(targetHeight * aspect, targetHeight);
            }
        }

        // Load locks:
        actionLocks["main"] = true;
        actionLocks["menu"] = true;
        actionLocks["question"] = true;
        actionLocks["item"] = true;

        // Hide the question panel by default
        if (questionPanel != null)
            questionPanel.SetActive(false);

        // Load items
        this.items = ItemLoader.GetAllItems();
        Debug.Log($"Loaded items: {this.items?.items?.Count ?? -1}");

        // Populate displayableItems with all items
        displayableItems.Clear();
        if (items != null && items.items != null) {
            foreach (var item in items.items) {
                displayableItems.Add(item);
            }
        }

        // Load button sprites
        string[] spriteNames = { "Solve", "Help", "Item", "Skip" };
        menuSelectedSprites = new List<Sprite>();
        menuUnselectedSprites = new List<Sprite>();

        foreach (var spriteName in spriteNames) {
            var selected = Resources.Load<Sprite>("UI/Combat/" + spriteName + "Selected");
            var unselected = Resources.Load<Sprite>("UI/Combat/" + spriteName);

            menuSelectedSprites.Add(selected);
            menuUnselectedSprites.Add(unselected);
        }

        // Load question sprites
        string[] questionSpriteNames = { "A", "B", "C", "D" };
        questionSelectedSprites = new List<Sprite>();
        questionUnselectedSprites = new List<Sprite>();

        foreach (var spriteName in questionSpriteNames) {
            var selected = Resources.Load<Sprite>("UI/Combat/" + spriteName + "Selected");
            var unselected = Resources.Load<Sprite>("UI/Combat/" + spriteName);

            questionSelectedSprites.Add(selected);
            questionUnselectedSprites.Add(unselected);
        }

        // Load the boss music.
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = currEnemy.musicVolume;

        var bossTheme = Resources.Load<AudioClip>("Audio/EnemyMusic/" + currEnemy.id);

        if (bossTheme != null) {
            musicSource.clip = bossTheme;
            musicSource.Play();
        }

        // Load the SFX player.
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;

        // Load the button player.
        buttonSource = gameObject.AddComponent<AudioSource>();
        buttonSource.loop = false;
        buttonSource.playOnAwake = false;

        // Start the dialogue.
        NextDialogue();
    }

    public void Update() {
        switch (state) {
            case CombatState.Dialogue:
                HandleDialogueInput();
                break;

            case CombatState.PlayerChoice:
                HandlePlayerChoiceInput();
                break;

            case CombatState.Question:
                HandleQuestionInput();
                break;

            case CombatState.ShowTip:
                HandleDialogueInput();
                break;

            case CombatState.ShowItems:
                HandleItemPanelInput();
                break;
        }

        DebugLocks();
        UpdateStar();
    }


    // UPDATE METHODS
    private void UpdateQuestionButtonSprites() {
        for (int i = 0; i < questionButtons.Count; ++i) {
            var img = questionButtons[i].GetComponent<Image>();

            img.sprite = (i == questionButtonIndex) ? questionSelectedSprites[i] : questionUnselectedSprites[i];
        }
    }

    private void UpdateStar() {
        starText.SetActive(!string.IsNullOrEmpty(dialogueText.text) ? true : false);
    }

    private void UpdateButtonSprites() {
        for (int i = 0; i < buttons.Count; ++i) {
            var img = buttons[i].GetComponent<Image>();

            img.sprite = (i == buttonIndex) ? menuSelectedSprites[i] : menuUnselectedSprites[i];
        }
    }

    private void UpdateItemOptions() {
        int optionCount = itemOptions != null ? itemOptions.Count : 0;
        int displayCount = displayableItems != null ? displayableItems.Count : 0;

        for (int i = 0; i < optionCount; ++i) {
            if (i < displayCount) {
                var item = displayableItems[i];
                int amount = 0;

                if (GameManager.GetInstance() != null)
                    amount = GameManager.GetInstance().GetItemAmount(item.itemName);

                if (itemOptions[i] != null) {
                    itemOptions[i].gameObject.SetActive(true);
                    itemOptions[i].text = $"{item.itemName} x{amount}";
                    itemOptions[i].color = (i == itemIndex) ? selectedColor : defaultColor;
                }
            } else {
                if (itemOptions[i] != null)
                    itemOptions[i].gameObject.SetActive(false);
            }
        }
    }

    // HANDLE METHODS
    private void HandlePlayerChoiceInput() {
        if (IsActionUnlocked("main")) {
            RegisterLock("main");
        }
        else {
            return;
        }

        if (!isButtonEnabled) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if (buttonIndex < buttons.Count - 1) buttonIndex++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if (buttonIndex > 0) buttonIndex--;
        }

        UpdateButtonSprites();

        if (!Input.GetKeyDown(KeyCode.Return)) return;

        RegisterUnlock("main");

        var selectClip = Resources.Load<AudioClip>("Audio/Combat/select");
        if (selectClip != null && sfxSource != null) {
            buttonSource.clip = selectClip;
            buttonSource.Play();
        }

        switch (buttonIndex) {
            case 0: // Solve
                StartQuestion();
                break;
            case 1: // Help
                ShowTip();
                break;
            case 2: // Item
                ShowItems();
                break;
            case 3: // Skip
                break;
        }
    }

    private void HandleQuestionInput() {
        if (IsActionUnlocked("question")) {
            RegisterLock("question");
        } else {
            return;
        }

        if (!isQuestionEnabled) return;

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
            if (questionButtonIndex < questionButtons.Count - 1) questionButtonIndex++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
            if (questionButtonIndex > 0) questionButtonIndex--;
        }

        UpdateQuestionButtonSprites();

        if (!Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) return;
        
        isQuestionEnabled = false;

        var correct = questionButtonIndex == correctAnswerIndex;

        if (correct) {
            var damage = 10;

            StartCoroutine(HandleDamageAndContinue(damage));
            currEnemyHp -= 10;
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

            RegisterUnlock("question");
        }

        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }

    private void HandleDialogueInput() {
        if (!Input.GetKeyDown(KeyCode.Return)) return;

        if (waitingForEnter)
            waitingForEnter = false;
        else
            skipTypewriter = true;
    }

    private void HandleItemPanelInput() {
        if (IsActionUnlocked("item")) {
            RegisterLock("item");
        } else {
            return;
        }

        if (displayableItems.Count == 0) {
            if (!Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Escape)) return;

            if (itemPanel != null)
                itemPanel.SetActive(false);

            state = CombatState.PlayerChoice;
            EnablePlayerChoice();

            return;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
            if (itemIndex < displayableItems.Count - 1) itemIndex++;
            UpdateItemOptions();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
            if (itemIndex > 0) itemIndex--;
            UpdateItemOptions();
        }
        if (Input.GetKeyDown(KeyCode.Return)) {
            UseSelectedItem();
        }

        if (!Input.GetKeyDown(KeyCode.Escape)) {
            RegisterUnlock("item");
            return;
        }

        if (itemPanel != null)
            itemPanel.SetActive(false);

        RegisterUnlock("item");

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }

    private IEnumerator HandleDamageAndContinue(int damage) {
        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);

        yield return StartCoroutine(ShowDamageText(damage));

        if (currEnemyHp <= 0) {
            EndCombat();
            yield break;
        }

        RegisterUnlock("question");

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }


    // COROUTINES
    private IEnumerator ShowDialogue() {
        textbox.SetActive(true);
        questionBox.SetActive(false);

        if (questionPanel != null)
            questionPanel.SetActive(false);
        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);

        while (dialogueIndex < currEnemy.dialogue.Length) {
            yield return StartCoroutine(TypeText(currEnemy.dialogue[dialogueIndex].text));
            waitingForEnter = true;

            while (waitingForEnter) {
                yield return null;
            }

            dialogueIndex++;
        }

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }

    private IEnumerator TypeText(string fullText) {
        dialogueText.text = "";
        skipTypewriter = false;

        foreach (char c in fullText) {
            if (skipTypewriter) {
                dialogueText.text = fullText;
                break;
            }

            dialogueText.text += c;

            yield return new WaitForSeconds(typewriterSpeed);
        }
    }
    private IEnumerator ShowTipCoroutine(string tip) {
        textbox.SetActive(true);
        questionBox.SetActive(false);

        if (questionPanel != null)
            questionPanel.SetActive(false);

        if (enemyImage != null)
            enemyImage.gameObject.SetActive(true);

        yield return StartCoroutine(TypeText(tip));

        waitingForEnter = true;
        while (waitingForEnter) {
            yield return null;
        }

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }
    private IEnumerator ShowDamageText(int damageAmount) {
        if (damageText == null || enemyImage == null)
            yield break;

        if (currEnemy.banter != null && currEnemy.banter.Length > 0) {
            int randomIndex = Random.Range(0, currEnemy.banter.Length);
            this.dialogueText.text = currEnemy.banter[randomIndex];
        } else {
            this.dialogueText.text = "";
        }

        // Load and play hit sound
        var hitClip = Resources.Load<AudioClip>("Audio/Combat/hit");
        if (hitClip != null && sfxSource != null) {
            sfxSource.clip = hitClip;
            sfxSource.Play();
        }

        isButtonEnabled = false;

        float waitTime = 0.75f;
        while (sfxSource != null && sfxSource.isPlaying && sfxSource.time < waitTime)
            yield return null;

        damageText.text = $"-{damageAmount}";
        damageText.gameObject.SetActive(true);

        RectTransform damageRect = damageText.rectTransform;
        Vector2 damageOriginalPos = damageRect.anchoredPosition;

        RectTransform enemyRect = enemyImage.rectTransform;
        Vector2 enemyOriginalPos = enemyRect.anchoredPosition;

        float bounceDuration = 0.4f;
        float totalDuration = 1.0f;
        float jumpHeight = 40f;
        float shakeAmount = 20f;
        int shakeCount = 3;

        float elapsed = 0f;

        while (elapsed < bounceDuration) {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / bounceDuration);

            float yOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            damageRect.anchoredPosition = damageOriginalPos + Vector2.up * yOffset;

            float xOffset = Mathf.Sin(t * Mathf.PI * shakeCount) * shakeAmount;
            enemyRect.anchoredPosition = enemyOriginalPos + Vector2.right * xOffset;

            yield return null;
        }

        damageRect.anchoredPosition = damageOriginalPos;
        enemyRect.anchoredPosition = enemyOriginalPos;

        yield return new WaitForSeconds(totalDuration - bounceDuration);

        damageText.gameObject.SetActive(false);

        isButtonEnabled = true;
    }


    // MISC
    private void StartQuestion() {
        isButtonEnabled = false;
        isQuestionEnabled = true;
        state = CombatState.Question;
        
        currentQuestion = QuestionLoader.GetRandomQuestion(currEnemy.unit, out var questionSprite);
        correctAnswerIndex = currentQuestion.correct;

        textbox.SetActive(false);
        questionBox.SetActive(true);
        questionImage.sprite = questionSprite;

        questionImage.preserveAspect = true;

        if (questionPanel != null && questionImage != null) {
            var panelRect = questionPanel.GetComponent<RectTransform>();
            var imageRect = questionImage.rectTransform;

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

    private void NextDialogue() {
        dialogueIndex = 0;
        state = CombatState.Dialogue;

        StartCoroutine(ShowDialogue());
    }

    private void UseSelectedItem() {
        if (displayableItems.Count == 0) return;
        var selectedItem = displayableItems[itemIndex];

        if (GameManager.GetInstance().GetItemAmount(selectedItem.itemName) <= 0) {
            return;
        }

        // Example: Remove one from inventory
        GameManager.GetInstance().RemoveItem(selectedItem.itemName, 1);

        // TODO: Apply item effect here (e.g., heal, buff, etc.)
        // For now, just show a message
        // StartCoroutine(ShowItemUsedCoroutine(selectedItem.itemName));

        // Hide item panel
        if (itemPanel != null)
            itemPanel.SetActive(false);
    }


    private void EnablePlayerChoice() {
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

    private void EnemyAttack() {
        playerLives -= 1;
        playerLivesText.text = "Lives: " + playerLives;

        if (playerLives <= 0) {
            GameOver();
            return;
        }

        state = CombatState.PlayerChoice;
        EnablePlayerChoice();
    }

    private void ShowTip() {
        isButtonEnabled = false;

        state = CombatState.ShowTip;
        string tip = TipLoader.GetRandomTip(currEnemy.unit);

        StartCoroutine(ShowTipCoroutine(tip));
    }

    private void ShowItems() {
        isButtonEnabled = false;
        state = CombatState.ShowItems;
        itemIndex = 0;

        if (itemPanel != null)
            itemPanel.SetActive(true);
        if (textbox != null)
            textbox.SetActive(false);
        if (questionBox != null)
            questionBox.SetActive(false);
        if (questionPanel != null)
            questionPanel.SetActive(false);
        if (starText != null)
            starText.SetActive(false);


        /*
        Old null checks..
        if (items == null || items.items == null || itemOptions == null || GameManager.GetInstance() == null) {
            if (itemOptions != null) {
                foreach (var option in itemOptions)
                    option.gameObject.SetActive(false);
            }
            return;
        }
        */

        UpdateItemOptions();
    }

    private void EndCombat() {
        state = CombatState.End;

        Debug.Log("Enemy Defeated");
        SceneManager.LoadScene(GameManager.GetInstance().previousScene);
    }

    private void GameOver() {
        state = CombatState.End;

        Debug.Log("Player Defeated");
    }

    // Helper Methods
    private bool IsActionUnlocked(string action) {
        return actionLocks.TryGetValue(action, out bool unlocked) && unlocked;
    }

    private void RegisterLock(string action) {
        foreach (var key in new List<string>(actionLocks.Keys))
            actionLocks[key] = false;

        actionLocks[action] = true;
    }

    private void RegisterUnlock(string action) {
        foreach (var key in new List<string>(actionLocks.Keys))
            actionLocks[key] = true;
    }

    private void DebugLocks() {
        foreach (var key in new List<string>(actionLocks.Keys))
            Debug.Log(key + " -> " + actionLocks[key]);
    }
}
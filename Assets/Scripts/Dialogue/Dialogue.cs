using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour {
    [SerializeField] public string dialogueFileName;

    public Region region;

    public GameObject dialogueUI;
    public GameObject player;

    public TextMeshProUGUI dialogueText;
    public Image speakerImage;

    private const float KTypewriterSpeed = 0.05f;

    private DialogueLine[] dialogueLines;
    private int dialogueIndex;

    private bool hasPlayed;
    private bool running;

    private bool waitingForEnter;
    private bool skipTypewriter;

    [Serializable]
    public class DialogueLine {
        public string text;
        public float duration;
        public string speaker;
    }

    public void Start() {
        LoadDialogueData();

        if (dialogueUI != null)
            dialogueUI.SetActive(false);
    }

    public void Update() {
        if (!running)
            if (region.Contains(player.transform.position.x, player.transform.position.y))
                if (GameManager.instance.randomAccess.TryAdd(dialogueFileName, "completed")) {
                    hasPlayed = true;
                    running = true;

                    NextDialogue();
                }

        if (!running || !Input.GetKeyDown(KeyCode.Return)) return;

        if (waitingForEnter)
            waitingForEnter = false;
        else
            skipTypewriter = true;
    }

    private void NextDialogue() {
        GameManager.instance.isDialoguePlaying = true;

        dialogueIndex = 0;
        if (dialogueUI != null)
            dialogueUI.SetActive(true);

        StartCoroutine(ShowDialogue());
    }

    private IEnumerator ShowDialogue() {
        while (dialogueIndex < dialogueLines.Length) {
            var currentSpeaker = dialogueLines[dialogueIndex].speaker;

            if (speakerImage != null && !string.IsNullOrEmpty(currentSpeaker)) {
                var speakerSprite = Resources.Load<Sprite>("Icons/" + currentSpeaker);

                if (speakerSprite != null) {
                    speakerImage.sprite = speakerSprite;
                    speakerImage.enabled = true;
                    speakerImage.preserveAspect = true;

                    var imageRect = speakerImage.rectTransform;

                    var targetWidth = imageRect.rect.width;
                    var aspect = speakerSprite.rect.height / speakerSprite.rect.width;

                    imageRect.sizeDelta = new Vector2(targetWidth, targetWidth * aspect);
                }
                else {
                    Debug.LogWarning("Speaker image not found for: " + currentSpeaker);
                    speakerImage.enabled = false;
                }
            }

            yield return StartCoroutine(TypeText(dialogueLines[dialogueIndex].text));
            waitingForEnter = true;

            while (waitingForEnter) yield return null;

            dialogueIndex++;
        }

        if (dialogueUI != null)
            dialogueUI.SetActive(false);

        GameManager.instance.isDialoguePlaying = false;
    }

    private IEnumerator TypeText(string fullText) {
        dialogueText.text = "";
        skipTypewriter = false;

        foreach (var c in fullText) {
            if (skipTypewriter) {
                dialogueText.text = fullText;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(KTypewriterSpeed);
        }
    }

    private void LoadDialogueData() {
        var jsonAsset = Resources.Load<TextAsset>("Dialogues/" + dialogueFileName);

        if (jsonAsset == null) {
            Debug.LogError("Dialogue file not found: " + dialogueFileName);
            return;
        }

        dialogueLines = JsonHelper.FromJson<DialogueLine>(jsonAsset.text);

        if (dialogueLines != null || dialogueLines.Length > 0)
            return;

        Debug.LogWarning("No dialogue lines found in file: " + dialogueFileName);
    }
}
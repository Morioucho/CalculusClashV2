using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FailedGame : MonoBehaviour {
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private float fadeDuration = 0.5f;

    private const string dialogueFile = "failed_game.json";
    private List<string> dialogues = new List<string>();
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI dialogueText;

    void Start() {
        canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

        StartCoroutine(LoadDialogues());
    }

    public void PlayDialogue() {
        StartCoroutine(PlayAllDialogues());
    }

    private IEnumerator PlayAllDialogues() {
        foreach (string line in dialogues) {
            Debug.Log(line);

            dialogueText.text = line;
            yield return StartCoroutine(FadeIn());

            float readTime = DetermineReadDuration(line);
            yield return new WaitForSecondsRealtime(readTime);

            yield return StartCoroutine(FadeOut());
        }

        SceneManager.LoadScene("PostCredits");
    }

    public IEnumerator LoadDialogues() {
        TextAsset jsonAsset = Resources.Load<TextAsset>("Dialogues/initial_dialogue");

        if (jsonAsset == null) {
            Debug.LogError("Dialogue JSON not found in Resources/Dialogues/initial_dialogue");
            yield break;
        }

        string jsonText = jsonAsset.text;
        string[] loadedDialogues = JsonHelper.FromJson<string>(jsonText);
        dialogues.AddRange(loadedDialogues);

        Debug.Log("Loaded JSON: " + jsonText);

        PlayDialogue();
        yield return null;
    }


    private IEnumerator FadeIn() {
        dialogueBox.SetActive(true);

        float t = 0f;

        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut() {
        float t = 0f;

        while (t < fadeDuration) {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    private float DetermineReadDuration(string sentence) {
        float duration = 0.0f;

        foreach (char c in sentence) {
            duration += (c == '.') ? 0.05f : 0.025f;
        }

        return duration;
    }
}

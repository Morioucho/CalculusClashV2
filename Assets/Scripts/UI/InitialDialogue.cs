using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;

public class InitialDialogue : MonoBehaviour {
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private float fadeDuration = 0.5f;

    private List<string> dialogues = new List<string>();
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI dialogueText;

    void Start() {
        canvasGroup = dialogueBox.GetComponent<CanvasGroup>();
        dialogueText = dialogueBox.GetComponentInChildren<TextMeshProUGUI>();

        // Create a better system later that reads from a file or something..
        dialogues.Add("AP Calc is so hard…. What are these symbols man…");
        dialogues.Add("It’s 3 AM, but I need to keep pushing.");
        dialogues.Add("Just… a little… longer…");

        PlayDialogue();
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

        SceneManager.LoadScene("Game");
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
            duration += (c == '.') ? 0.1f : 0.05f;
        }

        return duration;
    }
}

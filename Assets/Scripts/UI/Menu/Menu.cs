using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using static Konami;
using Unity.VisualScripting;

public class MenuScript : MonoBehaviour {
    public enum KonamiInput {
        W, S, A, D, RightClick, LeftClick, Space
    }

    private KonamiInput[] sequence = new KonamiInput[] {
        KonamiInput.W, KonamiInput.W,
        KonamiInput.S, KonamiInput.S,
        KonamiInput.A, KonamiInput.D,
        KonamiInput.A, KonamiInput.D,
        KonamiInput.RightClick,
        KonamiInput.LeftClick,
        KonamiInput.Space
    };

    private int sequenceIndex = 0;

    void Start() {
        GameManager.GetInstance().PlayMusic();
    }

    void Update() {
        if (CheckInput(sequence[sequenceIndex])) {
            sequenceIndex++;

            Debug.Log($"Konami sequence progress: {sequenceIndex}/{sequence.Length}");

            if (sequenceIndex >= sequence.Length) {
                special();
                sequenceIndex = 0;
            }
        } else if (AnyInputPressed()) {
            sequenceIndex = 0;
        }
    }
    
    public void Play() {
        Debug.Log("Play game clicked.");

        StartCoroutine(Transition("InitialStory"));
    }

    public void Credits() {
        Debug.Log("Credits clicked.");
        SceneManager.LoadScene("Credits");
    }

    public void Options() {
        Debug.Log("Options clicked.");
        SceneManager.LoadScene("Options");
    }

    public void Quit() {
        Debug.Log("Quitting game.");
        Application.Quit();
    }

    private IEnumerator Transition(string nextRoom) {
        const int fadeOutDuration = 2;

        Coroutine fadeMusic = StartCoroutine(GameManager.GetInstance().FadeOutMusic(fadeOutDuration));
        Coroutine fadeBackground = StartCoroutine(FadeOutEffect.GetInstance().FadeOut(fadeOutDuration));

        yield return fadeMusic;
        yield return fadeBackground;

        SceneManager.LoadScene(nextRoom);
    }

    private bool CheckInput(KonamiInput input) {
        switch (input) {
            case KonamiInput.W: return Input.GetKeyDown(KeyCode.W);
            case KonamiInput.S: return Input.GetKeyDown(KeyCode.S);
            case KonamiInput.A: return Input.GetKeyDown(KeyCode.A);
            case KonamiInput.D: return Input.GetKeyDown(KeyCode.D);
            case KonamiInput.RightClick: return Input.GetMouseButtonDown(1);
            case KonamiInput.LeftClick: return Input.GetMouseButtonDown(0);
            case KonamiInput.Space: return Input.GetKeyDown(KeyCode.Space);
            default: return false;
        }
    }

    private bool AnyInputPressed() {
        return Input.anyKeyDown || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1);
    }

    private void special() {
        StartCoroutine(Transition("room10"));
    }
}
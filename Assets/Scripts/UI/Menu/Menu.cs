using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {
    void Start() {
        GameManager.GetInstance().PlayMusic();
    }
    
    public void Play() {
        Debug.Log("Play game clicked.");

        StartCoroutine(Transition());
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

    private IEnumerator Transition() {
        const int fadeOutDuration = 2;

        Coroutine fadeMusic = StartCoroutine(GameManager.GetInstance().FadeOutMusic(fadeOutDuration));
        Coroutine fadeBackground = StartCoroutine(FadeOutEffect.GetInstance().FadeOut(fadeOutDuration));

        yield return fadeMusic;
        yield return fadeBackground;

        SceneManager.LoadScene("InitialStory");
    }
}
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
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
        yield return StartCoroutine(FadeOutEffect.instance.FadeOut());
        SceneManager.LoadScene("Game");
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public void Play() {
        Debug.Log("Play game clicked.");
        SceneManager.LoadScene("Game");
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
}
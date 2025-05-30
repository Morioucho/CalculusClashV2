using Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCondition : MonoBehaviour {
    void Update() {
        if (GameManager.instance.wonGame) {
            SceneManager.LoadScene("BeatGame");
        }
    }
}
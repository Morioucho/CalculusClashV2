using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour {
    public void Back() {
        Debug.Log("Going back to menu.");
        SceneManager.LoadScene("Menu");
    }
}

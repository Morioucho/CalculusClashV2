using UnityEngine;
using UnityEngine.SceneManagement;

public class Konami : MonoBehaviour {
    public enum KonamiInput {
        W, S, A, D, RightClick, LeftClick, Space
    }

    private KonamiInput[] sequence = new KonamiInput[]
    {
        KonamiInput.W, KonamiInput.W,
        KonamiInput.S, KonamiInput.S,
        KonamiInput.A, KonamiInput.D,
        KonamiInput.A, KonamiInput.D,
        KonamiInput.RightClick,
        KonamiInput.LeftClick,
        KonamiInput.Space
    };

    private int sequenceIndex = 0;

    void Update() {

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
        SceneManager.LoadScene("room10");
    }
}
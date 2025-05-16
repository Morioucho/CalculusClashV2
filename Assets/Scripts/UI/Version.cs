using TMPro;
using UnityEngine;

public class Version : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textBox;
    private void Awake() {
        string versionText = "";

        if (GameManager.developmentBuild) {
            versionText += "Development Build on " + GameManager.buildDate + " - ";
        }

        versionText += "Version: " + GameManager.version;

        textBox.text = versionText;
    }
}

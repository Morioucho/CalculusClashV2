using UnityEngine;
using TMPro;

public class Version : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textBox;
    
    private void Awake() {
        string versionText = "";

        versionText += "Version: " + GameManager.version;

        if (GameManager.developmentBuild) {
            versionText += " (DEV) " + GameManager.buildDate;
        }

        textBox.text = versionText;
    }
}

using UnityEngine;
using TMPro;

public class Version : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textBox;
    
    private void Awake() {
        string versionText = "";

        versionText += "Version: " + GameManager.KVersion;

        if (GameManager.KDevelopmentBuild) {
            versionText += " (DEV) " + GameManager.KBuildDate;
        }

        textBox.text = versionText;
    }
}

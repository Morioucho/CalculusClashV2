using UnityEngine;
using TMPro;

public class Version : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textBox;
    
    private void Awake() {
        string versionText = "";

        versionText += "Version: " + GameManager.KVersion;

#pragma warning disable
        if (GameManager.KDevelopmentBuild) {
            versionText += " (DEV) " + GameManager.KBuildDate;
        }
#pragma warning restore

        textBox.text = versionText;
    }
}

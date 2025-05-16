using TMPro;
using UnityEngine;

public class Version : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI textBox;
    private void Awake() {
        textBox.text = "Version: " + GameManager.version;
    }
}

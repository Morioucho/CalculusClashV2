using UnityEngine;

public class BossLoader : MonoBehaviour {
    private const string bossFile = "bosses.json";
    private List<BossData> bosses = new List<BossData>();

    public void Awake() {
        string filePath = Path.Combine(Application.streamingAssetsPath, bossFile);
        string jsonText;
        

    }
}
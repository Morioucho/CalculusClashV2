using UnityEngine;
using System.IO;

public static class EnemyLoader {
    public static EnemyData LoadEnemy(string enemyId) {
        string path = Path.Combine(Application.streamingAssetsPath, "Enemies", enemyId + ".json");

        if (File.Exists(path)) {
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<EnemyData>(json);
        }

        Debug.LogError("Enemy JSON not found: " + path);
        return null;
    }
}
using UnityEngine;

public static class EnemyLoader {
    public static EnemyData LoadEnemy(string enemyId) {
        string resourcePath = $"Enemies/{enemyId.ToLower()}/{enemyId.ToLower()}";
        TextAsset jsonAsset = Resources.Load<TextAsset>(resourcePath);

        if (jsonAsset != null) {
            return JsonUtility.FromJson<EnemyData>(jsonAsset.text);
        }

        Debug.LogError("Enemy JSON not found in Resources at: " + resourcePath);
        return null;
    }
}
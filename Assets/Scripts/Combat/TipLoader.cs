using UnityEngine;

using System.IO;
using System.Collections.Generic;

public static class TipLoader {
    public static string GetRandomTip(int unit) {
        string folder = Path.Combine(Application.streamingAssetsPath, "Questions", unit.ToString());
        string path = Path.Combine(folder, "tips.json");

        if (!File.Exists(path))
            return "";

        string json = File.ReadAllText(path);
        TipList tipList = JsonUtility.FromJson<TipList>(json);

        if (tipList == null || tipList.tips == null || tipList.tips.Count == 0)
            return "";

        int index = Random.Range(0, tipList.tips.Count);
        return tipList.tips[index];
    }
}
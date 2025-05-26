using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class QuestionLoader {
    public static QuestionData GetRandomQuestion(int unit, out Sprite sprite) {
        sprite = null;

        string folder = Path.Combine(Application.streamingAssetsPath, "Questions", unit.ToString());
        string path = Path.Combine(folder, "questions.json");

        if (!File.Exists(path)) {
            Debug.LogError("Question JSON not found: " + path);
            return null;
        }

        string rawJson = File.ReadAllText(path);
        string wrappedJson = "{\"questions\":" + rawJson + "}";
        QuestionDataList dataList = JsonUtility.FromJson<QuestionDataList>(wrappedJson);

        if (dataList.questions.Count == 0) {
            Debug.LogWarning("No questions found in list.");
            return null;
        }

        int randIndex = Random.Range(0, dataList.questions.Count);
        QuestionData question = dataList.questions[randIndex];

        string spritePath = $"Questions/{unit}/{question.spriteIndex + 1}";
        sprite = Resources.Load<Sprite>(spritePath);

        if (sprite == null) {
            Debug.LogError("Sprite not found at: " + spritePath);
        }

        return question;
    }
}

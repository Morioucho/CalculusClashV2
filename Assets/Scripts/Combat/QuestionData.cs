using System.Collections.Generic;

[System.Serializable]
public class QuestionData {
    public int spriteIndex;
    public int spriteWidth;
    public int spriteHeight;
    public int correct;
}

[System.Serializable]
public class QuestionDataList {
    public List<QuestionData> questions;
}

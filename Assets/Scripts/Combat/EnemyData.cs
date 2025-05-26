using UnityEngine;

[System.Serializable]
public class EnemyData {
    public string id;
    public string name;
    public int health;
    public DialogueLine[] dialogue;
    public string sprite;
    public string music;
    public float musicVolume;


    [System.Serializable]
    public class DialogueLine {
        public string text;
        public float duration;
    }
}

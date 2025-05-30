    using UnityEngine;

    [System.Serializable]
    public class EnemyData {
        public string id;
        public string name;
        public int[] unit;
        public int health;
        public int qt;
        public DialogueLine[] dialogue;
        public string[] banter;
        public string sprite;
        public string music;
        public float musicVolume;

        [System.Serializable]
        public class DialogueLine {
            public string text;
            public float duration;
        }
    }

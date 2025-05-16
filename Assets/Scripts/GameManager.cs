using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public const string version = "1.0.0";
    public float timeOfDay = 0.0f;

    void Start() {
        if (instance == null) {
            lock (this) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }

    void Update(){
        timeOfDay += Time.deltaTime / 60f;

        if(timeOfDay >= 24f) {
            timeOfDay = 0f;
        }
    }
}

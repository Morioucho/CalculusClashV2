using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private AudioSource musicSource;

    public const string version = "1.1.0";
    public const bool developmentBuild = true;
    public const string buildDate = "05-16-2025";

    public float timeOfDay = 0.0f;

    void Awake() {
        if (instance == null) {
            lock (this) {
                instance = this;

                this.musicSource = GetComponent<AudioSource>();

                DontDestroyOnLoad(gameObject);
            }
        } else {
            Destroy(gameObject);
        }
    }
    public static GameManager GetInstance() {
        return instance;
    }

    void Update(){
        timeOfDay += Time.deltaTime / 60f;

        if(timeOfDay >= 24f) {
            timeOfDay = 0f;
        }
    }

    public void PlayMusic() {
        Debug.Log("Playing music.");

        if (!this.musicSource.isPlaying) {
            this.musicSource.Play();
        }
    }

    public IEnumerator FadeOutMusic(int seconds) {
        float currentVolume = this.musicSource.volume;

        float time = 0f;
        while(time < seconds) {
            time += Time.unscaledDeltaTime;
            this.musicSource.volume = Mathf.Lerp(currentVolume, 0f, time / seconds);
            yield return null;
        }

        this.musicSource.volume = 0f;
        this.musicSource.Stop();
    }
    
    public void StopMusic() {
        this.musicSource.Stop();
    }
}

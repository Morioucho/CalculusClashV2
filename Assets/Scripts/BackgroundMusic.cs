using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    [SerializeField]
    public string musicFile;

    private static AudioSource musicSource;
    private static string currentMusicFile = "";
    private static float lastPlaybackTime = 0f;

    void Start() {
        if (musicSource == null){
            GameObject musicObj = new GameObject("BackgroundMusicSource");
            musicSource = musicObj.AddComponent<AudioSource>();
            musicSource.loop = true;
            DontDestroyOnLoad(musicObj);
        }

        if (currentMusicFile == musicFile) {
            if (!musicSource.isPlaying) {
                musicSource.time = lastPlaybackTime;
                musicSource.Play();
            }

            return;
        }

        AudioClip clip = Resources.Load<AudioClip>(musicFile);
        if (clip != null) {
            musicSource.Stop();
            musicSource.clip = clip;
            musicSource.time = 0f;
            musicSource.Play();
            currentMusicFile = musicFile;
            lastPlaybackTime = 0f;
        } else {
            Debug.LogWarning("Music file not found: " + musicFile);
        }
    }

    void Update() {
        if (GameManager.instance.isBattlePlaying) {
            if (musicSource.isPlaying) {
                lastPlaybackTime = musicSource.time;
                musicSource.Pause();
            }
        } else {
            if (!musicSource.isPlaying && musicSource.clip != null) {
                musicSource.time = lastPlaybackTime;
                musicSource.Play();
            }
        }
    }
}
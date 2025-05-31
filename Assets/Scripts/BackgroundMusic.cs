using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    [SerializeField] public string musicFile;

    private static AudioSource _musicSource;
    private static string _currentMusicFile = "";
    private static float _lastPlaybackTime;

    private void Start() {
        if (_musicSource == null) {
            var musicObj = new GameObject("BackgroundMusicSource");

            _musicSource = musicObj.AddComponent<AudioSource>();
            _musicSource.loop = true;

            DontDestroyOnLoad(musicObj);
        }

        if (_currentMusicFile == musicFile) {
            if (_musicSource.isPlaying) return;

            _musicSource.time = _lastPlaybackTime;
            _musicSource.Play();
        }

        var clip = Resources.Load<AudioClip>(musicFile);
        if (clip != null) {
            _musicSource.Stop();

            _musicSource.clip = clip;
            _musicSource.time = 0f;

            _musicSource.Play();

            _currentMusicFile = musicFile;
            _lastPlaybackTime = 0f;
        }
        else {
            Debug.LogWarning("Music file not found: " + musicFile);
        }
    }

    private void Update() {
        if (GameManager.instance.isBattlePlaying) {
            if (!_musicSource.isPlaying) return;

            _lastPlaybackTime = _musicSource.time;
            _musicSource.Pause();
        }
        else {
            if (_musicSource.isPlaying && _musicSource.clip == null) return;

            _musicSource.time = _lastPlaybackTime;
            _musicSource.Play();
        }
    }
}
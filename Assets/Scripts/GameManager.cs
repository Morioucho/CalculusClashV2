using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private AudioSource musicSource;

    public const string version = "1.1.0";
    public const bool developmentBuild = true;
    public const string buildDate = "05-28-2025";
    public const int roomAmount = 10;

    public float timeOfDay = 0.0f;

    public string encounterEnemyID;
    public string previousScene;

    public bool wentPrevious = false;
    public bool battleHandled = true;
    public bool isDialoguePlaying = false;
    public bool isBattlePlaying = false;
    public float transferPositionX, transferPositionY;

    public Dictionary<string, int> playerItems = new Dictionary<string, int>();
    public Dictionary<string, Position> roomPositions = new Dictionary<string, Position>();

    // i hate this design but we have no other options........
    public Dictionary<string, string> randomAccess = new Dictionary<string, string>();
    public Dictionary<string, Vector2> previousPositions = new Dictionary<string, Vector2>();

    void Awake() {
        if (instance == null) {
            lock (this) {
                instance = this;

                this.musicSource = GetComponent<AudioSource>();

                DontDestroyOnLoad(gameObject);

                var allItems = ItemLoader.GetAllItems();
                if (allItems != null && allItems.items != null) {
                    foreach (var item in allItems.items) {
                        if (!playerItems.ContainsKey(item.itemName)) {
                            playerItems[item.itemName] = 5;
                        }
                    }
                }

                for (int i = 0; i < roomAmount + 1; ++i) {
                    roomPositions["room" + i] = new Position(0, 0);
                }

                for (int i = 0; i < roomAmount + 1; ++i) {
                    roomPositions["Room" + i] = new Position(0, 0);
                }
            }
        } else {
            Destroy(gameObject);
        }
    }

    public static GameManager GetInstance() {
        return instance;
    }

    void Update() {
        timeOfDay += Time.deltaTime / 60f;

        if (timeOfDay >= 24f) {
            timeOfDay = 0f;
        }
    }

    public void PlayMusic() {
        Debug.Log("Playing music.");

        if (!this.musicSource.isPlaying) {
            this.musicSource.Play();
        }
    }

    public void SetEncounter(string enemyId, string returnScene) {
        this.encounterEnemyID = enemyId;
        this.previousScene = returnScene;
    }

    public IEnumerator FadeOutMusic(int seconds) {
        float currentVolume = this.musicSource.volume;

        float time = 0f;
        while (time < seconds) {
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

    public void AddItem(string itemName, int amount = 1) {
        if (playerItems.ContainsKey(itemName)) {
            playerItems[itemName] += amount;
        } else {
            playerItems[itemName] = amount;
        }
    }

    public bool RemoveItem(string itemName, int amount = 1) {
        if (playerItems.ContainsKey(itemName) && playerItems[itemName] >= amount) {
            playerItems[itemName] -= amount;
            return true;
        }
        return false;
    }

    public int GetItemAmount(string itemName) {
        return playerItems.ContainsKey(itemName) ? playerItems[itemName] : 0;
    }
}

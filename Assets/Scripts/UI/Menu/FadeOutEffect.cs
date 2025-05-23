using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOutEffect : MonoBehaviour {
    public static FadeOutEffect instance;

    [SerializeField] private GameObject panel;

    private Image panelImage;

    private void Awake() {
        if(instance == null) {
            lock (this) {
                instance = this;
            }
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        this.panelImage = panel.GetComponent<Image>();
    }

    public static FadeOutEffect GetInstance() {
        return instance;
    }

    public IEnumerator FadeOut(int fadeDuration) {
        panel.SetActive(true);

        Color color = panelImage.color;
        float startAlpha = color.a;
        float time = 0f;

        while (time < fadeDuration) {
            time += Time.unscaledDeltaTime;
            float t = time / fadeDuration;
            color.a = Mathf.Lerp(startAlpha, 1f, t);
            panelImage.color = color;

            yield return null;
        }
    }
}

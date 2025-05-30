using System.Collections;
using UnityEngine;

public class EndCredits : MonoBehaviour {
    public float scrollSpeed = 60f;
    public float scrollDuration = 70f;
    private RectTransform rectTransform;
    private float timer = 0f;
    private bool scrolling = true;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        if (!scrolling)
            return;

        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= scrollDuration) {
            scrolling = false;
        }
    }
}
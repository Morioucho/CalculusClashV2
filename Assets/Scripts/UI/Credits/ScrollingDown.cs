using UnityEngine;

public class ScrollingDown : MonoBehaviour {
    public float scrollSpeed = 60f;
    public float scrollDuration = 70f;

    private RectTransform rectTransform;

    private float timer;
    private bool scrolling = true;

    private void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update() {
        if (!scrolling)
            return;

        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        timer += Time.deltaTime;

        if (timer >= scrollDuration) scrolling = false;
    }
}
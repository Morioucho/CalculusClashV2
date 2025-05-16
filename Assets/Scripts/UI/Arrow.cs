using UnityEngine;

public class Arrow : MonoBehaviour{
    public static Arrow instance;

    [Header("Arrow Settings")]
    public RectTransform arrowRectTransform;
    public float arrowSpeed = 10.0f;
    public bool loaded = false;

    private Vector2 targetPosition;

    private void Awake() {
        if (instance == null) {
            lock (this) {
                instance = this;
            }
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (loaded) {
            arrowRectTransform.anchoredPosition = Vector2.Lerp(
                arrowRectTransform.anchoredPosition,
                targetPosition,
                Time.unscaledDeltaTime * arrowSpeed
            );
        }
    }

    public void moveTo(RectTransform button) {
        if (!loaded) {
            loaded = true;
        }

        Vector2 newPos = arrowRectTransform.anchoredPosition;
        newPos.y = button.anchoredPosition.y;

        targetPosition = newPos;

        if (!arrowRectTransform.gameObject.activeSelf)
            arrowRectTransform.gameObject.SetActive(true);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class HoverEffect : MonoBehaviour {
    private Outline outline;
    private Coroutine fadeCoroutine;

    [SerializeField] public RectTransform arrowRectTransform;

    public void Awake() {
        this.outline = GetComponent<Outline>();

        if(outline == null) {
            this.outline.enabled = false;
        }
    }

    public void OnMouseEnter() {
        if(outline != null) {
            outline.enabled = true;

            if(Arrow.instance != null) {
                Arrow.instance.moveTo(arrowRectTransform);
            }
        }
    }

    public void OnMouseExit() {
        if (outline != null) {
            outline.enabled = false;
        }
    }
}
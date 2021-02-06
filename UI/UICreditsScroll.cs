using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UICreditsScroll : MonoBehaviour {

    private readonly float SCROLL_SPEED = 0.07f;
    private float maxScroll;
    private ScrollRect myScrollRect;
    private RectTransform contenRectTransform;
    private Coroutine scroll = null;

    private void Awake() {
        myScrollRect = GetComponent<ScrollRect>();
        contenRectTransform = myScrollRect.content;
        contenRectTransform.anchoredPosition = Vector2.zero;
        maxScroll = contenRectTransform.rect.height - myScrollRect.GetComponent<RectTransform>().rect.height;
        Debug.Log(maxScroll);
    }

    private void OnEnable() {
        UIHolder.instance.credits.OnShowUI += OnScrollStart;
    }

    private void OnDisable() {
        UIHolder.instance.credits.OnShowUI -= OnScrollStart;
    }

    IEnumerator Scroll(float startHeight, float endHeight) {
        float progress = 0;

        Vector2 startVector = new Vector2(0, startHeight);
        Vector2 endVector = new Vector2(0, endHeight);

        Debug.Log($"Starting scroll from {startHeight} to {endHeight}");

        while (progress <= 1 && !GameManager.paused) {
            contenRectTransform.anchoredPosition = Vector2.Lerp(startVector, endVector, progress);
            progress += Time.deltaTime * SCROLL_SPEED;
            Debug.Log(contenRectTransform.position);
            Debug.Log("Scroll progress: " + progress * 100 + "%");
            yield return null;
        }

        OnScrollEnd();
    }

    public void OnScrollStart() {
        scroll = StartCoroutine(Scroll(contenRectTransform.anchoredPosition.y, maxScroll));
    }

    public void OnScrollStop() {
        OnScrollEnd();
    }

    public void OnScrollResume() {
        OnScrollStart();
    }

    public void OnScrollEnd() {
        if (scroll != null)
            StopCoroutine(scroll);
    }

}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderFade : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public float fadeSpeed = 5f;
    private bool isMouseOver = false;

    void Start()
    {
        // Get or add a CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Start hidden
        canvasGroup.alpha = 0;
    }

    public void OnPointerEnter()
    {
        isMouseOver = true;
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void OnPointerExit()
    {
        isMouseOver = false;
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f); // Optional delay before fading out
        while (canvasGroup.alpha > 0 && !isMouseOver)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class FadeInDelay : MonoBehaviour
{

    public float delay;
    public float alphaTarget = 1f;
    public float animTime = 6f;
    public AnimationCurve curve;
    private Text text;
    private CanvasGroup group;

    // Use this for initialization
    void Awake()
    {
        text = GetComponent<Text>();
        group = GetComponent<CanvasGroup>();
    }

    void OnEnable()
    {
        StartCoroutine(FadeIn());
    }

    void OnDisable()
    {
        StopAllCoroutines();     
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(delay);
        if (text != null)
            text.DOFade(alphaTarget, animTime).SetEase(curve);
        else if (group != null)
            group.DOFade(alphaTarget, animTime).SetEase(curve);
    }
}
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class JudgementDisplay : MonoBehaviour
{
    public Image marvelousImage;
    public Image greatImage;
    public Image missImage;
    public float displayTime = 1.0f; // durata più lunga
    public float fadeDuration = 0.5f; // durata dissolvenza

    void Start()
    {
        HideAll();
    }

    public void ShowJudgement(string judgement)
    {
        StartCoroutine(ShowJudgementCoroutine(judgement));
    }

    private IEnumerator ShowJudgementCoroutine(string judgement)
    {
        HideAll();
        Image imgToShow = null;

        switch(judgement)
        {
            case "Marvelous": imgToShow = marvelousImage; break;
            case "Great": imgToShow = greatImage; break;
            case "Miss": imgToShow = missImage; break;
        }

        if (imgToShow == null) yield break;

        imgToShow.gameObject.SetActive(true);
        imgToShow.canvasRenderer.SetAlpha(1f);

        yield return new WaitForSeconds(displayTime);

        // Dissolvenza graduale
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            imgToShow.canvasRenderer.SetAlpha(alpha);
            yield return null;
        }

        imgToShow.gameObject.SetActive(false);
    }

    public void HideAll()
    {
        marvelousImage.gameObject.SetActive(false);
        greatImage.gameObject.SetActive(false);
        missImage.gameObject.SetActive(false);
    }
}

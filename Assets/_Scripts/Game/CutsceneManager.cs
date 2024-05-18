using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Image fadePanel = null;

    public LetterboxController letterbox { get { return FindObjectOfType<LetterboxController>(); } private set { } }
    public MusicManager musicManager { get { return MusicManager.Instance(); } private set { } }

    public float fadeIncrement = 0.25f;
    public float fadeStartDelay = 5f;
    float alphaFade;

    public delegate void FadeInEventHandler(object sender, EventArgs e);
    public event FadeInEventHandler fadeInEventHandler;
    public delegate void FadeOutEventHandler(object sender, EventArgs e);
    public event FadeOutEventHandler fadeOutEventHandler;

    // Delays fade, then fades in panel and music
    public IEnumerator FadeIn(DialogueEventName eventName)
    {
        yield return new WaitForSeconds(fadeStartDelay);
        letterbox.gameObject.SetActive(true);
        alphaFade = 1;

        while (fadePanel.color.a > 0)
        {
            alphaFade -= fadeIncrement * Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alphaFade);
            yield return null;
        }

        fadeInEventHandler?.Invoke(this, EventArgs.Empty);
        letterbox.InitiateDialogue(eventName);
    }

    // Fades music, then panel, delays scene load then loads scene.
    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeStartDelay);
        alphaFade = 0;

        while (fadePanel.color.a < 1)
        {
            alphaFade += fadeIncrement * Time.deltaTime;
            fadePanel.color = new Color(0, 0, 0, alphaFade);
            yield return null;
        }

        yield return new WaitForSeconds(fadeStartDelay);
        fadeOutEventHandler?.Invoke(this, EventArgs.Empty);
    }
}
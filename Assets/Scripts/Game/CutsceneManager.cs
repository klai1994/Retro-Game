using UnityEngine;
using UnityEngine.UI;
using Game.CameraUI.Dialogue;
using Game.Audio;
using System.Collections;
using System;

namespace Game.Levels
{
    public class CutsceneManager : MonoBehaviour
    {
        [SerializeField] Image fadePanel = null;

        public LetterboxController letterbox { get { return FindObjectOfType<LetterboxController>(); } private set { } }
        public MusicManager musicManager { get { return MusicManager.Instance(); } private set { } }

        const float FADE_INCREMENT = 0.25f;
        const float FADE_START_DELAY = 5f;
        float alphaFade;

        public delegate void FadeInEventHandler(object sender, EventArgs e);
        public event FadeInEventHandler fadeInEventHandler;
        public delegate void FadeOutEventHandler(object sender, EventArgs e);
        public event FadeOutEventHandler fadeOutEventHandler;

        // Delays fade, then fades in panel and music
        public IEnumerator FadeIn(DialogueEventName eventName)
        {
            yield return new WaitForSeconds(FADE_START_DELAY);
            letterbox.gameObject.SetActive(true);
            alphaFade = 1;

            while (fadePanel.color.a > 0)
            {
                alphaFade -= FADE_INCREMENT * Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, alphaFade);
                yield return null;
            }

            fadeInEventHandler?.Invoke(this, EventArgs.Empty);
            letterbox.InitiateDialogue(eventName);
        }

        // Fades music, then panel, delays scene load then loads scene.
        public IEnumerator FadeOut()
        {
            yield return new WaitForSeconds(FADE_START_DELAY);
            alphaFade = 0;

            while (fadePanel.color.a < 1)
            {
                alphaFade += FADE_INCREMENT * Time.deltaTime;
                fadePanel.color = new Color(0, 0, 0, alphaFade);
                yield return null;
            }

            yield return new WaitForSeconds(FADE_START_DELAY);
            fadeOutEventHandler?.Invoke(this, EventArgs.Empty);
        }
    }
}
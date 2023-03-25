using UnityEngine;
using UnityEngine.SceneManagement;
using Game.CameraUI.Dialogue;
using System;

namespace Game.Levels
{
    //TODO Create cutscene manager that individual cutscene scripts can extend from
    public class PrologueManager : MonoBehaviour
    {
        [SerializeField] GameObject targetExit = null;
        CutsceneManager cutsceneManager;

        Actors.FollowAI officer;
        NameSelect nameselect;
        const float MUSIC_FADE = 0.01f;
        const int INTRO_SCENE_ID = 1;

        void Start()
        {
            cutsceneManager = FindObjectOfType<CutsceneManager>();
            officer = FindObjectOfType<Actors.FollowAI>();
            nameselect = FindObjectOfType<NameSelect>();
            NameSelect.NotifyNameSelected += StartPrologue;
        }
        
        void StartPrologue()
        {
            cutsceneManager.musicManager.PlayMusic(MusicName.LightIntro, MUSIC_FADE);
            StartCoroutine(cutsceneManager.FadeIn(DialogueEventName.Prologue));
            cutsceneManager.letterbox.eventEnded += EndScene;
        }

        void EndScene(object sender, EventArgs e)
         {
            officer.target = targetExit;
            cutsceneManager.musicManager.StopMusic(MUSIC_FADE);
            StartCoroutine(cutsceneManager.FadeOut());
            cutsceneManager.fadeOutEventHandler += CompletePrologue;
        }

        void CompletePrologue(object sender, EventArgs e)
        {
            SceneManager.LoadScene(INTRO_SCENE_ID);
        }

    }
}
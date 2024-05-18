using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public abstract class GridMenu : MonoBehaviour
{

    [SerializeField] LayoutGroup menuUIFrame = null;
    [SerializeField] Graphic defaultMenuItemPrefab = null;
    [SerializeField] GameObject cursorHandler = null;
    [SerializeField] AudioClip[] soundEffects = null;
    [SerializeField] Vector3 cursorOffset = new Vector3(-30f, -5f, 0);
    [SerializeField] Animator cursorAnimator;

    protected enum CursorSounds
    {
        Select = 0,
        CannotSelect = 1,
        Move = 2,
        Confirm = 3,
        Default = 4
    }
    protected Graphic[,] menuGrid;
    protected Graphic selectedMenuItem;

    Vector2 selectIndexPointer = Vector2.zero;
    AudioSource audioSource;

    protected const string SELECT_TRIGGER = "Select";
    protected const string CANNOT_SELECT_TRIGGER = "Cannot_Select";
    const float KEY_DELAY = 0.2f;
    float timePassedSinceKey = 0;

    protected void Init(int sizeX, int sizeY, UnityAction populateAction = null)
    {
        audioSource = GetComponent<AudioSource>();
        menuGrid = new Graphic[sizeX, sizeY];

        if (populateAction != null)
        {
            populateAction();
        }
        selectedMenuItem = menuGrid[0, 0];
    }

    protected virtual void AddGridMenuItem(int x, int y)
    {
        menuGrid[x, y] = Instantiate(defaultMenuItemPrefab, menuUIFrame.transform);
    }

    protected void PlayCursorAnim(string trigger)
    {
        cursorAnimator.SetTrigger(trigger);
    }

    protected void PlayAudio(CursorSounds soundToPlay)
    {
        audioSource.PlayOneShot(soundEffects[(int)soundToPlay]);
    }
    protected void ProcessKeyInput()
    {
        int xMax = menuGrid.GetLength(0) - 1;
        int yMax = menuGrid.GetLength(1) - 1;

        if (timePassedSinceKey > KEY_DELAY)
        {
            // Y values are reversed because the grid opens downwards
            if (Input.GetKey(KeyCode.W))
            {
                if (selectIndexPointer.y > 0)
                {
                    selectIndexPointer.y -= 1;
                }
                else
                {
                    selectIndexPointer.y = yMax;
                }
                MoveArrow();
            }

            else if (Input.GetKey(KeyCode.S))
            {
                if (selectIndexPointer.y < yMax)
                {
                    selectIndexPointer.y += 1;
                }
                else
                {
                    selectIndexPointer.y = 0;
                }

                MoveArrow();
            }

            else if (Input.GetKey(KeyCode.D))
            {
                if (selectIndexPointer.x < xMax)
                {
                    selectIndexPointer.x += 1;
                }
                else
                {
                    selectIndexPointer.x = 0;
                    if (selectIndexPointer.y < yMax)
                    {
                        selectIndexPointer.y += 1;
                    }
                }

                MoveArrow();
            }

            else if (Input.GetKey(KeyCode.A))
            {
                if (selectIndexPointer.x > 0)
                {
                    selectIndexPointer.x -= 1;
                }
                else
                {
                    selectIndexPointer.x = xMax;
                    if (selectIndexPointer.y > 0)
                    {
                        selectIndexPointer.y -= 1;
                    }
                }
                MoveArrow();

            }
        }
        timePassedSinceKey += Time.deltaTime;
    }

    void MoveArrow()
    {
        timePassedSinceKey = 0;
        PlayAudio(CursorSounds.Move);
        selectedMenuItem = menuGrid[(int)selectIndexPointer.x, (int)selectIndexPointer.y];
        cursorHandler.transform.position = selectedMenuItem.transform.position + cursorOffset;
    }

}
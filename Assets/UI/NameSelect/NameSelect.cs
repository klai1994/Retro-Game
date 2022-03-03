using UnityEngine;
using UnityEngine.UI;
using Game.CameraUI;

namespace Game.Levels
{
    public class NameSelect : GridMenu
    {
        public delegate void BroadcastNameSelected();
        public event BroadcastNameSelected NotifyNameSelected;

        [SerializeField] Text nameSelection = null;
        int charIndex;
       
        const int MAX_NAME_LENGTH = 13;
        const int GRID_SIZE_X = 7;
        const int GRID_SIZE_Y = 8;

        void Start()
        {
            Init(GRID_SIZE_X, GRID_SIZE_Y, PopulateLetterGrid);
        }

        void Update()
        {
            ProcessKeyInput();
            ProcessCommandInput();
        }

        void ProcessCommandInput()
        {
            // Select default name
            if (Input.GetKeyDown(KeyCode.V))
            {
                PlayAudio(CursorSounds.Confirm);
                nameSelection.text = "";
            }

            // Select character
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (nameSelection.text.Length < MAX_NAME_LENGTH)
                {
                    PlayAudio(CursorSounds.Select);
                    nameSelection.text += ((Text)selectedMenuItem).text;
                    PlayCursorAnim(SELECT_TRIGGER);
                }
                else
                {
                    PlayAudio(CursorSounds.CannotSelect);
                }
            }

            // Backspace
            if (Input.GetKeyDown(KeyCode.X))
            {
                PlayAudio(CursorSounds.CannotSelect);

                if (nameSelection.text != "")
                {
                    nameSelection.text = nameSelection.text.Substring(0, nameSelection.text.Length - 1);
                }
            }

            // Complete name selection
            if (Input.GetKeyDown(KeyCode.Return))
            {
                string selectedName = nameSelection.text.Trim();
                if (selectedName == "")
                {
                    PlayAudio(CursorSounds.CannotSelect);
                    nameSelection.text = "";
                    return;
                }

                PlayerData.GetPlayerData().Init(selectedName);
                PlayAudio(CursorSounds.Confirm);

                NotifyNameSelected();
                gameObject.transform.SetParent(Camera.main.transform);
                Destroy(this);
            }

        }

        protected override void AddGridMenuItem(int x, int y)
        {
            base.AddGridMenuItem(x, y);
            ((Text)menuGrid[x, y]).text += (char)charIndex;
        }

        void PopulateLetterGrid()
        {
            // Refer to ASCII table for char values
            const int SPACE = 32;
            const int UPPER_START = 65;
            const int UPPER_END = 90;
            const int LOWER_START = 97;
            const int LOWER_END = 122;

            charIndex = UPPER_START;

            for (int y = 0; y < GRID_SIZE_Y; y++)
            {
                for (int x = 0; x < GRID_SIZE_X; x++)
                {

                    // Skip ASCII keys between uppercase and lowercase letters
                    if (charIndex == (UPPER_END + 1))
                    {
                        charIndex = LOWER_START;
                    }

                    AddGridMenuItem(x, y);

                    // Begin printing spaces after lowercase letters complete
                    if (charIndex > LOWER_END)
                    {
                        charIndex = SPACE;
                    }

                    charIndex++;

                }
            }
        }

    }
}
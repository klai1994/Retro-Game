using System;
using UnityEngine;
using UnityEngine.UI;


public class NameSelect : GridMenu
{
    public static Action NotifyNameSelected;

    [SerializeField] Text nameSelectionField = null;
    int charIndex;

    int gridSizeX = 7;
    int grixSizeY = 8;
    int maxNameLength = 13;

    void Start()
    {
        Init(gridSizeX, grixSizeY, PopulateLetterGrid);
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
            nameSelectionField.text = "";
        }

        // Select character
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (nameSelectionField.text.Length < maxNameLength)
            {
                PlayAudio(CursorSounds.Select);
                nameSelectionField.text += ((Text)selectedMenuItem).text;
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

            if (nameSelectionField.text != "")
            {
                nameSelectionField.text = nameSelectionField.text.Substring(0, nameSelectionField.text.Length - 1);
            }
        }

        // Complete name selection
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string selectedName = nameSelectionField.text.Trim();
            if (selectedName == "")
            {
                PlayAudio(CursorSounds.CannotSelect);
                nameSelectionField.text = "";
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

        for (int y = 0; y < grixSizeY; y++)
        {
            for (int x = 0; x < gridSizeX; x++)
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
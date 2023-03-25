using System.Collections.Generic;
using UnityEngine;
using LitJson;


/// <summary>
/// The JsonReader extracts the text from a specified dialogue event .json file. 
/// It then translates it to a DialogueEventHolder struct.
/// </summary>
public static class JsonReader
{
    static Object[] dialogueEvents = Resources.LoadAll<Object>("DialogueJson");
    public static DialogueEventHolder GetDialogueEvent(DialogueEventName dialogueEventName)
    {
        int eventIndex = (int)dialogueEventName;
        string jsonData = dialogueEvents[eventIndex].ToString();
        return JsonMapper.ToObject<DialogueEventHolder>(jsonData);
    }
}

/// <summary>
/// This class is what .json dialogue event files are transformed into by LitJson. 
/// The list is the root node for each file, and each DialogueEventInfo struct
/// represents a child node to the root node of the file.
/// </summary>
public class DialogueEventHolder
{
    // Name must match root node in .json file
    public List<DialogueEventInfo> eventInfoList;
}

public struct DialogueEventInfo
{
    public string characterPortrait;
    public string dialogueText;
    public string nameText;
    public string voice;

    public string DialogueText
    {
        get
        {
            return dialogueText.Replace("@", PlayerData.GetPlayerData().ActorName);
        }
    }
}

// Values must correlate directly with order of files found in Resources/DialogueEvents
public enum DialogueEventName
{
    // TODO implement more manageable way of matching files to values
    Prologue,

    #region Sandbox Events
    SandboxEvent1,
    SandboxEvent2,
    SandboxEvent3,
    SandboxEvent4,
    SandboxEvent5
    #endregion
}

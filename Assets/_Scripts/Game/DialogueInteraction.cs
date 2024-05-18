using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DialogueInteraction : MonoBehaviour
{
    [SerializeField] DialogueEventName[] eventNames = null;

    [SerializeField] float interactionDistance = 2.5f;
    [SerializeField] bool canInteractWith = false;

    [SerializeField] PlayerAvatarControl player;

    // The index of text events for this particular actor
    int eventIndex;
    public int EventIndex
    {
        get { return eventIndex; }
        set
        {
            // Goes to next dialogue event to be prompted on the next initializaion
            if (value <= eventNames.Length - 1)
            {
                eventIndex = value;
            }

        }
    }

    void Start()
    {
        Init();
    }

    void OnDestroy()
    {
        PlayerAvatarControl.BroadcastPlayerInteraction -= Interact;
    }

    // Ensures subclasses can subscribe to the player input broadcast delegate
    protected void Init()
    {
        player = PlayerAvatarControl.GetPlayerInstance();
        canInteractWith = eventNames.Length > 0;

        if (canInteractWith)
        {
            PlayerAvatarControl.BroadcastPlayerInteraction -= Interact;
            EventIndex = 0;
            PlayerAvatarControl.BroadcastPlayerInteraction += Interact;
        }
    }

    public float GetDistance(GameObject target)
    {
        return Vector2.Distance(target.transform.position, transform.position);
    }

    // Used for interactable dialogue in overworld, otherwise call DialogueSystem.InitiateDialogue directly
    void Interact()
    {   
        ActorAvatar playerAvatar = player.GetComponent<ActorAvatar>();
        if (PlayerAvatarControl.PlayerIsFree && GetDistance(player.gameObject) < interactionDistance)
        {
            playerAvatar.FaceDirection(transform.position);
            // If interactable object is an avatar, face player
            if (GetType() == typeof(ActorAvatar))
            {
                ((ActorAvatar)this).FaceDirection(playerAvatar.transform.position);
            }

            LetterboxController.letterboxController.InitiateDialogue(eventNames[EventIndex]);
            EventIndex++;
        }
    }

}
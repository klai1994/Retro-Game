using System;
using UnityEngine;


[RequireComponent(typeof(ActorAvatar))]
public class PlayerAvatarControl : MonoBehaviour
{
    public static Action BroadcastPlayerInteraction;
    ActorAvatar avatar;
    static PlayerAvatarControl playerInstance;

    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    static bool playerIsFree;
    public static bool PlayerIsFree
    {
        get
        {
            // If there is no PlayerAvatarControl in the scene, it is true by default.
            if (!FindObjectOfType<PlayerAvatarControl>())
            {
                playerIsFree = true;
            }

            return playerIsFree;
        }
        set
        {
            Rigidbody2D rbody = GetPlayerInstance().GetComponent<Rigidbody2D>();

            // Locks player's avatar if value is false.
            if (value == true)
            {
                rbody.constraints = RigidbodyConstraints2D.None;
                rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }
            else
            {
                rbody.constraints = RigidbodyConstraints2D.FreezePositionX
                    | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            }

            playerIsFree = value;
        }
    }

    public static PlayerAvatarControl GetPlayerInstance()
    {
        if (!playerInstance)
        {
            playerInstance = FindObjectOfType<PlayerAvatarControl>();
        }
        return playerInstance;
    }

    // Use this for initialization
    void Awake()
    {
        avatar = GetComponent<ActorAvatar>();
        PlayerIsFree = true;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsFree = !(LetterboxController.letterboxController.EventOccuring);
        if (Input.GetKeyDown(KeyCode.C) && BroadcastPlayerInteraction != null)
        {
            BroadcastPlayerInteraction();
        }
        avatar.MoveAvatar(new Vector2(Input.GetAxisRaw(HORIZONTAL_AXIS), Input.GetAxisRaw(VERTICAL_AXIS)));
    }

}
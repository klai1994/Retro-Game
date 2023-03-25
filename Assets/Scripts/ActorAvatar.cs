using System.Collections;
using UnityEngine;

namespace Game.Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class ActorAvatar : DialogueInteraction
    {
        [SerializeField] float movementSpeed = 1.5f;
        Rigidbody2D rbody;
        Animator animator;

        const string ANIM_IS_WALKING = "isWalking";
        const string MOVEMENT_X = "movement_x";
        const string MOVEMENT_Y = "movement_y";
        public static float MovementScale = 0.10f;

        [SerializeField] bool isIdle = false;
        [SerializeField] float idleTurnRate = 3f;

        void Start()
        {
            rbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            // Starts avatar facing down
            animator.SetFloat(MOVEMENT_Y, -1f);

            if (isIdle)
            {
                StartCoroutine(IdleActions());
            }

            base.Init();
        }

        void Update()
        {
            // Ensures sprites display on top of eachother properly
            SpriteRenderer renderer = GetComponent<SpriteRenderer>(); 
            renderer.sortingOrder = (int)(-transform.position.y);
        }

        public void MoveAvatar(Vector2 moveDirection)
        {
            if (moveDirection != Vector2.zero && PlayerAvatarControl.PlayerIsFree)
            {
                animator.SetBool(ANIM_IS_WALKING, true);
                SetAnimatorDirection(moveDirection);
                rbody.MovePosition(rbody.position + moveDirection * (movementSpeed * MovementScale));
            }
            else
            {
                animator.SetBool(ANIM_IS_WALKING, false);
            }

        }

        public IEnumerator IdleActions()
        {
            while (isIdle)
            {
                if (PlayerAvatarControl.PlayerIsFree)
                {
                    animator.SetFloat(MOVEMENT_X, Random.Range(-1f, 1f));
                    animator.SetFloat(MOVEMENT_Y, Random.Range(-1f, 1f));
                    yield return new WaitForSeconds(Random.Range(1, idleTurnRate));
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        public void FaceDirection(Vector3 target)
        {
            // Set a target or create a new vector to face its direction. Ensures the actor is not walking to keep its direction.
            animator.SetBool(ANIM_IS_WALKING, false);
            SetAnimatorDirection((target - transform.position).normalized);
        }

        void SetAnimatorDirection(Vector2 direction)
        {
            animator.SetFloat(MOVEMENT_X, direction.x);
            animator.SetFloat(MOVEMENT_Y, direction.y);
        }

    }
}
using UnityEngine;


    [RequireComponent(typeof(ActorAvatar))]
    public class FollowAI : MonoBehaviour
    {
        ActorAvatar avatar;
        Vector3 startingPosition;
        public GameObject target;
        
        [SerializeField] float maxChaseDistance = 5f;
        [SerializeField] float destroyThreshold = 20f;
        public float stoppingDistance = 2f;

        public bool canChase = false;

        // Use this for initialization
        void Start()
        {
            avatar = GetComponent<ActorAvatar>();
            startingPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (target)
            {
                this.canChase = (target && avatar.GetDistance(target) > stoppingDistance && avatar.GetDistance(target) < maxChaseDistance);
                if (this.canChase == true)
                {
                avatar.MoveAvatar((target.transform.position - transform.position).normalized);
                }
                else
                {
                    avatar.MoveAvatar(Vector2.zero);
                }
                ResetAfterThreshold();
            }
        }

        void ResetAfterThreshold()
        {
            if (avatar.GetDistance(target) > destroyThreshold)
            {
                transform.position = startingPosition;
            }
        }

        public void ResetTarget()
        {
            target = gameObject;
        }

    }

using System;
using UnityEngine;

namespace Game
{
    public class ActorStats : ScriptableObject
    {
        [SerializeField] string actorName = null;
        public virtual void Init(string actorName)
        {
            ActorName = actorName;
        }
       
        public string ActorName
        {
            get
            {
                return actorName;
            }
            set
            {
                if (actorName != null && actorName != value)
                {
                    Debug.LogWarning(string.Format("Actor name changing from {0} to {1}!", actorName, value));
                }
                actorName = value;
                
            }
        }
       
    }
}
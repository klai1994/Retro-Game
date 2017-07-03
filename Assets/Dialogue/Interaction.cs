﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game.Actors;

namespace Game.Dialogue
{
    public class Interaction : MonoBehaviour
    {

        [SerializeField] DialogueEventName[] eventNames;
        [SerializeField] float interactionDistance = 2.5f;
        int interactionIndex = 0;
        Player player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C) && GetDistanceToPlayer() < interactionDistance)
            {
                Interact();
            }
        }

        void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactionDistance);    
        }

        float GetDistanceToPlayer ()
        {
            return (player.transform.position - transform.position).magnitude;
        }

        public void Interact()
        {
            player.StartedDialogue = true;
            player.FaceDirection(transform.position);

            DialogueControlHandler.InitializeEvent(eventNames[interactionIndex]);

            if (interactionIndex < eventNames.Length - 1)
            {
                interactionIndex++;
            }
        }

    }
}

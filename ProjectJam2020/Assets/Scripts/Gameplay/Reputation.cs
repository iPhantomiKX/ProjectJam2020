﻿using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using RPG.Saving;
using RPG.Stats;
using UnityEngine;

namespace RPG.Gameplay
{
    public class Reputation : MonoBehaviour, ISaveable
    {
        [SerializeField]int reputation;
        [SerializeField] float influenceDistance = 10f;
        [SerializeField]List<AllyAIController> allies;
        float distanceToAlly;


        private void Start()
        {
            reputation = (int)gameObject.GetComponent<BaseStats>().GetStat(Stat.Reputation);
        }

        private void Update()
        {
            GetAlly();
        }

        private void GetAlly()
        {
            foreach(var ally in GameObject.FindObjectsOfType<AllyAIController>())
            {
                if(ally == null)
                {
                    print("No Ally Nearby");
                    return;
                }

                distanceToAlly = Vector3.Distance(ally.transform.position, gameObject.transform.position);

                if(distanceToAlly <= influenceDistance 
                && Input.GetKeyDown(gameObject.GetComponent<PlayerController>().AddAllyButton))
                {
                    if(!ally.added 
                    && reputation >= (int)ally.GetComponent<BaseStats>().GetStat(Stat.Reputation) )
                    {
                        allies.Add(ally);
                        AddReputation((int)ally.GetComponent<BaseStats>().GetStat(Stat.Reputation));
                        ally.added = true;
                    }
                }
            }
        }

        int AddReputation(int allyReputation)
        {
            return reputation += allyReputation;
        }

        public int GetReputation()
        {
            return reputation;
        }

        public object CaptureState()
        {
            return reputation;
        }

        public void RestoreState(object state)
        {
            reputation = (int)state;

            if (reputation <= 0)
            {
                //GameOver
            }
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(transform.position, influenceDistance);
        }
    }
}


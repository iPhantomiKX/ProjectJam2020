using System.Collections;
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
        [SerializeField]AllyAIController[] allies;

        private void Awake()
        {
            reputation = 0;
            allies = GameObject.FindObjectsOfType<AllyAIController>();
        }

        private void Update()
        {
            allies = GameObject.FindObjectsOfType<AllyAIController>();

            foreach(var ally in allies)
            {
                if(!ally.added)
                {
                    AddReputation((int)ally.GetComponent<BaseStats>().GetStat(Stat.Reputation));
                    ally.added = true;
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
    }
}


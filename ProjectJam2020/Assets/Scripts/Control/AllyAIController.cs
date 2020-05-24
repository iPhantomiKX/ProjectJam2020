using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Movement;
using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AllyAIController : AIController
    {
        PlayerController player;
        public bool added;
        public override void Awake()
        {
            base.Awake();
            player = GameObject.FindObjectOfType<PlayerController>();
            GetAllEnemies(GameObject.FindGameObjectsWithTag("Enemy"));
        }
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            if(GetComponent<BaseStats>().GetStat(Stat.Reputation) > player.GetComponent<BaseStats>().GetStat(Stat.Reputation))
            {
                enemies.Add(player.gameObject);
                gameObject.AddComponent<CombatTarget>();
            }
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            GetAllEnemies(GameObject.FindGameObjectsWithTag("Enemy"));

            if (IsAggrevated() && fighter.CanAttack(ClosestEnemy(enemies)))
            {
                AttackBehaviour();
            }
            if(GameObject.FindGameObjectsWithTag("Zombie").Length >= 1)
            {
                //Runaway
                print(gameObject.name + " runaway");
                //Exit the area
                FollowPlayer(GameObject.FindObjectOfType<PlayerController>(), 2f);
            }
            if(player != null)
            {
                if(player.gameObject.GetComponent<NavMeshAgent>().remainingDistance < player.gameObject.GetComponent<NavMeshAgent>().stoppingDistance || !added)
                {
                    gameObject.GetComponent<Mover>().Cancel();
                }
                else
                {
                    FollowPlayer(GameObject.FindObjectOfType<PlayerController>(), 1f);
                }
            }
        }

        void FollowPlayer(PlayerController player, float speedFraction)
        {
            this.GetComponent<Mover>().StartMoveAction(player.gameObject.transform.position, speedFraction);
        }
    }
}


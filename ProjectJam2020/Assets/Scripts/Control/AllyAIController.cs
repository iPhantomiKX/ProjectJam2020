using System.Collections;
using System.Collections.Generic;
using RPG.Movement;
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
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            player = GameObject.FindObjectOfType<PlayerController>();
        }
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            enemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(ClosestEnemy(enemies)))
            {
                AttackBehaviour();
            }
            if(GameObject.FindGameObjectsWithTag("Zombie").Length >= 1)
            {
                //Runaway
                print(gameObject.name + " runaway");
                FollowPlayer(GameObject.FindObjectOfType<PlayerController>(), 2f);
            }
            if(player.gameObject.GetComponent<NavMeshAgent>().remainingDistance < player.gameObject.GetComponent<NavMeshAgent>().stoppingDistance || !added)
            {
                gameObject.GetComponent<Mover>().Cancel();
            }
            else
            {
                FollowPlayer(GameObject.FindObjectOfType<PlayerController>(), 1f);
            }
        }

        void FollowPlayer(PlayerController player, float speedFraction)
        {
            this.GetComponent<Mover>().StartMoveAction(player.gameObject.transform.position, speedFraction);
        }


    }
}


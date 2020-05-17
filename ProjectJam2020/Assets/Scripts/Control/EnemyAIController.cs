using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAIController : AIController
    {
         public override void Awake()
        {
            base.Awake();
            enemies = GameObject.FindGameObjectsWithTag("Ally");
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
            enemies = GameObject.FindGameObjectsWithTag("Ally");

            if (health.IsDead()) return;

            if (IsAggrevated() && fighter.CanAttack(ClosestEnemy(enemies)))
            {
                AttackBehaviour();
            }
            if(GameObject.FindGameObjectsWithTag("Zombie").Length >= 1)
            {
                //Runaway
                print(gameObject.name + " runaway");
            }
            else
            {
                PatrolBehaviour();
            }
        }
    }
}


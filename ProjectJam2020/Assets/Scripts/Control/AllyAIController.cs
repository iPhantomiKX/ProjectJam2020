using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AllyAIController : AIController
    {
        public override void Awake()
        {
            base.Awake();
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
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

            if (fighter.CanAttack(ClosestEnemy(enemies)))
            {
                AttackBehaviour();
            }
            else
            {
                //PatrolBehaviour();
            }
        }
    }
}


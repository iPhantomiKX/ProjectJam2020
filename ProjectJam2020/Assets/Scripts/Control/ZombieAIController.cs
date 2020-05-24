using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace RPG.Control
{
    public class ZombieAIController : AIController
    {
        public GameObject[] playerAllies;
        public GameObject[] playerEnemies;

         public override void Awake()
        {
            base.Awake();
            playerAllies = GameObject.FindGameObjectsWithTag("Ally");
            playerEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            
        }
        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            GetAllEnemies(playerAllies.Concat(playerEnemies).ToArray());
            
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
            GetAllEnemies(playerAllies.Concat(playerEnemies).ToArray());

            if (health.IsDead()) return;

            if (fighter.CanAttack(ClosestEnemy(enemies)))
            {
                AttackBehaviour();
            }
        }
    }
}

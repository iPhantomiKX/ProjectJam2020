using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using UnityEngine;

namespace RPG.Control
{
    public class EnemyAIController : AIController
    {
         public override void Awake()
        {
            base.Awake();
            GetAllEnemies(GameObject.FindGameObjectsWithTag("Ally"));
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
            //GetAllEnemies(GameObject.FindGameObjectsWithTag("Ally"));
            foreach(var enemy in enemies)
            {
                if(!enemy.GetComponent<Health>().IsDead() && enemies.IndexOf(enemy) < 0)
                {
                    enemies.Add(enemy);
                }
                if(enemy.GetComponent<Health>().IsDead())
                {
                    enemies.Remove(enemy);
                }
            }

            if (fighter.CanAttack(ClosestEnemy(enemies)))
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


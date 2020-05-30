using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RPG.Attributes;

namespace RPG.Control
{
    public class ZombieAIController : AIController
    {
        [SerializeField] GameObject[] playerAllies;
        [SerializeField] GameObject[] playerEnemies;
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

            AttackBehaviour();

            print(gameObject.name + " : " + fighter.GetTarget());
        }
    }
}

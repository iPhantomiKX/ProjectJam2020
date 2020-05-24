using RPG.Combat;
using UnityEngine;
using RPG.Core;
using RPG.Movement;
using RPG.Attributes;
using RPG.Utils;
using System;
using System.Collections.Generic;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] public float suspicionTime  = 3f;
        [SerializeField] float aggroCooldownTime = 5f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float waypointDwellTime = 3f;
        [SerializeField] float shoutDistance = 5f;

        
        public Fighter fighter;
        public Health health;
        public Mover mover;
        public List<GameObject> enemies;

        LazyValue<Vector3> guardPosition;
        float timeSinceLastSawEnemy = Mathf.Infinity;
        float timeSinceArrivedAtWaypoint = Mathf.Infinity;
        float timeSinceAggrevated = Mathf.Infinity;
        int currentWaypointIndex = 0;
        float distanceToEnemy = 0;

        public virtual void Awake()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = new LazyValue<Vector3>(GetGuardPosition);
        }

        public Vector3 GetGuardPosition()
        {
            return transform.position;
        }

        public virtual void Start()
        {
            guardPosition.ForceInit();
        }

        public virtual void Update()
        {
            if(health.IsDead()) return;
            UpdateTimer();
        }

        public void AddEnemyToList(GameObject Enemy)
        {
            enemies.Add(Enemy);
        }

        public void GetAllEnemies(GameObject[] enemies)
        {
            foreach(var enemy in enemies)
            {
                AddEnemyToList(enemy);
            }
        }

        public GameObject ClosestEnemy(List<GameObject> _enemies)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject t in enemies)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
            return tMin;
        }

        public void Aggrevate()
        {
            // Set timer
            timeSinceAggrevated = 0;
        }

        public void UpdateTimer()
        {
            timeSinceLastSawEnemy += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        public void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition.value;

            if (patrolPath != null)
            {
                if(AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            if (timeSinceArrivedAtWaypoint > waypointDwellTime)
                mover.StartMoveAction(nextPosition, 1f);
        }

        public bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWayPoint());
            return distanceToWaypoint < waypointTolerance;
        }

        public Vector3 GetCurrentWayPoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        public void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        public void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public void AttackBehaviour()
        {
            timeSinceLastSawEnemy = 0;
            foreach(var enemy in enemies)
            {
                fighter.Attack(ClosestEnemy(enemies));
            }

            AggrevateNearbyAllies();
        }

        public void AggrevateNearbyAllies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            // Loop over all the hits
            foreach(RaycastHit hit in hits)
            {
                // Find any enemy components
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                
                // aggrevate those enemies
                ai.Aggrevate();
            }
        }

        public bool IsAggrevated()
        {
            foreach(var enemy in enemies)
            {
                if(enemy == null)  
                {
                    return false;
                }
                distanceToEnemy = Vector3.Distance(enemy.transform.position, transform.position);
                // check aggrevated
            }
            return distanceToEnemy < chaseDistance || timeSinceAggrevated < aggroCooldownTime;
            
        }

        //Called By Unity
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, shoutDistance);
        }
    }
}

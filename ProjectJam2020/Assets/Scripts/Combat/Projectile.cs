using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using RPG.Combat;
using RPG.Attributes;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 1;
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
        [SerializeField] float maxLifeTime = 10;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] float lifeAfterImpact = 2;
        [SerializeField] UnityEvent onHit;

    Health target = null;
    GameObject instigator = null;
    float damage = 0;

    void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        if(isHoming && !target.IsDead())
            transform.LookAt(GetAimLocation());

        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    public void SetTarget( Health target, GameObject instigator, float damage)
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;


        Destroy(gameObject, maxLifeTime);
    }

    public Health GetTarget()
    {
        return target;
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (target == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Health>() != target) return;
        if (target.IsDead()) return;
        target.TakeDamage(instigator, damage);

        speed = 0;

        onHit.Invoke();

        StartCoroutine(GameObject.FindGameObjectWithTag("Player").GetComponent<Fighter>().OnHitEffect());

        foreach (GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }

        Destroy(gameObject, lifeAfterImpact);

    }
}

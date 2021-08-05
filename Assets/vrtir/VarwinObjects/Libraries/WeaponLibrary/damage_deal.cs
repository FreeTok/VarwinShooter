using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_deal : MonoBehaviour
{
    public int damage_per_hit = 10;
    [SerializeField] private float cooldownTime = 1;

    private float cooldownTimer = 0;
    private bool canAttack = true;

    private void Update()
    {
        if(cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }
        else
        {
            canAttack = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canAttack) return;

        damage_get handler = other.GetComponent<damage_get>();
        if (handler)
        {
            handler.TakeDamage(damage_per_hit);
            cooldownTimer = cooldownTime;
            canAttack = false;
        }
    }
}

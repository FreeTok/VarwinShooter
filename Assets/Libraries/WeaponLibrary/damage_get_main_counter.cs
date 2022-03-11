using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Varwin;
using Varwin.Public;

public class damage_get_main_counter : MonoBehaviour
{
    public List<GameObject> deathparticles;

    [SerializeField] private List<damage_get> damage_handlers;

    public float maxHealth = 100f;
    private float health;

    public TextMeshProUGUI HPText;

    public float DieTime = 2;
    
    public delegate void DieEventHandler();

    [Event(English: "die event")]
    public event DieEventHandler dieEvent;

    public GameObject mesh;
    private Animator animator;
    public float damageAnimDuration = 3f;

    private void Start()
    {
        animator = mesh.GetComponent<Animator>();
        
        ResetHealth();
        HPText.text = health.ToString();
        foreach(damage_get handler in damage_handlers)
        {
            handler.Counter = this;
        }
    }

    private void FixedUpdate()
    {
        if (CameraManager.CurrentCamera)
        {
            HPText.transform.LookAt(CameraManager.CurrentCamera.transform);
        }
    }

    public void TakeDamage(float damage, float damageMultiplier)
    {
        damage *= damageMultiplier;
        
        if (damage < 0) throw new System.Exception("Damage count must be non negative!");
        
        health -= damage;
        
        if (health > 0)
        {
            PlayDamageAnim();
            HPText.text = health.ToString();
        }

        else
        {
            HPText.text = "0";
            
            dieEvent.Invoke();
            Debug.Log("Ragdoll_dead");

            foreach (GameObject particle in deathparticles)
            {
                particle.SetActive(true);
            }
        }
    }

    [Action(English: "reset health")]
    public void ResetHealth()
    {
        health = maxHealth;
        HPText.text = health.ToString();
    }

    void PlayDamageAnim()
    {
        print("Playing anim");
        animator.SetTrigger("Damaged");

        Invoke(nameof(SetWalkingEnemy), damageAnimDuration);
    }

    void SetWalkingEnemy()
    {
        print("reset trigger");
        animator.ResetTrigger("Damaged");
    }
}

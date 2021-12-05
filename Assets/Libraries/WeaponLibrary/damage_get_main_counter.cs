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

    public GameObject damagedEnemy, wallkingEnemy;
    public float damageAnimDuration = 2.2f;

    public void TakeDamage(float damage, float damageMultiplier)
    {
        damage *= damageMultiplier;
        
        if (damage < 0) throw new System.Exception("Damage count must be non negative!");
        
        health -= damage;
        
        if (health > 0)
        {
            SetWalkOrDamage(true);
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

    private void Start()
    {
        damagedEnemy.SetActive(false);
        wallkingEnemy.SetActive(true);
        ResetHealth();
        HPText.text = health.ToString();
        foreach(damage_get handler in damage_handlers)
        {
            handler.Counter = this;
        }
    }

    [Action(English: "reset health")]
    public void ResetHealth()
    {
        health = maxHealth;
        HPText.text = health.ToString();
    }

    void SetWalkOrDamage(bool isDamaged)
    {
        print("Playing anim");
        damagedEnemy.SetActive(isDamaged);
        wallkingEnemy.SetActive(!isDamaged);

        Invoke(nameof(SetWalkingEnemy), damageAnimDuration);
    }

    void SetWalkingEnemy()
    {
        damagedEnemy.SetActive(false);
        wallkingEnemy.SetActive(true);
    }
}

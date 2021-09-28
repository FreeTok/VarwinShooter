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

    public GameObject mob;

    [SerializeField] private List<damage_get> damage_handlers;

    [SerializeField] private float health = 100;

    public TextMeshProUGUI HPText;

    public float DieTime = 2;
    
    public delegate void DieEventHandler();
    
    [Event(English: "die event")]
    public event DieEventHandler dieEvent;

    public void TakeDamage(float damage, float damageMultiplier)
    {
        damage *= damageMultiplier;
        
        if (damage < 0) throw new System.Exception("Damage count must be non negative!");
        
        health -= damage;
        
        if (health > 0)
        {
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
            
            Destroy(mob, DieTime);
        }
    }

    private void Start()
    {
        HPText.text = health.ToString();
        foreach(damage_get handler in damage_handlers)
        {
            handler.Counter = this;
        }
    }
}

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

    public float MaxHealth = 100;
    private float health;

    public TextMeshProUGUI HPText;

    public float DieTime = 2;
    
    public delegate void DieEventHandler();

    [Event(English: "die event")]
    public event DieEventHandler dieEvent;

    [Action("Reset Health")]
    [Locale(SystemLanguage.English,"reset health")]
    public void ResetHealth()
    {
        health = MaxHealth;
        HPText.text = health.ToString();
    }

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
            
            Debug.Log("Ragdoll_dead");

            foreach (GameObject particle in deathparticles)
            {
                Instantiate(particle);
            }

            KillEnemy(gameObject.GetWrapper());
            dieEvent?.Invoke();
        }
    }

    void KillEnemy(Wrapper Enemy)
    {
        Enemy.Deactivate();
    }

    private void Start()
    {
        ResetHealth();
        foreach(damage_get handler in damage_handlers)
        {
            handler.Counter = this;
        }
    }
}

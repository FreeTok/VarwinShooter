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

    private Rigidbody rigd;

    private MeshRenderer rendm;

    // public GameObject me_self_monster;

    [SerializeField] private List<damage_get> damage_handlers;

    [SerializeField] private float health = 100;

    public TextMeshProUGUI HPText;

    public void TakeDamage(float damage)
    {
        if (damage < 0) throw new System.Exception("Damage count must be non negative!");
        health -= damage;
        HPText.text = health.ToString();
        if (health < 0) health = 0;
        if (health == 0)
        {
            Debug.Log("Ragdoll_dead");

            rigd = gameObject.GetComponent<Rigidbody>();

            rigd.isKinematic = true;

            deathparticles.ForEach(x => x.SetActive(true));

            rendm = gameObject.GetComponent<MeshRenderer>();

            rendm.enabled = false;
            Destroy(mob, 2);
            // Destroy(me_self_monster, 6);
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

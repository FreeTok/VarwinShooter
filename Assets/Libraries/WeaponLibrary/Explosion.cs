using UnityEngine;

public class Explosion : MonoBehaviour
{
    public string element;
    private void Awake()
    {
        InvokeRepeating("explosion",0,.01f);
    }
    void explosion()
    {
        if (transform.localScale.x < 15f)
            transform.localScale += Vector3.one*.1f;
        else
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        damage_get handler = other.gameObject.GetComponent<damage_get>();
        if (handler)
        {
            handler.TakeDamage(10f, element);
        }
    }
}

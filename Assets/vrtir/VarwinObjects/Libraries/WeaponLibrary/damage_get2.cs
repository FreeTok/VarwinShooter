using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Varwin;
using Varwin.Public;

namespace WeaponLibrary
{
    public class damage_get2 : VarwinObject
    {
        public float MaxHP;
        private float HP;
        public TextMeshProUGUI hpText;
        
        private void Start()
        {
            HP = MaxHP;
            if (hpText)
            {
                hpText.text = HP.ToString();
            }
        }

        private void FixedUpdate()
        {
            print(HP);
        }

        public void TakeDamage(float damage)
        {
            HPChanged(HP, damage, hpText);
        }
        
        public void HPChanged(float HP, float Damage, TextMeshProUGUI hpText)
        {
            HP -= Damage;
            print(HP);
            if (hpText)
            {
                hpText.text = HP.ToString();
            }
        }
    }
}
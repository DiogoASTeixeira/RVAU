using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    private readonly static float ATTACK_DAMAGE = 0.1f;
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter(Collider collider)
    {
        float damage_mod = 1;
        if(collider.CompareTag("Enemy"))
        {
            PlayerScript.Stance playerStance = playerObject.GetComponent<PlayerScript>().GetStance();
            switch(playerStance)
            {
                case PlayerScript.Stance.Defend:
                    {
                        damage_mod *= 0.03f;
                        break;
                    }
                case PlayerScript.Stance.Attack:
                    {
                        damage_mod *= 1.0f;
                        break;
                    }
            }
            collider.gameObject.GetComponent<EnemyScript>().DamageHealth(ATTACK_DAMAGE * damage_mod);
        }
    }
}

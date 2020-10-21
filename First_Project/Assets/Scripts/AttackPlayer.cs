using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    private readonly static float ATTACK_DAMAGE = 0.1f;

    // Start is called before the first frame update
    void Start(){}

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.tag == "Enemy")
        {
            collider.gameObject.GetComponent<HealthScript>().DamageHealth(ATTACK_DAMAGE);
        }
    }
}

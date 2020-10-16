 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Hero : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onTriggerEnter(Collider collider) {
        if(collider.tag == "Villain")
        {
            collider.gameObject.GetComponent<HealthScript>().DamageHealth(0.1f);
        }
    }
}

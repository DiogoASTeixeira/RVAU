using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanceScript : MonoBehaviour
{
    public PlayerScript.Stance stance;
    public GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        //particleSystem = gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            playerObject.GetComponent<PlayerScript>().ChangeStance(stance);
        }
    }
}

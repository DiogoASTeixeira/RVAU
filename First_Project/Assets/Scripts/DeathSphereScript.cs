using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSphereScript : MonoBehaviour
{

    private readonly float DAMAGE_VALUE = 1.0f;
    private readonly float SPHERE_DURATION = 6.0f;

    private readonly float seedHeight = 0.3f;
    private readonly float finalHeigh = 4.0f;
    private readonly float speed = 0.1f;

    private float counter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, SPHERE_DURATION);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        float newScale = Mathf.Lerp(seedHeight, finalHeigh, counter * speed);
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerScript playerScript = collision.gameObject.GetComponent<PlayerScript>();

            if(playerScript.GetStance() != PlayerScript.Stance.Defend)
            {
                playerScript.DamageHealth(DAMAGE_VALUE);
            }

            Destroy(gameObject, 0.2f);
        }
    }
}

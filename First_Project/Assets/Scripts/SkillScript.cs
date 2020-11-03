using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class SkillScript : MonoBehaviour
{
    public Sprite active;
    public Sprite disabled;

    private readonly float COOLDOWN_TIME = 10.0f;
    private bool onCooldown = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DestroyAllFireballs()
    {
        Debug.Log("You have clicked the button!");

        if (!onCooldown)
        {
            GameObject[] list = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject elem in list)
            {
                Destroy(elem);
            }
            //PlaceSkillOnCooldown();
        }
    }

    private void PlaceSkillOnCooldown()
    {
        onCooldown = true;
        Sprite spr = gameObject.GetComponent<SpriteRenderer>().sprite;
        spr = disabled;

        StartCoroutine(StartCooldown());

        IEnumerator StartCooldown()
        {
            yield return new WaitForSeconds(COOLDOWN_TIME);
            onCooldown = false;
            spr = active;
        }
    }
}

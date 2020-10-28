using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;

    public Image image;
    public GameObject character;
    public GameObject playerObject;

    private float health;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        adjustRotation();
    }

    /**
     * Rotate this character to face the player
     **/
    private void adjustRotation()
    {
        Vector3 currPosition = transform.position;
        Vector3 playerPosition = playerObject.transform.position;

        float strength = 0.5f;
        Quaternion targetRotation = Quaternion.LookRotation(playerPosition - currPosition);
        strength = Mathf.Min(strength * Time.deltaTime, 1); //Avoid overshoot
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, strength);
    }

    public void DamageHealth(float damage)
    {
        if(isDead) {
            return;
        }

        health -= damage;
        image.fillAmount = health;

        if(health <= 0) {
            anim.SetTrigger("Dead");
            isDead = true;
            StartCoroutine(removeEnemy());
            WinPlayer();
        }

        IEnumerator removeEnemy() {
            yield return new WaitForSeconds(3);
            character.SetActive(false);
        }

        void WinPlayer() {
            Animator player = playerObject.GetComponent<Animator>();
            player.SetTrigger("Win");
        }
    }
}

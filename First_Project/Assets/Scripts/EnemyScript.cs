using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;

    public Image healthBar;
    public GameObject character;
    public GameObject playerObject;
    public GameObject projectile;
    public GameObject worldObject;

    private float health;
    private bool isDead = false;
    private readonly float shootForce = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        health = 1.0f;
        InvokeRepeating(nameof(LaunchProjectile), 0.5f, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
    }

    private void LaunchProjectile()
    {
        GameObject shot = Instantiate(projectile, transform.position + new Vector3(0.0f, 0.2f, 0.1f), transform.rotation);
        shot.transform.parent = worldObject.transform;
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * shootForce);
        Destroy(shot, 2.0f);
    }

    /**
     * Rotate this character to face the player
     **/
    private void AdjustRotation()
    {
        Vector3 currPosition = transform.position;
        Vector3 playerPosition = playerObject.transform.position + new Vector3(0.0f, 0.5f, 0.0f);

        float strength = 0.8f;
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
        healthBar.fillAmount = health;

        if(health <= 0) {
            anim.SetTrigger("Dead");
            isDead = true;
            StartCoroutine(removeEnemy());
            playerObject.GetComponent<PlayerScript>().WinPlayer();
        }

        IEnumerator removeEnemy() {
            yield return new WaitForSeconds(3);
            character.SetActive(false);
        }
    }
}

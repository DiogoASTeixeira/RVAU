using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public Animator anim;

    public Image healthBar;
    public GameObject character;
    public GameObject playerObject;
    public GameObject fireball;
    public GameObject deathSphere;
    public ParticleSystem deathSpherePS;
    public GameObject worldObject;

    private readonly float MAX_HEALTH = 1.0f;
    private readonly float SHOOT_FORCE = 100.0f;
    private readonly float SPHERE_DELAY= 2.5f;
    private readonly float SPHERE_DURATION = 8.5f;

    private float health;
    private bool isDead = false;
    private bool secondPhase = false;

    // Start is called before the first frame update
    void Start()
    {
        health = MAX_HEALTH;
        deathSpherePS = Instantiate(deathSpherePS, transform.position, transform.rotation);
        deathSpherePS.transform.parent = worldObject.transform;
        deathSpherePS.Stop();
        InvokeRepeating(nameof(LaunchProjectile), 2.0f, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        AdjustRotation();
        if(!secondPhase && health <= MAX_HEALTH / 2.0f)
            EnterSecondPhase();
    }

    private void EnterSecondPhase()
    {
        secondPhase = true;
        InvokeRepeating(nameof(DeathSphereSetup), 0.0f, 15.0f);
    }

    private void LaunchProjectile()
    {
        GameObject shot = Instantiate(fireball, transform.position + new Vector3(0.0f, 0.2f, 0.1f), transform.rotation);
        shot.transform.parent = worldObject.transform;
        shot.GetComponent<Rigidbody>().AddForce(transform.forward * SHOOT_FORCE);
        Destroy(shot, 2.0f);
    }

    private void DeathSphereSetup()
    {
        deathSpherePS.Play();
        StartCoroutine(StopCloud());

        StartCoroutine(CreateDeathSphere());

        IEnumerator CreateDeathSphere()
        {
            yield return new WaitForSeconds(SPHERE_DELAY);
            GameObject sphere = Instantiate(deathSphere, transform.position, transform.rotation);
            sphere.transform.parent = worldObject.transform;
        }

        IEnumerator StopCloud()
        {
            yield return new WaitForSeconds(SPHERE_DURATION);
            deathSpherePS.Stop();
        }
    }

    /**
     * Rotate this character to face the player
     **/
    private void AdjustRotation()
    {
        Vector3 currPosition = transform.position;
        Vector3 playerPosition = playerObject.transform.position + new Vector3(0.0f, 0.5f, 0.0f);

        float strength = 0.2f;
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
        healthBar.fillAmount = health / MAX_HEALTH;

        if(health <= 0) {
            OnDeath();
        }

       
    }

    private void OnDeath()
    {
        CancelInvoke();
        anim.SetTrigger("Dead");
        isDead = true;
        StartCoroutine(removeEnemy());
        playerObject.GetComponent<PlayerScript>().WinPlayer();

        IEnumerator removeEnemy()
        {
            yield return new WaitForSeconds(3);
            character.SetActive(false);
            SceneManager.LoadScene(0);
        }
    }
}

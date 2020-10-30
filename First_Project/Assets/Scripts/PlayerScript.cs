using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public enum Stance
    {
        Attack,
        Defend
    }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    public ParticleSystem particleSystem;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    public Animator animator;
    public Image healthBar;

    private Stance currentStance;
    private float health;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentStance = Stance.Attack;

        var mainPS = particleSystem.main;
        mainPS.startColor = new Color(0.85f, 0.30f, 0.17f);

        var emission = particleSystem.emission;
        emission.enabled = true;

        health = 1.0f;

    }

    internal void DamageHealth(float damage)
    {
        if (isDead)
        {
            return;
        }

        health -= damage;
        healthBar.fillAmount = health;

        if (health <= 0)
        {
            LossPlayer();
        }


    }

    public void WinPlayer()
    {
        animator.SetTrigger("Win");
    }

    public void LossPlayer()
    {
        animator.SetTrigger("Loss");
        isDead = true;
        StartCoroutine(removePlayer());

        IEnumerator removePlayer()
        {
            yield return new WaitForSeconds(4);
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeStance(Stance stance)
    {
        if (stance != currentStance)
        {
            currentStance = stance;
            var mainPS = particleSystem.main;
            switch (stance)
            {
                case Stance.Attack:
                    {
                        mainPS.startColor = new Color(0.85f, 0.30f, 0.17f);
                        //TODO animation change, etc
                        break;
                    }
                case Stance.Defend:
                    {
                        mainPS.startColor = new Color(0.17f, 0.19f, 0.85f);
                        //TODO animation change, etc
                        break;
                    }
            }
        }
    }

    public Stance getStance()
    {
        return currentStance;
    }
}

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
    private readonly float MAX_HEALTH = 1.0f;
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

        health = MAX_HEALTH;

    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void DamageHealth(float damage)
    {
        if (isDead)
        {
            return;
        }

        float dmgMod = 1.0f;

        switch (currentStance)
        {
            case Stance.Attack:
                {
                    dmgMod = 1.0f;
                    break;
                }
            case Stance.Defend:
                {
                    dmgMod = 0.2f;
                    break;
                }
        }

        health -= damage * dmgMod;
        healthBar.fillAmount = health / MAX_HEALTH;

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
                        animator.SetTrigger("Attack");
                        break;
                    }
                case Stance.Defend:
                    {
                        mainPS.startColor = new Color(0.17f, 0.19f, 0.85f);
                        animator.SetTrigger("Defend");
                        break;
                    }
            }
        }
    }

    public Stance GetStance()
    {
        return currentStance;
    }
}

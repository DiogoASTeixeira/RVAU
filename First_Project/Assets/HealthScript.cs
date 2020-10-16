using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour
{

    public float health;
    public Animator anim;

    public Image image;
    public GameObject character;
    private bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        health = 1.0f;
       // image = GameObject.Find("Villain");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamageHealth(float damage)
    {
        if(isDead) {
            return;
        }

        health -= damage;
        image.fillAmount = health;

        if(health <= 0) {
            //anim.SetTrigger("Dead");
            isDead = true;
            StartCoroutine(removeEnemy());
            WinPlayer();
        }

        IEnumerator removeEnemy() {
            yield return new WaitForSeconds(3);
            character.SetActive(false);
        }

        void WinPlayer() {
            //Animator player = GameObject.Find("Hero").GetComponent<Animator>();
        }
    }
}

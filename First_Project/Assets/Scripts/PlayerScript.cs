using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum Stance
    {
        Attack,
        Defend
    }
    public ParticleSystem particleSystem;

    private Stance currentStance;

    // Start is called before the first frame update
    void Start()
    {
        currentStance = Stance.Attack;
        var emission = particleSystem.emission;
        emission.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeStance(Stance stance)
    {
        if(stance != currentStance)
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

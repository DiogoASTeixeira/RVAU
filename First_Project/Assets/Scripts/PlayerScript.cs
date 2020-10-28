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

    private Stance currentStance = Stance.Attack;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeStance(Stance stance)
    {
        if(stance != currentStance)
        {
            switch(stance)
            {
                case Stance.Attack:
                    {


                        //TODO animation change, etc
                        break;
                    }
                case Stance.Defend:
                    {
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

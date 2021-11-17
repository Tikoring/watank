using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTwice : Skill
{
    private Attack attack;
    // Start is called before the first frame update
    void Start()
    {
        attack = GetComponent<Attack> (); 
    }

    // Update is called once per frame

    public override void Activate()
    {
        attack.Twice = true;
        Debug.Log("skill Activating");
    }

    public override void DeActivate () {
        attack.Twice = false;
        Debug.Log("skill DeActivating");
    }
}

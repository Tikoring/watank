using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScaleDown : Skill
{
    private AssetProjectile assetProjectile;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                assetProjectile = Go.GetComponent<Attack>().ProjectilePrefab;
            }
            else
            {
                assetProjectile = null;
            }

        }

        active = false;
    }

    public override void Activate()
    {
        active = true;
        assetProjectile.ExpScale = 0.5f;
        assetProjectile.Damage = 60f;
        Debug.Log("skill Activating");
    }

    public override void DeActivate () {
        active = false;
        assetProjectile.ExpScale = 1;
        assetProjectile.Damage = 30f;
        Debug.Log("skill DeActivating");
    }
}
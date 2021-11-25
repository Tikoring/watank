using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScaleDown : Skill
{
    private AssetProjectile assetProjectile;
    // Start is called before the first frame update
    void Start()
    {
        assetProjectile = GameObject.Find ("AssetTank").GetComponent<Attack> ().ProjectilePrefab;
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
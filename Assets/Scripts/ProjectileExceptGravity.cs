using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projetile 직사화
public class ProjectileExceptGravity : Skill
{
    private AssetProjectile assetProjectile;
    // Start is called before the first frame update
    void Start()
    {
        assetProjectile = GameObject.Find ("AssetTank").GetComponent<Attack> ().ProjectilePrefab;
        active = false;
    }

    public override void Activate () {
        active = true;
        assetProjectile.GravityScale = 0.0f;
        Debug.Log("skill Activating");
    }
    public override void DeActivate () {
        active = false;
        assetProjectile.GravityScale = 1.0f;
        Debug.Log("skill DeActivating");
    }
}

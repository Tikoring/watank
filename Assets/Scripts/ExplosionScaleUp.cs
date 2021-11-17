using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScaleUp : Skill
{
    private AssetProjectile assetProjectile;
    // Start is called before the first frame update
    void Start()
    {
        assetProjectile = GameObject.Find ("AssetTank").GetComponent<Attack> ().ProjectilePrefab;
    }

    public override void Activate()
    {
        assetProjectile.ExpScale = new Vector3 (2, 2, 2);
        Debug.Log("skill Activating");
    }

    public override void DeActivate () {
        assetProjectile.ExpScale = new Vector3 (1, 1, 1);
        Debug.Log("skill DeActivating");
    }
}

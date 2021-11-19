using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectile의 색상 변경 스킬
public class ProjectileCololr : Skill
{
    private AssetProjectile assetProjectile;
    // Start is called before the first frame update
    void Start()
    {
        assetProjectile = GameObject.Find ("AssetTank").GetComponent<Attack> ().ProjectilePrefab;
    }

    public override void Activate()
    {
        assetProjectile.ProjetileColor = Color.green;
        Debug.Log("skill Activating");
    }

    public override void DeActivate () {
        assetProjectile.ProjetileColor = Color.white;
        Debug.Log("skill DeActivating");
    }
}

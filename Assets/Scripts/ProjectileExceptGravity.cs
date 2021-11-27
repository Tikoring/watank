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
        foreach (GameObject Go in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                assetProjectile = Go.GetComponent<Attack>().ProjectilePrefab;

            }
            else
            {
                assetProjectile = null;
                Debug.Log("Error. Can't find assetProjectile");
            }
        }
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

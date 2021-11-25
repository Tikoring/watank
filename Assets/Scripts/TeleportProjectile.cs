using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//projectile의 탄착지점으로 이동하는 스킬(폭발, 데미지 제거)
//outline으로 나가게 되면 현재 체력의 절반만큼 피해를 입음
public class TeleportProjectile : Skill
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
        assetProjectile.Teleport = true;
        Debug.Log("skill Activating");
    }

    public override void DeActivate () {
        active = false;
        assetProjectile.Teleport = false;
        Debug.Log("skill DeActivating");
    }
}

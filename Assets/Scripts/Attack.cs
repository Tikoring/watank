using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;    // 투사체 프리팹 
    [SerializeField]
    private GameObject attackPos;   // 투사체 시작 위치

    void Update()
    {
        // Fire();
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Fire();
        }
    }

    private void Fire() 
    {
        // attackPos 위치와 각도를 기준으로 projectile 생성
        // 나중에 오브젝트 풀링으로 수정
        print(attackPos.transform.rotation);
        Instantiate(projectilePrefab, attackPos.transform.position, attackPos.transform.rotation);
        
    }
}

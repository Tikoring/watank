using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private Projectile projectilePrefab;    // 투사체 프리팹 
    [SerializeField]
    private GameObject attackPos;   // 투사체 시작 위치
    [SerializeField]
    private Text powerText;     // Power text
    private bool firePermission;
    private bool charging;
    private bool reverseCharging;
    private float power;

    void Start() {
        charging = false;
        power = 4f;
        firePermission = projectilePrefab.FirePermission;
    }

    void Update()
    {
        if (firePermission) {
            CheckInput();
        }
        UpdatePowerText();
        firePermission = projectilePrefab.FirePermission;   //포탄이 파괴 됐을 때 값의 변경을 위해 Update에서 계속 초기화
    }

    private void UpdatePowerText()
    {
        powerText.text = "Power : " + power.ToString();
    }
    private void CheckInput()
    {
        // 키를 누르고 있으면 차징 활성화
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
        }

        // 차징 상태일 때 power가 점점 올라감
        if (charging) 
        {
            if (reverseCharging)
            {
                power -= 10f * Time.deltaTime;    
            } else
            {
                power += 10f * Time.deltaTime;
            }
        }

        // power min ~ max 체크
        // temporary 10 max
        if (power > 20)
        {
            reverseCharging = true;
        }
        // temporary 0 min
        if (power < 4)
        {
            reverseCharging = false;
        }

        // 키를 뗄 때 Fire, charging, reverseCharging, power 초기화
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (charging) {
                projectilePrefab.Fire(power * 2.5f, attackPos);
            }
            firePermission = projectilePrefab.FirePermission;
            reverseCharging = false;
            charging = false;
            power = 4f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Attack : MonoBehaviour
{
    [SerializeField]
    private AssetProjectile projectilePrefab;    // 투사체 프리팹 
    [SerializeField]
    private GameObject attackPos;   // 투사체 시작 위치
    [SerializeField]
    private Text powerText;     // Power text
    private bool firePermission;
    private bool charging;
    private bool reverseCharging;
    private float power;
    private bool twice;
    public bool Twice {
        get {return twice;}
        set {twice = value;}
    }
    public AssetProjectile ProjectilePrefab => projectilePrefab;
    CameraControl cam;

    void Start() {
        charging = false;
        cam = GameObject.FindObjectOfType<CameraControl>();
        power = 4f;
        firePermission = projectilePrefab.FirePermission;
        twice = false;
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
            if (charging && !twice) {
                cam.FocusBullet = true;
                charging = false;
                reverseCharging = false;
                projectilePrefab.Fire(power * 1.5f, attackPos);
                firePermission = projectilePrefab.FirePermission;
                power = 4f;
            } 
            if (charging && twice) {
                StartCoroutine ("AttackTwice");     //skill 사용을 위한 coroutine
                twice = false;
            }
        }
    }
    private IEnumerator AttackTwice () {
        cam.FocusBullet = true;
        charging = false;
        reverseCharging = false;
        projectilePrefab.Fire(power * 1.5f, attackPos);
        yield return new WaitForSeconds (2.5f);
        projectilePrefab.Fire(power * 1.5f, attackPos);
        firePermission = projectilePrefab.FirePermission;
        power = 4f;
    }
}

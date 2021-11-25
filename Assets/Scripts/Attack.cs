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
    private Slider powerBar;     // Power bar
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
        power = 4f;
        firePermission = projectilePrefab.FirePermission;
        twice = false;
        cam = GameObject.FindObjectOfType<CameraControl>();
    }

    void Update()
    {
        if (firePermission) {
            CheckInput();
        }
        UpdatePowerBar();
        firePermission = projectilePrefab.FirePermission;   //포탄이 파괴 됐을 때 값의 변경을 위해 Update에서 계속 초기화
        if (AssetProjectile.bullet == null)
            cam.FocusBullet = false;
    }

    private void UpdatePowerBar()
    {
        // 4: default power, 16: max power - default power
        powerBar.value = ((float)power - 4) / 16;
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
                //Audio.instance.PlaySound("Attack", attackClip);
                AudioManager.Instance.PlaySFXSound("BulletSound");
                power = 4f;
            } 
            if (charging && twice) {
                StartCoroutine ("AttackTwice");     //skill 사용을 위한 coroutine
                twice = false;
            }
            this.gameObject.GetComponent<TankControll> ().SkillManager.Access = 0;
        }
    }
    private IEnumerator AttackTwice () {
        cam.FocusBullet = true;
        charging = false;
        reverseCharging = false;
        projectilePrefab.Fire(power * 1.5f, attackPos);
        AudioManager.Instance.PlaySFXSound("BulletSound");
        yield return new WaitForSeconds (1f);
        projectilePrefab.Fire(power * 1.5f, attackPos);
        firePermission = projectilePrefab.FirePermission;
        AudioManager.Instance.PlaySFXSound("BulletSound");
        power = 4f;
    }

    
}
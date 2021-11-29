using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class Attack : MonoBehaviourPunCallbacks
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
    public GameObject PlayerGameObject;


    public GameObject CameraObject;

    public bool Twice {
        get {return twice;}
        set {twice = value;}
    }

    // 이런 방식이면 한명의 플레이어가 조작하면 모든 클라이언트도 똑같이 조작된다. 왜냐햐면
    // 신에는 플레이어 만큼의 동일한 수의 오브젝트들이 존재하기 때문임. 버그가 나지 않는 선이라면 괜찮겠지만 
    // 그렇지 않다면 반드시 PV.IsMine 체크

    public AssetProjectile ProjectilePrefab => projectilePrefab;
    CameraControl cam;

    private void Awake()
    {
        foreach(GameObject Go in GameObject.FindGameObjectsWithTag("Player"))
        {
            
            if(Go.GetComponent<PlayerScript>().PV.IsMine)
            {
                PlayerGameObject = Go;
            }

            // 위 코드로 인해 이제 Player.GameObject 변수는 나 자신만을 관리하는 player 게임 오브젝트 변수로 활용이 가능함. 
        }
    }
    void Start() {

        //powerBar 를 AssetTank는 사용하지만 
        //클라이언트에 Local하기 위치되어 있기 때문에. Single Tag 로 링크가 가능하다.
        powerBar = GameObject.FindGameObjectWithTag("PowerBar").GetComponent<Slider>();
        charging = false;
        power = 4f;
        //firePermission = projectilePrefab.FirePermission;
        firePermission = true;
        twice = false;
        cam = GameObject.FindObjectOfType<CameraControl>();
        CameraObject = GameObject.FindGameObjectWithTag("MainCamera");

    }


    public void AttackState()
    {
        if (firePermission)
        {
            CheckInput();
        }
        UpdatePowerBar();
        firePermission = true;   //포탄이 파괴 됐을 때 값의 변경을 위해 Update에서 계속 초기화

    }

    void Update()
    {
       

    }
    public void Fire(float _power, GameObject attackPos)
    {
        AssetProjectile instance = projectilePrefab;
        //print("Fire");
        instance.power = _power;
        Vector3 pos = attackPos.transform.rotation * Vector3.right / 3;
        //Vector3 addPosition = new Vector3(0, 3.0f, 0);

        // 이 부분이 불렛 할당 부분
        // bullet 변수가 나중에 쓰이므로, 생성하고 해당 미사일을 bullet에 할당해주지뭐.
        // bullet에 크게 의미는 없고 bullet을 Attack에서 유일하게 생성하기 위해 static을 사용한것으로 보임. 주석처리해도 되는듯
        //bullet = Instantiate(instance, attackPos.transform.position + pos, attackPos.transform.rotation);
        PhotonNetwork.Instantiate("AssetProjectile", attackPos.transform.position + pos, attackPos.transform.rotation);
        

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
        // 이부분이 매우 중요하다. 발사할때의 위치, 발사하는 물체 포지션을 잘 잡아야하므로.
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (charging && !twice) {
                cam.FocusBullet = true;
                charging = false;
                reverseCharging = false;
                
                // 이 부분이 발사 
                this.Fire(power * 1.5f, attackPos);

                //projectilePrefab.PV.RPC("FireRPC", RpcTarget.AllBuffered, power * 1.5f, attackPos);

                //firePermission = projectilePrefab.FirePermission;
                firePermission = true;


                //Audio.instance.PlaySound("Attack", attackClip);
                //AudioManager.Instance.PlaySFXSound("BulletSound");
                power = 4f;
            } 
            if (charging && twice) {
                StartCoroutine ("AttackTwice");     //skill 사용을 위한 coroutine
                twice = false;
            }

            // 이 부분도 크게 문제가 될까? 
            this.gameObject.GetComponent<TankControll> ().SkillManager.Access = 0;
        }
    }
    
    // 기묘한게 미사일을 발사하는 함수는 미사일 안에 있고, 그 미사일 안에 있는 함수를 불러와서
    // 미사일을 발사함. 
    // 해결책> AssetProjectile 부분에 있는 Fire 스크립트를 Attack 으로 옮긴다.
    // TankControll 스크립트의 경우 PV를 사용하지 않고 플레이어에서 그 함수를 직접 호출해서
    // 사용하였는데, 그건 TankControll 스크립트에서 photonNetwork를 직접 사용하여
    // 동기화 하는 함수들이 존재하지 않았기 때문으로 보인다. 지금은 PhotonNetwork의 Instantiate를
    // 사용하여야한다.
    // 아니지
    // TankControll Script 에서는, update를 사용해서 컨트롤을 했다.
    // 이를 동기화하기 위해서 
    // 다시말하면 update 문에 포함된 함수를 void 형식으로 새롭게 재정의하여 해당 오브젝트가
    // 내것일떄 동기화 하는 방식으로 구현했는데,
    //

    private IEnumerator AttackTwice () {
        cam.FocusBullet = true;
        charging = false;
        reverseCharging = false;
        this.Fire(power * 1.5f, attackPos);
        //AudioManager.Instance.PlaySFXSound("BulletSound");
        yield return new WaitForSeconds (1f);
        this.Fire(power * 1.5f, attackPos);
        //firePermission = projectilePrefab.FirePermission;
        firePermission = true;

        //AudioManager.Instance.PlaySFXSound("BulletSound");
        // Singletone 이므로
        // 두번 쏘는걸 구현할때 오디오 매니저를 두번 생성하면 안된다.

        power = 4f;
    }

    
}
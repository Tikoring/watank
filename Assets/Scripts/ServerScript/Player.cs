using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;


public class Player : MonoBehaviourPunCallbacks , IPunObservable
{
    public Rigidbody2D RB;
    public PhotonView PV;
    Vector3 curPos;


    // 내 화면에서 상대방도 같이 조작되는 버그를 해결하기 위해선, transfrom을 조작하는 Update문은 유일하게 구현되어야 함.
    // TankControll.cs Start
    private TankMove2D move2D;
    private TankRotate2D rotate2D;
    private Transform rotateTransform;
    private Vector3 l = new Vector3(-1.0f, 1.0f, 1.0f);
    private Vector3 r = new Vector3(1.0f, 1.0f, 1.0f);
    private float angle;
    private RaycastHit2D hit;
   

    // Attack.cs 
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



    // Start is called before the first frame update
    void Start()
    {
        // TankControll.cs Start
        move2D = GetComponent<TankMove2D>();
        move2D.Speed = 3.0f;
        rotate2D = GetComponent<TankRotate2D>();
        rotate2D.Speed = 25.0f;
        rotateTransform = transform.Find("RotatePoint");
        // TankControll.cs End 


        // Attack.cs
        charging = false;
        power = 4f;
        firePermission = projectilePrefab.FirePermission;

    }

    private void FixedUpdate()
    {
        if(PV.IsMine)
        {
            hit = Physics2D.Raycast(transform.position, Vector2.down, 1.0f, LayerMask.GetMask("Field"));

            if (hit)
            {
                angle = Vector2.Angle(hit.normal, Vector2.up);
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, angle);  //현재 지면의 각도와 tank의 기본각도를 맞춤
            }


        }

        // 상대방이 쏘는 각도를 보는게 필요하지 않다면, 동기화 할 필요는 없다.
        // 상대방의 포탄만 동기화 하면 되기 때문.
    }


    //Attack.cs
    private void UpdatePowerText()
    {
        powerText.text = "Power : " + power.ToString();
    }

    //Attack.cs
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
            }
            else
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
            if (charging)
            {
                

                projectilePrefab.Fire(power * 2.5f, attackPos);
            }
            firePermission = projectilePrefab.FirePermission;
            reverseCharging = false;
            charging = false;
            power = 4f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        // 직접 동작 구현 이하 TankControll.cs 
        if (PV.IsMine)
        {
            //Attack.cs
            if (firePermission)
            {
                CheckInput();
            }
            UpdatePowerText();
            firePermission = projectilePrefab.FirePermission;   //포탄이 파괴 됐을 때 값의 변경을 위해 Update에서 계속 초기화

            //TankControll.cs
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (transform.rotation.y != 180) { transform.eulerAngles = new Vector3(0, 180, transform.rotation.z); }
                move2D.Dir = Vector3.left;
                move2D.MoveX();
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (transform.rotation.y != 180) { transform.eulerAngles = new Vector3(0, 0, transform.rotation.z); }
                move2D.Dir = Vector3.right;
                move2D.MoveX();
            }


            // 포신 조절 매개 변수를 서버에 사용하는가? 일단 나는 봐야지.
            if (Input.GetKey(KeyCode.UpArrow))
            {
                rotate2D.RotateZ(1, rotateTransform);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                rotate2D.RotateZ(-1, rotateTransform);
            }
        }
        else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    // TankControll.cs Start
    private void LateUpdate()
    {
        if(PV.IsMine) rotate2D.LimitRotate(rotateTransform);
    }
    // TankControll.cs End 


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }


}

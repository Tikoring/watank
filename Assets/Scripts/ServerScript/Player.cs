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


    // �� ȭ�鿡�� ���浵 ���� ���۵Ǵ� ���׸� �ذ��ϱ� ���ؼ�, transfrom�� �����ϴ� Update���� �����ϰ� �����Ǿ�� ��.
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
    private Projectile projectilePrefab;    // ����ü ������ 
    [SerializeField]
    private GameObject attackPos;   // ����ü ���� ��ġ
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
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, angle);  //���� ������ ������ tank�� �⺻������ ����
            }


        }

        // ������ ��� ������ ���°� �ʿ����� �ʴٸ�, ����ȭ �� �ʿ�� ����.
        // ������ ��ź�� ����ȭ �ϸ� �Ǳ� ����.
    }


    //Attack.cs
    private void UpdatePowerText()
    {
        powerText.text = "Power : " + power.ToString();
    }

    //Attack.cs
    private void CheckInput()
    {
        // Ű�� ������ ������ ��¡ Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
        }

        // ��¡ ������ �� power�� ���� �ö�
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

        // power min ~ max üũ
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

        // Ű�� �� �� Fire, charging, reverseCharging, power �ʱ�ȭ
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

        // ���� ���� ���� ���� TankControll.cs 
        if (PV.IsMine)
        {
            //Attack.cs
            if (firePermission)
            {
                CheckInput();
            }
            UpdatePowerText();
            firePermission = projectilePrefab.FirePermission;   //��ź�� �ı� ���� �� ���� ������ ���� Update���� ��� �ʱ�ȭ

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


            // ���� ���� �Ű� ������ ������ ����ϴ°�? �ϴ� ���� ������.
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

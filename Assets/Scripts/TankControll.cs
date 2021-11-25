using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankControll : MonoBehaviour
{
    private TankMove2D move2D;
    private TankRotate2D rotate2D;
    private TankAnimator tankAnimator;
    private Transform rotateTransform;
    private SkillManage skillManager;
    private Vector3 l = new Vector3 (-1.0f, 1.0f, 1.0f);
    private Vector3 r = new Vector3 (1.0f, 1.0f, 1.0f);
    private bool skillLock = false;     //skill이 온되면 다른 skill을 온 할수 없게 함(off후에는 사용가능)
    //본인의 turn이라면 true, 아니라면 false
    private bool ownTurn = true;        //ownTurn이 false가 되면, SkillManage의 access가 0이 되게해야함(턴 구현 되면 추가해야하는 요소)
    private float angle;
    private RaycastHit2D hit;

    public float maxMoveDuration = 5f;      // 최대 이동 시간
    public float moveDuration = 0.0f;   // 좌우 키 입력 시간, 턴 시작할 때 이 부분 초기화
    public Slider moveBar;     // Power bar
    public Slider playerHeadHp;

    public float maxTime = 60f;
    public float remainTime;    // 턴 시작할 때 이 부분 초기화
    public Text remainTimeText;

    public bool SkillLock {
        get {return skillLock;}
        set {skillLock = value;}
    }
    public SkillManage SkillManager => skillManager;

    private void UpdateUI()
    {
        // 4: default power, 16: max power - default power
        moveBar.value = (maxMoveDuration - moveDuration)/ maxMoveDuration;
        remainTimeText.text = ((int)remainTime).ToString();
    }

    public void Start()
    {
        move2D = GetComponent<TankMove2D> ();
        rotate2D = GetComponent<TankRotate2D> ();
        tankAnimator = GetComponent<TankAnimator> ();
        skillManager = GetComponent<SkillManage> ();
        rotate2D.Speed = 25.0f;
        rotateTransform = transform.Find ("RotatePoint");
        tankAnimator.WheelAnimator = transform.Find("Wheel").GetComponent<Animator> ();       //wheel's animator
        tankAnimator.Parent = this.gameObject;
        if (move2D.Speed == 0) {
            move2D.Speed = 3.0f;
        }

        // for UI
        remainTime = maxTime;
    }
    
    private void Update () {
        // 이동 게이지 업데이트
        UpdateUI();
        remainTime -= Time.deltaTime;   // remainTime < 0이 되면 턴 종료 event
        //기존의 fixed update와 update간 충돌이 발생해서 update문으로 통일
        //공중에 있는 동안에는 이동을 제어하기 위해 hit 여부에 따라 제어
        hit = Physics2D.Raycast (transform.position, Vector2.down, 2.0f, LayerMask.GetMask ("Field")); 
        Debug.DrawRay (transform.position, Vector3.down * 2, Color.green);
        if (hit) {
            if (Input.GetKey (KeyCode.LeftArrow)) {
                playerHeadHp.transform.localScale = new Vector3(-0.16f, 0.16f, 0.16f);
                moveDuration += Time.deltaTime;
                if (transform.eulerAngles.y != 180) {transform.eulerAngles = new Vector3 (0,180, 180 - transform.eulerAngles.z);}
                move2D.Dir = Vector3.left;

                if (maxMoveDuration - moveDuration > 0)
                {
                    move2D.MoveX ();
                    if (AudioManager.go == null)
                        AudioManager.Instance.PlaySFXSound("TankMoveAudio");
                    tankAnimator.AddMoveEffect ();
                    tankAnimator.isMove (true);   //좌, 우 입력이 있다면 move로 이동
                }
            }
            if (Input.GetKey (KeyCode.RightArrow)) {
                playerHeadHp.transform.localScale = new Vector3(0.16f, 0.16f, 0.16f);
                moveDuration += Time.deltaTime;
                if (transform.eulerAngles.y == 180) {transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);}
                move2D.Dir = Vector3.right;

                if (maxMoveDuration - moveDuration > 0)
                {
                    move2D.MoveX ();
                    if (AudioManager.go == null)
                        AudioManager.Instance.PlaySFXSound("TankMoveAudio");
                    tankAnimator.AddMoveEffect ();
                    tankAnimator.isMove (true);   //좌, 우 입력이 있다면 move로 이동
                }
            }

            angle = Vector2.Angle (hit.normal, Vector2.right);
            angle -= 90;
            if (transform.eulerAngles.y == 180) {angle *= -1;}
            transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, angle);  //현재 지면의 각도와 tank의 기본각도를 맞춤
        }

        //좌-우 입력이 없다면 idle로 이동
        if (!(Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.RightArrow))) {
            tankAnimator.isMove (false);
            tankAnimator.DeleteMoveEffect ();
        }

        if (Input.GetKey (KeyCode.UpArrow)) {
            rotate2D.RotateZ (1, rotateTransform);
        }

        if (Input.GetKey (KeyCode.DownArrow)) {
            rotate2D.RotateZ (-1, rotateTransform);
        }

        //skill은 alpha numpad에서 입력
        if (!skillLock){
            if (Input.GetKeyDown (KeyCode.Alpha1)) {
                skillManager.Access = KeyCode.Alpha1;
            } else if (Input.GetKeyDown (KeyCode.Alpha2)) {
                skillManager.Access = KeyCode.Alpha2;
            } else if (Input.GetKeyDown (KeyCode.Alpha3)) {
                skillManager.Access = KeyCode.Alpha3;
            } else if (Input.GetKeyDown (KeyCode.Alpha4)) {
                skillManager.Access = KeyCode.Alpha4;
            }
        }
        
        //skill 활성화(enter키로 활성화)
        if (Input.GetKeyDown (KeyCode.Return) && (skillManager.Access >= KeyCode.Alpha1 && skillManager.Access <= KeyCode.Alpha4) && !skillLock) {
            skillManager.Use ();
            skillLock = true;
        }
        //skill 비활성화(backspace키로 비 활성화)
        if (Input.GetKeyDown (KeyCode.Backspace) && skillLock) {
            skillManager.DisUse ();
            skillLock = false;
        }
    }

    private void LateUpdate () {
        rotate2D.LimitRotate (rotateTransform);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankControll : MonoBehaviour
{
    private TankMove2D move2D;
    private TankRotate2D rotate2D;
    private TankAnimator tankAnimator;
    private Transform rotateTransform;
    private Vector3 l = new Vector3 (-1.0f, 1.0f, 1.0f);
    private Vector3 r = new Vector3 (1.0f, 1.0f, 1.0f);
    private float angle;
    private RaycastHit2D hit;

    public void Start()
    {
        move2D = GetComponent<TankMove2D> ();
        if (move2D.Speed == 0) {
            move2D.Speed = 3.0f;
        }
        rotate2D = GetComponent<TankRotate2D> ();
        rotate2D.Speed = 25.0f;
        rotateTransform = transform.Find ("RotatePoint");
        tankAnimator = GetComponent<TankAnimator> ();
        tankAnimator.WheelAnimator = transform.Find("Wheel").GetComponent<Animator> ();       //wheel's animator
        tankAnimator.Parent = this.gameObject;
    }
    
    private void Update () {
        if (Input.GetKey (KeyCode.LeftArrow)) {
            if (transform.eulerAngles.y != 180) {transform.eulerAngles = new Vector3 (0,180, 180 - transform.eulerAngles.z);}
            move2D.Dir = Vector3.left;
            move2D.MoveX ();
            tankAnimator.AddMoveEffect ();
            tankAnimator.isMove (true);   //좌, 우 입력이 있다면 move로 이동
        }

        if (Input.GetKey (KeyCode.RightArrow)) {
            if (transform.eulerAngles.y == 180) {transform.eulerAngles = new Vector3 (0, 0, transform.eulerAngles.z);}
            move2D.Dir = Vector3.right;
            move2D.MoveX ();
            tankAnimator.AddMoveEffect ();
            tankAnimator.isMove (true);   //좌, 우 입력이 있다면 move로 이동
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
        //기존의 fixed update와 update간 충돌이 발생해서 update문으로 통일
        hit = Physics2D.Raycast (transform.position, Vector2.down, 1.0f, LayerMask.GetMask ("Field")); 

        if (hit) {
            angle = Vector2.Angle (hit.normal, Vector2.up);
            if (transform.eulerAngles.y == 180) {angle *= -1;}
            transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, angle);  //현재 지면의 각도와 tank의 기본각도를 맞춤
        }
    }

    private void LateUpdate () {
        rotate2D.LimitRotate (rotateTransform);
    }
}

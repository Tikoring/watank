using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAnimator : MonoBehaviour
{
    [SerializeField]
    private GameObject moveEffect;          //이동시 발생하는 effect prefab
    private GameObject moveInstanse;
    private Animator wheelAnimator;         //tank의 animation 제어체
    private GameObject parent;              //effect prefab의 위치 조정을 위한 parent object
    private bool checkMoveEffect;
    public GameObject Parent {
        get {return parent;}
        set {parent = value;}
    }

    public Animator WheelAnimator {
        get {return wheelAnimator;}
        set {wheelAnimator = value;}
    }
    //animation state 전환을 위한 parameter 변경
    public void isMove (bool check) {
        wheelAnimator.SetBool ("isMoving", check);
        checkMoveEffect = check;
    }

    public void AddMoveEffect () {
        if (!checkMoveEffect) {     //coroutine 함수 실행을 제어하기 위해 설정
            Destroy (moveInstanse); //기존에 존재하는 경우 삭제
            StartCoroutine ("SetMoveEffect");
        }
    }

    public void DeleteMoveEffect () {
        if (!checkMoveEffect) {StopCoroutine ("SetMoveEffect");}
    }

    private IEnumerator SetMoveEffect () {
        while (true) {
            Vector3 pos = parent.transform.position;
            moveInstanse = Instantiate (moveEffect, pos, Quaternion.identity);
            moveInstanse.transform.parent = parent.transform;
            yield return new WaitForSeconds (2.0f);
        }
    }
}
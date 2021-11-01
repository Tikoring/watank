using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//포신의 회전, 상대 값을 이용해서 0~90도만큼만 움직이게 조정)
public class TankRotate2D : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed;
    public float Speed {
        get {return speed;}
        set {speed = value;}
    }

    public void RotateZ (int dir, Transform rotateTransform) {
        rotateTransform.Rotate (0, 0, dir *Time.deltaTime * speed, Space.Self);
    }

    public void LimitRotate (Transform rotateTransform) {
        if (rotateTransform.localEulerAngles.z <= 360 && rotateTransform.localEulerAngles.z >= 270) {
            rotateTransform.localEulerAngles = new Vector3 (0, 0, 0);
        }
        if (rotateTransform.localEulerAngles.z >= 90.0f && rotateTransform.localEulerAngles.z <= 180.0f) {
            rotateTransform.localEulerAngles = new Vector3 (0, 0, 90);
        }
    }
}
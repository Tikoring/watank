using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//2d 횡 움직임을 position으로 구현 
public class TankMove2D : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Vector3 dir = Vector3.zero;
    public float Speed {
        get {return speed;}
        set {speed = value;}
    }
    public Vector3 Dir {
        get {return dir;}
        set {dir = value;}
    }
    // Update is called once per frame
    
    public void MoveX () {
        transform.position += dir * speed * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask WhatIsGround;
    public GameObject boomClone;
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D overCollider2d = Physics2D.OverlapCircle(MousePosition, 0.01f, WhatIsGround);
            Debug.Log(overCollider2d);
            if (overCollider2d != null)
            {
                overCollider2d.transform.GetComponent<Ground>().MakeDot(MousePosition);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(boomClone, MousePosition, Quaternion.identity);
        }     
    }
}

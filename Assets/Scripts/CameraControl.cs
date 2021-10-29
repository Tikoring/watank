using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float cameraSpeed = 2.0f;
    private bool IsHold = true;
    private Vector3 targetPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //turn이 넘어가면 자동으로 focus 되도록 수정
        //bullet이 추가되면 bullet 발사할 때 focus가 bullet으로 가도록 수정

        if (Input.GetKeyUp("y"))
        {
            IsHold = !IsHold;
        }

        if (IsHold == true)
        {
            FocusCameara();
        }
        else
        {
            FreeCamera();
        }
    }

    void FreeCamera()
    {
        // map size 결정 이후 map 밖으로 안 나가게 수정

        if (Input.GetKey("w"))
        {
            targetPos += new Vector3(0, 0.1f, 0);
        }
        else if (Input.GetKey("s"))
        {
            targetPos += new Vector3(0, -0.1f, 0);
        }
        else if (Input.GetKey("a"))
        {
            targetPos += new Vector3(-0.1f, 0, 0);
        }
        else if (Input.GetKey("d"))
        {
            targetPos += new Vector3(0.1f, 0, 0);
        }
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }

    void FocusCameara()
    {
        targetPos.Set(CircleManager.instance.getCurrentCircle().transform.position.x,
            CircleManager.instance.getCurrentCircle().transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }
}


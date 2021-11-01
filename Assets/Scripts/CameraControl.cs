using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float cameraSpeed = 2.0f;
    private bool IsHold = true;
    private Vector3 targetPos;
    public GameObject playerPos;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //turn�� �Ѿ�� �ڵ����� focus �ǵ��� ����
        //bullet�� �߰��Ǹ� bullet �߻��� �� focus�� bullet���� ������ ����

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
        // map size ���� ���� map ������ �� ������ ����

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
        targetPos.Set(playerPos.transform.position.x,
            playerPos.transform.position.y, this.transform.position.z);
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }
}


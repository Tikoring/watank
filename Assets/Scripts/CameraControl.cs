using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private static float cameraSpeed = 5.0f;
    private bool IsHold = true;
    private Vector3 targetPos;
    public bool FocusBullet = false;
    GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        tank = GameObject.Find("AssetTank");
        
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

        if (FocusBullet == true && Projectile.bullet != null)
        {
            targetPos.Set(Projectile.bullet.transform.position.x, Projectile.bullet.transform.position.y, this.transform.position.z);
            FocusCamera(targetPos);
        }
        else if (IsHold == true)
        {
            targetPos.Set(tank.transform.position.x, tank.transform.position.y, this.transform.position.z);
            FocusCamera(targetPos);
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
        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }

    public void FocusCamera(Vector3 pos)
    {
        transform.position = Vector3.Lerp(transform.position, pos, cameraSpeed * Time.deltaTime);
    }

    public Vector3 GetCamPos()
    {
        Vector3 temp = transform.position;
        return temp;
    }
}


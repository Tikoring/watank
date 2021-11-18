using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera MainCamera;
    private static float cameraSpeed = 5.0f;
    private bool IsHold = true;
    private Vector3 targetPos;
    //private float distance = 0;
    public bool FocusBullet = false;
    GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GetComponent<Camera>();
        //when camera is focus mode, camera follow tank
        tank = GameObject.Find("AssetTank");
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
 
        if (Input.GetKeyUp("y"))
        {
            IsHold = !IsHold;
        }

        //follow bullet
        if (FocusBullet == true && AssetProjectile.bullet != null)
        {
            targetPos.Set(AssetProjectile.bullet.transform.position.x, AssetProjectile.bullet.transform.position.y, this.transform.position.z);
            FocusCamera(targetPos);
        }
        //follow tank
        else if (IsHold == true)
        {
            targetPos.Set(tank.transform.position.x, tank.transform.position.y, this.transform.position.z);
            FocusCamera(targetPos);
        }
        //free cam
        else
        {
            FreeCamera();
        }
    }

    void FreeCamera()
    {
        //자유시점
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
        MainCamera.transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed * Time.deltaTime);
    }

    //camera follow object
    public void FocusCamera(Vector3 pos)
    {
        MainCamera.transform.position = Vector3.Lerp(transform.position, pos, cameraSpeed * Time.deltaTime);
    }

    
    //Camera Zoom in out
    public void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * 5.0f;
        if (distance != 0)
        {
            //MainCamera.fieldOfView += distance;
            Camera.main.orthographicSize += distance;
        }
        Debug.Log(distance);
    }
}

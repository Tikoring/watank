using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 pos;
    GameObject cam;
    void Start()
    {
        cam = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        pos.Set(cam.transform.position.x, cam.transform.position.y, this.transform.position.z);
        transform.position = pos;
    }
}

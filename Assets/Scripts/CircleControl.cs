using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleControl : MonoBehaviour
{
    public int circleId;
    public bool IsTurn { get { return CircleManager.instance.IsMyTurn(circleId); } }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (!IsTurn)
            return;
        Move();
        if (Input.GetKeyUp("q"))
        {
            CircleManager.instance.nextCircle();
        }
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.LeftArrow)) 
        {
            transform.position = new Vector3(transform.position.x - 3.0f * Time.deltaTime, transform.position.y);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = new Vector3(transform.position.x + 3.0f * Time.deltaTime, transform.position.y);
        }
    }
    */
}

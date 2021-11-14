using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//이동시 발생할 effect update
public class MoveEffect : MonoBehaviour
{
    [SerializeField]
    private float deleteTime;
    void Update()
    {
        transform.localPosition = new Vector3 (-0.55f, -0.1f, 0);
        transform.eulerAngles = transform.parent.transform.eulerAngles;
        deleteTime -= Time.deltaTime;
        if (deleteTime <= 0) {
            Destroy (this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject explosionAreaGO;
    // Start is called before the first frame update
    void Start()
    {
        explosionAreaGO.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.transform.CompareTag("Field") ||
            collision.transform.CompareTag("Player") ) return;

        explosionAreaGO.SetActive(true);
        Destroy(this.gameObject, 0.02f);
    }
}

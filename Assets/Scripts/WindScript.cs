using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    // Start is called before the first frame update
    static Vector3 wind;
    void Start()
    {
        wind.Set(Random.RandomRange(0, 4f), Random.RandomRange(0, 4f), Random.RandomRange(0, 4f));
    }

    // Update is called once per frame

    public static void setWind()
    {
        wind.Set(Random.RandomRange(0, 4f), Random.RandomRange(0, 4f), Random.RandomRange(0, 4f));
    }
    public static Vector3 getWind()
    {
        return wind;
    }
}

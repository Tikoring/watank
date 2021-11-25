using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindScript : MonoBehaviour
{
    // Start is called before the first frame update
    static Vector2 wind;
    void Start()
    {
        wind.Set(Random.Range (-0.2f, 0.2f), 0);
    }

    // Update is called once per frame

    public static void setWind()
    {
        wind.Set(Random.Range (-0.2f, 0.2f), 0);
    }
    public static Vector2 getWind()
    {
        return wind;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text WindT;
    void Start()
    {
        //WindT = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 wind = WindScript.getWind();
        //벡터 방향값을 풍속으로 나타내도록
        float windSpeed = wind.magnitude;
        WindT.text = string.Format("{0:N2}", windSpeed) + " / ms";
    }
}

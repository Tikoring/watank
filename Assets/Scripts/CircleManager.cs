using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    private CircleControl[] circles;
    private static int circleIndex = 0;
    public static CircleManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        circles = GameObject.FindObjectsOfType<CircleControl>();
        for (int i = 0; i < circles.Length; i++)
        {
            circles[i].circleId = i;
        }
    }

    public void nextCircle()
    {
        StartCoroutine(NextCircleCoroutine());
    }
    public IEnumerator NextCircleCoroutine()
    {
        yield return new WaitForSeconds(0);

        circleIndex = circleIndex + 1;
        if (circleIndex >= circles.Length)
        {
            circleIndex = 0;
        }
    }

    public bool IsMyTurn(int i)
    {
        return i == circleIndex;
    }
    public CircleControl getCurrentCircle()
    {
        return circles[circleIndex];
    }
}


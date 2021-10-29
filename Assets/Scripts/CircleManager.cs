using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleManager : MonoBehaviour
{
    public CircleControl[] circles;
    private static int currentCircle = 0;
    public static CircleManager instance;
    // Start is called before the first frame update
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

    // Update is called once per frame

    public void nextCircle()
    {
        StartCoroutine(NextCircleCoroutine());
    }
    public IEnumerator NextCircleCoroutine()
    {

        yield return new WaitForSeconds(0);

        currentCircle = currentCircle + 1;
        if (currentCircle >= circles.Length)
        {
            currentCircle = 0;
        }
    }

    public bool IsMyTurn(int i)
    {
        return i == currentCircle;
    }
    public int getCurrentCircle()
    {
        return currentCircle;
    }
}


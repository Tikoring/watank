using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    //active를 활용해, skill ui에 활용할 수 있을 것으로 예상
    protected bool active;
    public bool Active => active;
    public abstract void Activate ();
    public abstract void DeActivate ();
}

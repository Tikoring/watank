using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempRemoveMap : MonoBehaviour
{
    // 게임 시작 후 임시 맵 바로 삭제
    void Start()
    {
        gameObject.SetActive(false);
    }
}

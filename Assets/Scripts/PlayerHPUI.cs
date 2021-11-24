using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPUI : MonoBehaviour
{
    public GameObject myTank;

    private Canvas playerHpCanvas;
    private Camera hpCamera;
    private RectTransform rectParent;
    private RectTransform rectHpBar;


    void Start()
    {
        playerHpCanvas = GetComponentInParent<Canvas>();
        hpCamera = playerHpCanvas.worldCamera;
        rectParent = playerHpCanvas.GetComponent<RectTransform>();
        rectHpBar = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        var screenPos = Camera.main.WorldToScreenPoint(myTank.transform.position);
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, hpCamera, out localPos);
    
        rectHpBar.localPosition = localPos;
    
    }
}

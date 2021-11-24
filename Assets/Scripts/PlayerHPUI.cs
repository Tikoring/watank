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

    // Start is called before the first frame update
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
        Debug.Log(myTank.transform.position);
        var screenPos = Camera.main.WorldToScreenPoint(myTank.transform.position);
        Debug.Log(screenPos);
        var localPos = Vector2.zero;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos, hpCamera, out localPos);
        Debug.Log(localPos);
        rectHpBar.localPosition = localPos;
    }
}

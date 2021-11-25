using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{   
    //최대 체력은 유니티UI에서 변경 가능(값을 조정하는 것으로 밸런싱)
    [SerializeField]
    private float maxHP;
    [SerializeField]
    // private Text HPtext;
    private Slider hpBar;     // Hp bar
    [SerializeField]
    private Slider headHpBar;
    private float currentHP;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;
    // Start is called before the first frame update
    void Start()
    {
        //hp 최솟값 설정
        if (maxHP == 0) {
            maxHP = 100f;
        }
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer> ();   
    }

    private void Update() {
        UpdateHPView ();
    }

    private void UpdateHPView () {
        // HPtext.text = "HP : " + currentHP.ToString ();
        hpBar.value = (float)currentHP/maxHP;
        headHpBar.value = (float)currentHP/maxHP;
    }
    // Update is called once per frame

    //Coroutine 함수의 경우 스프라이트 적용 후 적용예정
    public void TakeDamage (float damage) { //Hit 판정시 발생
        currentHP -= damage;

        //StartCoroutine ("HitColorAnimation");
        //StopCoroutine ("HitColorAnimation");
        Debug.Log ("Hit");
        if (currentHP <= 0) {
            Debug.Log ("Player HP : 0.. Die");
        }
    }

    /*private IEnumerator HitColorAnimation () {    //추후 sprite 적용시 피격판정 추가예정
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds (0.1f);
        spriteRenderer.color = Color.white;
    }*/
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//개인 skill 관리를 위한 매니저
public class SkillManage : MonoBehaviour
{
    Skill skill1;
    Skill skill2;
    Skill skill3;
    Skill skill4;
    private KeyCode access; //keycode를 저장해서 access할 스킬정보를 저장하는 방식
    public KeyCode Access {
        get {return access;}
        set {access = value;}
    }
    private void Start() {
        this.gameObject.AddComponent<AttackTwice> ();
        skill1 = GetComponent<AttackTwice> ();
        this.gameObject.AddComponent<ProjectileCololr> ();
        skill2 = GetComponent<ProjectileCololr> ();
        this.gameObject.AddComponent<ProjectileExceptGravity> ();
        skill3 = GetComponent<ProjectileExceptGravity> ();
        this.gameObject.AddComponent<ExplosionScaleUp> ();
        skill4 = GetComponent<ExplosionScaleUp> ();
    }
    public void Use () {
        switch (access) {
            case KeyCode.Alpha1 :
                skill1.Activate ();
                break;
            case KeyCode.Alpha2 :
                skill2.Activate ();
                break;
            case KeyCode.Alpha3 :
                skill3.Activate ();
                break;
            case KeyCode.Alpha4 :
                skill4.Activate ();
                break;
        }
    }

    public void DisUse () {
        switch (access) {
            case KeyCode.Alpha1 :
                skill1.DeActivate ();
                break;
            case KeyCode.Alpha2 :
                skill2.DeActivate ();
                break;
            case KeyCode.Alpha3 :
                skill3.DeActivate ();
                break;
            case KeyCode.Alpha4 :
                skill4.DeActivate ();
                break;
        }
        access = 0;
    }

    //추후 cooldown 관련 method
    public void TurnManaging () {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//개인 skill 관리를 위한 매니저
public class SkillManage : MonoBehaviour
{   
    private int[] skillNumber = new int[4];
    private Skill[] skillArray = new Skill[4];
    private KeyCode access; //keycode를 저장해서 access할 스킬정보를 저장하는 방식
    public KeyCode Access {
        get {return access;}
        set {access = value;}
    }
    private void Start() {
        for (int i = 0; i < 4; i++) {
            skillNumber[i] = (int) Random.Range (0, 6);
            for (int j = 0; j < i; j++) {
                if (skillNumber[i] == skillNumber[j]) {i--; break;}
            }
        }
        for (int i = 0; i < 4; i++) {
            switch (skillNumber[i]) {
                case 0 :
                    this.gameObject.AddComponent<TeleportProjectile> ();
                    skillArray[i] = GetComponent<TeleportProjectile> ();
                    break;
                case 1 :
                    this.gameObject.AddComponent<ProjectileExceptFiled> ();
                    skillArray[i] = GetComponent<ProjectileExceptFiled> ();
                    break;
                case 2 :
                    this.gameObject.AddComponent<ProjectileExceptGravity> ();
                    skillArray[i] = GetComponent<ProjectileExceptGravity> ();
                    break;
                case 3 :
                    this.gameObject.AddComponent<ExplosionScaleUp> ();
                    skillArray[i] = GetComponent<ExplosionScaleUp> ();
                    break;
                case 4 :
                    this.gameObject.AddComponent<ExplosionScaleDown> ();
                    skillArray[i] = GetComponent<ExplosionScaleDown> ();
                    break;
                case 5 :
                    this.gameObject.AddComponent<AttackTwice> ();
                    skillArray[i] = GetComponent<AttackTwice> ();
                    break;
            }
            Debug.Log(skillArray[i]);
        }
    }
    public void Use () {
        switch (access) {
            case KeyCode.Alpha1 :
                skillArray[0].Activate ();
                break;
            case KeyCode.Alpha2 :
                skillArray[1].Activate ();
                break;
            case KeyCode.Alpha3 :
                skillArray[2].Activate ();
                break;
            case KeyCode.Alpha4 :
                skillArray[3].Activate ();
                break;
        }
    }

    public void DisUse () {
        switch (access) {
            case KeyCode.Alpha1 :
                skillArray[0].DeActivate ();
                break;
            case KeyCode.Alpha2 :
                skillArray[1].DeActivate ();
                break;
            case KeyCode.Alpha3 :
                skillArray[2].DeActivate ();
                break;
            case KeyCode.Alpha4 :
                skillArray[3].DeActivate ();
                break;
        }
        access = 0;
    }
}

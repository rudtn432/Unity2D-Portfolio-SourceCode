using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillData : MonoBehaviour
{
    public Sprite defaultImg;               // 기본 이미지
    public string skillName;                // 스킬 이름
    [TextArea] public string skillDescribe; // 스킬 설명
    public int skillType;                   // 스킬 타입 ( 패시브:0, 액티브:1)
    public int subSkill;                    // 서브 스킬 (만약 액티브 스킬이라면 서브 스킬 레벨이 풀이여야 6레벨을 찍을 수 있다
    public int skillLv;                     // 스킬 레벨
    public int skillDmg;                    // 스킬 레벨에 따른 데미지
    public float skillCooltime;             // 스킬 쿨타임
    public bool isHit;                      // 몬스터가 맞았는가
    public bool isSkill = false;            // 스킬을 사용 할 수 있는가 (false면 사용 가능)
    public bool isSkillLvFull = false;      // 스킬레벨이 최대치인가  
}

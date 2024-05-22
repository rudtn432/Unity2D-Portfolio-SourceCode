using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobData : MonoBehaviour
{
    public string monsterName;
    public int hp;
    public float moveSpeed;
    public float fadeTime;
    public bool isHit;          // 피격 당했나 안당했나
    public bool isKnockBack;    // 넉백 당하는 중인지
    public bool isDeath;                                                        
}

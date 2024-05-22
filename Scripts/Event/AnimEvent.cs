using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    private Sfx sfx;

    private void Start()
    {
        sfx = FindObjectOfType<Sfx>();
    }

    void DestroyGameObj()       // 애니메이션 끝부분에 넣어서 재생이 끝나면 사라지기
    {
        Destroy(gameObject);
    }
    void ObjActiveFalse()       // 게임오브젝트 Active False
    {
        gameObject.SetActive(false);
    }
    void BossSpawn()
    {
        GameObject.Find("Manager").transform.Find("MobSpawnManager").GetComponent<MobSpawnManager>().BossSpawn();
    }
    void SfxWarning1()
    {
        sfx.SfxWarning1();
    }
    void SfxWarning2()
    {
        sfx.SfxWarning2();
    }
    void SfxWarning3BossRegen()
    {
        sfx.SfxWarning3();
        sfx.SfxBossRegen();
    }
}

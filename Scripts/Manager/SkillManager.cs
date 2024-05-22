using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Player player;

    public SkillData[] skillList;

    public int fullLvCount;             // 풀레벨인 스킬 카운트

    public float[] time;

    public bool isSkillKunai1;          // 1번 쿠나이 스킬이 활성화 됐나
    public bool isSkillKunai2;          // 2번 쿠나이 스킬이 활성화 됐나
    public bool isSkillShield;          // 쉴드 스킬이 활성화 됐나
    public bool daggerNoCreate;         // 풀레벨이면 대거 생성 그만
    SkillData kunaiInstance;            // Kunai1의 정보를 담아 변경시켜줄 인스턴스
    SkillData shieldInstance;           // Shield의 정보를 담아 변경시켜줄 인스턴스

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        time[0] += Time.deltaTime;
        if (skillList[8].skillLv >= 1)
            time[1] += Time.deltaTime;

        if (time[0] >= 1)    // 1초마다 총알 발사
        {
            time[0] = 0;
            if (skillList[0].skillLv == 6)
                StartCoroutine(BulletFireFullLv());
            else
                StartCoroutine(BulletFire());
        }
        if(time[1] >= 3)    // 3초마다 붐볼 발사
        {
            time[1] = 0;
            StartCoroutine(BombBallFire());
        }

        // Kunai 코드
        if (!isSkillKunai1 && skillList[9].skillLv >= 1)
        {
            isSkillKunai1 = true;
            kunaiInstance = Instantiate(skillList[9], new Vector2(player.transform.position.x, player.transform.position.y), Quaternion.identity);
        }
        if(isSkillKunai1)
        {
            switch (skillList[9].skillLv)
            {
                case 2:
                    kunaiInstance.skillDmg = 7;
                    kunaiInstance.skillLv = 2;
                    break;
                case 3:
                    kunaiInstance.skillDmg = 9;
                    kunaiInstance.skillLv = 3;
                    break;
                case 4:
                    kunaiInstance.skillDmg = 11;
                    kunaiInstance.skillLv = 4;
                    break;
                case 5:
                    kunaiInstance.skillDmg = 13;
                    kunaiInstance.skillLv = 5;
                    break;
                case 6:
                    kunaiInstance.skillDmg = 15;
                    kunaiInstance.skillLv = 6;
                    break;
                default:
                    break;
            }
        }
        if (!isSkillKunai2 && skillList[9].skillLv >= 6)
        {
            isSkillKunai2 = true;
            Instantiate(skillList[9], new Vector2(player.transform.position.x, player.transform.position.y), Quaternion.identity);
        }

        // Shield 코드
        if (!isSkillShield && skillList[3].skillLv >= 1)
        {
            isSkillShield = true;
            shieldInstance = Instantiate(skillList[3], new Vector2(player.transform.position.x, player.transform.position.y), Quaternion.identity);
        }
        if(isSkillShield)
        {
            switch(skillList[3].skillLv)
            {
                case 2:
                    shieldInstance.skillDmg = 6;
                    shieldInstance.skillLv = 2;
                    shieldInstance.transform.localScale = new Vector3(3, 3, 3);
                    break;
                case 3:
                    shieldInstance.skillDmg = 7;
                    shieldInstance.skillLv = 3;
                    shieldInstance.transform.localScale = new Vector3(4, 4, 4);
                    break;
                case 4:
                    shieldInstance.skillDmg = 8;
                    shieldInstance.skillLv = 4;
                    shieldInstance.transform.localScale = new Vector3(5, 5, 5);
                    break;
                case 5:
                    shieldInstance.skillDmg = 9;
                    shieldInstance.skillLv = 5;
                    shieldInstance.transform.localScale = new Vector3(6, 6, 6);
                    break;
                case 6:
                    shieldInstance.skillDmg = 10;
                    shieldInstance.skillLv = 6;
                    shieldInstance.transform.localScale = new Vector3(7, 7, 7);
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator BulletFire()    // 총알 발사
    {
        GameObject[] obj = new GameObject[6];
        for (int i = 0; i < skillList[0].skillLv; i++)
        {
            obj[i] = GameManager.instance.poolManager.Get(10);
            obj[i].transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            switch (skillList[0].skillLv)
            {
                case 1:
                    obj[i].GetComponent<SkillData>().skillDmg = 10;
                    break;
                case 2:
                    obj[i].GetComponent<SkillData>().skillDmg = 12;
                    break;
                case 3:
                    obj[i].GetComponent<SkillData>().skillDmg = 14;
                    break;
                case 4:
                    obj[i].GetComponent<SkillData>().skillDmg = 16;
                    break;
                case 5:
                    obj[i].GetComponent<SkillData>().skillDmg = 18;
                    break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator BulletFireFullLv()
    {
        GameObject[] obj = new GameObject[11];
        for (int i = 0; i < 10; i++)
        {
            obj[i] = GameManager.instance.poolManager.Get(11);
            obj[i].transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            obj[i].GetComponent<SkillData>().skillDmg = 20;
            yield return new WaitForSeconds(0.1f);
        }
    }
    IEnumerator BombBallFire()      // 붐볼 발사
    {
        GameObject[] obj = new GameObject[2];

        if (!skillList[8].isSkillLvFull)
        {
            obj[0] = GameManager.instance.poolManager.Get(13);
            obj[0].transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
            switch (skillList[8].skillLv)
            {
                case 1:
                    obj[0].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    obj[0].GetComponent<SkillData>().skillDmg = 5;
                    break;
                case 2:
                    obj[0].transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
                    obj[0].GetComponent<SkillData>().skillDmg = 6;
                    break;
                case 3:
                    obj[0].transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    obj[0].GetComponent<SkillData>().skillDmg = 7;
                    break;
                case 4:
                    obj[0].transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
                    obj[0].GetComponent<SkillData>().skillDmg = 8;
                    break;
                case 5:
                    obj[0].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    obj[0].GetComponent<SkillData>().skillDmg = 9;
                    break;
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                obj[i] = GameManager.instance.poolManager.Get(13);
                obj[i].transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
                obj[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                obj[i].GetComponent<SkillData>().skillDmg = 10;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }

    public IEnumerator DaggerCreate()       // 대거 생성 코루틴
    {
        if (skillList[2].isSkillLvFull)     // 풀레벨이면 대거 한번 생성하고 그만 생성
            daggerNoCreate = true;

        int count = skillList[2].skillLv;   // 대거 소환 갯수
        if (skillList[2].skillLv == 6)
            count = 5;
        GameObject[] objects;
        objects = new GameObject[count];

        for (int i = 0; i < count; i++)
        {
            float angle = Time.time * 1.5f * player.attackSpeed * i * (2 * Mathf.PI / count);  // 원주를 count로 나눈 각도
            float x = Mathf.Cos(angle) * 2;
            float y = Mathf.Sin(angle) * 2;

            objects[i] = GameManager.instance.poolManager.Get(12);
            objects[i].transform.position = new Vector2(player.transform.position.x + x, player.transform.position.y + y);
            objects[i].GetComponent<SkillData>().skillLv = skillList[2].skillLv;
            switch (skillList[2].skillLv)
            {
                case 1:
                    objects[i].GetComponent<SkillData>().skillDmg = 10;
                    break;
                case 2:
                    objects[i].GetComponent<SkillData>().skillDmg = 11;
                    break;
                case 3:
                    objects[i].GetComponent<SkillData>().skillDmg = 12;
                    break;
                case 4:
                    objects[i].GetComponent<SkillData>().skillDmg = 13;
                    break;
                case 5:
                    objects[i].GetComponent<SkillData>().skillDmg = 14;
                    break;
                case 6:
                    objects[i].GetComponent<SkillData>().skillDmg = 15;
                    break;
            }
            objects[i].GetComponent<Dagger>().count = count;
            objects[i].GetComponent<Dagger>().idNum = i;
        }
        yield return new WaitForSeconds(8f);
        if (!daggerNoCreate)
            StartCoroutine(DaggerCreate());         // 코루틴 다시 Call
    }

    public IEnumerator PoisonGasCreate()    // 독가스 생성 코루틴
    {
        Vector3 randomPos1 = player.transform.position + Random.insideUnitSphere * 4f;       // 플레이어 반경 4f 안의 랜덤 위치 반환
        Vector3 randomPos2 = player.transform.position + Random.insideUnitSphere * 4f;    
        Vector3 randomPos3 = player.transform.position + Random.insideUnitSphere * 4f;
        GameObject[] obj = new GameObject[3];

        switch (skillList[10].skillLv)
        {
            case 1:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 5;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1;
                break;
            case 2:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 6;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1;
                break;
            case 3:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 7;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1;
                obj[1] = GameManager.instance.poolManager.Get(14);
                obj[1].transform.position = randomPos2;
                obj[1].GetComponent<SkillData>().skillDmg = 7;
                obj[1].GetComponent<PoisonGas>().scaleValue = 1;
                break;
            case 4:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 8;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1;
                obj[1] = GameManager.instance.poolManager.Get(14);
                obj[1].transform.position = randomPos2;
                obj[1].GetComponent<SkillData>().skillDmg = 8;
                obj[1].GetComponent<PoisonGas>().scaleValue = 1;
                break;
            case 5:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 9;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1;
                obj[1] = GameManager.instance.poolManager.Get(14);
                obj[1].transform.position = randomPos2;
                obj[1].GetComponent<SkillData>().skillDmg = 9;
                obj[1].GetComponent<PoisonGas>().scaleValue = 1;
                obj[2] = GameManager.instance.poolManager.Get(14);
                obj[2].transform.position = randomPos3;
                obj[2].GetComponent<SkillData>().skillDmg = 9;
                obj[2].GetComponent<PoisonGas>().scaleValue = 1;
                break;
            case 6:
                obj[0] = GameManager.instance.poolManager.Get(14);
                obj[0].transform.position = randomPos1;
                obj[0].GetComponent<SkillData>().skillDmg = 10;
                obj[0].GetComponent<PoisonGas>().scaleValue = 1.5f;
                obj[1] = GameManager.instance.poolManager.Get(14);
                obj[1].transform.position = randomPos2;
                obj[1].GetComponent<SkillData>().skillDmg = 10;
                obj[1].GetComponent<PoisonGas>().scaleValue = 1.5f;
                obj[2] = GameManager.instance.poolManager.Get(14);
                obj[2].transform.position = randomPos3;
                obj[2].GetComponent<SkillData>().skillDmg = 10;
                obj[2].GetComponent<PoisonGas>().scaleValue = 1.5f;
                break;
        }

        if (!skillList[10].isSkillLvFull)           // 풀레벨이 아니면 7초만에 재생성
            yield return new WaitForSeconds(7f);
        else                                        // 풀레벨이면 5초만에 재생성
            yield return new WaitForSeconds(5f);
        StartCoroutine(PoisonGasCreate());         // 코루틴 다시 Call
    }
}

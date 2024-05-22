using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : SkillData
{
    private Sfx sfx;
    public Animator animator;
    private Player player;
    private Rigidbody2D rigid;
    [SerializeField] private GameObject prfHitEffect;
    public List<GameObject> FoundObjects;
    public GameObject mob;

    float radius = 1;
    float angle = 0;
    float time = 0;
    Vector3 dir;

    private bool isMob;         // 몹이 탐지됐는지 안됐는지
    private bool isMobNear;     // 몹 가까이 갔는지

    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();

        switch (skillLv)
        {
            case 1:
                skillDmg = 5;
                break;
            case 2:
                skillDmg = 7;
                break;
            case 3:
                skillDmg = 9;
                break;
            case 4:
                skillDmg = 11;
                break;
            case 5:
                skillDmg = 13;
                break;
            case 6:
                skillDmg = 15;
                break;
        }

        MobFound();
    }

    void Update()
    {
        if (isMob)
        {
            dir = mob.transform.position - transform.position;
            float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(_angle + 180f, Vector3.forward);    // 각도 몹 방향으로 변경

            if (mob.CompareTag("MobDie"))    // 가는 도중에 몹이 죽으면 다시 찾기
                MobFound();
            else
            {
                if (FoundObjects.Count == 1)    // 몹이 한 마리만 있으면 그 몹을 빙빙 돌면서 데미지 주기
                {
                    if (!isMobNear)
                    {
                        // 순간이동 안되게 자연스럽게 몹한테 가까이 가서 데미지를 주고 빙빙 돌기
                        transform.position = Vector3.MoveTowards(transform.position, mob.transform.position, 10f * player.attackSpeed * Time.deltaTime);
                        if (Vector2.Distance(transform.position, mob.transform.position) <= 0.1f)   // 몹과의 거리가 0.1이하면 isMobNear True
                            isMobNear = true;
                    }
                    else if (isMobNear)
                    {
                        if (Vector2.Distance(transform.position, mob.transform.position) >= 2.1f)   // 몹과의 거리가 2.1이상이면 isMobNear False
                        {
                            isMobNear = false;
                            return;
                        }
                        angle += 10f * player.attackSpeed * Time.deltaTime;
                        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f);                     // 각도를 원으로 도는 것처럼 변경
                        transform.position = new Vector3(mob.transform.position.x - 1f, mob.transform.position.y, 0)    // 원으로 돌기
                            + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

                    }
                }
                else
                {
                    isMobNear = false;
                    // 지정 몹 따라가기
                    transform.position = Vector3.MoveTowards(transform.position, mob.transform.position, 10f * player.attackSpeed * Time.deltaTime);
                }
                // 만약 거리가 0.1f이하면 데미지를 입었을 위치이니 다른 몹 찾기
                if (Vector2.Distance(transform.position, mob.transform.position) <= 0.1f)
                    MobFound();
            }
        }
        else if (!isMob)
            MobFound();
    }

    void MobFound()
    {
        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mob"));  // 몹 다시 찾기
        if (FoundObjects.Count == 0)    // 몹이 한 마리도 없으면
            isMob = false;
        else
        {
            mob = FoundObjects[Random.Range(0, FoundObjects.Count)];    // 몹이 있으면 다시 랜덤 지정
            isMob = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) // Trigger 충돌 체크
    {
        if (col.gameObject.tag == "Mob")
        {
            isHit = true;
            int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
            col.GetComponent<MobData>().hp -= totalDamage;
            col.GetComponent<MobData>().isHit = true;

            // HitEffect 생성하기
            GameObject hitEff = GameManager.instance.poolManager.Get(18);
            hitEff.transform.position = Vector2.Lerp(transform.position, col.transform.position, 0.7f);

            // 데미지 텍스트 생성하기
            GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
            dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
            dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
            dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();

            if (col.GetComponent<MobData>().monsterName == "파란달팽이" ||
                col.GetComponent<MobData>().monsterName == "리본돼지" ||
                col.GetComponent<MobData>().monsterName == "루팡")
                sfx.SfxMob(0);
            else if (col.GetComponent<MobData>().monsterName == "스티지")
                sfx.SfxMob(2);
            else if (col.GetComponent<MobData>().monsterName == "로랑" ||
                     col.GetComponent<MobData>().monsterName == "클랑")
                sfx.SfxMob(8);
            else if (col.GetComponent<MobData>().monsterName == "엄티")
                sfx.SfxMob(13);
            else if (col.GetComponent<MobData>().monsterName == "킹크랑")
                sfx.SfxMob(16);
            else if (col.GetComponent<MobData>().monsterName == "캡틴블랙 슬라임")
                sfx.SfxMob(19);
            else if (col.GetComponent<MobData>().monsterName == "주니어발록")
                sfx.SfxMob(24);
        }
    }
}

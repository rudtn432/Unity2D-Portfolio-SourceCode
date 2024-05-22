using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SkillData
{
    private Sfx sfx;
    public Animator animator;
    private Player player;
    private Rigidbody2D rigid;
    private float shortDis = 9999;
    private float dist;
    public List<GameObject> FoundObjects;
    public GameObject mob;
    private bool left = false;
    private bool isMob;         // 몹이 탐지됐는지 안됐는지

    Vector3 dir;

    void Awake()
    {
        sfx = FindObjectOfType<Sfx>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        StartCoroutine(SkillDisable());
        shortDis = 9999;
        mob = null;

        if (isHit)     // Hit를 하고 비활성화 됐다면
        {
            isHit = false;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            GetComponent<BoxCollider2D>().enabled = true;
        }

        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mob"));

        foreach (GameObject found in FoundObjects)      // 가장 가까운 몹 찾기
        {
            dist = Vector2.Distance(player.transform.position, found.transform.position);
            if (shortDis > dist)
            {
                shortDis = dist;
                mob = found;
            }
        }

        if (mob)
        {
            isMob = true;
            dir = mob.transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);    // 각도 몹 방향으로 변경
        }

        if (!mob && player.transform.localScale == new Vector3(1, 1, 1))
        {
            isMob = false;
            left = true;
            transform.localEulerAngles = new Vector3(0, 0, 0);        // rotation 값을 (0,0,0)으로
        }
        else if (!mob)
        {
            isMob = false;
            left = false;
            transform.localEulerAngles = new Vector3(0, 0, 180);        // rotation 값을 (0,0,180)으로
        }
    }

    void Update()
    {
        if (isMob && !isHit)    // 탐지되는 몹이 있으면 몹에게 발사, 아니면 왼쪽이나 오른쪽으로 그냥 발사
            rigid.velocity = dir.normalized * 10f * player.attackSpeed;
        else if (!isMob && !isHit)
        {
            if(left)
                rigid.velocity = Vector2.left * 10f * player.attackSpeed;
            else
                rigid.velocity = Vector2.right * 10f * player.attackSpeed;
        }
    }

    IEnumerator SkillDisable()
    {
        yield return new WaitForSeconds(2f);
        if (!isHit)     // 2초 안에 몬스터를 맞추지 못했다면 비활성화
            gameObject.SetActive(false);
    }

    void DestroySkillObj()      // 애니메이션 끝부분에 넣어서 재생이 끝나면 사라지기
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) // Trigger 충돌 체크
    {
        if (col.gameObject.tag == "Mob")
        {
            isHit = true;
            int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
            col.GetComponent<MobData>().hp -= totalDamage;
            col.GetComponent<MobData>().isHit = true;
            rigid.bodyType = RigidbodyType2D.Static;
            animator.SetBool("bHit", true);
            GetComponent<BoxCollider2D>().enabled = false;   // 콜라이더를 안꺼주면 Hit 애니메이션 상태에서 몬스터가 계속 맞음

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
            else if(col.GetComponent<MobData>().monsterName == "주니어발록")
                sfx.SfxMob(24);
        }
    }
}

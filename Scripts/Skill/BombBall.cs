using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBall : SkillData
{
    private Sfx sfx;
    public Animator animator;
    private Player player;
    private Rigidbody2D rigid;
    public List<GameObject> FoundObjects;
    public GameObject mob;
    private bool left = false;
    private bool isMob;         // 몹이 탐지됐는지 안됐는지
    int a = 0;

    public Transform hitBoxPos;
    public float radius;

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
        mob = null;

        if (isHit)     // Hit를 하고 비활성화 됐다면
        {
            isHit = false;
            rigid.bodyType = RigidbodyType2D.Dynamic;
            a = 0;
            GetComponent<CircleCollider2D>().enabled = true;
        }

        FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mob"));

        if (FoundObjects.Count == 0)
            mob = null;
        else
            mob = FoundObjects[Random.Range(0, FoundObjects.Count)];    // 랜덤으로 한 마리 지정

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
            rigid.velocity = dir.normalized * 7f * player.attackSpeed;
        else if (!isMob && !isHit)
        {
            if (left)
                rigid.velocity = Vector2.left * 7f * player.attackSpeed;
            else
                rigid.velocity = Vector2.right * 7f * player.attackSpeed;
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
            if(a == 0)  // 여러개와 충돌되어도 한번만 실행되게
            {
                animator.SetBool("bHit", true);
                transform.localScale = transform.localScale * 2;
                isHit = true;
                int totalDamage = Mathf.RoundToInt(skillDmg * player.power);

                // HitBox와 충돌하는 충돌체 모두 찾기
                Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(hitBoxPos.position, radius * (transform.localScale.x / 0.3f));

                foreach (Collider2D collider in collider2Ds)
                {
                    if (collider.gameObject.tag == "Mob")       // 충돌체 태그가 Mob이라면
                    {
                        collider.GetComponent<MobData>().hp -= totalDamage;
                        collider.GetComponent<MobData>().isHit = true;

                        // 데미지 텍스트 생성하기
                        GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
                        dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
                        dmgtxt.transform.position = new Vector2(collider.transform.position.x, collider.transform.position.y + 0.2f);
                        dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();
                    }
                }

                rigid.bodyType = RigidbodyType2D.Static;
                GetComponent<CircleCollider2D>().enabled = false;   // 콜라이더를 안꺼주면 Hit 애니메이션 상태에서 몬스터가 계속 맞음

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

                a++;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitBoxPos.position, radius * (transform.localScale.x / 0.3f));
    }
}

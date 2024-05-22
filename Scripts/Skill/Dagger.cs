using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : SkillData
{
    private Sfx sfx;
    private Rigidbody2D rigid;
    private Player player;

    public int idNum;           // 대거 번호
    public int count;           // 대거 갯수
    float radius = 2;           // 돌아가는 반지름
    float spinSpeed = 1.5f;     // 도는 속도
    float angle = 0;
    float time = 0;
    public float[] scaleTime;   // 스케일 조절하는 시간

    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        rigid = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<Player>();

        StartCoroutine(SkillDisable());
    }

    private void OnEnable()
    {
        StartCoroutine(SkillDisable());
        scaleTime[0] = 0;
        scaleTime[1] = 0;
        time = 0;
    }

    void Update()
    {
        time += Time.deltaTime;
        float angle = Time.time * spinSpeed * player.attackSpeed + idNum * (2 * Mathf.PI / count);    // idNum마다 다른 각도
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        if (player.nowHp > 0)
            transform.position = new Vector3(player.transform.position.x + x, player.transform.position.y + y, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg - 90f);

        if (time <= 1)
        {
            scaleTime[0] += Time.deltaTime;
            // 0.5초 안에 0에서 0.3까지 커지기
            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0.3f, 0.3f, 0.3f), scaleTime[0] / 0.5f);  
        }
        else if (time >= 4.5f && skillLv != 6)
        {
            scaleTime[1] += Time.deltaTime;
            // 0.5초 안에 0에서 0.3까지 작아지기
            transform.localScale = Vector3.Lerp(new Vector3(0.3f, 0.3f, 0.3f), new Vector3(0, 0, 0), scaleTime[1] / 0.5f);  
        }
    }

    IEnumerator SkillDisable()
    {
        yield return new WaitForSeconds(5f);
        if (skillLv < 6)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) // Trigger 충돌 체크
    {
        if (col.gameObject.tag == "Mob")
        {
            int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
            col.GetComponent<MobData>().hp -= totalDamage;
            col.GetComponent<MobData>().isHit = true;

            // 데미지 텍스트 생성하기
            GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
            dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
            dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
            dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();

            if (col.GetComponent<MobData>().monsterName == "파란달팽이" ||
                col.GetComponent<MobData>().monsterName == "리본돼지" ||
                col.GetComponent<MobData>().monsterName == "주니어발록" ||
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
        }
    }
}

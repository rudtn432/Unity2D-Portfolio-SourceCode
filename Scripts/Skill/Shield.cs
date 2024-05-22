using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : SkillData
{
    private Player player;
    float time;

    void Start()
    {
        player = FindObjectOfType<Player>();
        skillDmg = 5;
    }

    void Update()
    {
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D col) // Trigger 충돌 체크
    {
        if (col.gameObject.tag == "Mob")
        {
            int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
            col.GetComponent<MobData>().hp -= totalDamage;
            col.GetComponent<MobData>().moveSpeed = col.GetComponent<MobData>().moveSpeed * (1f - 20f / 100f);  // 몬스터 속도 20% 하락

            // 데미지 텍스트 생성하기
            GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
            dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
            dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
            dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if(col.gameObject.tag == "Mob")
        {
            time += Time.deltaTime;
            if(time >= 1f)
            {
                int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
                col.GetComponent<MobData>().hp -= totalDamage;

                // 데미지 텍스트 생성하기
                GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
                dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
                dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
                dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();
                time = 0;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Mob")
        {
            col.GetComponent<MobData>().moveSpeed = col.GetComponent<MobData>().moveSpeed * (1f + 25f / 100f);  // 몬스터 속도 25% 상승시켜서 원래대로
        }
    }
}

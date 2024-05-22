using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonGas : SkillData
{
    private Player player;

    public float[] scaleTime;   // 스케일 조절하는 시간
    public float scaleValue;    // 스케일 값
    float time = 0;
    float hitTime = 0;
    

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        StartCoroutine(SkillDisable());
    }

    private void OnEnable()
    {
        time = 0;
        scaleTime[0] = 0;
        scaleTime[1] = 0;
        StartCoroutine(SkillDisable());
    }

    void Update()
    {
        time += Time.deltaTime;

        if (time <= 1)
        {
            scaleTime[0] += Time.deltaTime;
            // 0.5초 안에 0에서 scaleValue까지 커지기
            transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(scaleValue, scaleValue, scaleValue), scaleTime[0] / 0.5f);  
        }
        else if (time >= 4.5f)
        {
            scaleTime[1] += Time.deltaTime;
            // 0.5초 안에 0에서 scaleValue까지 작아지기
            transform.localScale = Vector3.Lerp(new Vector3(scaleValue, scaleValue, scaleValue), new Vector3(0, 0, 0), scaleTime[1] / 0.5f);  
        }
    }

    IEnumerator SkillDisable()      // 5초 뒤 오브젝트 비활성화
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col) // Trigger 충돌 체크
    {
        if (col.gameObject.tag == "Mob")
        {
            int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
            col.GetComponent<MobData>().hp -= totalDamage;

            // 데미지 텍스트 생성하기
            GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
            dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
            dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
            dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Mob")
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 1f)
            {
                int totalDamage = Mathf.RoundToInt(skillDmg * player.power);
                col.GetComponent<MobData>().hp -= totalDamage;

                // 데미지 텍스트 생성하기
                GameObject dmgtxt = GameManager.instance.poolManager.Get(15);
                dmgtxt.transform.SetParent(GameObject.Find("DamageCanvas").transform);      // DamageCanvas의 자식으로 생성
                dmgtxt.transform.position = new Vector2(col.transform.position.x, col.transform.position.y + 0.2f);
                dmgtxt.GetComponent<DamageText>().text.text = totalDamage.ToString();
                hitTime = 0;
            }
        }
    }
}

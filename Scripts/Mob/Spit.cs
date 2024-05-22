using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spit : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private Rigidbody2D rigid;
    Vector2 dir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine("SpitDestroy");

        dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);    // 각도 플레이어 방향으로 변경
    }

    void Update()
    {
        if (rigid.bodyType == RigidbodyType2D.Dynamic)
            rigid.velocity = dir.normalized * 5f;
    }

    IEnumerator SpitDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    void DestroyObj()      // 애니메이션 끝부분에 넣어서 재생이 끝나면 사라지기
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!player.isHide)      // Hide 안한 상태면
            {
                animator.SetBool("bHit", true);
                rigid.bodyType = RigidbodyType2D.Static;
                GetComponent<BoxCollider2D>().enabled = false;   // 콜라이더를 안꺼주면 Hit 애니메이션 상태에서 계속 맞음

                player.nowHp -= 7;
                StopCoroutine("SpitDestroy");
            }
        }
    }
}

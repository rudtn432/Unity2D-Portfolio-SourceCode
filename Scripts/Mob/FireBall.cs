using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rigid;
    Vector2 dir;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine("BallDestroy");

        dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + 180f, Vector3.forward);    // 각도 플레이어 방향으로 변경
    }

    void Update()
    {
        rigid.velocity = dir.normalized * 8f;
    }

    IEnumerator BallDestroy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!player.isHide)      // Hide 안한 상태면
            {
                player.nowHp -= 10;
                StopCoroutine("BallDestroy");
                Destroy(gameObject);
            }
        }
    }
}

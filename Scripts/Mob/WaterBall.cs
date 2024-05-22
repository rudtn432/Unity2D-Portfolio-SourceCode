using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : MonoBehaviour
{
    private Player player;
    public int ballNum;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void OnEnable()
    {
        StartCoroutine("BallDestroy");
        if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Static)
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void DestroyObj()      // 애니메이션 끝부분에 넣어서 재생이 끝나면 사라지기
    {
        gameObject.SetActive(false);
    }

    IEnumerator BallDestroy()
    {
        yield return new WaitForSeconds(5f);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!player.isHide)      // Hide 안한 상태면
            {
                player.nowHp -= 20;
                StopCoroutine("BallDestroy");
                GetComponent<Animator>().SetBool("bHit", true);
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBall : MonoBehaviour
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
            if(!player.isHide)      // Hide 안한 상태면
            {
                player.nowHp -= 20;
                StopCoroutine("BallDestroy");
                gameObject.SetActive(false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Player player;
    private Sfx sfx;

    public string name;         // 아이템 이름

    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    // 충돌체크
    private void OnTriggerEnter2D(Collider2D col)
    {
        // 플레이어와 충돌할 때
        if (col.gameObject.tag == "Player")
        {
            Destroy(gameObject);                // 아이템 삭제
            switch(name)
            {
                case "RedPotion":               // 레드포션이랑 닿으면 nowHp + 10
                    sfx.SfxItemPotion();
                    player.nowHp += 10;
                    break;
                case "HideItem":                // 은신아이템이랑 닿으면 Player 은신
                    if (player.isHide)          // 은신 중인 상태에서 아이템을 또 먹으면 다시 재시작
                    {
                        player.StopCoroutine("Hide");
                        player.moveSpeed /= 1.3f;
                    }
                    player.StartCoroutine("Hide");
                    break;
            }
        }
    }
}

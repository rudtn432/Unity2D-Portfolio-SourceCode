using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clang : MobData
{
    Animator animator;
    private protected Rigidbody2D rigidbody;
    private protected Vector2 movement;

    float time;
    float hitTime;

    public Player player;

    private ItemManager itemManager;
    private SpriteRenderer renderer;
    private Sfx sfx;

    int a;
    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        itemManager = FindObjectOfType<ItemManager>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        time = 0;
        fadeTime = 2f;
        monsterName = "클랑";
        moveSpeed = 0.7f;
    }

    private void FixedUpdate()
    {
        if (hp > 0)
            PlayerTracking();
    }

    void Update()
    {
        if (isHit)
            StartCoroutine(KnockBack(3));

        if (hp <= 0)
        {
            if (a == 0)     // Update함수에 있어서 이게 없으면 경험치가 계속 올라감. 한번만 경험치가 오르게 하기위한 문장
            {
                sfx.SfxMob(10);
                MobDie();
                player.nowExp += 10;                                // 플레이어 경험치 증가
                itemManager.ItemDrop(this.transform.position);      // 아이템 드랍
                a++;
            }

            // die애니메이션이 실행중이면 false. 이게 없으면 애니메이션 무한반복
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("die")) animator.SetBool("bDie", false);

            if (time < fadeTime)
                renderer.color = new Color(1, 1, 1, 1f - time / fadeTime);
            else
            {
                time = 0;
                gameObject.SetActive(false);
            }
            time += Time.deltaTime;
        }
    }

    void PlayerTracking()
    {
        float dirX = player.transform.position.x - transform.position.x;
        dirX = (dirX < 0) ? -1 : 1;
        if (dirX < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);

        Vector3 me = transform.position;
        Vector3 target = player.transform.position;

        if (!isKnockBack)
        {
            transform.position = Vector3.MoveTowards(me, target, moveSpeed * Time.deltaTime);
            rigidbody.velocity = Vector2.zero;
        }
    }

    void MobDie()
    {
        rigidbody.bodyType = RigidbodyType2D.Static;        // 바디타입을 스태틱으로 바꿈으로써 죽는 애니메이션에서 공격도 안맞고 제자리에서 죽음
        gameObject.layer = 8;                               // MobDie 레이어로 변경해서 Player, Skill 레이어랑 충돌 안일어나게
        this.tag = "MobDie";                                // tag도 MobDie로 변경해서 스킬이 혼동 안일어나게
        renderer.sortingLayerName = "MobDie";               // Layer도 MobDie로 변경
        isDeath = true;
        animator.SetBool("bDie", true);
    }

    IEnumerator KnockBack(float _power)
    {
        isHit = false;
        isKnockBack = true;
        Vector3 dirVec = transform.position - player.transform.position;
        rigidbody.AddForce(dirVec.normalized * _power, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.2f);      // 0.2초간 넉백
        isKnockBack = false;
    }

    private void OnEnable()
    {
        if (isDeath)        // 몹이 죽었다가 다시 활성화되면 초기화
        {
            hp = GameManager.instance.mobSpawnManager.mobList[5].hp;
            time = 0;
            a = 0;
            moveSpeed = 0.7f;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;       // 바디타입을 다이나믹으로 바꿈
            gameObject.layer = 7;                               // Mob 레이어로 변경
            tag = "Mob";                                        // tag도 Mob으로 다시 변경
            renderer.sortingLayerName = "Mob";                  // Layer도 Mob으로 변경
            renderer.color = new Color(1, 1, 1, 1);
            isDeath = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0)
        {
            player.nowHp -= 3;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 0.5)
            {
                player.nowHp -= 3;
                hitTime = 0;
            }
        }
    }
}

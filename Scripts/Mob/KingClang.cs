using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KingClang : MobData
{
    Animator animator;
    private protected Rigidbody2D rigidbody;
    private protected Vector2 movement;
    private TimeManager timeManager;
    private MobSpawnManager mobSpawnManager;
    private CameraFollow cameraFollow;

    [SerializeField] private GameObject prfDangerSignal;    // 위험신호 프리팹
    [SerializeField] private GameObject prfHitEff;          // 플레이어 hit 이펙트
    [SerializeField] private GameObject canvas;     
    [SerializeField] private Image nowHpbar;
    [SerializeField] private TextMeshProUGUI tHpPercent;

    float time;
    float hitTime;

    float attTime;
    float rushTime;
    bool isAtt;
    float maxHp;
    bool isEmerge1;        // Emerge 타이밍을 따로 구분하기 위해 배열로 선언
    bool isEmerge2;        

    public Player player;

    private ItemManager itemManager;
    private SpriteRenderer renderer;
    private Sfx sfx;

    bool isRush;        // 대쉬공격 신호기

    int x;
    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        itemManager = FindObjectOfType<ItemManager>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = this.GetComponent<Rigidbody2D>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        canvas = GameObject.Find("Canvas").gameObject;
        nowHpbar = canvas.transform.Find("bgBossHp_bar").Find("BossHp_bar").gameObject.GetComponent<Image>();
        tHpPercent = canvas.transform.Find("bgBossHp_bar").Find("HpPercent").GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        mobSpawnManager = FindObjectOfType<MobSpawnManager>();

        isEmerge1 = false;
        isEmerge2 = false;
        attTime = Random.Range(8f, 10f);        // 일반 공격 8~10초마다 한번
        rushTime = Random.Range(5f, 7f);        // 돌진 5~7초마다 한번
        time = 0;
        fadeTime = 2f;
        hp = 8000;
        maxHp = hp;
        monsterName = "킹크랑";
        moveSpeed = 0.8f;
    }

    private void FixedUpdate()
    {
        if (hp > 0)
        {
            if (!isAtt && !animator.GetCurrentAnimatorStateInfo(0).IsName("die")
                && isEmerge2 && !isRush)
                PlayerTracking();
        }
    }

    void Update()
    {
        if(!isEmerge1 && timeManager.minute >= 3)
            StartCoroutine(Emerge());

        if (isEmerge2)
        {
            nowHpbar.fillAmount = Mathf.Lerp(nowHpbar.fillAmount, (float)hp / (float)maxHp, Time.deltaTime * 10f);      // 체력 바
            tHpPercent.text = ((hp / maxHp) * 100).ToString("F2") + "%";        // 보스 남은 체력 퍼센트
        }

        if (!isAtt && isEmerge2)
        {
            attTime -= Time.deltaTime;
            rushTime -= Time.deltaTime;
        }

        if (!isAtt && attTime <= 0)         // 일반 공격
        {
            isAtt = true;
            StartCoroutine(Att());
            attTime = Random.Range(8f, 10f);
        }
        else if(!isAtt && rushTime <= 0)    // 돌진
        {
            isAtt = true;
            StartCoroutine(Rush());
            rushTime = Random.Range(5f, 7f);
        }

        if (hp <= 0)
        {
            if (x == 0)     // Update함수에 있어서 이게 없으면 경험치가 계속 올라감. 한번만 경험치가 오르게 하기위한 문장
            {
                animator.speed = 1f;
                sfx.SfxMob(18);
                timeManager.timeStop = false;                       // 죽으면 시간이 다시 흐르게
                mobSpawnManager.regenStop = false;
                MobDie();
                player.nowExp += 100;                               // 플레이어 경험치 증가
                itemManager.ItemDrop(this.transform.position);      // 아이템 드랍
                StopAllCoroutines();                                // 코루틴 모두 멈추기
                canvas.transform.Find("bgBossHp_bar").gameObject.SetActive(false);  // 죽으면 보스 hpBar 안보이게 하기
                Destroy(GameObject.FindGameObjectWithTag("DangerSignal"));          // 위험신호가 있는 상태로 죽으면 객체가 안사라지니 찾아서 Destroy
                x++;
            }
            // die애니메이션이 실행중이면 false. 이게 없으면 애니메이션 무한반복
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("die")) animator.SetBool("bDie", false);

            if (time < fadeTime)
                renderer.color = new Color(1, 1, 1, 1f - time / fadeTime);
            else
            {
                time = 0;
                Destroy(gameObject);
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
        transform.position = Vector3.MoveTowards(me, target, moveSpeed * Time.deltaTime);
    }

    IEnumerator Emerge()      // 보스 출현
    {
        isEmerge1 = true;
        cameraFollow.ZoomInCoroutine(transform.position);   // ZoomIn 코루틴 부르기
        canvas.transform.Find("bgBossHp_bar").gameObject.SetActive(true);
        timeManager.timeStop = true;                        // 죽일때까지 시간 정지
        mobSpawnManager.regenStop = true;                   // 몹들 스폰도 정지
        tag = "Mob";
        animator.SetBool("bEmerge", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bEmerge", false);
        animator.SetBool("bMove", true);
        yield return new WaitForSeconds(0.85f);
        isEmerge2 = true;
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }

    IEnumerator Att()       // 공격
    {
        sfx.SfxMob(17);
        animator.SetBool("bAtt", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);
        GameObject dangerSignal = Instantiate(prfDangerSignal, transform.position, Quaternion.identity);      // 공격 위치에 미리 위험신호 표시
        dangerSignal.transform.localScale = new Vector2(9, 9);

        yield return new WaitForSeconds(0.75f);
        Destroy(dangerSignal);
        float dis = Vector2.Distance(player.transform.position, transform.position);    // 킹크랑과 플레이어 거리 계산
        if(dis <= 4.5f && !player.isNoDamage)         // 거리가 4.5이하면 Hit
        {
            player.nowHp -= 10;
            Instantiate(prfHitEff, player.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(1.45f);
        isAtt = false;
    }

    IEnumerator Rush()      // 돌진
    {
        isRush = true;
        float dirX = player.transform.position.x - transform.position.x;    // 플레이어방향으로 쳐다보기
        dirX = (dirX < 0) ? -1 : 1;
        if (dirX < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
        animator.speed = 2;                                                 // 돌진이라는걸 플레이어가 알아볼 수 있게 애니메이션 스피드 변경
        Vector2 pPos = player.transform.position - transform.position;      // 플레이어에게 날아가는 거리 계산
        yield return new WaitForSeconds(0.8f);
        rigidbody.AddForce(pPos.normalized * 415f, ForceMode2D.Impulse);    // 돌진

        yield return new WaitForSeconds(0.8f);
        rigidbody.velocity = new Vector2(0, 0);     // 0.8초 뒤 멈춤

        if (maxHp * 30 / 100 >= hp)          // hp가 maxHp의 30%이하라면 한번 더 돌진
        {
            yield return new WaitForSeconds(1f);
            dirX = player.transform.position.x - transform.position.x;    // 플레이어방향으로 쳐다보기
            dirX = (dirX < 0) ? -1 : 1;
            if (dirX < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else
                transform.localScale = new Vector3(-1, 1, 1);
            pPos = player.transform.position - transform.position;
            yield return new WaitForSeconds(0.8f);
            rigidbody.AddForce(pPos.normalized * 415f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.8f);
            rigidbody.velocity = new Vector2(0, 0);
        }
        animator.speed = 1;
        isRush = false;
        isAtt = false;
    }

    void MobDie()
    {
        rigidbody.bodyType = RigidbodyType2D.Static;        // 바디타입을 스태틱으로 바꿈으로써 죽는 애니메이션에서 공격도 안맞고 제자리에서 죽음
        gameObject.layer = 8;                               // MobDie 레이어로 변경해서 Player, Skill 레이어랑 충돌 안일어나게
        this.tag = "MobDie";                                // tag도 MobDie로 변경해서 스킬이 혼동 안일어나게
        renderer.sortingLayerName = "MobDie";               // Layer도 MobDie로 변경
        animator.SetBool("bDie", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0 && isEmerge2)
        {
            if (isRush)
            {
                player.nowHp -= 10;
                StartCoroutine(player.PlayerVelZero());
            }
            else
                player.nowHp -= 5;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0 && isEmerge2)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 0.5)
            {
                player.nowHp -= 5;
                hitTime = 0;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Mob" && isRush)
        {
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
    }
}

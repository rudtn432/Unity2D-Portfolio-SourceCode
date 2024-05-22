using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JuniorBalrog : MobData
{
    [SerializeField] private FireBall prfFireBall;
    [SerializeField] private GameObject prfEffAtt1;     // att1 이펙트
    [SerializeField] private GameObject prfEffAtt2;     // att2 이펙트
    [SerializeField] private GameObject canvas;     
    [SerializeField] private Image nowHpbar;
    [SerializeField] private TextMeshProUGUI tHpPercent;
    [SerializeField] private GameObject prfClear;
    private GameManager gameManager;
    private Camera camera;
    private CameraFollow cameraFollow;
    private Animator animator;
    private Player player;
    private SpriteRenderer renderer;
    private protected Rigidbody2D rigidbody;
    private GameObject AudioObject;
    private Sfx sfx;

    public bool isDie;
    float time;
    float hitTime;
    float maxHp;
    float coolTime;
    float value;
    int attackCount;
    public bool isAtt;
    bool isRush;        // 대쉬공격 신호기
    bool isFury;        // 체력이 40%이하라면 분노 상태
    int a;

    void Start()
    {
        AudioObject = GameObject.Find("BGM_Audio_Source");
        canvas = GameObject.Find("Canvas").gameObject;
        canvas.transform.Find("bgBossHp_bar").gameObject.SetActive(true);
        nowHpbar = canvas.transform.Find("bgBossHp_bar").Find("BossHp_bar").gameObject.GetComponent<Image>();
        tHpPercent = canvas.transform.Find("bgBossHp_bar").Find("HpPercent").GetComponent<TextMeshProUGUI>();
        gameManager = FindObjectOfType<GameManager>();
        sfx = FindObjectOfType<Sfx>();
        camera = FindObjectOfType<Camera>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        AudioObject.GetComponent<PlayMusicOperator>().PlayBGM("2");     // 배경음악을 바꾼다

        monsterName = "주니어발록";
        moveSpeed = 0.8f;
        maxHp = 30000;
        hp = 30000;
        coolTime = 0;
        value = 5;
        fadeTime = 1.2f;
        attackCount = 0;
        isAtt = false;
        isRush = false;
    }

    void Update()
    {
        nowHpbar.fillAmount = Mathf.Lerp(nowHpbar.fillAmount, (float)hp / (float)maxHp, Time.deltaTime * 10f);      // 체력 바
        tHpPercent.text = ((hp / maxHp) * 100).ToString("F2") + "%";        // 보스 남은 체력 퍼센트

        if (maxHp * 40 / 100 >= hp)     // 체력이 40%이라면 분노 상태
            isFury = true;

        if (!isAtt)
            coolTime += Time.deltaTime;

        if (isAtt == false && coolTime >= value)    // 쿨타임이 value값 이상이면 공격
        {
            if (attackCount == 0)
            {
                StartCoroutine("Att1");
                attackCount++;
            }
            else if (attackCount == 3)
            {
                StartCoroutine("Att3");
                attackCount = 0;
            }
            else
            {
                StartCoroutine("Att2");
                attackCount++;
            }
            coolTime = 0;
            value = Random.Range(3, 6);         // value값 랜덤으로 3~5초 사이로 설정
            isAtt = true;
        }

        // 공격 중이라면 가만히 멈춘 상태로 공격
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("att1") && !animator.GetCurrentAnimatorStateInfo(0).IsName("att2")
            && !animator.GetCurrentAnimatorStateInfo(0).IsName("die"))
            PlayerTracking();

        if(hp <= 0)
        {
            if(a == 0)
            {
                StopAllCoroutines();                            // 공격 중일때 죽을걸 대비해서 모든 코루틴 중지
                sfx.SfxMob(7);
                sfx.SfxClear();
                MobDie();
                // ClearAnim 게임오브젝트 Active True
                canvas.transform.Find("StageAnim").Find("ClearAnim").gameObject.SetActive(true);
                gameManager.StageClear1();
                player.collider.isTrigger = true;
                cameraFollow.ZoomInCoroutine(transform.position);   // ZoomIn 코루틴 부르기
                // 보스 죽이면 플레이어 체력을 높여서 안죽게
                player.maxHp *= 100;
                player.nowHp *= 100;
                a++;
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
        animator.SetBool("bMove", true);
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

    IEnumerator Att1()      // 일렉트릭볼 날리기 공격
    {
        int roundNum;       // 한번에 날리는 일렉트릭볼 갯수
        float angle;        // 각도
        animator.SetInteger("iAtt", 1);
        animator.SetBool("bAtt", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);
        yield return new WaitForSeconds(1.2f);
        Instantiate(prfEffAtt1, new Vector2(transform.position.x, transform.position.y + 0.5f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        animator.speed = 0f;                    // 애니메이션 자연스럽게 정지
        if (isFury)             // 분노 상태라면
        {
            roundNum = 18;
            angle = 10;
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 10도, 30도, 50도... 350도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 20f;                           // 각도 20도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 10도, 30도, 50도... 350도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 20f;                           // 각도 20도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
        }
        else
        {
            roundNum = 9;
            angle = 20;
            for (int i = 0; i < roundNum; i++)          // 0도, 40도, 80도... 320도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 20도, 60도, 100도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 40f;                           // 각도 40도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 40도, 80도... 320도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(16);
                obj.transform.position = new Vector2(transform.position.x, transform.position.y + 0.5f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
        }
        animator.speed = 1f;                            // 애니메이션 다시 재생
        yield return new WaitForSeconds(0.3f);
        isAtt = false;
    }

    IEnumerator Att2()      // 파이어볼 날리기 공격
    {
        animator.SetInteger("iAtt", 2);
        animator.SetBool("bAtt", true);
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);
        yield return new WaitForSeconds(0.1f);
        if (transform.localScale == new Vector3(-1, 1, 1))
        {
            GameObject instance = Instantiate(prfEffAtt2, new Vector2(transform.position.x + 0.127f, transform.position.y), Quaternion.identity);
            instance.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
            Instantiate(prfEffAtt2, new Vector2(transform.position.x - 0.127f, transform.position.y), Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        if(isFury)              // 분노 상태라면
        {
            for (int i = 0; i < 5; i++)         // 파이어볼 플레이어에게 5번 쏘기
            {
                Instantiate(prfFireBall, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            for (int i = 0; i < 3; i++)         // 파이어볼 플레이어에게 3번 쏘기
            {
                Instantiate(prfFireBall, new Vector2(transform.position.x, transform.position.y + 0.2f), Quaternion.identity);
                yield return new WaitForSeconds(0.2f);
            }
            yield return new WaitForSeconds(0.8f);
        }
        isAtt = false;
    }

    IEnumerator Att3()      // 돌진
    {
        animator.SetInteger("iAtt", 2);
        animator.SetBool("bAtt", true);
        Vector2 pPos = player.transform.position - transform.position;      // 플레이어에게 날아가는 거리 계산
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);
        yield return new WaitForSeconds(0.6f);
        isRush = true;
        rigidbody.AddForce(pPos.normalized * 415f, ForceMode2D.Impulse);    // 돌진

        yield return new WaitForSeconds(0.8f);
        rigidbody.velocity = new Vector2(0, 0);                             // 0.8초 뒤 멈춤
        isRush = false;

        if(isFury)          // 분노 상태라면 한번 더 돌진
        {
            yield return new WaitForSeconds(1f);
            animator.SetInteger("iAtt", 2);
            animator.SetBool("bAtt", true);
            pPos = player.transform.position - transform.position;
            yield return new WaitForSeconds(0.001f);
            animator.SetBool("bAtt", false);
            yield return new WaitForSeconds(0.6f);
            isRush = true;
            rigidbody.AddForce(pPos.normalized * 415f, ForceMode2D.Impulse);

            yield return new WaitForSeconds(0.8f);
            rigidbody.velocity = new Vector2(0, 0);
            isRush = false;
        }
        isAtt = false;
    }

    void MobDie()
    {
        rigidbody.bodyType = RigidbodyType2D.Static;    // 바디타입을 스태틱으로 바꿈으로써 죽는 애니메이션에서 공격도 안맞고 제자리에서 죽음
        gameObject.layer = 8;                           // MobDie 레이어로 변경해서 Player, Skill 레이어랑 충돌 안일어나게
        this.tag = "MobDie";                            // tag도 MobDie로 변경해서 스킬이 혼동 안일어나게
        renderer.sortingLayerName = "MobDie";           // Layer도 MobDie로 변경
        animator.SetBool("bDie", true);
        isDie = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0)
        {
            if (isRush)
            {
                player.nowHp -= 20;
                StartCoroutine(player.PlayerVelZero());
            }
            else
                player.nowHp -= 4;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 0.5)
            {
                player.nowHp -= 4;
                hitTime = 0;
            }
        }
    }
}

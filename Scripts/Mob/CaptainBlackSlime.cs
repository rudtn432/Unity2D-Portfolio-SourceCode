using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CaptainBlackSlime : MobData
{
    [SerializeField] private GameObject prfEffHit1;         // 플레이어가 맞았을 때 Hit1 이펙트
    [SerializeField] private GameObject prfEffHit2;         // 플레이어가 맞았을 때 Hit2 이펙트
    [SerializeField] private GameObject prfDangerSignal;    // 위험신호 프리팹
    [SerializeField] private GameObject square;       
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image nowHpbar;
    [SerializeField] private TextMeshProUGUI tHpPercent;
    [SerializeField] private GameObject prfClear;
    private GameManager gameManager;
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
    bool isHitAtt2;
    bool isFury;           // 체력 40퍼이하면 분노 상태
    int a;

    void Start()
    {
        AudioObject = GameObject.Find("BGM_Audio_Source");
        canvas = GameObject.Find("Canvas").gameObject;
        canvas.transform.Find("bgBossHp_bar").gameObject.SetActive(true);
        nowHpbar = canvas.transform.Find("bgBossHp_bar").Find("BossHp_bar").gameObject.GetComponent<Image>();
        tHpPercent = canvas.transform.Find("bgBossHp_bar").Find("HpPercent").GetComponent<TextMeshProUGUI>();
        square = GameObject.Find("Map").transform.GetChild(0).gameObject;
        gameManager = FindObjectOfType<GameManager>();
        sfx = FindObjectOfType<Sfx>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        AudioObject.GetComponent<PlayMusicOperator>().PlayBGM("5");     // 배경음악을 바꾼다

        monsterName = "캡틴블랙 슬라임";
        moveSpeed = 0f;
        maxHp = 50000;
        hp = 50000;
        coolTime = 0;
        value = 5;
        fadeTime = 1.2f;
        attackCount = 2;
        isAtt = false;
        isHitAtt2 = false;
        isFury = false;
    }

    void Update()
    {
        nowHpbar.fillAmount = Mathf.Lerp(nowHpbar.fillAmount, (float)hp / (float)maxHp, Time.deltaTime * 10f);      // 체력 바
        tHpPercent.text = ((hp / maxHp) * 100).ToString("F2") + "%";        // 보스 남은 체력 퍼센트

        if (!isAtt && !isDie)
        {
            coolTime += Time.deltaTime;
            PlayerLook();
        }

        if (maxHp * 40 / 100 >= hp)     // 체력이 40%이라면 분노 상태
            isFury = true;

        if (isAtt == false && coolTime >= value)    // 쿨타임이 value값 이상이면 공격
        {
            if (attackCount == 2)
            {
                StartCoroutine("Att1");
                attackCount = 0;
            }
            else
            {
                int randomNum = Random.Range(2, 4);
                if (randomNum == 2)
                    StartCoroutine("Att2");
                if (randomNum == 3)
                    StartCoroutine("Att3");
                attackCount++;
            }
            coolTime = 0;
            value = Random.Range(3, 6);         // value값 랜덤으로 3~5초 사이로 설정
            isAtt = true;
        }

        if (hp <= 0)
        {
            if (a == 0)
            {
                sfx.SfxMob(23);
                sfx.SfxClear();
                MobDie();
                StopAllCoroutines();                            // 공격 중일때 죽을걸 대비해서 모든 코루틴 중지
                // ClearAnim 게임오브젝트 Active True
                canvas.transform.Find("StageAnim").Find("ClearAnim").gameObject.SetActive(true);
                gameManager.StageClear2();
                animator.speed = 1f;
                player.collider.isTrigger = true;
                cameraFollow.ZoomInCoroutine(transform.position);                   // ZoomIn 코루틴 부르기
                Destroy(GameObject.FindGameObjectWithTag("DangerSignal"));          // 위험신호가 있는 상태로 죽으면 객체가 안사라지니 찾아서 Destroy
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

    void PlayerLook()
    {
        float dirX = player.transform.position.x - transform.position.x;
        dirX = (dirX < 0) ? -1 : 1;
        if (dirX < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    IEnumerator Att1()      // 텔레포트 후 공격
    {
        Vector3 _pos;       // 위치 저장 변수
        _pos = square.transform.position + (Random.insideUnitSphere * 7f);      // 맵 중앙 주위 반경 7f안의 랜덤 위치로 순간이동
        animator.SetInteger("iAtt", 1);
        animator.SetBool("bAtt", true);
        PlayerLook();
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);

        yield return new WaitForSeconds(0.5f);
        animator.speed = 0;
        GameObject dangerSignal = Instantiate(prfDangerSignal, _pos, Quaternion.identity);      // 순간이동할 위치에 미리 위험신호 표시
        sfx.SfxMob(20);

        yield return new WaitForSeconds(1f);
        animator.speed = 1;
        transform.position = _pos;
        PlayerLook();
        if (Vector2.Distance(transform.position, player.transform.position) <= 4f && !player.isNoDamage)      // 플레이어와 보스 거리가 4f이하면 Hit
        {
            player.nowHp -= 15;
            Instantiate(prfEffHit1, player.transform.position, Quaternion.identity);    // Hit 이펙트를 플레이어 위치에 생성
        }
        Destroy(dangerSignal);

        yield return new WaitForSeconds(1f);
        isAtt = false;
    }

    IEnumerator Att2()      // 근처에 있으면 독 감염
    {
        Vector3 _pos1;                                           // 위치 저장 변수
        Vector3 _pos2;                                           
        _pos1 = player.transform.position + (Random.insideUnitSphere * 5f);          // 플레이어 주위 반경 5f안의 랜덤 위치 반환
        _pos2 = player.transform.position + (Random.insideUnitSphere * 5f);          
        GameObject dangerSignal1 = Instantiate(prfDangerSignal, _pos1, Quaternion.identity);      // 독 뿌려지는 위치에 미리 위험신호 표시
        dangerSignal1.transform.localScale = new Vector2(6, 6);
        GameObject dangerSignal2 = Instantiate(prfDangerSignal, _pos2, Quaternion.identity);      // 독 뿌려지는 위치에 미리 위험신호 표시
        dangerSignal2.transform.localScale = new Vector2(6, 6);
        if (!isFury)    // 분노 상태가 아니라면 위험신호2 파괴
            Destroy(dangerSignal2);
        animator.SetInteger("iAtt", 2);
        animator.SetBool("bAtt", true);
        sfx.SfxMob(21);
        PlayerLook();
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);

        yield return new WaitForSeconds(0.91f);
        if (Vector2.Distance(_pos1, player.transform.position) <= 3f && !player.isNoDamage)           // 플레이어와 Att2 거리가 3f이하면 Hit
            StartCoroutine("Att2Hit");
        Destroy(dangerSignal1);
        if (isFury)     // 분노 상태면 독 2개 생성
        {
            if (Vector2.Distance(_pos2, player.transform.position) <= 3f && !player.isNoDamage)       // 플레이어와 Att2 거리가 3f이하면 Hit
                StartCoroutine("Att2Hit");
            Destroy(dangerSignal2);
        }

        yield return new WaitForSeconds(0.8f);
        isAtt = false;
    }
    IEnumerator Att2Hit()
    {
        for (int i = 0; i < 3; i++)         
        {
            if (isFury)                    // 분노 상태면 3초동안 1초마다 -5 Hp 아니면 -3 Hp
                player.nowHp -= 5;
            else
                player.nowHp -= 3;
            Instantiate(prfEffHit2, player.transform.position, Quaternion.identity);    // Hit 이펙트를 플레이어 위치에 생성
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator Att3()      // 워터볼 날리기 공격
    {
        int roundNum;       // 한번에 날리는 워터볼 갯수
        float angle;        // 각도
        float xPos;
        if (transform.localScale.x == 1)
            xPos = -0.684f;
        else
            xPos = 0.684f;
        animator.SetInteger("iAtt", 3);
        animator.SetBool("bAtt", true);
        sfx.SfxMob(22);
        PlayerLook();
        yield return new WaitForSeconds(0.001f);
        animator.SetBool("bAtt", false);
        yield return new WaitForSeconds(0.666f);
        animator.speed = 0f;                    // 애니메이션 자연스럽게 정지
        if (isFury)             // 분노 상태라면
        {
            roundNum = 18;
            angle = 10;
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 10도, 30도, 50도... 350도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 20f;                           // 각도 20도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 10도, 30도, 50도... 350도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 20f;                           // 각도 20도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 20도, 40도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
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
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 20도, 60도, 100도... 340도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                angle += 40f;                           // 각도 40도씩 +
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < roundNum; i++)          // 0도, 40도, 80도... 320도 발사
            {
                GameObject obj = GameManager.instance.poolManager.Get(17);
                obj.transform.position = new Vector2(transform.position.x + xPos, transform.position.y - 0.553f);
                Rigidbody2D rigid = obj.GetComponent<Rigidbody2D>();
                Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundNum), Mathf.Sin(Mathf.PI * 2 * i / roundNum));
                rigid.AddForce(dirVec.normalized * 5f, ForceMode2D.Impulse);
            }
        }
        animator.speed = 1f;                            // 애니메이션 다시 재생
        yield return new WaitForSeconds(0.5f);
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
            player.nowHp -= 5;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && player.nowHp >= 0)
        {
            hitTime += Time.deltaTime;
            if (hitTime >= 0.5)
            {
                player.nowHp -= 5;
                hitTime = 0;
            }
        }
    }
}

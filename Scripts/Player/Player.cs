using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody;
    public Animator animator;
    public BoxCollider2D collider;
    private SkillSelect skillSelect;
    private DieSelect dieSelect;
    [SerializeField] private GameObject prfGameOver;
    private SpriteRenderer renderer;
    private Camera camera;
    private CameraFollow cameraFollow;
    private Sfx sfx;
    private CoolTimeImage coolTimeImage;

    public int maxHp;           // 최대 체력
    public int nowHp;           // 현재 체력
    public float maxExp;        // 최대 경험치
    public float nowExp;        // 현재 경험치
    public float moveSpeed;     // 이동 속도
    public float power;         // 공격력
    public float attackSpeed;   // 공격 속도
    public int level;           // 현재 플레이어 레벨
    public int lvUpCount;       // 레벨업한 횟수(만약 대량 경험치를 획득해서 Lv.1에서 Lv.3으로 올랐을 떄 SkillSelect.Call을 두번하기 위해 만든 변수)
    public int sp;              // 스킬 포인트
    public bool isDie;          // 죽었는지 확인
    public bool isHide;         // Hide 했는지 확인
    public bool isNoDamage;     // 데미지를 받지않는 무적상태인지
    public int iRegenNum;       // 남은 부활 횟수
    public int a;               // 한번만 실행되도록 해주는 변수
    public Vector2 nowPos;      // 현재 포지션 저장할 변수

    public Image maxHpbar;
    public Image nowHpbar;
    public Image nowExpbar;
    public TextMeshProUGUI tlevel;
    public TextMeshProUGUI tSP1;
    public TextMeshProUGUI tSP2;

    bool isLeft;
    bool isRight;
    bool isUp;
    bool isDown;
    bool isPlayerAvoid;         // 플레이어 회피 신호기
    Vector2 avoidPos;           // 플레이어가 회피해야할 방향 벡터
    private float playerSizeX = 0.3f;       // 플레이어x 반지름 
    private float playerSizeY = 0.4f;       // 플레이어y 반지름 
    private float radius = 0.2f;            // 원으로 도는 반지름
    private float runningTime = 0;
    private Vector2 newPos = new Vector2();
    void Start()
    {
        coolTimeImage = GameObject.Find("Canvas").transform.Find("AvoidCoolTime").GetComponent<CoolTimeImage>();
        camera = FindObjectOfType<Camera>();
        cameraFollow = FindObjectOfType<CameraFollow>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        skillSelect = FindObjectOfType<SkillSelect>();
        dieSelect = FindObjectOfType<DieSelect>();
        sfx = FindObjectOfType<Sfx>();
        renderer = GetComponent<SpriteRenderer>();
        animator.SetBool("die", false);

        maxHp = 100;
        nowHp = 100;
        maxExp = 10;
        nowExp = 0;
        moveSpeed = 1f;
        power = 1f;
        attackSpeed = 1f;
        level = 1;
        lvUpCount = 0;
        sp = 0;
        iRegenNum = 2;
        isDie = false;
        isHide = false;
    }

    private void FixedUpdate()
    {
        Vector2 _hpBarPos = Camera.main.WorldToScreenPoint(new Vector2(transform.position.x, transform.position.y - 0.4f));
        animator.SetBool("move", false);
        float moveX = 0f;
        float moveY = 0f;
        isRight = false;
        isLeft = false;
        isUp = false;
        isDown = false;

        if (!isDie)  // 죽지 않았다면
        {
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) // 오른쪽 화살표
            {
                transform.localScale = new Vector3(-1, 1, 1);
                animator.SetBool("move", true);
                moveX += 1f;
                isRight = true;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) // 왼쪽 화살표
            {
                transform.localScale = new Vector3(1, 1, 1);
                animator.SetBool("move", true);
                moveX -= 1f;
                isLeft = true;
            }
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) // 위쪽 화살표
            {
                animator.SetBool("move", true);
                moveY += 1f;
                isUp = true;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) // 아래쪽 화살표
            {
                animator.SetBool("move", true);
                moveY -= 1f;
                isDown = true;
            }

            if (!isPlayerAvoid && Input.GetKey(KeyCode.Space))
                StartCoroutine("Avoid");

            if (transform.position.x + playerSizeX >= cameraFollow.maxPos.x)        // 카메라를 벗어나면 못움직이게
                transform.position = new Vector2(cameraFollow.maxPos.x - playerSizeX, transform.position.y);
            else if (transform.position.x - playerSizeX <= cameraFollow.minPos.x)    
                transform.position = new Vector2(cameraFollow.minPos.x + playerSizeX, transform.position.y);
            if (transform.position.y + playerSizeY >= cameraFollow.maxPos.y)    
                transform.position = new Vector2(transform.position.x, cameraFollow.maxPos.y - playerSizeY);
            else if (transform.position.y - playerSizeY <= cameraFollow.minPos.y)   
                transform.position = new Vector2(transform.position.x, cameraFollow.minPos.y + playerSizeY);

            transform.Translate(new Vector2(moveX, moveY) * Time.deltaTime * moveSpeed);
            maxHpbar.transform.position = _hpBarPos;
        }
    }

    void Update()
    {
        //nowHpbar.fillAmount = Mathf.Lerp(nowHpbar.fillAmount, (float)nowHp / (float)maxHp, Time.deltaTime * 10f);       //체력 바
        nowHpbar.fillAmount = (float)nowHp / (float)maxHp;       //체력 바
        nowExpbar.fillAmount = Mathf.Lerp(nowExpbar.fillAmount, (float)nowExp / (float)maxExp, Time.deltaTime * 10f);   //경험치 바

        if(nowHp > 0)
        {
            if (nowHp >= maxHp)
                nowHp = maxHp;

            if (nowExp >= maxExp)       // 레벨업
            {
                lvUpCount += 1;
                nowExp = nowExp - maxExp;
                maxExp += 5;
                level += 1;
                sp += 1;
                skillSelect.Call();
            }
        }
        else    // 죽었을 때
        {
            if(a == 0)      // 한번만 실행되게
            {
                sfx.SfxGameOver();
                Time.timeScale = 0f;
                isDie = true;
                animator.SetBool("die", true);
                collider.isTrigger = true;
                nowPos = this.transform.position;       // 현재 position 저장
                // GameOverAnim 게임오브젝트 Active True
                GameObject.Find("Canvas").transform.Find("StageAnim").Find("GameOverAnim").gameObject.SetActive(true);
                StartCoroutine("CallDieSelect");
                a++;
            }
            runningTime = Time.unscaledTime * 2f;      // 원으로 돌기
            float x = radius * Mathf.Cos(runningTime);
            float y = radius * Mathf.Sin(runningTime);
            newPos = new Vector2(x, y);
            this.transform.position = newPos + nowPos;  // 지금 위치에서 돌게 하기
        }

        tlevel.text = "Lv. " + level.ToString();
        tSP1.text = "SP : " + sp.ToString();
        tSP2.text = "현재 보유한 SP 포인트 : " + sp.ToString();

        if (Input.GetKeyDown(KeyCode.C))
            nowHp = 0;
        if (Input.GetKeyDown(KeyCode.B))
        {
            maxHp = 1000000;
            nowHp = 1000000;
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            nowExp = maxExp;
        }
    }

    IEnumerator CallDieSelect()
    {
        yield return new WaitForSecondsRealtime(2.0f);      // timeScale이 0이여도 시간이 흘러가게 Realtime 사용
        dieSelect.Call();
    }
    public IEnumerator Hide()       // 은신
    {
        isHide = true;
        renderer.color = new Color(1, 1, 1, 0.5f);      // 렌더링을 은신된 것처럼
        gameObject.layer = 10;                          // 몹이랑 충돌 안되게 layer 10번으로 변경
        moveSpeed *= 1.3f;                              // 이동속도 30% 증가
        yield return new WaitForSeconds(3.0f);          // 3초 뒤 원래대로
        isHide = false;
        gameObject.layer = 6;                           // 원래 레이어로 변경
        moveSpeed /= 1.3f;
        renderer.color = new Color(1, 1, 1, 1);
    }

    public IEnumerator PlayerVelZero()     // 플레이어가 돌진을 맞고 날아갈때 계속 날아가지 않게 시간초 정해주고 velocity 초기화
    {
        yield return new WaitForSeconds(0.5f);
        rigidbody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        rigidbody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        rigidbody.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        rigidbody.velocity = new Vector2(0, 0);
    }

    IEnumerator Avoid()     // 회피
    {
        isPlayerAvoid = true;
        isHide = true;
        collider.isTrigger = true;
        if (isLeft)
        {
            if (isDown)
                avoidPos = new Vector2(-1, -1);
            else if (isUp)
                avoidPos = new Vector2(-1, 1);
            else
                avoidPos = new Vector2(-1, 0);
        }
        else if (isRight)
        {
            if (isDown)
                avoidPos = new Vector2(1, -1);
            else if (isUp)
                avoidPos = new Vector2(1, 1);
            else
                avoidPos = new Vector2(1, 0);
        }
        else if (isUp)
            avoidPos = new Vector2(0, 1);
        else if (isDown)
            avoidPos = new Vector2(0, -1);
        else if (!isLeft && !isRight)
        {
            if (transform.localScale.x == 1)
                avoidPos = new Vector2(-1, 0);
            else
                avoidPos = new Vector2(1, 0);
        }

        rigidbody.AddForce(avoidPos.normalized * 300f, ForceMode2D.Impulse);    

        yield return new WaitForSeconds(0.1f);
        rigidbody.velocity = new Vector2(0, 0);
        collider.isTrigger = false;
        isHide = false;

        coolTimeImage.StartCoolTime(9.99999f);
        yield return new WaitForSeconds(9.99999f);      // 10초 뒤에 재사용 가능
        isPlayerAvoid = false;
    }

    IEnumerator Invincible()    // 부활 후 1초동안 무적
    {
        isNoDamage = true;
        renderer.color = new Color(1, 1, 1, 1);
        gameObject.layer = 10;                          // 몹이랑 충돌 안되게 layer 10번으로 변경
        for (int i = 0; i < 10; i++)
        {
            renderer.color = new Color(0.5f, 0.5f, 0.5f, 1);
            yield return new WaitForSeconds(0.1f);
            renderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
        gameObject.layer = 6;
        isNoDamage = false;
    }

    public void Revival()       // 부활
    {
        isDie = false;
        collider.isTrigger = false;
        animator.SetBool("die", false);
        animator.SetBool("regen", true);
        rigidbody.velocity = new Vector2(0, 0);
        transform.position = nowPos;            // 죽기 전 포지션으로 부활
        nowHp = maxHp;                          // 최대체력으로 부활
        iRegenNum -= 1;                         // 부활 횟수 차감
        a = 0;                                  // 한번만 실행되게 하는 변수 초기화
        StartCoroutine("Invincible");
    }
}

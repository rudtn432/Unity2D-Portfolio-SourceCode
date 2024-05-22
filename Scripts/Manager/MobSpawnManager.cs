using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobSpawnManager : MonoBehaviour
{
    Scene scene;

    public MobData[] mobList;

    [SerializeField] private GameObject prfBossSpawnEff;
    private GameObject canvas;
    public int[] mobNum;        // 몬스터 생성 할 때 한번당 몇마리
    public float[] createTime;
    private GameObject player;
    private TimeManager timeManager;
    private bool regenBoss = false;
    public bool regenStop = false;
    public List<GameObject> FoundObjects;
    public List<GameObject> blackCurtain;
    public float[] lerpTime;

    int a = 0;

    void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        timeManager = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
        scene = SceneManager.GetActiveScene();
        mobNum[0] = 8;
        mobNum[2] = 12;
        mobNum[3] = 8;
    }

    void Update()
    {
        if (scene.name == "Stage1")        // Stage1 이면
        {
            // 0 달팽이, 2 돼지, 3 스티지
            if(!regenStop)
            {
                createTime[0] += Time.deltaTime;
                if (timeManager.minute >= 3)            // 3분이 지났을 때 생성
                    createTime[1] += Time.deltaTime;
                if (timeManager.minute >= 1)            // 1분이 지났을 때 생성
                    createTime[2] += Time.deltaTime;
            }

            // 난이도 조절을 위해 1분마다 체력과 몹 생성되는 갯수 조절
            if (timeManager.minute >= 4)
            {
                mobNum[0] = 12;
                mobList[0].hp = 60;

                mobNum[2] = 16;
                mobList[2].hp = 200;

                mobNum[3] = 12;
                mobList[3].hp = 30;
            }
            else if (timeManager.minute >= 3)
            {
                mobNum[0] = 12;
                mobList[0].hp = 50;

                mobNum[2] = 12;

                mobNum[3] = 12;
            }
            else if (timeManager.minute >= 2)
            {
                mobNum[0] = 16;
                mobList[0].hp = 40;

                mobNum[3] = 12;
                mobList[3].hp = 20;
            }
            else if (timeManager.minute >= 1)
            {
                mobNum[0] = 12;
                mobList[0].hp = 30;

                mobNum[3] = 8;
            }

            if (!regenStop && createTime[0] >= 5)       // 시간0이 5초 지나면 몹0 생성
            {
                MobCreate(0);
                createTime[0] = 0;
            }
            if (!regenStop && createTime[1] >= 10)         // 시간1이 10초 지나면 몹2 생성
            {
                MobCreate(2);
                createTime[1] = 0;
            }
            if (!regenStop && createTime[2] >= 5)       // 시간2가 5초 지나면 몹3 생성
            {
                MobCreate(3);
                createTime[2] = 0;
            }
            if (regenBoss == false && timeManager.minute >= 5)  // 5분 지나면 보스 생성
            {
                // 보스 생성 이펙트
                Instantiate(prfBossSpawnEff, new Vector2(0, 3f), Quaternion.identity);
                regenBoss = true;
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 59.5f)     // 4분59.5초에 BlackCurtain 사라지기
            {
                if (a == 0)
                {
                    player.transform.position = Vector2.zero;   // 플레이어 위치 0,0으로
                    GameObject.Find("Map").transform.GetChild(0).gameObject.SetActive(true);        // 벽 세우기
                    a++;
                }
                lerpTime[1] += Time.deltaTime;
                // BlackCurtain3 게임오브젝트 Active True
                blackCurtain[2].SetActive(true);
                // 0.5초만에 지정된 위치로 이동
                blackCurtain[2].transform.position = Vector2.Lerp(canvas.transform.position,
                    new Vector2(canvas.transform.position.x - 2852, canvas.transform.position.y - 1604), lerpTime[1] / 0.5f);
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 58)     // 4분58초에 BlackCurtain
            {
                lerpTime[0] += Time.deltaTime;
                // BlackCurtain1,2 게임오브젝트 Active True
                blackCurtain[0].SetActive(true);
                blackCurtain[1].SetActive(true);
                // 1초만에 지정된 위치로 이동
                blackCurtain[0].transform.position = Vector2.Lerp(new Vector2(canvas.transform.position.x + 960, canvas.transform.position.y),
                    new Vector2(canvas.transform.position.x - 50, canvas.transform.position.y), lerpTime[0] / 1f);
                blackCurtain[1].transform.position = Vector2.Lerp(new Vector2(canvas.transform.position.x - 960, canvas.transform.position.y), 
                    canvas.transform.position, lerpTime[0] / 1f);
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 57)     // 4분57초에 Warning
            {
                // WarningAnim 게임오브젝트 Active True
                GameObject.Find("Canvas").transform.Find("StageAnim").Find("WarningAnim").gameObject.SetActive(true);
            }
            else if(regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 56)      // 4분56초에 몹 사망
            {
                FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mob"));
                foreach (GameObject found in FoundObjects)
                    found.GetComponent<MobData>().hp = 0;
                regenStop = true;   // 몹리젠 멈추기
            }
        }
        else                            // Stage2 이면
        {
            // 4 로랑, 5 클랑, 6 루팡, 7 엄티

            if(!regenStop)
            {
                createTime[0] += Time.deltaTime;
                if (timeManager.minute >= 1)            // 1분이 지났을 때 생성
                    createTime[2] += Time.deltaTime;
                if (timeManager.minute >= 2)            // 2분이 지났을 때 생성
                    createTime[3] += Time.deltaTime;
                if (timeManager.minute >= 3)            // 3분이 지났을 때 생성
                    createTime[1] += Time.deltaTime;
            }

            // 난이도 조절을 위해 1분마다 체력과 몹 생성되는 갯수 조절
            if (timeManager.minute >= 4)
            {
                mobNum[4] = 4;
                mobList[4].hp = 50;

                mobNum[6] = 8;

                mobNum[7] = 8;
                mobList[7].hp = 150;

                mobNum[5] = 12;
                mobList[5].hp = 200;

            }
            else if (timeManager.minute >= 3)
            {
                mobNum[4] = 8;
                mobList[4].hp = 40;

                mobNum[6] = 8;
                mobList[6].hp = 60;

                mobNum[7] = 8;
            }
            else if (timeManager.minute >= 2)
            {
                mobList[4].hp = 35;
            }
            else if (timeManager.minute >= 1)
            {
                mobNum[4] = 12;
                mobList[4].hp = 30;
            }

            if (!regenStop && createTime[0] >= 5)           // 시간0이 5초 지나면 몹4 생성
            {
                MobCreate(4);
                createTime[0] = 0;
            }
            if (!regenStop && createTime[2] >= 7)           // 시간2(1분)가 7초 지나면 몹6 생성
            {
                MobCreate(6);
                createTime[2] = 0;
            }
            if (!regenStop && createTime[3] >= 10)          // 시간3(2분)이 10초 지나면 몹7 생성
            {
                MobCreate(7);
                createTime[3] = 0;
            }
            if (!regenStop && createTime[1] >= 10)          // 시간1(3분)이 10초 지나면 몹5 생성
            {
                MobCreate(5);
                createTime[1] = 0;
            }
            if (regenBoss == false && timeManager.minute >= 5)  // 5분 지나면 보스 생성
            {
                // 보스 생성 이펙트
                Instantiate(prfBossSpawnEff, new Vector2(0, 3f), Quaternion.identity);
                regenBoss = true;
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 59.5f)     // 4분59.5초에 BlackCurtain 사라지기
            {
                if (a == 0)
                {
                    player.transform.position = Vector2.zero;   // 플레이어 위치 0,0으로
                    GameObject.Find("Map").transform.GetChild(0).gameObject.SetActive(true);    // 벽 세우기
                    a++;
                }
                lerpTime[1] += Time.deltaTime;
                // BlackCurtain3 게임오브젝트 Active True
                blackCurtain[2].SetActive(true);
                // 0.5초만에 지정된 위치로 이동
                blackCurtain[2].transform.position = Vector2.Lerp(canvas.transform.position,
                    new Vector2(canvas.transform.position.x - 2852, canvas.transform.position.y - 1604), lerpTime[1] / 0.5f);
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 58)     // 4분58초에 BlackCurtain
            {
                lerpTime[0] += Time.deltaTime;
                // BlackCurtain1,2 게임오브젝트 Active True
                blackCurtain[0].SetActive(true);
                blackCurtain[1].SetActive(true);
                // 1초만에 지정된 위치로 이동
                blackCurtain[0].transform.position = Vector2.Lerp(new Vector2(canvas.transform.position.x + 960, canvas.transform.position.y),
                    new Vector2(canvas.transform.position.x - 50, canvas.transform.position.y), lerpTime[0] / 1f);
                blackCurtain[1].transform.position = Vector2.Lerp(new Vector2(canvas.transform.position.x - 960, canvas.transform.position.y),
                    canvas.transform.position, lerpTime[0] / 1f);
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 57)     // 4분57초에 Warning
            {
                // WarningAnim 게임오브젝트 Active True
                GameObject.Find("Canvas").transform.Find("StageAnim").Find("WarningAnim").gameObject.SetActive(true);
            }
            else if (regenBoss == false && timeManager.minute >= 4 && timeManager.second >= 56)      // 4분56초에 몹 사망
            {
                FoundObjects = new List<GameObject>(GameObject.FindGameObjectsWithTag("Mob"));
                foreach (GameObject found in FoundObjects)
                    found.GetComponent<MobData>().hp = 0;
                regenStop = true;   // 몹리젠 멈추기
            }
        }
        
    }

    public void MobCreate(int mobIndex)    // mobIndex = 몹의 배열 번호
    {
        for(int i = 0;i < mobNum[mobIndex] / 4;i++)     // 플레이어의 위쪽, 왼쪽, 오른쪽, 아래쪽으로 4등분해서 위치는 랜덤하게 나오게 만들어줍니다.
        {
            // 화면에 보이는 카메라 바깥에서 생성되게 만들어줍니다.
            //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
            float randomX1 = Random.Range(player.transform.position.x - 7f, player.transform.position.x + 7f);
            GameObject mob1 = GameManager.instance.poolManager.Get(mobIndex);
            mob1.transform.position = new Vector2(randomX1, player.transform.position.y + 5.5f);
            float randomX2 = Random.Range(player.transform.position.x - 7f, player.transform.position.x + 7f);
            GameObject mob2 = GameManager.instance.poolManager.Get(mobIndex);
            mob2.transform.position = new Vector2(randomX2, player.transform.position.y - 5.5f);
            //적이 나타날 Y좌표를 랜덤으로 생성해 줍니다.
            float randomY1 = Random.Range(player.transform.position.y - 5.5f, player.transform.position.y + 5.5f);
            GameObject mob3 = GameManager.instance.poolManager.Get(mobIndex);
            mob3.transform.position = new Vector2(player.transform.position.x - 7f, randomY1);
            float randomY2 = Random.Range(player.transform.position.y - 5.5f, player.transform.position.y + 5.5f);
            GameObject mob4 = GameManager.instance.poolManager.Get(mobIndex);
            mob4.transform.position = new Vector2(player.transform.position.x + 7f, randomY2);
        }
    }

    public void BossSpawn()
    {
        if (scene.name == "Stage1")
            Instantiate(mobList[1], new Vector2(0, 3f), Quaternion.identity);
        else if (scene.name == "Stage2")
            Instantiate(mobList[9], new Vector2(0, 3f), Quaternion.identity);
    }
}

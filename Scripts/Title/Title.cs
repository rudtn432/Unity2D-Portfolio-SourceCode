using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Title : MonoBehaviour
{
    [SerializeField] private Button buttonStage2;
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI[] textMeshPro;
    [SerializeField] private bool[] textOn;
    [SerializeField] private bool isTextBounce;
    private PoolManager poolManager;
    private PlayMusicOperator playMusic;
    private Sfx sfx;

    float randomNum;        // 스폰 될 위치 랜덤
    float randomSpawnX;     // 스폰 X좌표 랜덤
    float randomSpawnY;     // 스폰 Y좌표 랜덤
    int randomScale;        // 보는 방향 랜덤
    public float time;
    bool spawnStart;        // 텍스트 회전이 끝나고 스폰되게 제어

    float time1;
    float _size = 3;
    float _upSizeTime = 0.2f;

    private void Start()
    {
        playMusic = FindObjectOfType<PlayMusicOperator>();
        sfx = FindObjectOfType<Sfx>();
        poolManager = FindObjectOfType<PoolManager>();

        DataManager.Instance.LoadGameData();
        spawnStart = false;
        textOn[0] = false;
        textOn[1] = false;
        Time.timeScale = 1f;
        time = 1f;

        StartCoroutine("txtRotate");
    }

    private void FixedUpdate()
    {
        if (textOn[0])
        {
            textMeshPro[0].transform.Rotate(new Vector3(0, 0, 2000f * Time.deltaTime));
            textMeshPro[0].fontSize += 10f * Time.deltaTime;        // 1초에 폰트사이즈 10씩 증가
        }
        if (textOn[1])
        {
            textMeshPro[1].transform.Rotate(new Vector3(0, 0, 2000f * Time.deltaTime));
            textMeshPro[1].fontSize += 10f * Time.deltaTime;
        }

        if(isTextBounce)
        {
            if (time1 <= _upSizeTime)                   // 크기가 커졌다가
            {
                textMeshPro[0].transform.localScale = new Vector2(0.02f, 0.02f) * (1 + _size * time1);
                textMeshPro[1].transform.localScale = new Vector2(0.02f, 0.02f) * (1 + _size * time1);
            }
            else if (time1 <= _upSizeTime * 2)          // 크기가 작아졌다가
            {
                textMeshPro[0].transform.localScale = new Vector2(0.02f, 0.02f) * (2 * _size * _upSizeTime + 1 - time1 * _size);
                textMeshPro[1].transform.localScale = new Vector2(0.02f, 0.02f) * (2 * _size * _upSizeTime + 1 - time1 * _size);
            }
            else
            {
                textMeshPro[0].transform.localScale = new Vector2(0.02f, 0.02f);
                textMeshPro[1].transform.localScale = new Vector2(0.02f, 0.02f);
            }
            time1 += Time.deltaTime;
        }
    }

    private void Update()
    {
        if (DataManager.Instance.data.isUnlock[1] == true)
            buttonStage2.interactable = true;
        else
            buttonStage2.interactable = false;

        if (spawnStart)
        {
            time += Time.deltaTime;

            if (time >= 1)
            {
                for (int i = 0; i < 6; i++)
                {
                    randomNum = Random.Range(0, 4);
                    randomScale = Random.Range(0, 2);
                    randomSpawnX = Random.Range(player.transform.position.x - 8f, player.transform.position.x + 8f);
                    randomSpawnY = Random.Range(player.transform.position.y - 8.5f, player.transform.position.y + 5.5f);

                    switch (randomNum)       // 스폰될 상하좌우 랜덤
                    {
                        case 0:
                            MobSpawn(0);
                            break;
                        case 1:
                            MobSpawn(1);
                            break;
                        case 2:
                            MobSpawn(2);
                            break;
                        case 3:
                            MobSpawn(3);
                            break;
                    }
                }
                time = 0;
            }
        }
    }

    void MobSpawn(int _num)     // 타이틀에서 날아다니는 몹 스폰
    {
        GameObject inst = GetComponent<GameObject>();
        switch (_num)
        {
            case 0:
                inst = poolManager.Get(Random.Range(0, 10));
                inst.transform.position = new Vector2(randomSpawnX, player.transform.position.y + 5.5f);
                break;
            case 1:
                inst = poolManager.Get(Random.Range(0, 10));
                inst.transform.position = new Vector2(randomSpawnX, player.transform.position.y - 8.5f);
                break;
            case 2:
                inst = poolManager.Get(Random.Range(0, 10));
                inst.transform.position = new Vector2(player.transform.position.x - 8f, randomSpawnY);
                break;
            case 3:
                inst = poolManager.Get(Random.Range(0, 10));
                inst.transform.position = new Vector2(player.transform.position.x + 8f, randomSpawnY);
                break;
        }
        Rigidbody2D rigidbody = inst.GetComponent<Rigidbody2D>();
        if (randomScale == 1)
            inst.transform.localScale = new Vector3(-1, 1, 1);
        StartCoroutine(prfAddForce(rigidbody, _num));
        StartCoroutine(GameObjActiveFalse(inst));
    }

    IEnumerator prfAddForce(Rigidbody2D rigid,int i)
    {
        yield return new WaitForSeconds(0.001f);

        switch (i)
        {
            case 0:         // 위에서 스폰 됐으면 아래로 이동
                rigid.AddForce(new Vector2(Random.Range(-8f, 8f), player.transform.position.y - 8.5f).normalized * Random.Range(5f, 20f), ForceMode2D.Impulse);
                break;
            case 1:         // 아래에서 스폰 됐으면 위로 이동
                rigid.AddForce(new Vector2(Random.Range(-8f, 8f), player.transform.position.y + 5.5f).normalized * Random.Range(5f, 20f), ForceMode2D.Impulse);
                break;
            case 2:         // 왼쪽에서 스폰 됐으면 오른쪽으로 이동
                rigid.AddForce(new Vector2(player.transform.position.x + 8f, Random.Range(-6, 6.5f)).normalized * Random.Range(5f, 20f), ForceMode2D.Impulse);
                break;
            case 3:         // 오른쪽에서 스폰 됐으면 왼쪽으로 이동
                rigid.AddForce(new Vector2(player.transform.position.x - 8f, Random.Range(-6, 6.5f)).normalized * Random.Range(5f, 20f), ForceMode2D.Impulse);
                break;
        }
    }

    IEnumerator txtRotate()         // 텍스트 회전
    {
        textOn[0] = true;
        textOn[1] = true;
        yield return new WaitForSeconds(1.5f);
        sfx.TitleTextStop();
        yield return new WaitForSeconds(0.5f);
        textOn[0] = false;
        textMeshPro[0].transform.localEulerAngles = new Vector3(0, 0, -11);
        yield return new WaitForSeconds(0.5f);
        sfx.TitleTextStop();
        yield return new WaitForSeconds(0.5f);
        textOn[1] = false;
        textMeshPro[1].transform.localEulerAngles = new Vector3(0, 0, 24);
        yield return new WaitForSeconds(1f);
        playMusic.BGM.enabled = true;
        playMusic.PlayBGM("3");             // 텍스트 회전이 멈추고 1초 뒤에 BGM 재생
        StartCoroutine("txtBounce");
        spawnStart = true;
    }


    IEnumerator txtBounce()         // 텍스트 커졌다가 작아졌다가
    {
        isTextBounce = true;
        yield return new WaitForSeconds(2f);
        isTextBounce = false;
        time1 = 0;
        yield return new WaitForSeconds(1f);
        StartCoroutine("txtBounce");
    }

    IEnumerator GameObjActiveFalse(GameObject obj)
    {
        yield return new WaitForSeconds(5f);
        obj.gameObject.SetActive(false);
    }
}

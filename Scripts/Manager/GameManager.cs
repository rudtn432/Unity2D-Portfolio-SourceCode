using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SkillManager skillManager;
    public MobSpawnManager mobSpawnManager;
    public PoolManager poolManager;
    private Player player;
    private ClearSelect clearSelect;

    bool stageClear1;
    bool stageClear2;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        clearSelect = FindObjectOfType<ClearSelect>();
        skillManager = FindObjectOfType<SkillManager>();
        mobSpawnManager = FindObjectOfType<MobSpawnManager>();
        player = FindObjectOfType<Player>();

        Time.timeScale = 1f;

        // 게임 시작할 때 스킬들 초기화
        skillManager.skillList[0].skillLv = 1;
        skillManager.skillList[0].isSkillLvFull = false;
        for(int i = 2; i < skillManager.skillList.Length; i++)
        {
            skillManager.skillList[i].skillLv = 0;
            skillManager.skillList[i].isSkillLvFull = false;
        }
        // 몹 초기화
        mobSpawnManager.mobList[0].hp = 20;
        mobSpawnManager.mobList[2].hp = 100;
        mobSpawnManager.mobList[3].hp = 10;

        mobSpawnManager.mobList[4].hp = 25;
        mobSpawnManager.mobList[5].hp = 100;
        mobSpawnManager.mobList[6].hp = 40;
        mobSpawnManager.mobList[7].hp = 120;

        player.transform.position = new Vector2(0, 0);          // 플레이어 위치 초기화
    }

    public void StageClear1()
    {
        stageClear1 = true;
        DataManager.Instance.data.isUnlock[1] = true;
        StartCoroutine(clearSelect.CallCoroutine());
    }
    public void StageClear2()
    {
        stageClear2 = true;
        StartCoroutine(clearSelect.CallCoroutine());
    }
}

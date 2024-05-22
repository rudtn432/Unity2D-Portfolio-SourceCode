using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Configuration : MonoBehaviour
{
    private SkillSelect skillSelect;
    private DieSelect dieSelect;
    private ClearSelect clearSelect;
    private Player player;

    [SerializeField] GameObject go_Config;
    public bool isActive;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        skillSelect = FindObjectOfType<SkillSelect>();
        dieSelect = FindObjectOfType<DieSelect>();
        clearSelect = FindObjectOfType<ClearSelect>();
        isActive = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isActive)
                Call();
            else
                CloseConfig();
        }
    }

    public void Call()
    {
        if (!player.isDie && !skillSelect.isActive && !clearSelect.isActive)          // 죽어있지 않고 다른 Select창들이 안켜져있을 때 켜지게
        {
            isActive = true;
            Time.timeScale = 0f;    // 게임 일시정지
            go_Config.SetActive(true);
        }
            
    }

    public void CloseConfig()
    {
        isActive = false;
        if (!skillSelect.isActive || !dieSelect.isActive || !clearSelect.isActive)      // 만약 다른 select창들이 안켜져있으면 다시 시간 흐르게
            Time.timeScale = 1f;
        go_Config.SetActive(false);
    }
}

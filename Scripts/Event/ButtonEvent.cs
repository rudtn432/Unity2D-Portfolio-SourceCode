using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonEvent : MonoBehaviour
{
    private Player player;
    private SkillManager skillManager;
    private SkillUI skillUI;
    private SkillSelect skillSelect;
    private SkillToolTip skillToolTip;
    private DieSelect dieSelect;
    private Configuration configuration;
    private PlayMusicOperator PlayMusicOperator;
    private Sfx sfx;

    void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
        skillUI = FindObjectOfType<SkillUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        skillSelect = FindObjectOfType<SkillSelect>();
        skillToolTip = FindObjectOfType<SkillToolTip>();
        dieSelect = FindObjectOfType<DieSelect>();
        configuration = FindObjectOfType<Configuration>();
        PlayMusicOperator = FindObjectOfType<PlayMusicOperator>();
        sfx = FindObjectOfType<Sfx>();
    }

    public void GoTitle()
    {
        DataManager.Instance.SaveGameData();
        SceneManager.LoadScene("Title");
    }

    public void GoStage2()
    {
        DataManager.Instance.SaveGameData();
        LoadingSceneManager.LoadScene("Stage2");
    }

    public void SKillLvUp1()
    {
        player.lvUpCount -= 1;
        SkillLvUp(skillSelect.skillListNum1, 0);

        if (player.lvUpCount >= 1)      // 레벨업을 한번에 해서 lvUpCount가 1이상이면 다시 Select창 Call
            skillSelect.Call();
    }
    public void SKillLvUp2()
    {
        player.lvUpCount -= 1;
        SkillLvUp(skillSelect.skillListNum2, 1);

        if (player.lvUpCount >= 1)
            skillSelect.Call();
    }
    public void SKillLvUp3()
    {
        player.lvUpCount -= 1;
        SkillLvUp(skillSelect.skillListNum3, 2);

        if (player.lvUpCount >= 1)
            skillSelect.Call();
    }
    void SkillLvUp(int _skillListNum,int _btNum)
    {
        if(skillManager.skillList[_skillListNum].skillType == 1)    // 공격 스킬이라면
        {
            if (player.sp >= 1 && skillManager.skillList[_skillListNum].skillLv < 6)        // 스킬 포인트가 있고 레벨이 6보다 낮으면
            {
                if (skillManager.skillList[_skillListNum].skillLv == 5 && player.sp >= 1
                    && skillManager.skillList[_skillListNum].skillType == 1)       //스킬 레벨이 5, 스킬 타입이 1이고 스킬 포인트가 3이상이면
                {
                    player.sp -= 1;
                    skillManager.skillList[_skillListNum].skillLv += 1;
                    skillManager.skillList[_skillListNum].isSkillLvFull = true;
                    skillManager.fullLvCount += 1;
                    skillSelect.CloseSkillSelect();
                    skillToolTip.HideToolTip();
                    Time.timeScale = 1f;
                }
                else if (skillManager.skillList[_skillListNum].skillLv == 0 && player.sp >= 1)   // 스킬 레벨이 0이면
                {
                    player.sp -= 1;
                    skillManager.skillList[_skillListNum].skillLv += 1;
                    if (skillUI.skillUI2.activeSelf == false)       // skillUI2에 스킬이 없다면
                    {
                        // skillUI2에 처음 고른 스킬의 이미지를 넣어주고
                        skillUI.skillUI2.GetComponent<Image>().sprite = skillManager.skillList[_skillListNum].defaultImg;
                        skillUI.skillNum1 = _skillListNum;          // skillNum1에 처음 고른 스킬의 번호 값을 넣는다.
                        skillUI.skillUI2.SetActive(true);
                    }
                    else                                            // skillUI2에 활성화 되어있다면 skillUI3에 정보를 넣어준다.
                    {
                        // skillUI3에 처음 고른 스킬의 이미지를 넣어주고
                        skillUI.skillUI3.GetComponent<Image>().sprite = skillManager.skillList[_skillListNum].defaultImg;
                        skillUI.skillNum2 = _skillListNum;          // skillNum2에 처음 고른 스킬의 번호 값을 넣는다.
                        skillUI.skillUI3.SetActive(true);
                    }
                    if (_skillListNum == 2)     // 대거 스킬이 1레벨이 되면 DaggerCreate 코루틴 시작
                        skillManager.StartCoroutine("DaggerCreate");
                    if (_skillListNum == 10)    // 독가스 스킬이 1레벨이 되면 PoisonGasCreate 코루틴 시작
                        skillManager.StartCoroutine("PoisonGasCreate");

                    skillSelect.CloseSkillSelect();
                    skillToolTip.HideToolTip();
                    Time.timeScale = 1f;
                }
                else if (skillManager.skillList[_skillListNum].skillLv < 5 && player.sp >= 1)   // 스킬 레벨이 5미만이면
                {
                    player.sp -= 1;
                    skillManager.skillList[_skillListNum].skillLv += 1;
                    skillSelect.CloseSkillSelect();
                    skillToolTip.HideToolTip();
                    Time.timeScale = 1f;
                }
            }
        }
        else if(skillManager.skillList[_skillListNum].skillType == 0)   // 패시브 스킬이라면
        {
            if (skillManager.skillList[_skillListNum].skillLv < 5 && player.sp >= 1)   // 스킬 레벨이 5미만이면
            {
                player.sp -= 1;
                if (_skillListNum != 7)
                    skillManager.skillList[_skillListNum].skillLv += 1;

                switch (_skillListNum)
                {
                    case 4:
                        player.power += 0.2f;
                        if (skillManager.skillList[_skillListNum].skillLv == 5)
                        {
                            skillManager.skillList[_skillListNum].isSkillLvFull = true;
                            skillManager.fullLvCount += 1;
                        }
                        break;
                    case 5:
                        player.attackSpeed += 0.2f;
                        if (skillManager.skillList[_skillListNum].skillLv == 5)
                        {
                            skillManager.skillList[_skillListNum].isSkillLvFull = true;
                            skillManager.fullLvCount += 1;
                        }
                        break;
                    case 6:
                        player.moveSpeed += 0.1f;
                        if (skillManager.skillList[_skillListNum].skillLv == 5)
                        {
                            skillManager.skillList[_skillListNum].isSkillLvFull = true;
                            skillManager.fullLvCount += 1;
                        }
                        break;
                    case 7:
                        player.nowHp += 50;
                        break;
                }

                skillSelect.CloseSkillSelect();
                skillToolTip.HideToolTip();
                Time.timeScale = 1f;
            }
        }
        
    }
    
    public void Bt_CloseSkillSelect()
    {
        player.lvUpCount -= 1;
        skillSelect.CloseSkillSelect();
        Time.timeScale = 1f;

        if (player.lvUpCount >= 1)
            skillSelect.Call();
    }

    public void Bt_CallConfig()
    {
        configuration.Call();
    }

    public void Bt_CloseConfig()
    {
        DataManager.Instance.SaveGameData();
        configuration.CloseConfig();
    }

    public void BgmMute()
    {
        //PlayMusicOperator.Mute();
    }
    public void SfxMute()
    {
        //sfx.Mute();
    }

    public void Bt_Revival()
    {
        dieSelect.CloseDieSelect();
        player.Revival();
        Time.timeScale = 1f;
    }

    public void GameOff()
    {
        DataManager.Instance.SaveGameData();
        Application.Quit();
    }
}

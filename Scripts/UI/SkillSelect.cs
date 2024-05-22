using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelect : MonoBehaviour
{
    SkillManager skillManager;
    private Player player;
    private SkillUI skillUI;

    [SerializeField] private TextMeshProUGUI text1;     // Skill1 Lv 텍스트
    [SerializeField] private TextMeshProUGUI text2;     // Skill2 Lv 텍스트
    [SerializeField] private TextMeshProUGUI text3;     // Skill3 Lv 텍스트
    [SerializeField] private TextMeshProUGUI text4;     // Skill1 필요 Sp 포인트 텍스트
    [SerializeField] private TextMeshProUGUI text5;     // Skill2 필요 Sp 포인트 텍스트
    [SerializeField] private TextMeshProUGUI text6;     // Skill3 필요 Sp 포인트 텍스트
    [SerializeField] private Button[] bt_Skill;         // Skill 버튼 배열
    [SerializeField] private GameObject go_SkillSelect; // SkillSelect의 GameObject

    public int[] skillSelectNum;                          // 처음으로 고른 스킬 번호
    public int skillListNum1;
    public int skillListNum2;
    public int skillListNum3;
    public bool isActive;

    void Start()
    {
        isActive = false;
        skillManager = FindObjectOfType<SkillManager>();
        skillUI = FindObjectOfType<SkillUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Call()
    {
        isActive = true;
        Time.timeScale = 0f;    // 게임 일시정지

        // 스킬 3개 랜덤 픽
        skillListNum1 = Random.Range(0, skillManager.skillList.Length);
        skillListNum2 = Random.Range(0, skillManager.skillList.Length);
        skillListNum3 = Random.Range(0, skillManager.skillList.Length);

        // 만약 skillUI에서 skillUI3가 활성화 되었다면 3가지 스킬을 모두 골랐으니 3가지 스킬 + 기본 스킬만 나오도록
        if (skillUI.skillUI3.activeSelf == true)   
        {
            if(skillManager.fullLvCount == 4)
            {
                // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (풀레벨인 스킬)이 나오면 다시
                while ((skillListNum1 == 1 || skillListNum1 == 7)
                    || (skillListNum1 != skillUI.skillNum1 && skillListNum1 != skillUI.skillNum2 && skillListNum1 != 0
                        && skillListNum1 != 4 && skillListNum1 != 5 && skillListNum1 != 6)
                    || skillManager.skillList[skillListNum1].isSkillLvFull)
                        skillListNum1 = Random.Range(0, skillManager.skillList.Length);
                // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (Num1과 같은 값), (풀레벨인 스킬)이 나오면 다시
                while ((skillListNum2 == 1 || skillListNum2 == 7)
                    || (skillListNum2 != skillUI.skillNum1 && skillListNum2 != skillUI.skillNum2 && skillListNum2 != 0
                        && skillListNum2 != 4 && skillListNum2 != 5 && skillListNum2 != 6)
                    || skillListNum2 == skillListNum1
                    || skillManager.skillList[skillListNum2].isSkillLvFull)
                        skillListNum2 = Random.Range(0, skillManager.skillList.Length);
                skillListNum3 = 7;
            }
            else if(skillManager.fullLvCount == 5)
            {
                // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (풀레벨인 스킬)이 나오면 다시
                while ((skillListNum1 == 1 || skillListNum1 == 7)
                    || (skillListNum1 != skillUI.skillNum1 && skillListNum1 != skillUI.skillNum2 && skillListNum1 != 0
                        && skillListNum1 != 4 && skillListNum1 != 5 && skillListNum1 != 6)
                    || skillManager.skillList[skillListNum1].isSkillLvFull)
                    skillListNum1 = Random.Range(0, skillManager.skillList.Length);
                skillListNum2 = 7;
                skillListNum3 = 7;
            }
            else if(skillManager.fullLvCount == 6)
            {
                skillListNum1 = 7;
                skillListNum2 = 7;
                skillListNum3 = 7;
            }
            else
            {
                do
                {
                    skillListNum1 = Random.Range(0, skillManager.skillList.Length);
                    skillListNum2 = Random.Range(0, skillManager.skillList.Length);
                    skillListNum3 = Random.Range(0, skillManager.skillList.Length);
                    // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (풀레벨인 스킬)이 나오면 다시
                    while (skillListNum1 == 1 || skillListNum1 == 7
                        || (skillListNum1 != skillUI.skillNum1 && skillListNum1 != skillUI.skillNum2 && skillListNum1 != 0
                            && skillListNum1 != 4 && skillListNum1 != 5 && skillListNum1 != 6)
                        || skillManager.skillList[skillListNum1].isSkillLvFull)
                        skillListNum1 = Random.Range(0, skillManager.skillList.Length);
                    // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (Num1과 같은 값), (풀레벨인 스킬)이 나오면 다시
                    while ((skillListNum2 == 1 || skillListNum2 == 7)
                        || (skillListNum2 != skillUI.skillNum1 && skillListNum2 != skillUI.skillNum2 && skillListNum2 != 0
                            && skillListNum2 != 4 && skillListNum2 != 5 && skillListNum2 != 6)
                        || skillListNum2 == skillListNum1
                        || skillManager.skillList[skillListNum2].isSkillLvFull)
                        skillListNum2 = Random.Range(0, skillManager.skillList.Length);
                    // 랜덤으로 (1,7), (고르지 않은 스킬,4,5,6이 아닌 스킬), (Num1 또는 Num2와 같은 값), (풀레벨인 스킬)이 나오면 다시
                    while ((skillListNum3 == 1 || skillListNum3 == 7)
                        || (skillListNum3 != skillUI.skillNum1 && skillListNum3 != skillUI.skillNum2 && skillListNum3 != 0
                            && skillListNum3 != 4 && skillListNum3 != 5 && skillListNum3 != 6)
                        || (skillListNum3 == skillListNum1 || skillListNum3 == skillListNum2)
                        || skillManager.skillList[skillListNum3].isSkillLvFull)
                        skillListNum3 = Random.Range(0, skillManager.skillList.Length);
                    
                } while ((skillManager.skillList[skillListNum1].skillLv == 5 && !skillManager.skillList[skillManager.skillList[skillListNum1].subSkill].isSkillLvFull && skillManager.skillList[skillListNum1].skillType == 1)
                      && (skillManager.skillList[skillListNum2].skillLv == 5 && !skillManager.skillList[skillManager.skillList[skillListNum2].subSkill].isSkillLvFull && skillManager.skillList[skillListNum2].skillType == 1)
                      && (skillManager.skillList[skillListNum3].skillLv == 5 && !skillManager.skillList[skillManager.skillList[skillListNum3].subSkill].isSkillLvFull && skillManager.skillList[skillListNum3].skillType == 1));
                        // 만약 skillListNum1,2,3이 액티브스킬 타입이고 모두 서브스킬이 풀레벨이 아닌 5레벨이라면 스킬을 선택할 수 없으니 다시
            }
        }
        else
        {
            // 랜덤으로 (1,7), (풀레벨인 스킬)이 나오면 다시
            while (skillListNum1 == 1 || skillListNum1 == 7
                || skillManager.skillList[skillListNum1].isSkillLvFull)
                skillListNum1 = Random.Range(0, skillManager.skillList.Length);
            // 랜덤으로 (1,7), (Num1과 같은 값), (풀레벨인 스킬)이 나오면 다시
            while (skillListNum2 == 1 || skillListNum2 == 7 || skillListNum2 == skillListNum1
                || skillManager.skillList[skillListNum2].isSkillLvFull)
                skillListNum2 = Random.Range(0, skillManager.skillList.Length);
            // 랜덤으로 (1,7), (Num1 또는 Num2와 같은 값), (풀레벨인 스킬)이 나오면 다시
            while (skillListNum3 == 1 || skillListNum3 == 7 || skillListNum3 == skillListNum1 || skillListNum3 == skillListNum2
                || skillManager.skillList[skillListNum3].isSkillLvFull)
                skillListNum3 = Random.Range(0, skillManager.skillList.Length);
        }

        if (skillListNum1 == 7)
            text1.text = "";
        else
            text1.text = "Lv. " + skillManager.skillList[skillListNum1].skillLv.ToString();
        bt_Skill[0].GetComponent<Image>().sprite = skillManager.skillList[skillListNum1].defaultImg;
        if (skillListNum2 == 7)
            text2.text = "";
        else
            text2.text = "Lv. " + skillManager.skillList[skillListNum2].skillLv.ToString();
        bt_Skill[1].GetComponent<Image>().sprite = skillManager.skillList[skillListNum2].defaultImg;
        if (skillListNum3 == 7)
            text3.text = "";
        else
            text3.text = "Lv. " + skillManager.skillList[skillListNum3].skillLv.ToString();
        bt_Skill[2].GetComponent<Image>().sprite = skillManager.skillList[skillListNum3].defaultImg;

        if (skillManager.skillList[skillListNum1].skillLv < 5)
        {
            text4.text = "필요한 SP 포인트\n1";
            BtOn_Skill(0);
        }
        else if (skillManager.skillList[skillListNum1].skillLv == 5)
        {
            // 만약 skillListNum1의 서브 스킬 레벨이 풀레벨이라면
            if(skillManager.skillList[skillManager.skillList[skillListNum1].subSkill].isSkillLvFull)
            {
                text4.text = "<color=#FFFFFF>조건 충족</color>";
                BtOn_Skill(0);
            }
            else
            {
                switch(skillListNum1)
                {
                    case 0:
                        text4.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 8:
                        text4.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 2:
                        text4.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 9:
                        text4.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 3:
                        text4.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                    case 10:
                        text4.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                }
                BtOff_Skill(0);
            }
        }

        if (skillManager.skillList[skillListNum2].skillLv < 5)
        {
            text5.text = "필요한 SP 포인트\n1";
            BtOn_Skill(1);
        }
        else if (skillManager.skillList[skillListNum2].skillLv == 5)
        {
            // 만약 skillListNum2의 서브 스킬 레벨이 풀레벨이라면
            if (skillManager.skillList[skillManager.skillList[skillListNum2].subSkill].isSkillLvFull)
            {
                text5.text = "<color=#FFFFFF>조건 충족</color>";
                BtOn_Skill(1);
            }
            else
            {
                switch (skillListNum2)
                {
                    case 0:
                        text5.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 8:
                        text5.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 2:
                        text5.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 9:
                        text5.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 3:
                        text5.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                    case 10:
                        text5.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                }
                BtOff_Skill(1);
            }
        }

        if (skillManager.skillList[skillListNum3].skillLv < 5)
        {
            text6.text = "필요한 SP 포인트\n1";
            BtOn_Skill(2);
        }
        else if (skillManager.skillList[skillListNum3].skillLv == 5)
        {
            // 만약 skillListNum2의 서브 스킬 레벨이 풀레벨이라면
            if (skillManager.skillList[skillManager.skillList[skillListNum3].subSkill].isSkillLvFull)
            {
                text6.text = "<color=#FFFFFF>조건 충족</color>";
                BtOn_Skill(2);
            }
            else
            {
                switch (skillListNum3)
                {
                    case 0:
                        text6.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 8:
                        text6.text = "<color=#FF0000>공격력 증가\n5레벨 필요</color>";
                        break;
                    case 2:
                        text6.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 9:
                        text6.text = "<color=#FF0000>공격속도 증가\n5레벨 필요</color>";
                        break;
                    case 3:
                        text6.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                    case 10:
                        text6.text = "<color=#FF0000>이동속도 증가\n5레벨 필요</color>";
                        break;
                }
                BtOff_Skill(2);
            }
        }

        go_SkillSelect.SetActive(true);
    }

    public void CloseSkillSelect()
    {
        isActive = false;
        go_SkillSelect.SetActive(false);
    }

    public void BtOn_Skill(int _num)
    {
        bt_Skill[_num].interactable = true;
    }
    public void BtOff_Skill(int _num)
    {
        bt_Skill[_num].interactable = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class SkillToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SkillManager skillManager;
    private SkillSelect skillSelect;

    public GameObject go_Base;
    [SerializeField] private GameObject canvas;

    [SerializeField] private TextMeshProUGUI Text_SkillName;        // 스킬 이름
    [SerializeField] private TextMeshProUGUI Text_SkillDesc1;       // 스킬 설명1
    [SerializeField] private TextMeshProUGUI Text_SkillDesc2;       // 스킬 설명2
    [SerializeField] private Image skill_Img;                       // 스킬의 이미지
    [SerializeField] private int iBtNum;                            // 인스펙터에서 버튼 번호 지정

    private void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
        skillSelect = FindObjectOfType<SkillSelect>();
    }

    private void Update()
    {
        if (go_Base.activeSelf == true)
            MouseFollow();
    }

    public void ShowToolTip()   // 툴팁 나타내기
    {
        switch(iBtNum)
        {
            case 1:
                skill_Img.sprite = skillManager.skillList[skillSelect.skillListNum1].defaultImg;
                Text_SkillName.text = skillManager.skillList[skillSelect.skillListNum1].skillName;
                Text_SkillDesc1.text = skillManager.skillList[skillSelect.skillListNum1].skillDescribe;
                SkillDesc2(skillSelect.skillListNum1, skillManager.skillList[skillSelect.skillListNum1].skillLv);
                break;
            case 2:
                skill_Img.sprite = skillManager.skillList[skillSelect.skillListNum2].defaultImg;
                Text_SkillName.text = skillManager.skillList[skillSelect.skillListNum2].skillName;
                Text_SkillDesc1.text = skillManager.skillList[skillSelect.skillListNum2].skillDescribe;
                SkillDesc2(skillSelect.skillListNum2, skillManager.skillList[skillSelect.skillListNum2].skillLv);
                break;
            case 3:
                skill_Img.sprite = skillManager.skillList[skillSelect.skillListNum3].defaultImg;
                Text_SkillName.text = skillManager.skillList[skillSelect.skillListNum3].skillName;
                Text_SkillDesc1.text = skillManager.skillList[skillSelect.skillListNum3].skillDescribe;
                SkillDesc2(skillSelect.skillListNum3, skillManager.skillList[skillSelect.skillListNum3].skillLv);
                break;
        }

        go_Base.SetActive(true);
    }

    public void SkillDesc2(int _skillListNum,int _skillLv)      // SkillDesc2의 선택을 위해 스킬리스트 번호와 레벨을 받는다
    {
        switch(_skillListNum)
        {
            case 0:                     // 총알
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 10, 1발";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 10, 1발\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 12, 2발";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 12, 2발\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 14, 3발";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 14, 3발\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 16, 4발";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 16, 4발\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 18, 5발";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 18, 5발\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 1발당 데미지 20, 계속 발사";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 1발당 데미지 20, 계속 발사";
                break;

            case 2:                     // 대거
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 10, 1개";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 10, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 11, 2개";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 11, 2개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 12, 3개";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 12, 3개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 13, 4개";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 13, 4개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 14, 5개";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 14, 5개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 15, 5개, 계속 유지";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 15, 5개, 계속 유지";
                break;

            case 3:                     // 여신의 가호
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 5, 크기 100%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 5, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 6, 크기 200%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 6, 크기 200%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 7, 크기 300%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 7, 크기 300%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 8, 크기 400%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 8, 크기 400%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 9, 크기 500%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 9, 크기 500%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 10, 크기 600%";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 10, 크기 600%";
                break;
            case 4:                     // 공격력 증가
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격력 120%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격력 120%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격력 140%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격력 140%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격력 160%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격력 160%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격력 180%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격력 180%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격력 200%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격력 200%";
                break;
            case 5:                     // 공격속도 증가
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격속도 120%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격속도 120%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격속도 140%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격속도 140%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격속도 160%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격속도 160%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격속도 180%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격속도 180%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 공격속도 200%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 공격속도 200%";
                break;
            case 6:                     // 이동속도 증가
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 이동속도 110%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 이동속도 110%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 이동속도 120%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 이동속도 120%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 이동속도 130%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 이동속도 130%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 이동속도 140%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 이동속도 140%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 이동속도 150%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 이동속도 150%";
                break;
            case 7:                     // 체력 물약
                Text_SkillDesc2.text = "";
                break;
            case 8:                     // 붐볼
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 5, 1발, 크기 100%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 5, 1발, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 6, 1발, 크기 116%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 6, 1발, 크기 116%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 7, 1발, 크기 133%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 7, 1발, 크기 133%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 8, 1발, 크기 150%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 8, 1발, 크기 150%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 9, 1발, 크기 166%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 9, 1발, 크기 166%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 10, 2발, 크기 166%";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 10, 2발, 크기 166%";
                break;
            case 9:                     // 쿠나이
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 5, 1개";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 5, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 7, 1개";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 7, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 9, 1개";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 9, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 11, 1개";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 11, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 13, 1개";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 13, 1개\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 15, 2개";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 15, 2개";
                break;
            case 10:                    // 독가스
                if (_skillLv == 0)
                    Text_SkillDesc2.text =
                        "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 5, 1개, 크기 100%";
                else if (_skillLv == 1)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 5, 1개, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 6, 1개, 크기 100%";
                else if (_skillLv == 2)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 6, 1개, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 7, 2개, 크기 100%";
                else if (_skillLv == 3)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 7, 2개, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 8, 2개, 크기 100%";
                else if (_skillLv == 4)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 8, 2개, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 9, 3개, 크기 100%";
                else if (_skillLv == 5)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 9, 3개, 크기 100%\n"
                        + "[다음레벨] " + (skillManager.skillList[_skillListNum].skillLv + 1) + ": 데미지 10, 3개, 크기 150%";
                else if (_skillLv == 6)
                    Text_SkillDesc2.text =
                        "[현재레벨] " + skillManager.skillList[_skillListNum].skillLv.ToString() + ": 데미지 10, 3개, 크기 150%";
                break;
        }
    }

    public void HideToolTip()   // 툴팁 숨기기
    {
        go_Base.SetActive(false);
    }

    public void MouseFollow()   // 툴팁 마우스 따라오기
    {
        //SkillToolTip의 Base의 중심을 맨왼쪽위로 설정해준다.
        Vector2 mousePos = Input.mousePosition;
        go_Base.transform.position = mousePos - (new Vector2(0f, 20f));

        // canvas의 xMax좌표를 넘어가지 못하게한다.
        if (go_Base.transform.position.x + 394f > canvas.transform.localPosition.x * 2)
            go_Base.transform.position = new Vector2(canvas.transform.localPosition.x * 2 - 394f, go_Base.transform.position.y);
        // canvas의 yMin좌표를 넘어가지 못하게한다.
        if (go_Base.transform.position.y - 165f < 0)
            go_Base.transform.position = new Vector2(go_Base.transform.position.x, 165f);
    }

    public void OnPointerEnter(PointerEventData eventData)  // 마우스 커서가 들어갈 때 발동
    {
        ShowToolTip();
    }

    public void OnPointerExit(PointerEventData eventData)   // 마우스 커서가 나올 때 발동
    {
        HideToolTip();
    }
}

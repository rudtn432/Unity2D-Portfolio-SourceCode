using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    SkillManager skillManager;

    public GameObject skillUI2;
    public GameObject skillUI3;

    public TextMeshProUGUI[] text;

    public int skillNum1;
    public int skillNum2;

    void Start()
    {
        skillManager = FindObjectOfType<SkillManager>();
    }

    void Update()
    {
        if (!skillManager.skillList[0].isSkillLvFull)       // 레벨이 풀렙이 아니라면
            text[0].text = "Lv. " + skillManager.skillList[0].skillLv.ToString();
        else                                                // 풀렙이면 MAX
            text[0].text = "MAX";                            
        if (!skillManager.skillList[skillNum1].isSkillLvFull)
            text[1].text = "Lv. " + skillManager.skillList[skillNum1].skillLv.ToString();
        else
            text[1].text = "MAX";
        if (!skillManager.skillList[skillNum2].isSkillLvFull)
            text[2].text = "Lv. " + skillManager.skillList[skillNum2].skillLv.ToString();
        else
            text[2].text = "MAX";
        if (!skillManager.skillList[4].isSkillLvFull)
            text[3].text = "Lv. " + skillManager.skillList[4].skillLv.ToString();
        else
            text[3].text = "MAX";
        if (!skillManager.skillList[5].isSkillLvFull)
            text[4].text = "Lv. " + skillManager.skillList[5].skillLv.ToString();
        else
            text[4].text = "MAX";
        if (!skillManager.skillList[6].isSkillLvFull)
            text[5].text = "Lv. " + skillManager.skillList[6].skillLv.ToString();
        else
            text[5].text = "MAX";

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DieSelect : MonoBehaviour
{
    private Player player;

    [SerializeField] private TextMeshProUGUI tRegenNum;         // 남은 부활 횟수 텍스트
    [SerializeField] private GameObject go_DieSelect;           // DieSelect의 GameObject
    [SerializeField] private Button bt_Regen;                   // 이어하기 버튼

    public bool isActive;

    void Start()
    {
        isActive = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void Call()
    {
        isActive = true;
        Time.timeScale = 0f;    // 게임 일시정지

        tRegenNum.text = "남은 부활 횟수\n" + player.iRegenNum.ToString();
        
        if(player.iRegenNum <= 0)
            bt_Regen.interactable = false;

        go_DieSelect.SetActive(true);
    }

    public void CloseDieSelect()
    {
        isActive = false;
        go_DieSelect.SetActive(false);
    }
}

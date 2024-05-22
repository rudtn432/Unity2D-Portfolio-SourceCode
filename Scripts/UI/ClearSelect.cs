using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClearSelect : MonoBehaviour
{
    private Player player;
    private TimeManager timeManager;

    [SerializeField] private GameObject go_ClearSelect;             // ClearSelect의 GameObject
    [SerializeField] private Button bt_NextStage;                   // 다음 스테이지로 버튼
    [SerializeField] private TextMeshProUGUI txt_ClearTime;         // 클리어타임 텍스트

    public bool isActive;

    void Start()
    {
        isActive = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        timeManager = FindObjectOfType<TimeManager>();
    }

    public IEnumerator CallCoroutine()
    {
        yield return new WaitForSecondsRealtime(5f);
        Call();
    }
    public void Call()
    {
        isActive = true;
        int min;
        float sec;
        min = timeManager.minute;
        sec = timeManager.second;
        txt_ClearTime.text = min.ToString("00") + ":" + sec.ToString("00.00");
        go_ClearSelect.SetActive(true);
    }

    public void CloseClearSelect()
    {
        isActive = false;
        go_ClearSelect.SetActive(false);
    }
}

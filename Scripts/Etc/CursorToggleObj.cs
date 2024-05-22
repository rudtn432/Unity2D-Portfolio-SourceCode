using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CursorToggleObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CursorManager.CursorType cursorType;

    private Sfx sfx;
    private Toggle toggle;
    private bool isToggleOn;        // 토글이 켜진지 꺼진지
    private bool isClicked;

    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        toggle = GetComponent<Toggle>();
        isClicked = false;
    }

    void ToggleOnOffCheck()     // 버튼이 켜져있는지 꺼져있는지 체크
    {
        if (toggle.interactable == false)
            isToggleOn = false;
        else
            isToggleOn = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ToggleOnOffCheck();
        if (isToggleOn)
        {
            isClicked = true;
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Clicked);
            sfx.SfxBtMouseClick();
        }
        else
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClicked = false;
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Click);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ToggleOnOffCheck();
        if (isToggleOn)
        {
            CursorManager.Instance.SetActiveCursorType(cursorType);
            sfx.SfxBtMouseOver();
        }
        else
            CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 클릭하고 Cursor 이미지를 Clicked로 되돌리는 Obj에 붙이는 스크립트
public class CursorBtClickObj : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CursorManager.CursorType cursorType;

    private Sfx sfx;
    private Button button;      
    private bool isBtOn;        // 버튼이 켜진지 꺼진지
    private bool isClicked;

    void Start()
    {
        sfx = FindObjectOfType<Sfx>();
        button = GetComponent<Button>();
        isClicked = false;
    }

    void ButtonOnOffCheck()     // 버튼이 켜져있는지 꺼져있는지 체크
    {
        if (button.interactable == false)   
            isBtOn = false;
        else
            isBtOn = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ButtonOnOffCheck();
        if (isBtOn)
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
        CursorManager.Instance.SetActiveCursorType(CursorManager.CursorType.Default);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ButtonOnOffCheck();
        if (isBtOn)
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

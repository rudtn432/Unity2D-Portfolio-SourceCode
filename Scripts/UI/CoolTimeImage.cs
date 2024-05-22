using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoolTimeImage : MonoBehaviour
{
    [SerializeField] private float coolTime;
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI tCoolTime;
    float fillTime;

    private void Start()
    {
        tCoolTime.gameObject.SetActive(false);
    }

    public void StartCoolTime(float _coolTime)
    {
        StartCoroutine(CoolTime(_coolTime));
    }

    IEnumerator CoolTime(float _coolTime)
    {
        fillTime = 0f;
        _coolTime = coolTime;
        tCoolTime.gameObject.SetActive(true);

        while (fillTime < _coolTime)
        {
            tCoolTime.text = (_coolTime - fillTime).ToString("F1");
            fillTime += Time.deltaTime;
            img.fillAmount = (1 - (fillTime / _coolTime));
            yield return new WaitForFixedUpdate();
        }
        tCoolTime.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    // 캔버스를 하나 만들어주고 캔버스의 렌더모드를 UI의 레이어를 가지면서 씬 안에 있는 오브젝트처럼 만들어주는 World Space로 바꿔주고
    // DamageCanvas에 생성되도록 해준다. 

    public Text text;
    private float time;
    private float fadeTime;
    Vector3 dir;
    Color alpha;

    void Awake()
    {
        text = GetComponent<Text>();

        time = 0;
        fadeTime = 1f;
        alpha = new Color(1, 1, 1, 1);

        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1).normalized;
    }

    void Update()
    {
        // 랜덤 방향으로 움직이기
        transform.Translate(dir * Time.deltaTime);

        if (time < fadeTime)    // 시간이 지날수록 점점 사라지기
        {
            alpha.a = Mathf.Lerp(alpha.a, 0, 1 * Time.deltaTime);
            text.color = alpha;
        }
        else
        {
            time = 0;
            gameObject.SetActive(false);
        }
        time += Time.deltaTime;
    }

    private void OnEnable()
    {
        time = 0;
        alpha = new Color(1, 1, 1, 1);
        dir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), -1).normalized;
    }
}

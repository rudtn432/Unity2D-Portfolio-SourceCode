using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneManager : MonoBehaviour
{
    // 참고 사이트: https://wergia.tistory.com/183

    public static string nextScene;
    [SerializeField] Sprite[] backGroundSprite;
    [SerializeField] Image backGroundImage;
    [SerializeField] Image progressBar;
    [SerializeField] TextMeshProUGUI loadingText;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private void Update()
    {
        if (nextScene == "Stage1")          // 다음 씬이 무엇인지에 따라 백그라운드 이미지 다르게 설정
            backGroundImage.sprite = backGroundSprite[0];
        else
            backGroundImage.sprite = backGroundSprite[1];
        loadingText.text = "로딩중..." + (progressBar.fillAmount * 100f).ToString("F1") + "%";     // 로딩 퍼센트 알려주는 텍스트
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);     // 비동기 방식으로 씬 호출
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds(0.5f);      // 로딩씬이 너무 빨리 넘어가서 넘어가는 속도 늦추기
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}

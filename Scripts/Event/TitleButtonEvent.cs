using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtonEvent : MonoBehaviour
{
    public void Bt_Stage1()
    {
        LoadingSceneManager.LoadScene("Stage1");
    }
    public void Bt_Stage2()
    {
        LoadingSceneManager.LoadScene("Stage2");
    }
    public void Bt_Apply()
    {
        DataManager.Instance.SaveGameData();
    }
    public void GameExit()
    {
        DataManager.Instance.SaveGameData();
        Application.Quit();
    }
}

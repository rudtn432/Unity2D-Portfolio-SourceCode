using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicOperator : MonoBehaviour
{
    [System.Serializable]
    public struct BgmType
    {
        public string name;
        public AudioClip audio;
    }

    // Inspector 에표시할 배경음악 목록
    [SerializeField] public string sceneName;
    public BgmType[] BGMList;

    public AudioSource BGM;
    private string NowBGMname = "";

    void Start()
    {
        BGM = GetComponent<AudioSource>();
        BGM.loop = true;
        if (BGMList.Length > 0)
            PlayBGM(BGMList[0].name);

        switch(sceneName)
        {
            case "Title":
                BGM.enabled = false;
                break;
            case "Stage1":
                PlayBGM("1");
                break;
            case "Stage2":
                PlayBGM("4");
                break;
        }
    }

    public void PlayBGM(string name)
    {
        if (NowBGMname.Equals(name)) return;

        for (int i = 0; i < BGMList.Length; ++i)
            if (BGMList[i].name.Equals(name))
            {
                BGM.clip = BGMList[i].audio;
                BGM.Play();
                NowBGMname = name;
            }
    }

    public void Mute()
    {
        if (!DataManager.Instance.data.BGMMute)
        {
            BGM.mute = true;
            DataManager.Instance.data.BGMMute = true;
        }
        else
        {
            BGM.mute = false;
            DataManager.Instance.data.BGMMute = false;
        }
    }
}

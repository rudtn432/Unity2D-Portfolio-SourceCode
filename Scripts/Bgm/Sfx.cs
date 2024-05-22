using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sfx : MonoBehaviour
{
    // 효과음은 여러가지 한꺼번에 나니까 PlayOneShot으로 재생
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] BtSfxList;
    [SerializeField] private AudioClip[] ItemSfxList;
    [SerializeField] private AudioClip[] MobSfxList;
    [SerializeField] private AudioClip[] SkillSfxList;
    [SerializeField] private AudioClip[] OtherSfxList;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Mute()
    {
        if (!DataManager.Instance.data.SfxMute)
        {
            DataManager.Instance.data.SfxMute = true;
            audioSource.mute = true;
        }
        else
        {
            DataManager.Instance.data.SfxMute = false;
            audioSource.mute = false;
        }
    }

    // 버튼 효과음
    public void SfxBtMouseOver()
    {
        audioSource.PlayOneShot(BtSfxList[0]);
    }
    public void SfxBtMouseClick()
    {
        audioSource.PlayOneShot(BtSfxList[1]);
    }

    // 아이템 효과음
    public void SfxItemPotion()
    {
        audioSource.PlayOneShot(ItemSfxList[0]);
    }

    // 몹 효과음
    public void SfxMob(int _num)
    {
        if (_num == 16 || _num == 19 || _num == 24)                 // 상대가 보스라면 맞는 음량을 줄인채로 출력
            audioSource.PlayOneShot(MobSfxList[_num], 0.3f);
        else
            audioSource.PlayOneShot(MobSfxList[_num]);
        
    }

    // 스킬 효과음

    // 기타 효과음
    public void SfxClear()
    {
        audioSource.PlayOneShot(OtherSfxList[0]);
    }
    public void SfxGameOver()
    {
        audioSource.PlayOneShot(OtherSfxList[1]);
    }
    public void TitleTextStop()
    {
        audioSource.PlayOneShot(OtherSfxList[2]);
    }
    public void SfxBossRegen()
    {
        audioSource.PlayOneShot(OtherSfxList[3]);
    }
    public void SfxWarning1()
    {
        audioSource.PlayOneShot(OtherSfxList[4]);
    }
    public void SfxWarning2()
    {
        audioSource.PlayOneShot(OtherSfxList[5]);
    }
    public void SfxWarning3()
    {
        audioSource.PlayOneShot(OtherSfxList[6]);
    }
}

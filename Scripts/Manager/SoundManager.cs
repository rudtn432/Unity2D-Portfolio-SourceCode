using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    AudioSource btnSource;

    PlayMusicOperator playMusicOperator;
    Sfx sfx;

    [SerializeField] Slider sd_BgmValue;
    [SerializeField] Slider sd_SfxValue;
    [SerializeField] public Toggle tg_BgmMute;
    [SerializeField] public Toggle tg_SfxMute;

    private void Start()
    {
        audioSource = GameObject.Find("BGM_Audio_Source").GetComponent<AudioSource>();
        btnSource = GameObject.Find("Sfx_Audio_Source").GetComponent<AudioSource>();
        playMusicOperator = FindObjectOfType<PlayMusicOperator>();
        sfx = FindObjectOfType<Sfx>();
        DataManager.Instance.LoadGameData();
        SoundCheck();
    }

    private void Update()
    {
        if (tg_BgmMute.isOn)
        {
            audioSource.mute = true;
            DataManager.Instance.data.BGMMute = true;
        }
        else
        {
            audioSource.mute = false;
            DataManager.Instance.data.BGMMute = false;
        }
        if (tg_SfxMute.isOn)
        {
            btnSource.mute = true;
            DataManager.Instance.data.SfxMute = true;
        }
        else
        {
            btnSource.mute = false;
            DataManager.Instance.data.SfxMute = false;
        }
    }

    void SoundCheck()
    {
        if (!DataManager.Instance.data.BGMMute)
        {
            audioSource.mute = false;
            tg_BgmMute.isOn = false;
        }
        else
        {
            audioSource.mute = true;
            tg_BgmMute.isOn = true;
        }
        if (!DataManager.Instance.data.SfxMute)
        {
            btnSource.mute = false;
            tg_SfxMute.isOn = false;
        }
        else
        {
            btnSource.mute = true;
            tg_SfxMute.isOn = true;
        }
        audioSource.volume = DataManager.Instance.data.BGMVolume;
        sd_BgmValue.value = DataManager.Instance.data.BGMVolume;
        btnSource.volume = DataManager.Instance.data.SfxVolume;
        sd_SfxValue.value = DataManager.Instance.data.SfxVolume;
    }

    public void SetAudioVolume(float volume)
    {
        audioSource.volume = volume;
        DataManager.Instance.data.BGMVolume = sd_BgmValue.value;
    }

    public void SetSfxVolume(float volume)
    {
        btnSource.volume = volume;
        DataManager.Instance.data.SfxVolume = sd_SfxValue.value;
    }

    public void OnSfx()
    {
        btnSource.Play();
    }
}

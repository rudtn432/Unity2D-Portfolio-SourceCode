using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable] // 직렬화

public class Data
{
    // 각 챕터의 잠금여부를 저장할 배열
    public bool[] isUnlock = new bool[2];
    // 음량
    public float BGMVolume;
    public float SfxVolume;
    public bool BGMMute;
    public bool SfxMute;
}


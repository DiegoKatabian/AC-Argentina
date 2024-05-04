using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    void Start()
    {
        DontDestroyOnLoad(this);
    }

}

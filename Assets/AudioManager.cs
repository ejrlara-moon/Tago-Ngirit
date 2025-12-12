using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MelenitasDev.SoundsGood;
public class AudioManager : MonoBehaviour
{
    private Music bgmIG = new Music(Track.inGameBGM).SetVolume(0.3f);
    // Start is called before the first frame update
    void Start()
    {
        bgmIG.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

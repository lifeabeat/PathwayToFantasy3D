using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerUpdateVer1 : BaseManager<AudioManagerUpdateVer1>
{
    // key and default value for saving volumne
    private const string BGM_VOLUME_KEY = "BGM_VOLUME_KEY";
    private const string SE_VOLUME_KEY = "SE_VOLUME_KEY";
    private const float BGM_VOLUME_DEFULT = 0.2f;
    private const float SE_VOLUME_DEFULT = 1.0F;

    // Time it takes for the background music to fade
    public const float BGM_FADE_SPEED_RATE_HIGH = 0.9f;
    public const float BGM_FADE_SPEED_RATE_LOW = 0.9f;
    private float bgmFadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH;

    // Next BGM name, SE name
    private string nextBGMName;
    private string nextSEName;

    //Is the backgrond music fading out ?
    private bool isFadeOut = false;

    //Separate audio sources for BGM and SE
    public AudioSource AttachBGMSource;
    public AudioSource AttachSESource;

    // Array dung chi de lay giu lieu
    // List dung de them xoa sua, tim kiem
    // Chi tim kiem thoi thi dung dictionary

    //Keep all audio
    private Dictionary<string, AudioClip> bgmDic, seDic;
    protected override void Awake()
    {
        base.Awake();
        //Load all SE && BGM files from resource folder

        bgmDic = new Dictionary<string, AudioClip>();
        seDic = new Dictionary<string, AudioClip>();
        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] seList = Resources.LoadAll("Audio/SE");

        // sau khi Load xong thi co 4 obj vi co 4 file trong audio, moi obj co 2 value key va value
        foreach (AudioClip bgm in bgmList)
        {
            bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            seDic[se.name] = se;
        }


    }

    //  

    private void Start()
    {
        AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
        AttachSESource.volume = PlayerPrefs.GetFloat(SE_VOLUME_KEY, SE_VOLUME_DEFULT);
    }

    public void PlaySE(string seName, float delay = 0.0f)
    {
        if (!seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "There is no Se named");
            return;
        }

        nextSEName = seName;
        Invoke("DelayPlaySE", delay);
    }

    private void DelayPlaySE()
    {
        AttachSESource.PlayOneShot(seDic[nextSEName] as AudioClip);
    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FADE_SPEED_RATE_HIGH)
    {
        // Kiem tra keyname da co tu truoc chua
        if (!bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "There is no BGM named");
            return;
        }

        // if BGM is not currently playing, play it as is

        if (!AttachBGMSource.isPlaying)
        {
            nextBGMName = "";
            AttachBGMSource.clip = bgmDic[bgmName] as AudioClip;
            AttachBGMSource.Play();
        }

        // when a different BGM is playing, fade out the BGM that is playing before playing the BGM
        // Through when the same BGM is playing
        else if (AttachBGMSource.clip.name != bgmName)
        {
            nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public void FadeOutBGM(float fadeSpeedRate = BGM_FADE_SPEED_RATE_LOW)
    {
        bgmFadeSpeedRate = fadeSpeedRate;
        isFadeOut = true;
    }

    private void Update()
    {
        if (!isFadeOut)
        {
            return;
        }

        // Gradually lower the volume, and when the volume reaches 0
        // return the volume and play the next song

        AttachBGMSource.volume -= Time.deltaTime * bgmFadeSpeedRate;
        if (AttachBGMSource.volume <= 0)
        {
            AttachBGMSource.Stop();
            AttachBGMSource.volume = PlayerPrefs.GetFloat(BGM_VOLUME_KEY, BGM_VOLUME_DEFULT);
            isFadeOut = false;

            if (!string.IsNullOrEmpty(nextBGMName))
            {
                PlayBGM(nextBGMName);
            }
        }

    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        AttachBGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(BGM_VOLUME_KEY, BGMVolume);
    }

    public void ChangeSEVolume(float SEVolume)
    {
        AttachSESource.volume = SEVolume;
        PlayerPrefs.SetFloat(SE_VOLUME_KEY, SEVolume);
    }

    public void PlayRandomSEPitch()
    {
        AttachSESource.pitch = Random.Range(.9f, 1.1f);
        
    }

    /// ung dung vao buoc chan
    /// cu cach 0.5 vao trog ham update moi cho chay 1 lan
}

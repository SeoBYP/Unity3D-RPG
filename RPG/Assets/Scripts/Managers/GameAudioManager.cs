using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : Manager<GameAudioManager>
{
    private AudioSource backgound;
    private List<AudioSource> effectAudioList = new List<AudioSource>();
    private Dictionary<string, AudioClip> clipDic = new Dictionary<string, AudioClip>();

    private int limitCount = 5;
    private float intervalTime = 1;
    private float prevTime = 0;

    private float backgroundVolum = 0.5f;
    private float effectVolum = 0.5f;
    private float buttonVolum = 0.5f;
    public override void Init()
    {
        backgound = gameObject.AddComponent<AudioSource>();
        backgound.spatialBlend = 0;
        backgound.volume = 1.0f;
        backgound.playOnAwake = false;
    }

    public void PlayBacground(string name)
    {
        if (clipDic.ContainsKey(name))
        {
            backgound.clip = clipDic[name];
            backgound.volume = backgroundVolum;
            backgound.Play();
            backgound.loop = true;
        }
    }

    AudioSource Pooling()
    {
        AudioSource audioSource = null;
        for(int i = 0; i < effectAudioList.Count; i++)
        {
            if(effectAudioList[i].gameObject.activeSelf == false)
            {
                audioSource = effectAudioList[i];
                audioSource.gameObject.SetActive(true);
                break;
            }
        }
        if(audioSource == null)
        {
            audioSource = Util.CreateObject<AudioSource>(transform);
            effectAudioList.Add(audioSource);
        }
        return audioSource;
    }

    IEnumerator IDeactiveAudio(AudioSource audio)
    {
        yield return new WaitForSeconds(audio.clip.length);
        audio.gameObject.SetActive(false);
    }

    public void Play(string name, float spatialBlend)//,float volum, Vector3 position)
    {
        if (clipDic.ContainsKey(name) == false)
            return;
        AudioSource audioSource = Pooling();
        audioSource.clip = clipDic[name];
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = effectVolum;
        audioSource.Play();
        StartCoroutine(IDeactiveAudio(audioSource));
    }

    public void PlayButton(string name, float spatialBlend)//,float volum, Vector3 position)
    {
        if (clipDic.ContainsKey(name) == false)
            return;
        AudioSource audioSource = Pooling();
        audioSource.clip = clipDic[name];
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = buttonVolum;
        audioSource.Play();
        StartCoroutine(IDeactiveAudio(audioSource));
    }

    public void Play2DSound(string name)//, float volum = 0.5f)
    {
        Play(name, 0);//, volum, Vector3.zero);
    }

    public void PlayButtonSound(string name)
    {
        PlayButton(name, 1);
    }

    public void PlayLoopInTime(string name,float spatialBlend,bool state, float looptime)
    {
        if (clipDic.ContainsKey(name) == false)
            return;
        AudioSource audioSource = Pooling();
        audioSource.clip = clipDic[name];
        audioSource.spatialBlend = spatialBlend;
        audioSource.volume = effectVolum;
        audioSource.Play();
        audioSource.loop = state;
        StartCoroutine(LoopInTime(audioSource, looptime));
    }

    IEnumerator LoopInTime(AudioSource audio,float time)
    {
        yield return new WaitForSeconds(time);
        audio.gameObject.SetActive(false);
    }

    public void LoadSounds()
    {
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio");
        for(int i = 0; i < audioClips.Length; i++)
        {
            if(clipDic.ContainsKey(audioClips[i].name) == false)
            {
                clipDic.Add(audioClips[i].name, audioClips[i]);
            }
        }
    }

    public void SetGameBGMVolum(float bgmVolum)
    {
        backgroundVolum = bgmVolum;
        if (backgound != null)
            backgound.volume = backgroundVolum;
    }

    public void SetGameEffectVolum(float effVolum)
    {
        effectVolum = effVolum;
    }

    public void SetGameButtonVolum(float btnVolum)
    {
        buttonVolum = btnVolum;
    }

    private void Update()
    {
        if(effectAudioList.Count > limitCount)
        {
            float elapsedTime = Time.time - prevTime;
            if(elapsedTime > intervalTime)
            {
                for(int i = 0; i < effectAudioList.Count; i++)
                {
                    if(effectAudioList[i].gameObject.activeSelf == false)
                    {
                        AudioSource audioSource = effectAudioList[i];
                        effectAudioList.RemoveAt(i);
                        prevTime = Time.time;
                        Destroy(audioSource.gameObject);
                        return;
                    }
                }
            }
        }
    }
}

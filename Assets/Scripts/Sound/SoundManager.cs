using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour 
{

	#region Singleton
	static private SoundManager _instance;
	static public SoundManager GetInstance()
	{
		if(_instance == null)
		{
			_instance = new GameObject("SoundManager", typeof(SoundManager)).GetComponent<SoundManager>();
			_instance.LoadSounds ();
			GameObject.DontDestroyOnLoad(_instance);;
		}	
		return _instance; 
	}
	#endregion

	//Audio Sources
	public AudioSource			bgmAudioSource;
	public List<AudioSource>	sfxAudioSources = new List<AudioSource>();
	//Control
	public int					sfxSources = 5;
	public int					sfxCounter = 0;

	//Audio Clips
	public AudioClip			bgmClip;
    public List<AudioClip>      sfxClips;

    //Audio Volumes
    private bool bgmMute = false;
    private bool sfxMute = false;

	private void LoadSounds()
	{
		bgmAudioSource = new GameObject("BGMAudioSource", typeof(AudioSource)).GetComponent<AudioSource>();
		bgmAudioSource.transform.parent = _instance.transform;
		for (int i = 0; i < sfxSources; i ++) 
		{
			sfxAudioSources.Add(new GameObject ("SFXAudioSource" + i.ToString(), typeof(AudioSource)).GetComponent<AudioSource> ());
			sfxAudioSources[sfxAudioSources.Count -1].gameObject.transform.parent = _instance.transform;
		}
		bgmClip = Resources.Load<AudioClip> ("Sounds/Music/TheFunkasticGrooves-TheGrooverman");
        sfxClips = new List<AudioClip>();
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/ButtonPress"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/Error"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitRotated"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitSold"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitBought_PoliceCamera"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitBought_PoliceMan"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitBought_PoliceStation"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/UnitBought_PoliceCar"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/MatchWon"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/MatchLost"));
        sfxClips.Add(Resources.Load<AudioClip>("Sounds/SFX/MatchCountdownTick"));

    }
    public void PlayBGM()
	{
		if (bgmAudioSource.isPlaying)
			return;
		bgmAudioSource.clip = bgmClip;
        bgmAudioSource.volume = SoundVolumes.bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play ();
	}
    public void PlaySFX(SFXType p_sfxType ,float p_volume = 1f, float p_pitch = 1f)
    {
        sfxAudioSources[sfxCounter].volume = p_volume;
        sfxAudioSources[sfxCounter].pitch = p_pitch;
        sfxAudioSources[sfxCounter].clip = sfxClips[(int)p_sfxType];
        sfxAudioSources[sfxCounter].Play();
        IncreaseSFXCounter();
    }
    private void IncreaseSFXCounter()
	{
		sfxCounter ++;
		if (sfxCounter == sfxAudioSources.Count)
			sfxCounter = 0;
	}

    public void InvertBGMVolume()
    {
        bgmAudioSource.mute = !bgmAudioSource.mute;
    }
    public void InvertSFXVolume()
    {
        foreach (AudioSource __as in sfxAudioSources)
            __as.mute =! __as.mute;
    }
}

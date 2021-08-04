using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField]
    private AudioSource MusicSource;
    [SerializeField]
    private AudioSource EffectsSource;

	[SerializeField] private AudioClip buttonClick;
	[SerializeField] private AudioClip game;
	[SerializeField] private AudioClip coinProduceFinished;
	[SerializeField] private AudioClip hired;
	[SerializeField] private AudioClip levelUp;

	private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        EventManager.Instance.SoundStatuChanged += OnSoundStatuChanged;
        EventManager.Instance.MusicStatuChanged += OnMusicStatuChanged;
        EventManager.Instance.SoundActivated += OnSoundActivated;
        Instance = this;
    }

    public void OnSoundActivated(SoundType type)
    {
        switch (type)
        {
            case SoundType.Game:
                MusicSource.clip = game;
                MusicSource.Play();
                break;
            case SoundType.ButtonClick:
                EffectsSource.PlayOneShot(buttonClick);
                break;
            case SoundType.CoinProduceFinished:
                EffectsSource.PlayOneShot(buttonClick);
                break;
            case SoundType.Hired:
                EffectsSource.PlayOneShot(hired);
                break;
            case SoundType.LevelUp:
                EffectsSource.PlayOneShot(levelUp);
                break;
        }
    }

    private void OnDestroy()
    {
        EventManager.Instance.SoundStatuChanged -= OnSoundStatuChanged;
        EventManager.Instance.MusicStatuChanged -= OnMusicStatuChanged;
    }
    public void OnMusicStatuChanged(bool b)
    {
        MusicSource.volume = b ? 0.5f : 0;
    }

    public void OnSoundStatuChanged(bool b)
    {
        EffectsSource.volume = b ? 1 : 0;
    }
}
public enum SoundType
{
	ButtonClick,
	Game,
	CoinProduceFinished,
	Hired,
	LevelUp
}

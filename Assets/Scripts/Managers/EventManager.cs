using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public delegate void OnPanelChange(PanelType panel);
    public event OnPanelChange OnPanelChanged;

    public delegate void OnMusicStatuChange(bool b);
    public event OnMusicStatuChange MusicStatuChanged;

    public delegate void OnSoundStatuChange(bool b);
    public event OnSoundStatuChange SoundStatuChanged;

    public delegate void OnSoundActive(SoundType type);
    public event OnSoundActive SoundActivated;

    public void SoundActivate(SoundType type) => SoundActivated?.Invoke(type);
    public void MusicStatuChange(bool b) => MusicStatuChanged?.Invoke(b);
    public void SoundStatuChange(bool b) => SoundStatuChanged?.Invoke(b);
    public void PanelChange(PanelType panel) => OnPanelChanged?.Invoke(panel);

    
}



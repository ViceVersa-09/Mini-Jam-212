using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sFXSource;
    [SerializeField] AudioMixer audioMixer;

    [Header("Clips")]
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip button;
    [SerializeField] AudioClip log;
    [SerializeField] AudioClip trap;
    [SerializeField] AudioClip spider;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        musicSource.clip = music;
        musicSource.Play();

        SetMixers();
    }

    private void Update()
    {
        SetMixers();
    }   

    public void PlaySFX(AudioClip sFXClip)
    {
        sFXSource.PlayOneShot(sFXClip);
    }

    void SetMixers()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(PlayerPrefs.GetFloat("Master", 1)) * 20);
        audioMixer.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music", 1)) * 20);
        audioMixer.SetFloat("SFX", Mathf.Log10(PlayerPrefs.GetFloat("SFX", 1)) * 20);
    }
}

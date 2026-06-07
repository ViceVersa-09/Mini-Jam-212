using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    AudioManager audioManager;

    private void Awake()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        sFXSlider.value = PlayerPrefs.GetFloat("SFX", 1);
    }

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void SetMaster()
    {
        PlayerPrefs.SetFloat("Master", masterSlider.value);
    }

    public void SetMusic()
    {
        PlayerPrefs.SetFloat("Music", musicSlider.value);
    }

    public void SetSFX()
    {
        PlayerPrefs.SetFloat("SFX", sFXSlider.value);
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();

        masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        sFXSlider.value = PlayerPrefs.GetFloat("SFX", 1);

        audioManager.PlaySFX(audioManager.button);
    }
}

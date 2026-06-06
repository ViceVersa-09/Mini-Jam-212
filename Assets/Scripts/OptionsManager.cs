using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    private void Awake()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master", 1);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 1);
        sFXSlider.value = PlayerPrefs.GetFloat("SFX", 1);
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
    }
}

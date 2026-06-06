using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sFXSlider;

    private void Awake()
    {
        masterSlider.value = PlayerPrefs.GetFloat("Master");
        musicSlider.value = PlayerPrefs.GetFloat("Music");
        sFXSlider.value = PlayerPrefs.GetFloat("SFX");
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
}

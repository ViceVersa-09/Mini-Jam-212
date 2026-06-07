using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject options;

    AudioManager audioManager;

    private void Start()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        audioManager.PlaySFX(audioManager.button);
    }

    public void OpenOptionsButton()
    {
        options.SetActive(true);
        gameObject.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
    }

    public void CloseOptionsButton()
    {
        gameObject.SetActive(true);
        options.SetActive(false);
        audioManager.PlaySFX(audioManager.button);
    }
}

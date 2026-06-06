using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject options;

    public void StartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenOptionsButton()
    {
        options.SetActive(true);
        gameObject.SetActive(false);
    }

    public void CloseOptionsButton()
    {
        gameObject.SetActive(true);
        options.SetActive(false);
    }
}

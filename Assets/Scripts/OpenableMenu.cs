using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class OpenableMenu : MonoBehaviour
{
    [SerializeField] int[] noOpenLevels;
    [SerializeField] GameObject childParent;

    bool openable;

    InputAction menuAction;
    AudioManager audioManager;

    public static OpenableMenu instance;

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
        menuAction = InputSystem.actions.FindAction("Menu");
        audioManager = FindAnyObjectByType<AudioManager>();

        CheckLevel();
    }

    private void OnLevelWasLoaded()
    {
        CheckLevel();
    }

    private void Update()
    {
        if (menuAction.triggered && openable && !childParent.activeInHierarchy)
        {
            OpenMenu();
        }
        else if (menuAction.triggered && openable && childParent.activeInHierarchy)
        {
            CloseMenu();
        }
    }

    void CheckLevel()
    {
        childParent.SetActive(false);

        if (noOpenLevels.Contains(SceneManager.GetActiveScene().buildIndex))
        {
            openable = false;
        }
        else
        {
            openable = true;
        }
    }

    void OpenMenu()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.button);
        }

        childParent.SetActive(true);
    }

    public void CloseMenu()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.button);
        }

        childParent.SetActive(false);
    }

    public void MainMenu()
    {
        if (audioManager != null)
        {
            audioManager.PlaySFX(audioManager.button);
        }

        SceneManager.LoadScene(0);
    }
}

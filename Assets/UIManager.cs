using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public GameObject deathScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true); // Activate the death screen
        }
        else
        {
            Debug.LogError("Death screen GameObject is not assigned in the UIManager.");
        }
    }

    public void HideDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(false);
        }
    }

    public void OnRespawnButtonClicked()
    {
        PlayerMovement player = Object.FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            player.Respawn();
        }
    }
}
using UnityEngine;

public class GlobalGameManager : MonoBehaviour
{
    private static GlobalGameManager Instance { get; set; }

    [Range(0f, 1f)] 
    public float timeScaleFactor;

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

    private void Update()
    {
        bool isMovementKeyPressed = 
            Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || 
            Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;

        Time.timeScale = isMovementKeyPressed ? 1.0f : timeScaleFactor;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Refs UI")]
    public GameObject canvasPrincipal;   
    public GameObject gameOverUI;         
    public GameObject checkpointUI;       
    public Transform player;

    private Vector3 lastCheckpointPos;


    private bool bluePortalOn;
    private bool orangePortalOn;
    [SerializeField] private Image crossair;

    [Header("Images for crossair")]
    [SerializeField] private Sprite crossairOB;
    [SerializeField] private Sprite crossairO;
    [SerializeField] private Sprite crossairB;
    private void OnEnable()
    {
        PortalEvents.OnBluePortalActivated += OnBluePortal;
        PortalEvents.OnOrangePortalActivated += OnOrangePortal;
    }

    private void OnBluePortal()
    {
        bluePortalOn = true;
        UpdateCrossair();
    }

    private void OnOrangePortal()
    {
        orangePortalOn = true;
        UpdateCrossair();
    }
    private void UpdateCrossair()
    {
        if (bluePortalOn)
        {
            if (orangePortalOn)
            {
                crossair.sprite = crossairOB;
                return;
            }
            crossair.sprite = crossairB;
            return;
        }
        if (orangePortalOn)
        {
            crossair.sprite = crossairO;
            return;
        }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (canvasPrincipal != null)
            canvasPrincipal.SetActive(true);

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (checkpointUI != null)
            checkpointUI.SetActive(false);
    }

    public void SetCheckpoint(Vector3 pos)
    {
        lastCheckpointPos = pos;

        if (checkpointUI != null)
        {
            checkpointUI.SetActive(true);
            CancelInvoke(nameof(OcultarCheckpointMensaje));
            Invoke(nameof(OcultarCheckpointMensaje), 2f);
        }
    }

    private void OcultarCheckpointMensaje()
    {
        if (checkpointUI != null)
            checkpointUI.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0f;

        if (gameOverUI != null)
            gameOverUI.SetActive(true);

        if (checkpointUI != null)
            checkpointUI.SetActive(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Retry()
    {
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (lastCheckpointPos != Vector3.zero)
            player.position = lastCheckpointPos;
        else
            SceneManager.LoadScene("GameplayScene");
        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<FPS_Controller>().enabled = true;
    }

    public void ExitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}




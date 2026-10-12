using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ItemCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private GameObject victoryPanel;

    private int collectedItems;
    private int totalItems;

    private void Start()
    {
        Time.timeScale = 1f;

        if (victoryPanel != null)
            victoryPanel.SetActive(false);

        totalItems = GameObject.FindGameObjectsWithTag("Collectible").Length;
        UpdateUI();
    }

    public void AddItem()
    {
        collectedItems++;
        UpdateUI();

        if (collectedItems >= totalItems)
            Win();
    }

    private void UpdateUI()
    {
        counterText.text = "Items: " + collectedItems + " / " + totalItems;
    }

    private void Win()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);

        Time.timeScale = 0f; // pausa el juego
    }

    // Para el botón "Reiniciar"
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
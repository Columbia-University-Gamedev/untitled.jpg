using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI attemptsText;
    private float timeLeft = 20.0f;
    private int attempts = 0;
    public static Manager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            attempts = PlayerPrefs.GetInt("Attempts", 0);
            countdownText = GameObject.FindWithTag("Time").GetComponent<TextMeshProUGUI>();
            attemptsText = GameObject.FindWithTag("Attempt").GetComponent<TextMeshProUGUI>();
            StartCoroutine(StartCountdown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        attemptsText.text = "Attempt: "+attempts.ToString();
    }

    private IEnumerator StartCountdown()
    {
        while (timeLeft > 0)
        {
            countdownText.text = "Timer: "+timeLeft.ToString("F1");
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
        }

        countdownText.text = "0.0";
        Reset();
    }

    private void Reset()
    {
        attempts++; // Increment attempts
        PlayerPrefs.SetInt("Attempts", attempts); // Save attempts count
        PlayerPrefs.Save(); // Ensure the save is written immediately
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ChangeScene(int index) {
        PlayerPrefs.DeleteAll();//delete prayer 
        SceneManager.LoadScene(index);//load game scene
    }
}
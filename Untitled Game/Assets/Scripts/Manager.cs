using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI tutorial1Text;
    public TextMeshProUGUI tutorial2Text;
    private float timeLeft = 100.0f;
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
            tutorial1Text = GameObject.FindWithTag("Tutorial1").GetComponent<TextMeshProUGUI>();
            tutorial2Text = GameObject.FindWithTag("Tutorial2").GetComponent<TextMeshProUGUI>();
            StartCoroutine(StartCountdown());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            attemptsText.text = "Attempt: " + attempts.ToString();
            if (Player.tutorial1)
            {
                tutorial1Text.color = new Color(0,0,0,0);
                tutorial2Text.color = new Color(1,1,1,1);
            }
            if (Player.tutorial2)
            {
                tutorial2Text.color = new Color(0, 0, 0, 0);
            }
        }
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
    public void Reset()
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
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class StartSceneController : MonoBehaviour
{
    public string serverUrl = "http://127.0.0.1:5000/image";
    public TextMeshProUGUI countdownText;
    public Image startImage;

    public float pollInterval = 0.2f;
    public float countdownDuration = 5f;
    public float startBlinkDuration = 4f;
    public float blinkInterval = 0.3f;
    public string mainSceneName = "Main";

    private float countdownTimer = 0f;
    private bool isCountingDown = false;
    private bool HumanDetected = false;
    private Coroutine blinkAndLoadCoroutine = null;
    private Coroutine countdownCoroutine = null;

    void Start()
    {
        countdownText.text = "";
        countdownText.enabled = false;
        startImage.enabled = true;
        StartCoroutine(FetchImageLoop());
    }

    IEnumerator FetchImageLoop()
    {
        while (!HumanDetected)
        {
            using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(serverUrl))
            {
                yield return req.SendWebRequest();
                         
                if (req.result != UnityWebRequest.Result.ConnectionError &&
                    req.result != UnityWebRequest.Result.ProtocolError)

                {
                    Texture2D tex = DownloadHandlerTexture.GetContent(req);
                    bool detected = HasPerson(tex);

                    if (detected)
                    {
                        if (!isCountingDown && countdownCoroutine == null)
                        {
                            isCountingDown = true;
                            countdownCoroutine = StartCoroutine(CountdownAndStart());
                        }
                    }
                    else
                    {
                        if (isCountingDown)
                        {
                            isCountingDown = false;

                            if (countdownCoroutine != null)
                            {
                                StopCoroutine(countdownCoroutine);
                                countdownCoroutine = null;
                            }

                            countdownTimer = countdownDuration;
                            countdownText.text = "";
                            countdownText.enabled = false;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("通信エラー: " + req.error);
                }
            }

            yield return new WaitForSeconds(pollInterval);
        }
    }

    IEnumerator CountdownAndStart()
    {
        countdownTimer = countdownDuration;
        countdownText.enabled = true;

        while (countdownTimer > 0f)
        {
            int secondsLeft = Mathf.CeilToInt(countdownTimer);
            countdownText.text = secondsLeft.ToString();

            if (secondsLeft == 3)
            {
                SoundManager.Instance.PlayCountdownBeep();
            }

            yield return new WaitForSeconds(1f);
            countdownTimer -= 1f;
        }

        countdownText.text = "";
        countdownText.enabled = false;
        HumanDetected = true;

        if (blinkAndLoadCoroutine == null)
            blinkAndLoadCoroutine = StartCoroutine(BlinkAndLoadScene(startBlinkDuration));

        countdownCoroutine = null;
    }

    IEnumerator BlinkAndLoadScene(float duration)
    {
        Debug.Log($"点滅開始 → {duration} 秒後にシーン遷移します");
        float elapsed = 0f;
        startImage.enabled = true;

        SoundManager.Instance.PlayStartSound();

        while (elapsed < duration)
        {
            startImage.enabled = !startImage.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        startImage.enabled = false;
        Debug.Log("解像度を1024×768に変更します");
        Screen.SetResolution(1024, 768, false);
        SceneManager.LoadScene(mainSceneName);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && blinkAndLoadCoroutine == null)
        {
            blinkAndLoadCoroutine = StartCoroutine(BlinkAndLoadScene(startBlinkDuration));
        }
    }

    bool HasPerson(Texture2D tex)
    {
        Color32[] pixels = tex.GetPixels32();
        int count = 0;
        foreach (var p in pixels)
        {
            if (p.a > 200) count++;
            if (count > 5000) return true;
        }
        return false;
    }
}
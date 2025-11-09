using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;
//using System.Drawing;

public class PythonImageReceiver : MonoBehaviour
{
    public Vector2 lastScreenPosition; // 最後に表示されたスクリーン座標
    public RawImage rawImage;
    private string url = "http://127.0.0.1:5000/image";
    private Texture2D lastTexture;
    private Coroutine imageCoroutine;
    public float timerDuration = 5f; // タイマー時間（秒）
    public float timerMargin = 1f; //生成までの調節用
    public TextMeshProUGUI countdownText;
    private bool isTimerRunning = false;
    private Canvas canvas;
    public Camera mainCamera;//カメラ取得用変数

    int NumAnimals2=0;
    bool AnimalPlusFlag=false;
    bool seentrueFlag=false; //animalがplusされた後に一度以上trueを見たらonになる
    private CreateManager creMane;

    public AnimalGenerator generator; // インスペクターで設定

    void Start()
    {
        // 画像表示位置を変更
        rawImage.rectTransform.anchoredPosition = new Vector2(0, 100); //rawimageの座標
        imageCoroutine = StartCoroutine(GetImage());
        generator = Object.FindFirstObjectByType<AnimalGenerator>();
        
        if (rawImage != null)
        {
            canvas = rawImage.GetComponentInParent<Canvas>();
        }

        Debug.Log("timerを起動");
        StartCoroutine(TimerStop());
        isTimerRunning = true;

        GameObject target = GameObject.Find("GameManager");
        if (target != null)
        {
            creMane = target.GetComponent<CreateManager>();
        }
        else
        {
            Debug.LogError("GameManager が見つかりませんでした");
        }

    }
    
    void Update()
    {
        TrackAnimalIncrease();
        HandleAnimalSettled();
    }   

    IEnumerator GetImage()
    {
        while (true)
        {
      
            if (rawImage == null || !rawImage.gameObject.activeSelf)
            {
                Debug.Log("RawImage が非表示なので画像取得を終了します");
                StopCoroutine(imageCoroutine);
                imageCoroutine = null;
                //yield break;
            }
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.ConnectionError &&
                www.result != UnityWebRequest.Result.ProtocolError)

            {
                Texture2D tex = DownloadHandlerTexture.GetContent(www);
                rawImage.texture = tex;
                lastTexture = tex;
                if (tex != null)
                {
                    rawImage.texture = tex;
                }
                else
                {
                    Debug.LogWarning("画像取得に失敗しました（tex が null）");
                }

            }
            else{
                Debug.Log("ねっとわーくえらーーー！！！！！");
            }
            yield return new WaitForSeconds(0.05f); //ここで取得速度変更　()の値が低ければ速い
        }
    }


    IEnumerator TimerStop()
    {
        float remainingTime = timerDuration;
        while (remainingTime > 0f)
        {
            if (countdownText != null)
            {
                countdownText.text = $"{(int)remainingTime}";
                countdownText.color = Color.white;//通常時は白色
            }
            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }
        //タイマーが０になった時の表示と色変更
        if(countdownText != null)
        {
            countdownText.text = "0";
            countdownText.color = Color.red;
        }

        //yield return new WaitForSeconds(timerDuration);
        yield return new WaitForSeconds(timerMargin);

        try
        {

            if (lastTexture != null)
            {
                string fileName = "image_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
                string path = Application.dataPath + "/Resources/" + fileName;
                byte[] bytes = lastTexture.EncodeToPNG();
                System.IO.File.WriteAllBytes(path, bytes);
                Debug.Log("画像を保存しました: " + path);

                #if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
                #endif
            }

            if (rawImage != null)
            {
                rawImage.texture = null;
                //rawImage.gameObject.SetActive(false);
                rawImage.enabled = false; //RawImage(script) を非表示
                Debug.Log("RawImage を非表示にしました");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("TimerStop coroutine error: " + e.Message);
        }
        finally
        {
            isTimerRunning = false; // 例外が起きても必ず false に戻す
        }
    }


    void TrackAnimalIncrease()
    {
        if (creMane.NumAnimals > NumAnimals2)
        {
            AnimalPlusFlag = true;
            NumAnimals2 = creMane.NumAnimals;
        }
    }   

    void HandleAnimalSettled()
    {
        if (!AnimalPlusFlag) return;    

        if (!seentrueFlag)
        {
            if (CreateManager.CheckMove(Animal.isMoves))
            {
                seentrueFlag = true;
            }
            return;
        }   

        if (!CreateManager.CheckMove(Animal.isMoves))
        {
            if (!rawImage.enabled)
            {
                if (generator != null)
                {
                    // generator.MoveCameraUp(1.0f);
                }
                rawImage.rectTransform.anchoredPosition = new Vector2(0, 100);
                rawImage.enabled = true;
            }   

            if (!isTimerRunning)
            {
                StartCoroutine(TimerStop());
                isTimerRunning = true;
                AnimalPlusFlag = false;
                seentrueFlag = false;
            }
        }
    }
}
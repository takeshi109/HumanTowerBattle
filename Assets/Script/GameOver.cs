using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class GameOver : MonoBehaviour
{
    public static bool isGameOver = false;
    public float timerDuration = 5f; // タイマー時間（秒）
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        // 指定秒数待つ
        yield return new WaitForSeconds(timerDuration);

        // シーンを読み込む
        SceneManager.LoadScene("start");
    }
    // Update is called once per frame
    void Update()
    {

    }
}

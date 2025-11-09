using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class Clear : MonoBehaviour
{
    public static bool isClear = false;
    public float timerDuration = 5f; // タイマー時間（秒）
    // Start is called before the first frame update
    public AudioClip sound1;
    AudioSource audioSource;
    void Start()
    {/*
        string[] files = Directory.GetFiles(
              @"Assets/Resources", "*.png", SearchOption.AllDirectories
              );
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }*/
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(sound1);

        StartCoroutine(ClearWaitAndLoad());
    }

    IEnumerator ClearWaitAndLoad()
    {
        // 指定秒数待つ
        yield return new WaitForSeconds(timerDuration);

        // シーンを読み込む
        SceneManager.LoadScene("start");
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown (0)) {
        SceneManager.LoadScene ("Main");
      }
    }
}

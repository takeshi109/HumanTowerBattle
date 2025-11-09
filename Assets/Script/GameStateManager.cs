using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public CreateManager createManager; // CreateManager への参照
    public int clearThreshold = 3;
    private bool hasCleared = false;

    void Start()
    {
        if (createManager == null)
        {
            createManager = Object.FindFirstObjectByType<CreateManager>();
            if (createManager == null)
            {
                Debug.LogError("CreateManager がシーン内に見つかりませんでした。");
            }
        }
    }

    void Update()
    {
        if (createManager == null || createManager.people == null)
        {
            return;
        }

        if (CheckGameOver(createManager.people))
        {
            GameData.FinalTowerCount = createManager.NumAnimals;

            if (createManager.NumAnimals >= clearThreshold)
            {
                SceneManager.LoadScene("Clear");
                SoundManager.Instance.PlaySEAndChangeSceneClear();
            }
            else
            {
                SceneManager.LoadScene("GameOver");
                SoundManager.Instance.PlaySEAndChangeSceneGameOver();
            }
        }
    }

    bool CheckGameOver(List<GameObject> people)
    {
        foreach (GameObject obj in people)
        {
            if (obj != null && obj.transform.position.y < -5)
            {
                return true;
            }
        }
        return false;
    }
}
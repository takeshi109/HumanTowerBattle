using UnityEngine;
using TMPro;

public class GameOverDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        int remaining = 5 - GameData.FinalTowerCount;
        resultText.text = $"残念！あと{remaining}個でクリアだよ！";
    }
}
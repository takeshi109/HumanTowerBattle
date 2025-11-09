using UnityEngine;
using TMPro;

public class ClearDisplay : MonoBehaviour
{
    public TextMeshProUGUI resultText;

    void Start()
    {
        resultText.text = $"やったね！{GameData.FinalTowerCount - 1}個積み上げたよ！";
    }
}
using UnityEngine;

/// <summary>
/// カメラにアタッチされ、2Dトリガーでの衝突を検知するコントローラー。
/// 衝突中かどうかを他のスクリプトから確認できるようにする。
/// </summary>
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// 衝突中かどうかを示すフラグ（true = 衝突中）
    /// </summary>
    public static bool isCollision;

    /// <summary>
    /// 他のCollider2Dと接触したときに呼ばれる（衝突開始）
    /// </summary>
    /// <param name="collision">接触したCollider2D</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("衝突開始");
        isCollision = true;
    }

    /// <summary>
    /// 他のCollider2Dとの接触が終了したときに呼ばれる（衝突終了）
    /// </summary>
    /// <param name="collision">離れたCollider2D</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("衝突終了");
        isCollision = false;
    }
}
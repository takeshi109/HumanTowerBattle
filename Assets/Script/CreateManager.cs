using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine.SceneManagement;
using TMPro;

public class CreateManager : MonoBehaviour
{
    private HashSet<string> createdFiles = new HashSet<string>(); // 生成済みファイル名の記録
    private GameObject obj;
    public List<GameObject> people;//どうぶつ取得配列
    public bool isFall;
    int file_length;
    public float pivotHeight = 3;//生成位置の基準
    public Camera mainCamera;//カメラ取得用変数
    public GameObject cameracontroller;
    public int NumAnimals=0;
    bool AnimalPlusFlag=false; 
    public TextMeshProUGUI TowercountText;
    // Start is called before the first frame update
    void Init()
    {
        Animal.isMoves.Clear();//移動してる動物のリストを初期化
        string[] files = Directory.GetFiles(
            @"Assets/Resources", "*.png", SearchOption.AllDirectories
            ).ToArray();
        file_length = files.Length;
 //       obj = null;
    }

    void Start()
    {
        //Resourcesに画像が入れられたときspriteに変換
        string[] files = Directory.GetFiles(
              @"Assets/Resources", "*.png", SearchOption.AllDirectories
              );
        foreach (string file in files)
        {
            File.SetAttributes(file, FileAttributes.Normal);
            File.Delete(file);
        }
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if(AnimalPlusFlag){
            AnimalPlusFlag = false;

            //効果音追加
            SoundManager.Instance.PlayImageReceivedSound();

            NumAnimals+=1;

            if (TowercountText != null){
                TowercountText.text = $"積み上げた数\n         {(int)NumAnimals}個";
                TowercountText.color = Color.white;
            }
            else{
                Debug.Log("TowercountTextがない");
            }
        


        }
        
        if (CheckMove(Animal.isMoves))
        {
            return;//移動中なら処理はここまで
        }
        string[] files = Directory.GetFiles(
            @"Assets/Resources", "*.png", SearchOption.AllDirectories
            ).OrderBy(f => File.GetLastWriteTime(f) ).ToArray();
        if (files.Length == 0){
            return;
        }

        string latestFile = files[files.Length - 1];
        string tar = Path.GetFileNameWithoutExtension(latestFile); // ファイル名だけ取得

        
        if (!createdFiles.Contains(tar))
        {
            Sprite img = Resources.Load<Sprite>(tar);
            if (img != null)
            {
                createdFiles.Add(tar);// 生成済みとして記録
                Create(img);
                Debug.Log("ゲームオブジェクト誕生");
                //Debug.Log(CheckMove(Animal.isMoves));
                AnimalPlusFlag=true;
            }
            else
            {
                Debug.LogWarning($"画像 {tar} の読み込みに失敗しました。");
            }
        }

    }
        

    void Create(Sprite img)
    {
        /*while (CameraController.isCollision)
        {
            Debug.Log("collision_start");
            cameracontroller.transform.Translate(0, 2.0f, 0);
            mainCamera.transform.Translate(0, 2.0f, 0);//カメラを少し上に移動
            pivotHeight += 2.0f;//生成位置も少し上に移動
        Debug.Log("collision_fin");
        }*/
        if (CameraController.isCollision)
        {
            Debug.Log("collision_start");
            cameracontroller.transform.Translate(0, 3.0f, 0);
            mainCamera.transform.Translate(0, 3.0f, 0);
            pivotHeight += 3.0f;
            Debug.Log("collision_fin");
        }
        isFall = false;
        obj = new GameObject();
        obj.AddComponent<SpriteRenderer>();
        obj.GetComponent<SpriteRenderer>().sprite = img;
        obj.AddComponent<PolygonCollider2D>();
        obj.AddComponent<Rigidbody2D>();
        //obj.GetComponent<Rigidbody2D>().isKinematic = true;
        obj.AddComponent<Animal>();
        obj.transform.position = new Vector3(0.0f, pivotHeight, 0.0f); 
        people.Add(obj);
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(obj.transform.position);

        // RawImage の位置をスクリーン座標に設定
        FindObjectOfType<PythonImageReceiver>().rawImage.rectTransform.position = screenPos;

        // RawImage を表示（必要なら）
        FindObjectOfType<PythonImageReceiver>().rawImage.gameObject.SetActive(true);
        obj.transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
    }

    /// <summary>
    /// 移動中かチェック
    /// </summary>
    /// <param name="isMoves"></param>
    /// <returns></returns>
    public static bool CheckMove(List<Moving> isMoves)
    {
        if (isMoves == null)
        {
            Debug.Log("ぬるぽ");
            return false;
        }
        foreach (Moving b in isMoves)
        {
            if (b.isMove)
            {
               // Debug.Log("移動中");
                return true;
            }
        }
        return false;
    }
}


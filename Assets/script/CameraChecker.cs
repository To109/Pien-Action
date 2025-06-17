using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChecker : MonoBehaviour
{
    // メインカメラへの参照
    private Camera mainCamera;

    void Start()
    {
        // シーン内のメインカメラを取得
        mainCamera = Camera.main;
    }

    void Update()
    {
        // オブジェクトのワールド座標をビューポート座標に変換
        // ビューポート座標は、画面左下が(0, 0)、右上が(1, 1)
        Vector3 viewportPos = mainCamera.WorldToViewportPoint(transform.position);

        // オブジェクトが画面の左端より外（x < 0）に出たら削除
        if (viewportPos.x < 0 || viewportPos.y < 0)
        {
            Destroy(gameObject); //　ゲームオブジェクトを削除
        }
    }
}
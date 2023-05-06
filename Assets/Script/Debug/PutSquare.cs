using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutSquare : MonoBehaviour
{

    LineRenderer linerend;


    // Start is called before the first frame update
    void Start()
    {
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = gameObject.AddComponent<LineRenderer>();

        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // 開始点
            new Vector3(8, 0, 0),               
            new Vector3(8, 8, 0),               
            new Vector3(0, 8, 0),               
            new Vector3(0, 0, 0),              // 終了点
        }; 

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);

    }

}

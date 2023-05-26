using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CreateFigure : MonoBehaviour
{
    [SerializeField] GameObject ResidenceObj_Preafab;
    // Start is called before the first frame update
    int Area = 400;
    void Start()
    {   
        int x = Random.Range(1, (int)Math.Sqrt(Area));
        int y = (int)(Area / x);

        Debug.Log(x + " , " + y);

        GameObject ResidenceObj = Instantiate(ResidenceObj_Preafab);
        ResidenceObj.GetComponent<DrowLine>().SetXY(x, y);
        ResidenceObj.GetComponent<DrowLine>().DrowLines();



        //// LineRendererコンポーネントをゲームオブジェクトにアタッチする
        //var lineRenderer = ResidenceObj.AddComponent<LineRenderer>();

        //var positions = new Vector3[]{
        //    new Vector3(0, 0, 0),               // 開始点
        //    new Vector3(x, 0, 0),
        //    new Vector3(x, -y, 0),
        //    new Vector3(0, -y, 0),
        //};

        //// 点の数を指定する
        //lineRenderer.positionCount = positions.Length;
        //lineRenderer.loop = true;

        //// 線を引く場所を指定する
        //lineRenderer.SetPositions(positions);
    }
}

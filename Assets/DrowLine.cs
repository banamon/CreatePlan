using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowLine : MonoBehaviour
{
    [SerializeField] GameObject SiteObject;
    [SerializeField] GameObject ResidenceObject;
    LineRenderer linerend;


    // Start is called before the first frame update
    void Start()
    {
        DrowSite();
        DroweResidence();
    }


    //敷地の描写
    void DrowSite() {
        var lineRenderer = SiteObject.AddComponent<LineRenderer>();


        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // 開始点
            new Vector3(0, -116, 0),            
            new Vector3(117, -116, 0),            
            new Vector3(117, -150, 0),            
            new Vector3(140, -150, 0),            
            new Vector3(140, -100, 0),            
            new Vector3(220, -100, 0),            
            new Vector3(220, 0, 0),            
            new Vector3(0,  0, 0),              
        };

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;

        //マテリアルの設定
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //色を指定する
        lineRenderer.SetColors(Color.white, Color.white);


        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }
    
    
    //居住地の描写
    void DroweResidence() {
        var lineRenderer = ResidenceObject.AddComponent<LineRenderer>();


        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // 開始点
            new Vector3(0, -50, 0),                    
            new Vector3(80, -50, 0),            
            new Vector3(80, 0, 0),            
            new Vector3(0,  0, 0),              
        };

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;

        //マテリアルの設定
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //色を指定する
        lineRenderer.SetColors(Color.white, Color.white);


        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

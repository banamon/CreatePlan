using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeLine : MonoBehaviour
{
    [SerializeField] GameObject Line2Object;
    [SerializeField] GameObject Line1Object;

    Vector3[] positions1;
    Vector3[] positions2;

    // Start is called before the first frame update
    void Start()
    {
        var lineRenderer1 = Line1Object.AddComponent<LineRenderer>();
        var lineRenderer2 = Line2Object.AddComponent<LineRenderer>();

        positions1 = new Vector3[]{
            new Vector3(120, -100, 0),             
            new Vector3(120,100, 0),
        };
        positions2 = new Vector3[]{
            new Vector3(-100, 0, 0),             
            new Vector3(100,  0, 0),
        };

        // 点の数を指定する
        lineRenderer1.positionCount = positions1.Length;
        lineRenderer2.positionCount = positions2.Length;
        //マテリアルの設定
        lineRenderer1.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer2.material = new Material(Shader.Find("Sprites/Default"));
        //色を指定する
        lineRenderer1.SetColors(Color.white, Color.white);
        lineRenderer2.SetColors(Color.red, Color.red);
        // 線を引く場所を指定する
        lineRenderer1.SetPositions(positions1);
        lineRenderer2.SetPositions(positions2);
    }

    private void Update() {
        Judge(positions1, positions2);
    }

    bool Judge(Vector3[] site_side, Vector3[] residence_side) {
        bool flag = false;

        var tc1 = (site_side[0].x - site_side[1].x) * (residence_side[0].y - site_side[0].y) + (site_side[0].y - site_side[1].y) * (site_side[0].x - residence_side[0].x);
        var tc2 = (site_side[0].x - site_side[1].x) * (residence_side[1].y - site_side[0].y) + (site_side[0].y - site_side[1].y) * (site_side[0].x - residence_side[1].x);

        var td1 = (residence_side[0].x - residence_side[1].x) * (site_side[0].y - residence_side[0].y) + (residence_side[0].y - residence_side[1].y) * (residence_side[0].x - site_side[0].x);
        var td2 = (residence_side[0].x - residence_side[1].x) * (site_side[1].y - residence_side[0].y) + (residence_side[0].y - residence_side[1].y) * (residence_side[0].x - site_side[1].x);



        if (tc1 * tc2 < 0 && td1 * td2 < 0) {
            flag = true;
            Debug.Log("重なりあり");
        }
        else {
            Debug.Log("重なりなし" + site_side[0] + " " + site_side[1] + " " + residence_side[0] + " " + residence_side[1]);
        }

        return flag;
    }
}

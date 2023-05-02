using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 敷地オブジェクト取得
// 居住地オブジェクト取得
/*
 * 1.敷地取得
 * 2.居住地取得
 * 3.居住地配置
 * 4.重なり判定
 * 5.描写
 * 
 */


public class PutObject : MonoBehaviour
{
    [SerializeField] GameObject SiteObject;
    [SerializeField] GameObject ResidenceObjectPrefab;
    [SerializeField] GameObject PrkingObjectPrefab;

    string FigStatus_inside = "inside";
    string FigStatus_outside = "outside";
    string FigStatus_overlap = "overlap";

    // Start is called before the first frame update
    void Start()
    {
        // 敷地図形の座標取得
        Vector3[] site_positons = GetWorldLinepositons(SiteObject);
        //居住地図形の座標取得
        GameObject residence_obj = Instantiate(ResidenceObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        Vector3[] residencepositons_world = GetWorldLinepositons(residence_obj);
        Debug.Log(JudgeLayer(site_positons, residencepositons_world));


        GameObject parking_obj = Instantiate(PrkingObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);

        Debug.Log("敷地と駐車場" + JudgeLayer(site_positons, parking_obj_positons_world));
        Debug.Log("住居と駐車場" + JudgeLayer(residencepositons_world, parking_obj_positons_world));
    }

    /// <summary>
    /// ワールド座標の図形座標集合の取得
    /// </summary>
    /// <param name="fig">取得したいGameObject</param>
    /// <returns>ワールド座標集合</returns>
    Vector3[] GetWorldLinepositons(GameObject fig_obj) {
        LineRenderer fig_linerender = fig_obj.GetComponent<LineRenderer>();
        Vector3[] fig_positons = new Vector3[fig_linerender.positionCount];
        Vector3[] fig_positons_world = new Vector3[fig_linerender.positionCount];
        fig_linerender.GetPositions(fig_positons);

        for (int i = 0; i < fig_positons_world.Length; i++) {
            fig_positons_world[i] = fig_positons[i] + fig_obj.transform.position;
        }
        return fig_positons_world;
    }

    /// <summary>
    /// 図形Aに対する図形の内分，重なり判定
    /// </summary>
    /// <param name="A">判定に使う図形の座標集合</param>
    /// <param name="target">判定をする図形の座標集合</param>
    /// <returns>内分："inside",外分："outside"，重なり："overlap"</returns>
    string JudgeLayer(Vector3[] A, Vector3[] target) {

        bool flag_side = false;

        for (int i = 0;i < target.Length; i++) {
            if (i == 0) {
                flag_side = CheckPoint(A, target[i]);
                continue;
            }

            if (flag_side != CheckPoint(A, target[i])) {
                Debug.Log(i + " " + flag_side + " " +  CheckPoint(A, target[i]) + " " + target[i]);
                return FigStatus_overlap;
            }
        }


        return flag_side ? FigStatus_inside : FigStatus_outside;
    }


    /// <summary>
    /// 図形に対する点の内外判定
    /// </summary> 
    /// <param name="points">図形の座標配列</param>
    /// <param name="target">判定する点の座標</param>
    /// <returns>内分の場合True，外分の場合Flase</returns>
    public bool CheckPoint(Vector3[] points, Vector3 target) {
        Vector3 normal = new Vector3(1f, 0f, 0f);
        //Vector3 normal = Vector3.up;//(0, 1, 0)
        // XY平面上に写像した状態で計算を行う
        Quaternion rot = Quaternion.FromToRotation(normal, -Vector3.forward);

        Vector3[] rotPoints = new Vector3[points.Length];

        for (int i = 0; i < rotPoints.Length; i++) {
            rotPoints[i] = rot * points[i];
        }

        target = rot * target;

        int wn = 0;
        float vt = 0;

        for (int i = 0; i < rotPoints.Length; i++) {
            // 上向きの辺、下向きの辺によって処理を分ける

            int cur = i;
            int next = (i + 1) % rotPoints.Length;

            // 上向きの辺。点PがY軸方向について、始点と終点の間にある。（ただし、終点は含まない）
            if ((rotPoints[cur].y <= target.y) && (rotPoints[next].y > target.y)) {
                // 辺は点Pよりも右側にある。ただし重ならない
                // 辺が点Pと同じ高さになる位置を特定し、その時のXの値と点PのXの値を比較する
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // 上向きの辺と交差した場合は+1
                    wn++;
                }
            }
            else if ((rotPoints[cur].y > target.y) && (rotPoints[next].y <= target.y)) {
                // 辺は点Pよりも右側にある。ただし重ならない
                // 辺が点Pと同じ高さになる位置を特定し、その時のXの値と点PのXの値を比較する
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // 下向きの辺と交差した場合は-1
                    wn--;
                }
            }
        }

        return wn != 0;
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}

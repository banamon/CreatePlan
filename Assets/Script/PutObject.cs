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


    Vector2 SiteMinPos;
    Vector2 SiteMaxPos;

    //居住地オブジェクト
    GameObject residence_obj;
    GameObject parking_obj;

    string FigStatus_inside = "inside";
    string FigStatus_outside = "outside";
    string FigStatus_overlap = "overlap";

    // Start is called before the first frame update
    void Start()
    {
        // 敷地図形の座標取得
        Vector3[] site_positons = GetWorldLinepositons(SiteObject);
        GetRangeSite(site_positons);

        ////居住地図形の座標取得
        //Vector3[] residencepositons_world = PutResidence();
        //Debug.Log(JudgeLayer(site_positons, residencepositons_world));

        ////駐車場の設置
        //parking_obj = Instantiate(PrkingObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        ////駐車場の座標取得
        //Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);

        //Debug.Log("敷地と居住地" + JudgeLayer(site_positons, residencepositons_world));
        //Debug.Log("敷地と駐車場" + JudgeLayer(site_positons, parking_obj_positons_world));
        //Debug.Log("住居と駐車場" + JudgeLayer(residencepositons_world, parking_obj_positons_world));
    }

    /// <summary>
    /// 敷地の座標の最大と最小から範囲を出す関数
    /// </summary>
    /// <param name="sitepos">敷地図形の座標集合</param>
    void GetRangeSite(Vector3[] sitepos) {
        SiteMinPos = sitepos[0];
        SiteMaxPos = sitepos[0];
        for (int i = 0; i<sitepos.Length;i++) {
            //x座標について最小最大の取得
            if (sitepos[i].x < SiteMinPos.x) {
                SiteMinPos.x = sitepos[i].x;
            }else if (sitepos[i].x > SiteMaxPos.x) {
                SiteMaxPos.x = sitepos[i].x;
            }
            
            //y座標について最大最小の取得
            if (sitepos[i].y < SiteMinPos.y) {
                SiteMinPos.y = sitepos[i].y;
            }else if (sitepos[i].y > SiteMaxPos.y) {
                SiteMaxPos.y = sitepos[i].y;
            }
        }
    }


    /// <summary>
    /// 住居地を敷地内にランダムに配置する
    /// ここに設置条件をどんどん反映していく形になるかと思います．
    /// </summary>
    /// <returns></returns>
    Vector3[] PutResidence() {


        //居住地の最大幅，最大縦を取得
        Vector3[] ResidencePrefab_pos = GetWorldLinepositons(ResidenceObjectPrefab);

        //住居地の幅のMax＆Minを取得し，其の内容もランダム範囲の条件に組み込むべき
        int Random_x = Random.Range((int)SiteMinPos.x, (int)SiteMaxPos.x - Get_MaxWidth(ResidencePrefab_pos));
        int Random_y = Random.Range((int)SiteMinPos.y + Get_MaxHeight(ResidencePrefab_pos), (int)SiteMaxPos.y);
        //居住地図形の設置
        Vector3 randampos = new Vector3(Random_x, Random_y, 0);



        //敷地内に入るまで繰り返す
        Vector3[] residencepositons_world = new Vector3[ResidenceObjectPrefab.GetComponent<LineRenderer>().positionCount];
        for (int i = 0; i<15;i++) {
            residence_obj = Instantiate(ResidenceObjectPrefab, randampos, Quaternion.identity);
            Vector3[] residencepositons_temp = GetWorldLinepositons(residence_obj);

            if (JudgeLayer(GetWorldLinepositons(SiteObject), residencepositons_temp).Equals(FigStatus_inside)) {
                Debug.Log("内分");
                residencepositons_world = residencepositons_temp;
                return residencepositons_world;
            }
            else {
                Destroy(residence_obj);
            }
        }
        
        return residencepositons_world;
    }


    void PutParking() {

        Vector3[] ParkingePrefab_pos = GetWorldLinepositons(PrkingObjectPrefab);

        //住居地の幅のMax＆Minを取得し，其の内容もランダム範囲の条件に組み込むべき
        int Random_x = Random.Range((int)SiteMinPos.x, (int)SiteMaxPos.x - Get_MaxWidth(ParkingePrefab_pos));
        int Random_y = Random.Range((int)SiteMinPos.y + Get_MaxHeight(ParkingePrefab_pos), (int)SiteMaxPos.y);
        Vector3 randampos = new Vector3(Random_x, Random_y, 0);

        ////駐車場の設置
        //parking_obj = Instantiate(PrkingObjectPrefab, randampos, Quaternion.identity);
        ////駐車場の座標取得
        //Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);


        //敷地内に入るまで繰り返す
        for (int i = 0; i < 15; i++) {
            parking_obj = Instantiate(PrkingObjectPrefab, randampos, Quaternion.identity);
            Vector3[] parking_obj_positons_temp = GetWorldLinepositons(parking_obj);

            if (JudgeLayer(GetWorldLinepositons(SiteObject), parking_obj_positons_temp).Equals(FigStatus_inside) && JudgeLayer(GetWorldLinepositons(residence_obj), parking_obj_positons_temp).Equals(FigStatus_outside)) {
                Debug.Log("外分");
                //parking_obj_positons_world = parking_obj_positons_temp;
                return;
            }
            else {
                Destroy(parking_obj);
            }
        }

    }

    public void Button_PutResidence() {
        Destroy(residence_obj);
        PutResidence();
    }
    
    public void Button_PutParking() {
        Destroy(parking_obj);
        PutParking();
    }

    /// <summary>
    /// LineRenderのオブジェクトの最大の幅の取得
    /// </summary>
    /// <param name="target">オブジェクトの集合座標</param>
    /// <returns></returns>
    int Get_MaxWidth(Vector3[] target) {
        Vector2 target_MinPos = target[0];
        Vector2 target_MaxPos = target[0];
        for (int i = 0; i < target.Length; i++) {
            //x座標について最小最大の取得
            if (target[i].x < target_MinPos.x) {
                target_MinPos.x = target[i].x;
            }
            else if (target[i].x > target_MaxPos.x) {
                target_MaxPos.x = target[i].x;
            }
        }
        return Mathf.Abs((int)(target_MaxPos.x - target_MinPos.x));
    }
    
    /// <summary>
    /// LineRenderのオブジェクトの最大の高さの取得
    /// </summary>
    /// <param name="target">オブジェクトの集合座標</param>
    /// <returns></returns>
    int Get_MaxHeight(Vector3[] target) {
        Vector2 target_MinPos = target[0];
        Vector2 target_MaxPos = target[0];
        for (int i = 0; i < target.Length; i++) {
            //x座標について最小最大の取得
            if (target[i].y < target_MinPos.y) {
                target_MinPos.y = target[i].y;
            }
            else if (target[i].y > target_MaxPos.y) {
                target_MaxPos.y = target[i].y;
            }
        }
        return Mathf.Abs((int)(target_MaxPos.y - target_MinPos.y));
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
                //Debug.Log(i + " " + flag_side + " " +  CheckPoint(A, target[i]) + " " + target[i]);
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRooms : MonoBehaviour
{

    [SerializeField] GameObject ResidenceObject;
    [SerializeField] GameObject Comonspace_Pfb;
    [SerializeField] GameObject Room_Pfb;

    Vector3[] residence_positons;
    Vector3[] commonspace_positons;



    // Start is called before the first frame update
    void Start()
    {
        residence_positons = GetWorldLinepositons(ResidenceObject);

        ////共有スペースの配置
        GameObject commonspace_obj = Instantiate(Comonspace_Pfb, Vector3.zero, Quaternion.identity);
        commonspace_obj.transform.SetParent(ResidenceObject.transform, true);
        commonspace_positons = GetWorldLinepositons(commonspace_obj);
        Vector3 commonspace_position = (residence_positons[3] + residence_positons[0]) / 2;
        commonspace_position.x -= (commonspace_positons[3].x - commonspace_positons[0].x) / 2;
        commonspace_obj.transform.position = commonspace_position;
        commonspace_positons = GetWorldLinepositons(commonspace_obj);


        Debug.Log(Calc_areasize(residence_positons));
        Debug.Log(Calc_areasize(commonspace_positons));
        Split_2(residence_positons, commonspace_positons);
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

        Debug.Log("ワールド座標" + fig_obj.transform.position);

        for (int i = 0; i < fig_positons_world.Length; i++) {
            fig_positons_world[i] = fig_positons[i] + fig_obj.transform.position;
        }
        return fig_positons_world;
    }

    /// <summary>
    /// 図形の面積計算
    /// </summary>
    /// <param name="positons"></param>
    /// <returns></returns>
    float Calc_areasize(Vector3[] pos) {
        float area = 0;
        for (int i = 0; i< pos.Length;i++) {
            if (i == pos.Length - 1) {

            area += (pos[i].x * pos[0].y - pos[i].y * pos[0].x) / 2;
            }
            else {
            area += (pos[i].x * pos[i + 1].y - pos[i].y * pos[i + 1].x) / 2;

            }
        }
        return Math.Abs(area);
    }

    /// <summary>
    /// 【実装中】
    /// 共有スペースの配置後に，居住スペースと共有スペースが重なった座標集合を取得すれば，分割計算が楽な気がする
    /// </summary>
    void GetLine_Overlap_Residence_Commonspace(Vector3[] respos, Vector3[] comonpos) {

    }

    /// <summary>
    /// お試し2分割 住居空間を
    /// </summary>
    void Split_2(Vector3[] respos, Vector3[] comonpos) {


        Debug.Log("住宅スペース");
        for (int i = 0; i<respos.Length;i++) {
            Debug.Log(respos[i]);
        }
        
        Debug.Log("共有スペース");
        for (int i = 0; i< comonpos.Length;i++) {
            Debug.Log(comonpos[i]);
        }















        //共有スペースの下の辺の中心点の取得
        Vector3 middlepoint_common = (comonpos[1] + comonpos[2]) / 2;

        //中心点から下におろした辺が何番目の辺なのか特定
        int residence_index_befor = 0;
        int residence_index_after = 0;
        for (int i = 0; i<respos.Length-1; i++) {
            // 判定する点の座標
            int x1 = (int)middlepoint_common.x;
            int y1 = (int)middlepoint_common.y;

            // 残りの2点の座標を取得
            int x2 = (int) respos[i].x;
            int y2 = (int) respos[i].y;
            int x3 = (int) respos[i+1].x;
            int y3 = (int) respos[i+1].y;
            float xIntercept = x2 + ((float)(y3 - y2) / (float)(x3 - x2)) * (float)(x1 - x2);
            // 垂直線が線分と交わるかどうかを判断
            if (xIntercept >= Math.Min(x2, x3) && xIntercept <= Math.Max(x2, x3)) {
                residence_index_befor = i;
                residence_index_after = i+1;
                break;
            }
           
        }

        Vector3 middlepoint_residence = (respos[residence_index_befor] + respos[residence_index_after]) / 2;


        Debug.Log(residence_index_befor + " " + residence_index_after);
        Debug.Log(middlepoint_common + " " + middlepoint_residence);

        //辺ABの場合，Aから下がっていくRoomとBから上がっていくRoomを描写
        Vector3[] residence_1 = new Vector3[(residence_index_befor + 1) + 4];
        Vector3[] residence_2 = new Vector3[(respos.Length - residence_index_after + 1) + 3];

        //1つ目の図形
        int residence_1_index = 0;
        residence_1[residence_1_index++] = middlepoint_common;
        residence_1[residence_1_index++] = middlepoint_residence;
        for (int i = residence_index_befor; i > -1; i--) {
            residence_1[residence_1_index++] = respos[i];
        }
        residence_1[residence_1_index++] = comonpos[0];
        residence_1[residence_1_index++] = comonpos[1];
        createbox(residence_1, Vector3.zero);

        //2つ目の図形
        int residence_2_index = 0;
        residence_2[residence_2_index++] = middlepoint_common;
        residence_2[residence_2_index++] = middlepoint_residence;
        for (int i = residence_index_after; i < (respos.Length - residence_index_after + 1) + 1; i++) {
            residence_2[residence_2_index++] = respos[i];
        }
        residence_2[residence_2_index++] = comonpos[3];
        residence_2[residence_2_index++] = comonpos[2];
        createbox(residence_2, Vector3.zero);

    }

    /// <summary>
    /// 座標集合を渡すとオブジェクトを描写
    /// </summary>
    /// <param name="boxpos"></param>
    void createbox(Vector3[] boxlinepos,Vector3 boxpos) {
        /*
         * 
         * boxlineposはローカル座標で，原点0にすべきなのか？？
         * 
         */

        GameObject newRoom = Instantiate(Room_Pfb, boxpos, Quaternion.identity);

        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = newRoom.GetComponent<LineRenderer>();

        var positions = new Vector3[boxlinepos.Length];
        for (int i = 0; i < boxlinepos.Length; i++) {
            positions[i] = boxlinepos[i];
            //positions[i] = boxlinepos[i] - boxlinepos[0];
        }
        

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;     // ローカル座標

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }
}

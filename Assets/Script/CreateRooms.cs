using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

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
        residence_positons = Vector3Utils.GetWorldLinepositons(ResidenceObject);

        ////共有スペースの配置
        GameObject commonspace_obj = Instantiate(Comonspace_Pfb, Vector3.zero, Quaternion.identity);
        commonspace_obj.transform.SetParent(ResidenceObject.transform, true);
        commonspace_positons = Vector3Utils.GetWorldLinepositons(commonspace_obj);
        Vector3 commonspace_position = (residence_positons[3] + residence_positons[0]) / 2;
        commonspace_position.x -= (commonspace_positons[3].x - commonspace_positons[0].x) / 2;
        commonspace_obj.transform.position = commonspace_position;
        commonspace_positons = Vector3Utils.GetWorldLinepositons(commonspace_obj);


        Debug.Log(Vector3Utils.Calc_areasize(residence_positons));
        Debug.Log(Vector3Utils.Calc_areasize(residence_positons));
        Split_2(residence_positons, commonspace_positons);

        Vector3[] overVec =  GetLine_Overlap_Residence_Commonspace(residence_positons, commonspace_positons);
        Vector3Utils.DrowLine(overVec,Vector3.zero,Room_Pfb);
    }





    /// <summary>
    /// 共有スペースの配置後に，居住スペースと共有スペースが重なった座標集合を取得すれば，分割計算が楽な気がする
    /// </summary>
    Vector3[] GetLine_Overlap_Residence_Commonspace(Vector3[] respos, Vector3[] comonpos) {
        Vector3[] overlapvec = new Vector3[respos.Length + comonpos.Length];

        int line_start_index = 0;
        //階段室と建物部分が接している辺の特定
        for (int i = 0; i<respos.Length;i++) {
            Vector3 start = respos[i];
            Vector3 end = (i+1 == respos.Length) ? respos[0]: respos[i+1];
            
            if (JudgePointLine(start, end, comonpos[0])) {
                line_start_index = i;
                break;
            }
        }

        Debug.Log("StartPointIndex" + line_start_index);
        Debug.Log(" overlapvec.Length" + overlapvec.Length);

        //座標合成
        int comonpos_reversindex = 0;
        for (int i = 0; i < overlapvec.Length; i++) {
            if (i > line_start_index && comonpos_reversindex <= comonpos.Length - 1) {
                Debug.Log("overlap[" + i + "] = compos[" + (comonpos.Length - 1 - comonpos_reversindex) + "]");
                overlapvec[i] = comonpos[comonpos.Length-1 - comonpos_reversindex++];
                Debug.Log("合成中" + i + ":" + overlapvec[i] + " " + comonpos_reversindex);
            }else{
                Debug.Log("overlap[" + i + "] = respos[" + (i - comonpos_reversindex) + "]");
                overlapvec[i] = respos[i - comonpos_reversindex];
                Debug.Log("入力中" + i + ":" + overlapvec[i - comonpos_reversindex]);
            }
        }

        return overlapvec;
    }


    /// <summary>
    /// 点が線分上かどうかの判定
    /// 点Pが線分AB上にあるか
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="P"></param>
    /// <returns></returns>
    bool JudgePointLine(Vector3 A, Vector3 B, Vector3 P) {
        bool flag = false;
        if ((A.x <= P.x && P.x <= B.x) || (B.x <= P.x && P.x <= A.x)) {
            if ((A.y <= P.y && P.y <= B.y) || (B.y <= P.y && P.y <= A.y)) {
                if ((P.y * (A.x - B.x)) + (A.y * (B.x - P.x)) + (B.y * (P.x - A.x)) == 0) {
                    // 点Pが線分AB上にある
                    flag = true;
                }
            }
        }
        return flag;
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
        Vector3Utils.DrowLine(residence_1, Vector3.zero,Room_Pfb);

        //2つ目の図形
        int residence_2_index = 0;
        residence_2[residence_2_index++] = middlepoint_common;
        residence_2[residence_2_index++] = middlepoint_residence;
        for (int i = residence_index_after; i < (respos.Length - residence_index_after + 1) + 1; i++) {
            residence_2[residence_2_index++] = respos[i];
        }
        residence_2[residence_2_index++] = comonpos[3];
        residence_2[residence_2_index++] = comonpos[2];
        Vector3Utils.DrowLine(residence_2, Vector3.zero, Room_Pfb);

    }


    /// <summary>
    /// 住戸空間をｎ分割
    /// 返り値はn個の座標配列（オブジェクトでもいいかも）を持つオブジェクト
    /// </summary>
    /// <param name="respos">建物部分座標集合</param>
    /// <param name="n">分割住戸数</param>
    void SplitRes(Vector3[] respos, int n) {

    }





}

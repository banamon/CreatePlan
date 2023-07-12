using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using UnityEngine.UIElements;
using System.Linq;

public class Debug_CreateRoom : MonoBehaviour
{
    //建物配置部分の取得
    [SerializeField] GameObject　ResidenceObject;
    [SerializeField] GameObject Comonspace_Pfb;


    Vector3[] ResVecs;
    //道路に接する辺のIndex
    int[] roadside_point = new int[] { 2, 3 };
    string EvacuationRoute = "EvacuationRoute";

    // Start is called before the first frame update
    void Start()
    {
        ResVecs = Vector3Utils.GetWorldLinepositons(ResidenceObject);

        //for (int i = 0; i < ResVecs.Length; i++) {
        //    int startline_index = i;
        //    int endline_index = (i == ResVecs.Length-1) ? 0 : i+1;
        //    PutEntrance(new int[] { startline_index, endline_index }, ResVecs);
        //}

        int index_s = 0;
        int index_e = 1;
        PutEntrance(new int[] {index_s,index_e}, ResVecs,Vector3.zero);
    }



    void PutEntrance(int[] LineIndexs, Vector3[] ResVecs,Vector3 DrowPos) {
        Vector3 startline = ResVecs[LineIndexs[0]];
        Vector3 endline = ResVecs[LineIndexs[1]];
        int Entrance_width = 100;
        int EvacuationRouteWidth = 50;

        //道の管理が決まってないからあとでかわりそう
        bool EvacuationRouteFlag = !(roadside_point.Contains(LineIndexs[0]) && roadside_point.Contains(LineIndexs[1]));
        if (EvacuationRouteFlag) { 
            var result = PutEvacuationRoute(ResVecs, LineIndexs, Entrance_width, EvacuationRouteWidth);
            GameObject temp1 = Vector3Utils.DrowLine(result.evarooute1, DrowPos, EvacuationRoute);
            temp1.transform.SetParent(ResidenceObject.transform, true);
            GameObject temp2 = Vector3Utils.DrowLine(result.evarooute2, DrowPos, EvacuationRoute);
            temp2.transform.SetParent(ResidenceObject.transform, true);
        }


        GameObject commonspace_obj = Instantiate(Comonspace_Pfb, Vector3.zero,Quaternion.identity);
        commonspace_obj.transform.SetParent(ResidenceObject.transform, true);
        Vector3[] EntranceVecs = Vector3Utils.GetWorldLinepositons(commonspace_obj);
        //float Entrance_width = (EntranceVecs[0] - EntranceVecs[3]).magnitude;
        // ベクトルABを計算
        Vector3 vectorAB = endline - startline;
        // ベクトルABを左側に90度回転させる
        Vector3 rotatedVector = new Vector3(vectorAB.y, -vectorAB.x, vectorAB.z);
        Vector3 commonspace_position = ((startline + endline) / 2) + (Entrance_width/2) * vectorAB.normalized;
        commonspace_position = (EvacuationRouteFlag) ? commonspace_position + EvacuationRouteWidth * -1 * rotatedVector.normalized: commonspace_position;


        //角度調整
        commonspace_obj.transform.rotation = Quaternion.LookRotation(Vector3.forward, rotatedVector);
        //位置調整
        commonspace_obj.transform.position = commonspace_position;
    }


    (Vector3[] evarooute1, Vector3[] evarooute2) PutEvacuationRoute(Vector3[] ResVecs, int[] LineIndexs,int Entrance_width,int EvacuationRouteWidth) {

        Vector3[] EvacuationRouteVecs_plas = GetEvacuationRoute_plus(ResVecs, LineIndexs, Entrance_width, EvacuationRouteWidth);
        Vector3[] EvacuationRouteVecs_minus = GetEvacuationRoute_minus(ResVecs, LineIndexs, Entrance_width, EvacuationRouteWidth);

        return (EvacuationRouteVecs_plas, EvacuationRouteVecs_minus);
    }

    /// <summary>
    /// /エントランスの避難経路を右回りで探す
    /// </summary>
    /// <param name="ResVecs"></param>
    /// <param name="LineIndexs"></param>
    /// <param name="Entrance_width"></param>
    /// <param name="EvacuationRouteWidth"></param>
    /// <returns></returns>
    Vector3[] GetEvacuationRoute_plus(Vector3[] ResVecs, int[] LineIndexs, int Entrance_width, int EvacuationRouteWidth) {
        int startindex = LineIndexs[0];
        int endindex = LineIndexs[1];
        List<Vector3> resposlist = new List<Vector3>();

        //外側
        List<Vector3> res_out = new List<Vector3>();
        List<Vector3> res_in = new List<Vector3>();

        GetResRange getResRange = new GetResRange();

        // ベクトルABを計算
        Vector3 vectorAB = ResVecs[endindex] - ResVecs[startindex];
        // ベクトルABを左側に90度回転させる
        Vector3 startPos = ((ResVecs[startindex] + ResVecs[endindex]) / 2) - (Entrance_width / 2) * vectorAB.normalized;
        res_out.Add(startPos);
        Vector3 rotatedVectorAB = new Vector3(-vectorAB.y, vectorAB.x, vectorAB.z);
        res_in.Add(startPos + (EvacuationRouteWidth * rotatedVectorAB.normalized));


        //道路に接する辺までの座標取得
        for (int i = 0; i < ResVecs.Length; i++) {

            int theindex = (endindex + i < ResVecs.Length) ? endindex + i : endindex + i - ResVecs.Length;

            Debug.Log("theindex" + theindex);
            Vector3 A = (theindex - 1 >= 0) ? ResVecs[theindex - 1] : ResVecs[ResVecs.Length - 1];
            Vector3 B = ResVecs[theindex];
            Vector3 C = (theindex + 1 < ResVecs.Length) ? ResVecs[theindex + 1] : ResVecs[0];


            if (roadside_point.Contains(theindex)) {
                res_out.Add(ResVecs[theindex]);
                res_in.Add(getResRange.CalculatePointP(A, B, C, EvacuationRouteWidth, 0));
                break;
            }

            res_out.Add(ResVecs[theindex]);
            res_in.Add(getResRange.CalculatePointP(A, B, C, EvacuationRouteWidth, EvacuationRouteWidth));
        }

        resposlist.AddRange(res_out);
        res_in.Reverse();
        resposlist.AddRange(res_in);
        return resposlist.ToArray();
    }


    /// <summary>
    /// エントランスの避難経路を左回りで探索
    /// </summary>
    /// <param name="ResVecs"></param>
    /// <param name="LineIndexs"></param>
    /// <param name="Entrance_width"></param>
    /// <param name="EvacuationRouteWidth"></param>
    /// <returns></returns>
    Vector3[] GetEvacuationRoute_minus(Vector3[] ResVecs, int[] LineIndexs, int Entrance_width ,int EvacuationRouteWidth) {
        int startindex = LineIndexs[0];
        int endindex = LineIndexs[1];
        List<Vector3> resposlist = new List<Vector3>();

        //外側
        List<Vector3> res_out = new List<Vector3>();
        List<Vector3> res_in = new List<Vector3>();

        GetResRange getResRange = new GetResRange();

        // ベクトルABを計算
        Vector3 vectorAB = ResVecs[endindex] - ResVecs[startindex];
        // ベクトルABを左側に90度回転させる
        Vector3 startPos = ((ResVecs[startindex] + ResVecs[endindex]) / 2) + (Entrance_width / 2) * vectorAB.normalized;
        res_out.Add(startPos);
        Vector3 rotatedVectorAB = new Vector3(-vectorAB.y, vectorAB.x, vectorAB.z);
        res_in.Add(startPos + (EvacuationRouteWidth * rotatedVectorAB.normalized));


        //道路に接する辺までの座標取得
        for (int i = 0; i < ResVecs.Length; i++) {

            int theindex = (startindex - i >= 0) ? startindex - i : startindex - i + ResVecs.Length;

            Debug.Log("theindex" + theindex);
            Vector3 A = (theindex - 1 >= 0) ? ResVecs[theindex - 1] : ResVecs[ResVecs.Length - 1];
            Vector3 B = ResVecs[theindex];
            Vector3 C = (theindex + 1 < ResVecs.Length) ? ResVecs[theindex + 1] : ResVecs[0];


            if (roadside_point.Contains(theindex)) {
                res_out.Add(ResVecs[theindex]);
                res_in.Add(getResRange.CalculatePointP(A, B, C, 0, EvacuationRouteWidth));
                break;
            }

            res_out.Add(ResVecs[theindex]);
            res_in.Add(getResRange.CalculatePointP(A, B, C, EvacuationRouteWidth, EvacuationRouteWidth));
        }

        resposlist.AddRange(res_out);
        res_in.Reverse();
        resposlist.AddRange(res_in);
        return resposlist.ToArray();
    }
}

using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetResRange : MonoBehaviour
{
    [SerializeField] GameObject landform;
    [SerializeField] GameObject ResPreafb;

    int Distanceboundary = 610;
    Vector3[] landrormvec;


    // Start is called before the first frame update
    void Start()
    {
        landrormvec = Vector3Utils.GetWorldLinepositons(landform);
        Vector3[] possiblerange_vecs = new Vector3[landrormvec.Length];


        for (int i = 0;i < possiblerange_vecs.Length; i++) {
            Vector3 A = (i-1 >= 0) ? landrormvec[i - 1] : landrormvec[landrormvec.Length - 1];
            Vector3 B = landrormvec[i];
            Vector3 C = (i + 1 < landrormvec.Length) ? landrormvec[i + 1] : landrormvec[0];
            
            // 距離Xを指定します
            int distanceX = Distanceboundary;
            Vector3 P = CalculatePointP(A, B, C, distanceX);
            possiblerange_vecs[i] = P;

            Debug.Log("目標：" + distanceX + " " + CalculateDistance(A,B,P) + " " + CalculateDistance(B, C, P));
        }
        Vector3Utils.DrowLine(possiblerange_vecs, Vector3.zero, ResPreafb);
    }

    /// <summary>
    /// 全ての境界線から一定距離離れた座標集合の取得
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="C"></param>
    /// <param name="distance"></param>
    /// <returns></returns>
    Vector3 CalculatePointP(Vector3 A,Vector3 B, Vector3 C, int distance) {
        Debug.LogWarning("=="+A + " " + B + " " + C + "==");

        // ベクトルBCの取得
        Vector3 vectorBC = C - B;

        //内心Iとその単位ベクトルの取得
        Vector3 vectorBI = Get_innercenter(A, B, C) - B;
        Vector3 normalizedBI = vectorBI.normalized;

        //ベクトルBPの長さを求める
        Vector3 normalizedBC = vectorBC.normalized;
        float sin = (-1 * normalizedBC.y * normalizedBI.x + normalizedBC.x * normalizedBI.y);
        float distance_I = distance / sin;


        Vector3 vectorBP = distance_I * normalizedBI;

        return vectorBP + B;
    }

    public Vector3 Get_innercenter(Vector3 A, Vector3 B, Vector3 C) {

        float c = Vector3.Distance(A, B);
        float b = Vector3.Distance(A, C);
        float a = Vector3.Distance(B, C);
        var v = ((a * A + b * B + c * C) / (a + b + c));
        //Vector3Int innercenter = Vector3Int.RoundToInt(v);

        return v;
    }

    // 辺ABと点Pの距離を計算する関数
    public double CalculateDistance(Vector3 A, Vector3 B, Vector3 P) {
        // 辺ABのベクトルを計算
        Vector3 vectorAB = B - A;

        // 点Pから点Aへのベクトルを計算
        Vector3 vectorAP = P - A;

        // 辺ABの単位ベクトルを計算
        double lengthAB = Math.Sqrt(vectorAB.x * vectorAB.x + vectorAB.y * vectorAB.y);
        Vector3 unitVectorAB = vectorAB / (float)lengthAB;

        // 点Pから辺ABへの垂線の長さを計算
        double perpendicularLength = vectorAP.x * unitVectorAB.y - vectorAP.y * unitVectorAB.x;

        // 距離を絶対値で返す
        return Math.Abs(perpendicularLength);
    }

    public static float Vector2Cross(Vector2 lhs, Vector2 rhs) {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }
}

using Assets.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetResRange : MonoBehaviour
{
    [SerializeField] GameObject landform;
    [SerializeField] GameObject ResPreafb;

    int Distanceboundary = 10;
    //int Distanceboundary = 610;
    Vector3[] landrormvec;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start");
        landrormvec = Vector3Utils.GetWorldLinepositons(landform);

        int[] distances = new int[landrormvec.Length];
        for (int i = 0; i < landrormvec.Length; i++) {
            distances[i] = (i % 2 == 0) ? 5 : 10;
            //distances[i] = i * 2 + 5;
            Debug.Log(distances[i]);
        }

        Vector3Utils.DrowLine(GetPossibleArea(landrormvec,distances), Vector3.zero, ResPreafb);
    }


    public Vector3[] GetPossibleArea(Vector3[] landvecs, int[] distances) {
        Vector3[] possiblerange_vecs = new Vector3[landvecs.Length];

        for (int i = 0; i < possiblerange_vecs.Length; i++) {
            //���͂̎擾
            Vector3 A = (i - 1 >= 0) ? landvecs[i - 1] : landvecs[landvecs.Length - 1];
            Vector3 B = landvecs[i];
            Vector3 C = (i + 1 < landvecs.Length) ? landvecs[i + 1] : landvecs[0];
            int distanceY = distances[i];
            int distanceX = (i - 1 >= 0) ? distances[i - 1] : distances[distances.Length - 1];

            // �_P�擾
            Vector3 P = CalculatePointP(A, B, C, distanceX, distanceY);
            possiblerange_vecs[i] = P;

            //Debug.Log("�ڕW�F" + distanceX + "/" + distanceY + " " + CalculateDistance(A, B, P) + "/" + CalculateDistance(B, C, P));
        }

        return possiblerange_vecs;
    }

    /// <summary>
    /// ��AB����distanceY�C��BC����distanceX�̋������Ƃ�����̍��W�W���̎擾
    /// </summary>
    /// <param name="A"></param>
    /// <param name="B"></param>
    /// <param name="C"></param>
    /// <param name="distanceY"></param>
    /// <param name="distanceX"></param>
    /// <returns></returns>
    public Vector3 CalculatePointP(Vector3 A, Vector3 B, Vector3 C, int distanceY, int distanceX) {
        Debug.LogWarning("==" + A + " " + B + " " + C + " X:" + distanceX + " Y:" + distanceY + "==");;

        // BA,BC�̒P�ʃx�N�g���擾
        Vector3 normalizedBA = (A - B).normalized;
        Vector3 normalizedBC = (C - B).normalized;

        float distanceA1 = normalizedBA.magnitude;
        float distanceC1 = ((float)distanceY / (float)distanceX) * distanceA1;

        if (distanceY * distanceX == 0) {
            distanceA1 = (distanceY == 0) ? normalizedBA.magnitude : 0;
            distanceC1 = (distanceY == 0) ? 0 : normalizedBC.magnitude;
        }


        Vector2 normalizedBI = (distanceA1 * normalizedBA + distanceC1 * normalizedBC).normalized;
        float sin = (-1 * normalizedBC.y * normalizedBI.x + normalizedBC.x * normalizedBI.y);
        float distance_I = distanceX / sin;
        if (distanceX == 0) {
            sin = (-1 * normalizedBA.y * normalizedBI.x + normalizedBA.x * normalizedBI.y);
            distance_I = distanceY / (-1 * sin);
        }

        Vector3 vectorBP = distance_I * normalizedBI;
        return vectorBP + B;
    }




    public Vector3 Get_innercenter(Vector3 A, Vector3 B, Vector3 C) {

        float c = Vector3.Distance(A, B);
        float b = Vector3.Distance(A, C);
        float a = Vector3.Distance(B, C);
        var v = ((a * A + b * B + c * C) / (a + b + c));
        return v;
    }

    // ��AB�Ɠ_P�̋������v�Z����֐�
    public double CalculateDistance(Vector3 A, Vector3 B, Vector3 P) {
        // ��AB�̃x�N�g�����v�Z
        Vector3 vectorAB = B - A;

        // �_P����_A�ւ̃x�N�g�����v�Z
        Vector3 vectorAP = P - A;

        // ��AB�̒P�ʃx�N�g�����v�Z
        double lengthAB = Math.Sqrt(vectorAB.x * vectorAB.x + vectorAB.y * vectorAB.y);
        Vector3 unitVectorAB = vectorAB / (float)lengthAB;

        // �_P�����AB�ւ̐����̒������v�Z
        double perpendicularLength = vectorAP.x * unitVectorAB.y - vectorAP.y * unitVectorAB.x;

        // �������Βl�ŕԂ�
        return Math.Abs(perpendicularLength);
    }

    //
    public static float Vector2Cross(Vector2 lhs, Vector2 rhs) {
        return lhs.x * rhs.y - rhs.x * lhs.y;
    }
}

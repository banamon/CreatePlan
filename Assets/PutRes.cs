using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;
using System;

/// <summary>
/// �����z�u
/// </summary>
public class PutRes : MonoBehaviour
{
    [SerializeField]GameObject landform;
    [SerializeField]GameObject ResPreafb;

    // �������FBuilding Coverage Ratio
    float BCR = 0.8f;

    bool calcres = false;

    Vector3[] landrormvec;
    int landformarea;
    int targetArea;

    int LineX;
    bool calcareafirst = false;
    bool areaflag = false;
    int count_areacalc = 0;

    // Start is called before the first frame update
    void Start()
    {
        //���W�擾
        landrormvec = Vector3Utils.GetWorldLinepositons(landform);
        landformarea = (int)Vector3Utils.Calc_areasize(landrormvec);
        targetArea = (int) (landformarea * BCR);

        float x1 = landrormvec[0].x;
        float x2 = landrormvec[1].x;
        float y1 = landrormvec[0].y;
        float y2 = landrormvec[1].y;
        LineX = (int)(targetArea / Math.Sqrt((Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2))));
        Debug.Log("�n�^�ʐρF" + landformarea + " �ڕW�ʐρF" + targetArea + "fist LineX" + LineX);
        count_areacalc = 0;
        calcareafirst = true;
        calcres = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (calcres) {
            Vector3[] tempres = Getintersection(landrormvec, LineX);
            int temparea = (int) Vector3Utils.Calc_areasize(tempres);
            LineX = (temparea > targetArea) ? LineX - 1: LineX + 1;

            if (calcareafirst) {
                areaflag = (temparea > targetArea);
                calcareafirst = false;
                return;
            }

            count_areacalc++;
            if (areaflag != (temparea > targetArea)) {
                Debug.Log("Finish �ڕW:" + targetArea + " ����:" + temparea + "�v�Z��" + count_areacalc );
                Vector3Utils.DrowLine(tempres, Vector3.zero, ResPreafb);
                calcres = false;
            }
        }    
    }

    /// <summary>
    /// �����狗��x�̐����������ꍇ�̌��������̍��W�W���̎擾
    /// </summary>
    /// <param name="landformpos">�n�^���W��ԁ˂��ƂŌ����z�u�������W�W���ɕύX</param>
    /// <param name="x">��������x���W</param>
    /// <returns></returns>
    Vector3[] Getintersection(Vector3[] landformpos,int x){
        List<Vector3> resposlist = new List<Vector3>();

        bool Getlandrompos = true;
        //�K�i���ƌ����������ڂ��Ă���ӂ̓���
        for (int i = 0; i < landformpos.Length; i++) {
            Vector3 start = landformpos[i];
            Vector3 end = (i + 1 == landformpos.Length) ? landformpos[0] : landformpos[i + 1];

            if (Getlandrompos) {
                resposlist.Add(start);
            }
            if ((start.x < x && x < end.x)||(start.x > x && x > end.x)) {
                float y = (end.y - start.y) / (end.x - start.x) * (x - end.x) + end.y;
                Vector3 intersection = new Vector3(x, y, 0);
                resposlist.Add(intersection);
                Getlandrompos = !Getlandrompos;
            }
        }

        Vector3[] respos = resposlist.ToArray();
        return respos;
        //Vector3Utils.DrowLine(respos,Vector3.zero, ResPreafb);
    }



    void GetBuildinglayoutRange(Vector3[] landrormvec) {
        Vector3 temp = landrormvec[0];

        //int a = 
    }



}

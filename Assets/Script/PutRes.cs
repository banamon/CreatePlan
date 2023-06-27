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
    float BCR = 0.2f;

    bool calcres = false;

    Vector3[] landrormvec;
    /// <summary>
    /// �n�^�ʐ�
    /// </summary>
    long landformarea;
    long targetArea;

    long LineX;
    bool calcareafirst = false;
    bool areaflag = false;
    long count_areacalc = 0;

    // Start is called before the first frame update
    void Start()
    {
        //���W�擾
        landrormvec = Vector3Utils.GetWorldLinepositons(landform);
        landformarea = (long)Vector3Utils.Calc_areasize(landrormvec);
        targetArea = (long) (landformarea * BCR);

        ////�n�^��x�͈̔͂̎擾
        //int min_x = 0;
        //int max_x = 0;
        //for (int i = 0; i < landrormvec.Length; i++) {
        //    if (max_x < landrormvec[i].x) {
        //        max_x = (int) landrormvec[i].x;
        //    }
        //}


        ////2���T���̂��߁CX�͈̔͂̒����l�������l�Ƃ��ē����
        //Debug.Log(
        //    "�n�^�ʐρF" + landformarea + 
        //    " �ڕW�ʐρF" + targetArea + 
        //    " ���W�͈�(" + min_x + " �` " + max_x
        //);


        //int LineX = SearchX(min_x, max_x, landrormvec, targetArea);
        //Vector3[] AreaVec = Getintersection(landrormvec, LineX);
        Vector3Utils.DrowLine(GetResArea(landrormvec,targetArea), Vector3.zero, ResPreafb);
    }


    public Vector3[] GetResArea(Vector3[] landrormvec, long targetArea) {
        int min_x = 0;
        int max_x = 0;
        for (int i = 0; i < landrormvec.Length; i++) {
            if (max_x < landrormvec[i].x) {
                max_x = (int)landrormvec[i].x;
            }
        }
        int LineX = SearchX(min_x, max_x, landrormvec, targetArea);
        Vector3[] AreaVec = Getintersection(landrormvec, LineX);
        
        return AreaVec;
    }






    /// <summary>
    /// �ڕW�ʐς��Ƃ��x���W��2���T���Ō�����
    /// </summary>
    /// <param name="min_x"></param>
    /// <param name="max_x"></param>
    /// <param name="landrormvec"></param>
    /// <param name="targetArea"></param>
    /// <param name="min_diff"></param>
    /// <returns></returns>
    int SearchX(int min_x, int max_x , Vector3[] landrormvec, long targetArea) {
        int count = 0;
        int closet_LineX = 0;

        long min_diff = max_x;

        while (min_x <= max_x) {
            Debug.Log(min_x + "�`" + min_x + " " + min_diff); 
            //���Ԓn�̎擾
            int tempLineX = (int)((min_x + max_x) / 2);
            Vector3[] tempres = Getintersection(landrormvec, tempLineX + 1);
            int temparea = (int)Vector3Utils.Calc_areasize(tempres);

            //�����l�̍��E�̒l�Ɣ�r(tempLineX�Ɣ͈͂̊m�F���K�v�j
            //�E��
            Vector3[] tempres_right = Getintersection(landrormvec, tempLineX + 1);
            long temparea_right = (int)Vector3Utils.Calc_areasize(tempres_right);
            long min_diff_right = Math.Abs(targetArea - temparea_right);
            //����
            Vector3[] tempres_left = Getintersection(landrormvec, tempLineX - 1);
            long temparea_left = (int)Vector3Utils.Calc_areasize(tempres_left);
            long min_diff_left = Math.Abs(targetArea - temparea_left);


            //�ŏ��l�̍X�V
            if (min_diff > min_diff_right) {
                min_diff = min_diff_right;
                closet_LineX = tempLineX + 1;
            }
            else if (min_diff > min_diff_left) { 
                min_diff = min_diff_left;
                closet_LineX = tempLineX - 1;
            }

            //�T���͈͂̋��߂�i2���T���j
            if (temparea < targetArea) {
                min_x = tempLineX + 1;
            }else if (temparea > targetArea) {
                max_x = tempLineX - 1;
            }
            else {
                return closet_LineX;
            }
            count++;
        }

        Debug.Log("�v�Z�K��" + count + "�� closet_LineX:" + closet_LineX);
        return closet_LineX;
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

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

        ////���L�X�y�[�X�̔z�u
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
    /// ���L�X�y�[�X�̔z�u��ɁC���Z�X�y�[�X�Ƌ��L�X�y�[�X���d�Ȃ������W�W�����擾����΁C�����v�Z���y�ȋC������
    /// </summary>
    Vector3[] GetLine_Overlap_Residence_Commonspace(Vector3[] respos, Vector3[] comonpos) {
        Vector3[] overlapvec = new Vector3[respos.Length + comonpos.Length];

        int line_start_index = 0;
        //�K�i���ƌ����������ڂ��Ă���ӂ̓���
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

        //���W����
        int comonpos_reversindex = 0;
        for (int i = 0; i < overlapvec.Length; i++) {
            if (i > line_start_index && comonpos_reversindex <= comonpos.Length - 1) {
                Debug.Log("overlap[" + i + "] = compos[" + (comonpos.Length - 1 - comonpos_reversindex) + "]");
                overlapvec[i] = comonpos[comonpos.Length-1 - comonpos_reversindex++];
                Debug.Log("������" + i + ":" + overlapvec[i] + " " + comonpos_reversindex);
            }else{
                Debug.Log("overlap[" + i + "] = respos[" + (i - comonpos_reversindex) + "]");
                overlapvec[i] = respos[i - comonpos_reversindex];
                Debug.Log("���͒�" + i + ":" + overlapvec[i - comonpos_reversindex]);
            }
        }

        return overlapvec;
    }


    /// <summary>
    /// �_�������ォ�ǂ����̔���
    /// �_P������AB��ɂ��邩
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
                    // �_P������AB��ɂ���
                    flag = true;
                }
            }
        }
        return flag;
    }

    /// <summary>
    /// ������2���� �Z����Ԃ�
    /// </summary>
    void Split_2(Vector3[] respos, Vector3[] comonpos) {


        Debug.Log("�Z��X�y�[�X");
        for (int i = 0; i<respos.Length;i++) {
            Debug.Log(respos[i]);
        }
        
        Debug.Log("���L�X�y�[�X");
        for (int i = 0; i< comonpos.Length;i++) {
            Debug.Log(comonpos[i]);
        }



        //���L�X�y�[�X�̉��̕ӂ̒��S�_�̎擾
        Vector3 middlepoint_common = (comonpos[1] + comonpos[2]) / 2;

        //���S�_���牺�ɂ��낵���ӂ����Ԗڂ̕ӂȂ̂�����
        int residence_index_befor = 0;
        int residence_index_after = 0;
        for (int i = 0; i<respos.Length-1; i++) {
            // ���肷��_�̍��W
            int x1 = (int)middlepoint_common.x;
            int y1 = (int)middlepoint_common.y;

            // �c���2�_�̍��W���擾
            int x2 = (int) respos[i].x;
            int y2 = (int) respos[i].y;
            int x3 = (int) respos[i+1].x;
            int y3 = (int) respos[i+1].y;
            float xIntercept = x2 + ((float)(y3 - y2) / (float)(x3 - x2)) * (float)(x1 - x2);
            // �������������ƌ���邩�ǂ����𔻒f
            if (xIntercept >= Math.Min(x2, x3) && xIntercept <= Math.Max(x2, x3)) {
                residence_index_befor = i;
                residence_index_after = i+1;
                break;
            }
           
        }

        Vector3 middlepoint_residence = (respos[residence_index_befor] + respos[residence_index_after]) / 2;


        Debug.Log(residence_index_befor + " " + residence_index_after);
        Debug.Log(middlepoint_common + " " + middlepoint_residence);

        //��AB�̏ꍇ�CA���牺�����Ă���Room��B����オ���Ă���Room��`��
        Vector3[] residence_1 = new Vector3[(residence_index_befor + 1) + 4];
        Vector3[] residence_2 = new Vector3[(respos.Length - residence_index_after + 1) + 3];

        //1�ڂ̐}�`
        int residence_1_index = 0;
        residence_1[residence_1_index++] = middlepoint_common;
        residence_1[residence_1_index++] = middlepoint_residence;
        for (int i = residence_index_befor; i > -1; i--) {
            residence_1[residence_1_index++] = respos[i];
        }
        residence_1[residence_1_index++] = comonpos[0];
        residence_1[residence_1_index++] = comonpos[1];
        Vector3Utils.DrowLine(residence_1, Vector3.zero,Room_Pfb);

        //2�ڂ̐}�`
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
    /// �Z�ˋ�Ԃ�������
    /// �Ԃ�l��n�̍��W�z��i�I�u�W�F�N�g�ł����������j�����I�u�W�F�N�g
    /// </summary>
    /// <param name="respos">�����������W�W��</param>
    /// <param name="n">�����Z�ː�</param>
    void SplitRes(Vector3[] respos, int n) {

    }





}

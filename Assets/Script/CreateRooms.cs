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

        ////���L�X�y�[�X�̔z�u
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
    /// ���[���h���W�̐}�`���W�W���̎擾
    /// </summary>
    /// <param name="fig">�擾������GameObject</param>
    /// <returns>���[���h���W�W��</returns>
    Vector3[] GetWorldLinepositons(GameObject fig_obj) {
        LineRenderer fig_linerender = fig_obj.GetComponent<LineRenderer>();
        Vector3[] fig_positons = new Vector3[fig_linerender.positionCount];
        Vector3[] fig_positons_world = new Vector3[fig_linerender.positionCount];
        fig_linerender.GetPositions(fig_positons);

        Debug.Log("���[���h���W" + fig_obj.transform.position);

        for (int i = 0; i < fig_positons_world.Length; i++) {
            fig_positons_world[i] = fig_positons[i] + fig_obj.transform.position;
        }
        return fig_positons_world;
    }

    /// <summary>
    /// �}�`�̖ʐόv�Z
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
    /// �y�������z
    /// ���L�X�y�[�X�̔z�u��ɁC���Z�X�y�[�X�Ƌ��L�X�y�[�X���d�Ȃ������W�W�����擾����΁C�����v�Z���y�ȋC������
    /// </summary>
    void GetLine_Overlap_Residence_Commonspace(Vector3[] respos, Vector3[] comonpos) {

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
        createbox(residence_1, Vector3.zero);

        //2�ڂ̐}�`
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
    /// ���W�W����n���ƃI�u�W�F�N�g��`��
    /// </summary>
    /// <param name="boxpos"></param>
    void createbox(Vector3[] boxlinepos,Vector3 boxpos) {
        /*
         * 
         * boxlinepos�̓��[�J�����W�ŁC���_0�ɂ��ׂ��Ȃ̂��H�H
         * 
         */

        GameObject newRoom = Instantiate(Room_Pfb, boxpos, Quaternion.identity);

        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = newRoom.GetComponent<LineRenderer>();

        var positions = new Vector3[boxlinepos.Length];
        for (int i = 0; i < boxlinepos.Length; i++) {
            positions[i] = boxlinepos[i];
            //positions[i] = boxlinepos[i] - boxlinepos[0];
        }
        

        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;     // ���[�J�����W

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }
}

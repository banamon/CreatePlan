using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �~�n�I�u�W�F�N�g�擾
// ���Z�n�I�u�W�F�N�g�擾
/*
 * 1.�~�n�擾
 * 2.���Z�n�擾
 * 3.���Z�n�z�u
 * 4.�d�Ȃ蔻��
 * 5.�`��
 * 
 */


public class PutObject : MonoBehaviour
{
    [SerializeField] GameObject SiteObject;
    [SerializeField] GameObject ResidenceObjectPrefab;
    [SerializeField] GameObject PrkingObjectPrefab;

    string FigStatus_inside = "inside";
    string FigStatus_outside = "outside";
    string FigStatus_overlap = "overlap";

    // Start is called before the first frame update
    void Start()
    {
        // �~�n�}�`�̍��W�擾
        Vector3[] site_positons = GetWorldLinepositons(SiteObject);
        //���Z�n�}�`�̍��W�擾
        GameObject residence_obj = Instantiate(ResidenceObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        Vector3[] residencepositons_world = GetWorldLinepositons(residence_obj);
        Debug.Log(JudgeLayer(site_positons, residencepositons_world));


        GameObject parking_obj = Instantiate(PrkingObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);

        Debug.Log("�~�n�ƒ��ԏ�" + JudgeLayer(site_positons, parking_obj_positons_world));
        Debug.Log("�Z���ƒ��ԏ�" + JudgeLayer(residencepositons_world, parking_obj_positons_world));
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

        for (int i = 0; i < fig_positons_world.Length; i++) {
            fig_positons_world[i] = fig_positons[i] + fig_obj.transform.position;
        }
        return fig_positons_world;
    }

    /// <summary>
    /// �}�`A�ɑ΂���}�`�̓����C�d�Ȃ蔻��
    /// </summary>
    /// <param name="A">����Ɏg���}�`�̍��W�W��</param>
    /// <param name="target">���������}�`�̍��W�W��</param>
    /// <returns>�����F"inside",�O���F"outside"�C�d�Ȃ�F"overlap"</returns>
    string JudgeLayer(Vector3[] A, Vector3[] target) {

        bool flag_side = false;

        for (int i = 0;i < target.Length; i++) {
            if (i == 0) {
                flag_side = CheckPoint(A, target[i]);
                continue;
            }

            if (flag_side != CheckPoint(A, target[i])) {
                Debug.Log(i + " " + flag_side + " " +  CheckPoint(A, target[i]) + " " + target[i]);
                return FigStatus_overlap;
            }
        }


        return flag_side ? FigStatus_inside : FigStatus_outside;
    }


    /// <summary>
    /// �}�`�ɑ΂���_�̓��O����
    /// </summary> 
    /// <param name="points">�}�`�̍��W�z��</param>
    /// <param name="target">���肷��_�̍��W</param>
    /// <returns>�����̏ꍇTrue�C�O���̏ꍇFlase</returns>
    public bool CheckPoint(Vector3[] points, Vector3 target) {
        Vector3 normal = new Vector3(1f, 0f, 0f);
        //Vector3 normal = Vector3.up;//(0, 1, 0)
        // XY���ʏ�Ɏʑ�������ԂŌv�Z���s��
        Quaternion rot = Quaternion.FromToRotation(normal, -Vector3.forward);

        Vector3[] rotPoints = new Vector3[points.Length];

        for (int i = 0; i < rotPoints.Length; i++) {
            rotPoints[i] = rot * points[i];
        }

        target = rot * target;

        int wn = 0;
        float vt = 0;

        for (int i = 0; i < rotPoints.Length; i++) {
            // ������̕ӁA�������̕ӂɂ���ď����𕪂���

            int cur = i;
            int next = (i + 1) % rotPoints.Length;

            // ������̕ӁB�_P��Y�������ɂ��āA�n�_�ƏI�_�̊Ԃɂ���B�i�������A�I�_�͊܂܂Ȃ��j
            if ((rotPoints[cur].y <= target.y) && (rotPoints[next].y > target.y)) {
                // �ӂ͓_P�����E���ɂ���B�������d�Ȃ�Ȃ�
                // �ӂ��_P�Ɠ��������ɂȂ�ʒu����肵�A���̎���X�̒l�Ɠ_P��X�̒l���r����
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // ������̕ӂƌ��������ꍇ��+1
                    wn++;
                }
            }
            else if ((rotPoints[cur].y > target.y) && (rotPoints[next].y <= target.y)) {
                // �ӂ͓_P�����E���ɂ���B�������d�Ȃ�Ȃ�
                // �ӂ��_P�Ɠ��������ɂȂ�ʒu����肵�A���̎���X�̒l�Ɠ_P��X�̒l���r����
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // �������̕ӂƌ��������ꍇ��-1
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

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


    Vector2 SiteMinPos;
    Vector2 SiteMaxPos;

    //���Z�n�I�u�W�F�N�g
    GameObject residence_obj;
    GameObject parking_obj;

    string FigStatus_inside = "inside";
    string FigStatus_outside = "outside";
    string FigStatus_overlap = "overlap";

    // Start is called before the first frame update
    void Start()
    {
        // �~�n�}�`�̍��W�擾
        Vector3[] site_positons = GetWorldLinepositons(SiteObject);
        GetRangeSite(site_positons);

        ////���Z�n�}�`�̍��W�擾
        //Vector3[] residencepositons_world = PutResidence();
        //Debug.Log(JudgeLayer(site_positons, residencepositons_world));

        ////���ԏ�̐ݒu
        //parking_obj = Instantiate(PrkingObjectPrefab, new Vector3(5, -3, 0), Quaternion.identity);
        ////���ԏ�̍��W�擾
        //Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);

        //Debug.Log("�~�n�Ƌ��Z�n" + JudgeLayer(site_positons, residencepositons_world));
        //Debug.Log("�~�n�ƒ��ԏ�" + JudgeLayer(site_positons, parking_obj_positons_world));
        //Debug.Log("�Z���ƒ��ԏ�" + JudgeLayer(residencepositons_world, parking_obj_positons_world));
    }

    /// <summary>
    /// �~�n�̍��W�̍ő�ƍŏ�����͈͂��o���֐�
    /// </summary>
    /// <param name="sitepos">�~�n�}�`�̍��W�W��</param>
    void GetRangeSite(Vector3[] sitepos) {
        SiteMinPos = sitepos[0];
        SiteMaxPos = sitepos[0];
        for (int i = 0; i<sitepos.Length;i++) {
            //x���W�ɂ��čŏ��ő�̎擾
            if (sitepos[i].x < SiteMinPos.x) {
                SiteMinPos.x = sitepos[i].x;
            }else if (sitepos[i].x > SiteMaxPos.x) {
                SiteMaxPos.x = sitepos[i].x;
            }
            
            //y���W�ɂ��čő�ŏ��̎擾
            if (sitepos[i].y < SiteMinPos.y) {
                SiteMinPos.y = sitepos[i].y;
            }else if (sitepos[i].y > SiteMaxPos.y) {
                SiteMaxPos.y = sitepos[i].y;
            }
        }
    }


    /// <summary>
    /// �Z���n��~�n���Ƀ����_���ɔz�u����
    /// �����ɐݒu�������ǂ�ǂ񔽉f���Ă����`�ɂȂ邩�Ǝv���܂��D
    /// </summary>
    /// <returns></returns>
    Vector3[] PutResidence() {


        //���Z�n�̍ő啝�C�ő�c���擾
        Vector3[] ResidencePrefab_pos = GetWorldLinepositons(ResidenceObjectPrefab);

        //�Z���n�̕���Max��Min���擾���C���̓��e�������_���͈͂̏����ɑg�ݍ��ނׂ�
        int Random_x = Random.Range((int)SiteMinPos.x, (int)SiteMaxPos.x - Get_MaxWidth(ResidencePrefab_pos));
        int Random_y = Random.Range((int)SiteMinPos.y + Get_MaxHeight(ResidencePrefab_pos), (int)SiteMaxPos.y);
        //���Z�n�}�`�̐ݒu
        Vector3 randampos = new Vector3(Random_x, Random_y, 0);



        //�~�n���ɓ���܂ŌJ��Ԃ�
        Vector3[] residencepositons_world = new Vector3[ResidenceObjectPrefab.GetComponent<LineRenderer>().positionCount];
        for (int i = 0; i<15;i++) {
            residence_obj = Instantiate(ResidenceObjectPrefab, randampos, Quaternion.identity);
            Vector3[] residencepositons_temp = GetWorldLinepositons(residence_obj);

            if (JudgeLayer(GetWorldLinepositons(SiteObject), residencepositons_temp).Equals(FigStatus_inside)) {
                Debug.Log("����");
                residencepositons_world = residencepositons_temp;
                return residencepositons_world;
            }
            else {
                Destroy(residence_obj);
            }
        }
        
        return residencepositons_world;
    }


    void PutParking() {

        Vector3[] ParkingePrefab_pos = GetWorldLinepositons(PrkingObjectPrefab);

        //�Z���n�̕���Max��Min���擾���C���̓��e�������_���͈͂̏����ɑg�ݍ��ނׂ�
        int Random_x = Random.Range((int)SiteMinPos.x, (int)SiteMaxPos.x - Get_MaxWidth(ParkingePrefab_pos));
        int Random_y = Random.Range((int)SiteMinPos.y + Get_MaxHeight(ParkingePrefab_pos), (int)SiteMaxPos.y);
        Vector3 randampos = new Vector3(Random_x, Random_y, 0);

        ////���ԏ�̐ݒu
        //parking_obj = Instantiate(PrkingObjectPrefab, randampos, Quaternion.identity);
        ////���ԏ�̍��W�擾
        //Vector3[] parking_obj_positons_world = GetWorldLinepositons(parking_obj);


        //�~�n���ɓ���܂ŌJ��Ԃ�
        for (int i = 0; i < 15; i++) {
            parking_obj = Instantiate(PrkingObjectPrefab, randampos, Quaternion.identity);
            Vector3[] parking_obj_positons_temp = GetWorldLinepositons(parking_obj);

            if (JudgeLayer(GetWorldLinepositons(SiteObject), parking_obj_positons_temp).Equals(FigStatus_inside) && JudgeLayer(GetWorldLinepositons(residence_obj), parking_obj_positons_temp).Equals(FigStatus_outside)) {
                Debug.Log("�O��");
                //parking_obj_positons_world = parking_obj_positons_temp;
                return;
            }
            else {
                Destroy(parking_obj);
            }
        }

    }

    public void Button_PutResidence() {
        Destroy(residence_obj);
        PutResidence();
    }
    
    public void Button_PutParking() {
        Destroy(parking_obj);
        PutParking();
    }

    /// <summary>
    /// LineRender�̃I�u�W�F�N�g�̍ő�̕��̎擾
    /// </summary>
    /// <param name="target">�I�u�W�F�N�g�̏W�����W</param>
    /// <returns></returns>
    int Get_MaxWidth(Vector3[] target) {
        Vector2 target_MinPos = target[0];
        Vector2 target_MaxPos = target[0];
        for (int i = 0; i < target.Length; i++) {
            //x���W�ɂ��čŏ��ő�̎擾
            if (target[i].x < target_MinPos.x) {
                target_MinPos.x = target[i].x;
            }
            else if (target[i].x > target_MaxPos.x) {
                target_MaxPos.x = target[i].x;
            }
        }
        return Mathf.Abs((int)(target_MaxPos.x - target_MinPos.x));
    }
    
    /// <summary>
    /// LineRender�̃I�u�W�F�N�g�̍ő�̍����̎擾
    /// </summary>
    /// <param name="target">�I�u�W�F�N�g�̏W�����W</param>
    /// <returns></returns>
    int Get_MaxHeight(Vector3[] target) {
        Vector2 target_MinPos = target[0];
        Vector2 target_MaxPos = target[0];
        for (int i = 0; i < target.Length; i++) {
            //x���W�ɂ��čŏ��ő�̎擾
            if (target[i].y < target_MinPos.y) {
                target_MinPos.y = target[i].y;
            }
            else if (target[i].y > target_MaxPos.y) {
                target_MaxPos.y = target[i].y;
            }
        }
        return Mathf.Abs((int)(target_MaxPos.y - target_MinPos.y));
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
                //Debug.Log(i + " " + flag_side + " " +  CheckPoint(A, target[i]) + " " + target[i]);
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

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
    [SerializeField] GameObject ResidenceObject;
    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // �~�n�}�`�̍��W�擾
        LineRenderer site_linerender = SiteObject.GetComponent<LineRenderer>();
        site_linerender.useWorldSpace = false;
        Vector3[] site_positons = new Vector3[site_linerender.positionCount];
        site_linerender.GetPositions(site_positons);

        //���Z�n�}�`�̍��W�擾
        GameObject residence_obj = Instantiate(ResidenceObjectPrefab, new Vector3(1, -20, 0), Quaternion.identity);
        LineRenderer residencepf_linerender = residence_obj.GetComponent<LineRenderer>();
        residencepf_linerender.useWorldSpace = false;
        Vector3[] residence_positions = new Vector3[residencepf_linerender.positionCount];
        residencepf_linerender.GetPositions(residence_positions);

        Vector3[] residencepositons_world = new Vector3[residencepf_linerender.positionCount];
        for (int i = 0; i<residencepositons_world.Length; i++) {
            residencepositons_world[i] = residence_positions[i] + residence_obj.transform.position;
            Debug.Log(residence_positions[i] + " => " + residencepositons_world[i]);
        }


        bool flag = false;
        for (int i = 0; i< residencepositons_world.Length;i++) {
            Vector3[] residence_side = new Vector3[2];
            if (i+1 == residencepositons_world.Length) {//�I�_
                residence_side[0] = residencepositons_world[i];
                residence_side[1] = residencepositons_world[0];
            }
            else {
                residence_side[0] = residence_positions[i];
                residence_side[1] = residence_positions[i+1];
            }
            for (int j = 0;j<site_positons.Length ;j++) {
                Vector3[] site_side = new Vector3[2];
                if (j+1 == site_positons.Length) {//�I�_
                    site_side[0] = site_positons[j];
                    site_side[1] = site_positons[0];
                    Debug.Log("�~�n�I�_���m" + j);
                }
                else {
                    site_side[0] = site_positons[j];
                    site_side[1] = site_positons[j+1];
                }

                if ((flag = Judge(site_side, residence_side))) {
                    Debug.Log("�d�Ȃ茟�o");
                    break;
                }
            }
            if (flag) {
                break;
            }
        }
        Debug.Log(flag);
    }





    bool Judge(Vector3[] site_side, Vector3[] residence_side) {
        bool flag = false;

        var tc1 = (site_side[0].x - site_side[1].x) * (residence_side[0].y - site_side[0].y) + (site_side[0].y - site_side[1].y) * (site_side[0].x - residence_side[0].x);
        var tc2 = (site_side[0].x - site_side[1].x) * (residence_side[1].y - site_side[0].y) + (site_side[0].y - site_side[1].y) * (site_side[0].x - residence_side[1].x);

        var td1 = (residence_side[0].x - residence_side[1].x) * (site_side[0].y - residence_side[0].y) + (residence_side[0].y - residence_side[1].y) * (residence_side[0].x - site_side[0].x);
        var td2 = (residence_side[0].x - residence_side[1].x) * (site_side[1].y - residence_side[0].y) + (residence_side[0].y - residence_side[1].y) * (residence_side[0].x - site_side[1].x);



        if (tc1 * tc2 <= 0 && td1*td2 <= 0){
            flag = true;
            Debug.Log("�d�Ȃ肠��" + residence_side[0] + " " + residence_side[1] + "||" + site_side[0] + " " + site_side[1]);
        }
        else {
            Debug.Log("�d�Ȃ�Ȃ�" + residence_side[0] + " " + residence_side[1] + "||" + site_side[0] + " " + site_side[1]);
        }

        return flag;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

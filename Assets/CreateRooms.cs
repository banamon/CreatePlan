using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRooms : MonoBehaviour
{

    [SerializeField] GameObject SiteObjectPrefab;
    [SerializeField] GameObject Comonspace_Pfb;

    Vector3[] site_positons;



    // Start is called before the first frame update
    void Start()
    {
        
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
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRooms : MonoBehaviour
{

    [SerializeField] GameObject ResidenceObject;
    [SerializeField] GameObject Comonspace_Pfb;

    Vector3[] residence_positons;
    Vector3[] commonspace_positons;



    // Start is called before the first frame update
    void Start()
    {
        residence_positons = GetWorldLinepositons(ResidenceObject);

        ////共有スペースの配置
        //GameObject commonspace_obj = Instantiate(Comonspace_Pfb, Vector3.zero, Quaternion.identity);
        //commonspace_obj.transform.SetParent(ResidenceObject.transform, true);
        //commonspace_positons = GetWorldLinepositons(commonspace_obj);
        //Vector3 commonspace_position = (residence_positons[3] + residence_positons[0]) / 2;
        //commonspace_position.x -= (commonspace_positons[3].x - commonspace_positons[0].x) / 2;
        //commonspace_obj.transform.position = commonspace_position;


        Debug.Log(Calc_areasize(residence_positons));
        Debug.Log(Calc_areasize(commonspace_positons));
    }

    /// <summary>
    /// ワールド座標の図形座標集合の取得
    /// </summary>
    /// <param name="fig">取得したいGameObject</param>
    /// <returns>ワールド座標集合</returns>
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
    /// 図形の面積計算
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
}

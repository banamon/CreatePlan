using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script {
    internal class Vector3Utils : MonoBehaviour {

        /// <summary>
        /// 図形の面積計算
        /// </summary>
        /// <param name="positons"></param>
        /// <returns></returns>
        public static long Calc_areasize(Vector3[] pos) {
            long area = 0;
            for (int i = 0; i < pos.Length; i++) {
                if (i == pos.Length - 1) {

                    area += (long)(pos[i].x * pos[0].y - pos[i].y * pos[0].x) / 2;
                }
                else {
                    area += (long)(pos[i].x * pos[i + 1].y - pos[i].y * pos[i + 1].x) / 2;

                }
            }
            return Math.Abs(area);
        }

        /// <summary>
        /// ワールド座標の図形座標集合の取得
        /// </summary>
        /// <param name="fig">取得したいGameObject</param>
        /// <returns>ワールド座標集合</returns>
        public static Vector3[] GetWorldLinepositons(GameObject fig_obj) {
            LineRenderer fig_linerender = fig_obj.GetComponent<LineRenderer>();
            Vector3[] fig_positons = new Vector3[fig_linerender.positionCount];
            Vector3[] fig_positons_world = new Vector3[fig_linerender.positionCount];
            fig_linerender.GetPositions(fig_positons);

            //Debug.Log("ワールド座標" + fig_obj.transform.position);

            for (int i = 0; i < fig_positons_world.Length; i++) {
                fig_positons_world[i] = fig_positons[i] + fig_obj.transform.position;
            }
            return fig_positons_world;
        }


        /// <summary>
        /// 座標集合を渡すとオブジェクトを描写
        /// </summary>
        /// <param name="boxpos"></param>
        public static GameObject DrowLine(Vector3[] boxlinepos, Vector3 boxpos, GameObject Preafb) {
            /*
             * 
             * boxlineposはローカル座標で，原点0にすべきなのか？？
             * 
             */

            GameObject newRoom = Instantiate(Preafb, boxpos, Quaternion.identity);

            // LineRendererコンポーネントをゲームオブジェクトにアタッチする
            var lineRenderer = newRoom.GetComponent<LineRenderer>();

            var positions = new Vector3[boxlinepos.Length];
            for (int i = 0; i < boxlinepos.Length; i++) {
                positions[i] = boxlinepos[i];
                //positions[i] = boxlinepos[i] - boxlinepos[0];
            }


            // 点の数を指定する
            lineRenderer.positionCount = positions.Length;
            lineRenderer.loop = true;
            lineRenderer.useWorldSpace = false;     // ローカル座標

            // 線を引く場所を指定する
            lineRenderer.SetPositions(positions);

            return newRoom;
        }

        /// <summary>
        /// 座標集合を渡すとオブジェクトを描写
        /// </summary>
        /// <param name="boxpos"></param>
        public static GameObject DrowLine(Vector3[] boxlinepos, Vector3 boxpos,string boxname) {
            /*
             * 
             * boxlineposはローカル座標で，原点0にすべきなのか？？
             * 
             */

            //GameObject newRoom = Instantiate(Preafb, boxpos, Quaternion.identity);
            GameObject newRoom = new GameObject();
            newRoom.transform.position = boxpos;
            newRoom.name = boxname;

            // LineRendererコンポーネントをゲームオブジェクトにアタッチする
            var lineRenderer = newRoom.AddComponent< LineRenderer>();

            var positions = new Vector3[boxlinepos.Length];
            for (int i = 0; i < boxlinepos.Length; i++) {
                positions[i] = boxlinepos[i];
                //positions[i] = boxlinepos[i] - boxlinepos[0];
            }


            // 点の数を指定する
            lineRenderer.positionCount = positions.Length;
            lineRenderer.loop = true;
            lineRenderer.useWorldSpace = false;     // ローカル座標

            // 線を引く場所を指定する
            lineRenderer.SetPositions(positions);

            return newRoom;
        }
    }
}

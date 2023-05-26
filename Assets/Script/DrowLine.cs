using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowLine : MonoBehaviour
{
    [SerializeField] int x;
    [SerializeField] int y;

    public void SetXY(int _x, int _y) {
        x = _x;
        y = _y;
    }

    public void DrowLines() {
        // LineRendererコンポーネントをゲームオブジェクトにアタッチする
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // 開始点
            new Vector3(x, 0, 0),
            new Vector3(x, -y, 0),
            new Vector3(0, -y, 0),
        };

        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;

        // 線を引く場所を指定する
        lineRenderer.SetPositions(positions);
    }
}

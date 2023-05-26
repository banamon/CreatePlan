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
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = gameObject.GetComponent<LineRenderer>();

        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // �J�n�_
            new Vector3(x, 0, 0),
            new Vector3(x, -y, 0),
            new Vector3(0, -y, 0),
        };

        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }
}

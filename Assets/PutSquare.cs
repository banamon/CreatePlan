using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutSquare : MonoBehaviour
{

    LineRenderer linerend;


    // Start is called before the first frame update
    void Start()
    {
        // LineRenderer�R���|�[�l���g���Q�[���I�u�W�F�N�g�ɃA�^�b�`����
        var lineRenderer = gameObject.AddComponent<LineRenderer>();

        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // �J�n�_
            new Vector3(8, 0, 0),               
            new Vector3(8, 8, 0),               
            new Vector3(0, 8, 0),               
            new Vector3(0, 0, 0),              // �I���_
        }; 

        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);

    }

}

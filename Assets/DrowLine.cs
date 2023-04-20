using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrowLine : MonoBehaviour
{
    [SerializeField] GameObject SiteObject;
    [SerializeField] GameObject ResidenceObject;
    LineRenderer linerend;


    // Start is called before the first frame update
    void Start()
    {
        DrowSite();
        DroweResidence();
    }


    //�~�n�̕`��
    void DrowSite() {
        var lineRenderer = SiteObject.AddComponent<LineRenderer>();


        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // �J�n�_
            new Vector3(0, -116, 0),            
            new Vector3(117, -116, 0),            
            new Vector3(117, -150, 0),            
            new Vector3(140, -150, 0),            
            new Vector3(140, -100, 0),            
            new Vector3(220, -100, 0),            
            new Vector3(220, 0, 0),            
            new Vector3(0,  0, 0),              
        };

        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;

        //�}�e���A���̐ݒ�
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //�F���w�肷��
        lineRenderer.SetColors(Color.white, Color.white);


        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }
    
    
    //���Z�n�̕`��
    void DroweResidence() {
        var lineRenderer = ResidenceObject.AddComponent<LineRenderer>();


        var positions = new Vector3[]{
            new Vector3(0, 0, 0),               // �J�n�_
            new Vector3(0, -50, 0),                    
            new Vector3(80, -50, 0),            
            new Vector3(80, 0, 0),            
            new Vector3(0,  0, 0),              
        };

        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;

        //�}�e���A���̐ݒ�
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //�F���w�肷��
        lineRenderer.SetColors(Color.white, Color.white);


        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

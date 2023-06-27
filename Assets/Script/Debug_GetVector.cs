using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Debug_GetVector : MonoBehaviour
{

    // StreamingAssets�t�H���_���Ȃ̂ŁA�v���b�g�t�H�[��������Ă��ȉ��̃p�X�w��ő��v
    private static readonly string FolderPath = Application.streamingAssetsPath;
    private static readonly string FilePath_exe = FolderPath + "/dxftest.exe";
    private static readonly string FilePath_json = FolderPath + "/dxf_data.txt";
    // Start is called before the first frame update

    [SerializeField]
    public GameObject LineObjectPrefab;

    //private Process _process;

    void Start()
    {
        ProcessStartInfo processInfo = new ProcessStartInfo {
            FileName = FilePath_exe,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        Process process = Process.Start(processInfo);
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();

        // JSON�z�� �� C#�z��ւ̕ϊ�
        dxf[] dxfs = JsonHelper.FromJson<dxf>(output);
        // netDef��Vector2����Unity��Vector2�ɕϊ�
        foreach(dxf d in dxfs) {
            d.startVec = new Vector3(d.dxf_startPoint.X, d.dxf_startPoint.Y,0);
            d.endVec = new Vector3(d.dxf_endPoint.X, d.dxf_endPoint.Y,0);
            Debug.Log(d.dxf_entity + " " + d.startVec + " " + d.endVec);
            DrowLine(d);
        }
    }
    public void DrowLine(dxf dxf) {

        var lineobj = Instantiate(LineObjectPrefab, Vector3.zero, Quaternion.identity);


        var positions = new Vector3[]{
            dxf.startVec,
            dxf.endVec
        };

        var lineRenderer = lineobj.GetComponent<LineRenderer>();
        // �_�̐����w�肷��
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;

        lineRenderer.startWidth = 0.1f;                   // �J�n�_�̑�����0.1�ɂ���
        lineRenderer.endWidth = 0.1f;                     // �I���_�̑�����0.1�ɂ���

        // ���������ꏊ���w�肷��
        lineRenderer.SetPositions(positions);
    }


}




[Serializable]
public class dxf {
    public string dxf_entity;
    public netdxf_Vector2 dxf_startPoint;
    public netdxf_Vector2 dxf_endPoint;
    public Vector3 startVec;
    public Vector3 endVec;
}


[Serializable]
public class netdxf_Vector2 {
    public int X;
    public int Y;
    public bool IsNormalized;
}

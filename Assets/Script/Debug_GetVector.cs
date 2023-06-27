using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Debug_GetVector : MonoBehaviour
{

    // StreamingAssetsフォルダ内なので、プラットフォームが違っても以下のパス指定で大丈夫
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

        // JSON配列 → C#配列への変換
        dxf[] dxfs = JsonHelper.FromJson<dxf>(output);
        // netDefのVector2からUnityのVector2に変換
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
        // 点の数を指定する
        lineRenderer.positionCount = positions.Length;
        lineRenderer.loop = true;

        lineRenderer.startWidth = 0.1f;                   // 開始点の太さを0.1にする
        lineRenderer.endWidth = 0.1f;                     // 終了点の太さを0.1にする

        // 線を引く場所を指定する
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Script;

public class CreatePlan : MonoBehaviour {
    [SerializeField] GameObject landform;
    [SerializeField] GameObject ResPreafb;
    GetResRange getResRange;
    PutRes putres;

    // Start is called before the first frame update
    void Start()
    {
        getResRange = new GetResRange();
        putres = new PutRes();



        //地型座標ベクトル取得
        Vector3[] landrormvec = Vector3Utils.GetWorldLinepositons(landform);
        int[] distances = new int[landrormvec.Length];
        for (int i = 0; i < landrormvec.Length; i++) {
            if (i == 2) {
                distances[i] = 80;
            }
            else {
                distances[i] = 15;
            }
        }

        Vector3[] ResRangevecs = getResRange.GetPossibleArea(landrormvec, distances);

        //GameObject range =  Vector3Utils.DrowLine(getResRange.GetPossibleArea(landrormvec, distances), Vector3.zero, ResPreafb);

        Vector3Utils.DrowLine(ResRangevecs, Vector3.zero, ResPreafb);

        long landare = Vector3Utils.Calc_areasize(landrormvec);
        Vector3[] ResAreavecs = putres.GetResArea(ResRangevecs, (long)(landare * 0.5));

        Vector3Utils.DrowLine(ResAreavecs, Vector3.zero, ResPreafb);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

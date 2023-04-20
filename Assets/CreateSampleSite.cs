using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSampleSite : MonoBehaviour {

    [SerializeField] SpriteRenderer testSprite;


    // Start is called before the first frame update
    void Start() {
        Debug.Log("四角のサイズは " + testSprite.bounds.size + " です"); // 四角のサイズは (1.0, 2.0, 0.2) です
        Debug.Log("四角の横の長さは " + testSprite.bounds.size.x + " です"); // 四角の横の長さは 1 です
        Debug.Log("四角の縦の長さは " + testSprite.bounds.size.y + " です"); // 四角の縦の長さは 2 です
        Debug.Log("中心からの距離は " + testSprite.bounds.extents + " です");//中心からの距離は (0.5, 1.0, 0.1) です
        Debug.Log("中心の座標は " + testSprite.bounds.center + " です");//中心の座標は (-0.5, 0.2, 0.0) です
        Debug.Log("右上の座標は " + testSprite.bounds.max + " です");//右上の座標は (0.0, 1.2, 0.1) です
        Debug.Log("左下の座標は " + testSprite.bounds.min + " です");//左下の座標は (-1.0, -0.8, -0.1) です    
        Debug.Log("面積" + testSprite.bounds.size.x * testSprite.bounds.size.y);
    }
}

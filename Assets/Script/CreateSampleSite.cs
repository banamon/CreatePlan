using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSampleSite : MonoBehaviour {

    [SerializeField] SpriteRenderer testSprite;


    // Start is called before the first frame update
    void Start() {
        Debug.Log("�l�p�̃T�C�Y�� " + testSprite.bounds.size + " �ł�"); // �l�p�̃T�C�Y�� (1.0, 2.0, 0.2) �ł�
        Debug.Log("�l�p�̉��̒����� " + testSprite.bounds.size.x + " �ł�"); // �l�p�̉��̒����� 1 �ł�
        Debug.Log("�l�p�̏c�̒����� " + testSprite.bounds.size.y + " �ł�"); // �l�p�̏c�̒����� 2 �ł�
        Debug.Log("���S����̋����� " + testSprite.bounds.extents + " �ł�");//���S����̋����� (0.5, 1.0, 0.1) �ł�
        Debug.Log("���S�̍��W�� " + testSprite.bounds.center + " �ł�");//���S�̍��W�� (-0.5, 0.2, 0.0) �ł�
        Debug.Log("�E��̍��W�� " + testSprite.bounds.max + " �ł�");//�E��̍��W�� (0.0, 1.2, 0.1) �ł�
        Debug.Log("�����̍��W�� " + testSprite.bounds.min + " �ł�");//�����̍��W�� (-1.0, -0.8, -0.1) �ł�    
        Debug.Log("�ʐ�" + testSprite.bounds.size.x * testSprite.bounds.size.y);
    }
}

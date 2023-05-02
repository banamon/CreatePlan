using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudgeInsideScirpt : MonoBehaviour
{

    [SerializeField] GameObject SiteObject;
    Vector3[] site_positions;

    // Start is called before the first frame update
    void Start()
    {
        var site_lines = SiteObject.GetComponent<LineRenderer>();
        site_positions = new Vector3[site_lines.positionCount];
        site_lines.GetPositions(site_positions);
        for (int i = 0; i<site_positions.Length;i++) {
        Debug.Log(site_positions[i]);

        }
    }

    // Update is called once per frame
    void Update()
    {
        // �J�[�\���ʒu���擾
        Vector3 mousePosition = Input.mousePosition;
        // �J�[�\���ʒu��z���W��10��
        mousePosition.z = 10;
        // �J�[�\���ʒu�����[���h���W�ɕϊ�
        Vector3 target = Camera.main.ScreenToWorldPoint(mousePosition);

        Debug.Log(Check(site_positions,target));
    }

    static public bool Check(Vector3[] points, Vector3 target) {
        Vector3 normal = new Vector3(1f, 0f, 0f);
        //Vector3 normal = Vector3.up;//(0, 1, 0)
        // XY���ʏ�Ɏʑ�������ԂŌv�Z���s��
        Quaternion rot = Quaternion.FromToRotation(normal, -Vector3.forward);

        Vector3[] rotPoints = new Vector3[points.Length];

        for (int i = 0; i < rotPoints.Length; i++) {
            rotPoints[i] = rot * points[i];
        }

        target = rot * target;

        int wn = 0;
        float vt = 0;

        for (int i = 0; i < rotPoints.Length; i++) {
            // ������̕ӁA�������̕ӂɂ���ď����𕪂���

            int cur = i;
            int next = (i + 1) % rotPoints.Length;

            // ������̕ӁB�_P��Y�������ɂ��āA�n�_�ƏI�_�̊Ԃɂ���B�i�������A�I�_�͊܂܂Ȃ��j
            if ((rotPoints[cur].y <= target.y) && (rotPoints[next].y > target.y)) {
                // �ӂ͓_P�����E���ɂ���B�������d�Ȃ�Ȃ�
                // �ӂ��_P�Ɠ��������ɂȂ�ʒu����肵�A���̎���X�̒l�Ɠ_P��X�̒l���r����
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // ������̕ӂƌ��������ꍇ��+1
                    wn++;
                }
            }
            else if ((rotPoints[cur].y > target.y) && (rotPoints[next].y <= target.y)) {
                // �ӂ͓_P�����E���ɂ���B�������d�Ȃ�Ȃ�
                // �ӂ��_P�Ɠ��������ɂȂ�ʒu����肵�A���̎���X�̒l�Ɠ_P��X�̒l���r����
                vt = (target.y - rotPoints[cur].y) / (rotPoints[next].y - rotPoints[cur].y);

                if (target.x < (rotPoints[cur].x + (vt * (rotPoints[next].x - rotPoints[cur].x)))) {
                    // �������̕ӂƌ��������ꍇ��-1
                    wn--;
                }
            }
        }

        return wn != 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalcLimit : MonoBehaviour
{
    //�~�n�ʐ�
    [SerializeField] InputField sitesize_input;    
    //�ː�
    [SerializeField] InputField housenum_input;
    //�K��
    [SerializeField] InputField floornum_input;    
    //���L�X�y�[�X�ʐ�
    [SerializeField] InputField commonarea_size_input;  
    //������
    [SerializeField] InputField BuildingCoverageRatio_input;
    
    //���ԏ�
    int parkingnum;
    double defaltparkingsize = 5f * 2.5f;
    double parkingsize;
    //�����^�C�v
    string roomtype = "";
    //���ݔ��ʐ�
    double dustboxsize;
    //�Βn�ʐ�
    double greenspasesize;
    //���]�Ԓu����ʐ�
    double bicycleparkingsize;
    double defaltbicycleparkingsize=0.5*2;
    //�܂��c���Ă���ʐ�
    double allsitesize;

    public void StartCalc() {
        float sitesize = float.Parse(sitesize_input.text);
        int housenum = int.Parse(housenum_input.text);
        int floornum = int.Parse(floornum_input.text);
        int commonarea_size = int.Parse(commonarea_size_input.text);
        double BuildingCoverageRatio = double.Parse(BuildingCoverageRatio_input.text) / 100;
        allsitesize = sitesize;
        double floorsize = sitesize * BuildingCoverageRatio;

        Debug.Log("--------------���؊J�n--------------");
        Debug.Log(
            "�~���F" + sitesize + "�u" +
            "�������F" + BuildingCoverageRatio * 100 + "%" +
            "��]�ː�" + housenum + "��");

        
        
        if (housenum % floornum != 0) {
            Debug.Log("1�K�w�̌ː�" + housenum / floornum);
            return;
        }


        double housesize = (floorsize - commonarea_size) / (int)(housenum / floornum);

        if (housesize < 30) {
            roomtype = "1R";
        }
        else {
            roomtype = "FR";
        }

        //���ݔ��v�Z
        dustboxsize = calc_dustboxsize(roomtype,housenum);

        //���]�Ԓu����v�Z
        bicycleparkingsize = calc_bicycleparkingsize(roomtype);

        //�Βn�v�Z
        greenspasesize = calc_greenspasesize(sitesize);

        //���ԏ�v�Z
        parkingnum = calc_parkingnum(roomtype, housesize, housenum);
        parkingsize = parkingnum * defaltparkingsize;


        //�ʐς���v�������쐬�\���ǂ������f����
        Debug.Log(floorsize + parkingsize + dustboxsize + bicycleparkingsize + greenspasesize);
        allsitesize = allsitesize - (floorsize + parkingsize+ dustboxsize + bicycleparkingsize + greenspasesize);
        if (allsitesize > 0) {
            Debug.Log("�v�����쐬�\");
        }
        else {
            Debug.Log("�v�����쐬�s�\�F�Œ���K�v�Ȓ��ԏ�̃X�y�[�X������܂���" + allsitesize);
            return;
        }

        Debug.Log("�����^�C�v�F" + roomtype);
        Debug.Log("��˓�����̍L���F" + housesize);
        Debug.Log("���ԏ�䐔�F" + parkingnum);
        Debug.Log("���ԏ�ʐρF" + parkingsize);
        Debug.Log("�S�~�̂ď�ʐρF" + dustboxsize);
        Debug.Log("���֏�ʐρF" + bicycleparkingsize);
        Debug.Log("�Βn�ʐ�" + greenspasesize);
    }


    //���ԏ�̑䐔�v�Z
    public int calc_parkingnum(string roomtype, double housesize, int housenum) {
        int parking = 0;
        if (roomtype.Equals("1R")) {
            parking = (int) Math.Round(housenum * 0.45);
        }
        else if(roomtype.Equals("FR")) {
            parking = (int) Math.Round(housenum * 0.8);
        }

        parking -= (int)Math.Floor(parking * 0.2);

        return parking;
    }

    //�S�~�̂ď�ʐόv�Z
    public double calc_dustboxsize(string roomtype,int housenum) {
        double dustboxsize = 0;

        if (5<=housenum && housenum <= 9) {
            dustboxsize = 3;//������2�u�C��������1�u
        }else if (10 <= housenum && housenum <= 19) {
            dustboxsize = 4;//������2.5�u�C��������1.5�u
        }else if (20<=housenum) {
            dustboxsize = 1.28 * 0.666 * (housenum/10 + housenum/15);
            return dustboxsize;
        }

        if (roomtype.Equals("1R")) {
            dustboxsize /= 2;
        }

        return dustboxsize;
    }

    //���]�Ԓu����ʐόv�Z
    public double calc_bicycleparkingsize(string roomtype) {
        double bicycleparkingsize = 0;

        if (roomtype.Equals("1R")) {
            bicycleparkingsize = defaltbicycleparkingsize * 0.4;
        }
        else if(roomtype.Equals("FR")) {
            bicycleparkingsize = defaltbicycleparkingsize;
        }

        return bicycleparkingsize;
    }

    public double calc_greenspasesize(double sitesize) {
        return sitesize * 0.15;
    }
}

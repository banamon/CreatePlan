using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalcLimit : MonoBehaviour
{
    //敷地面積
    [SerializeField] InputField sitesize_input;    
    //戸数
    [SerializeField] InputField housenum_input;
    //階数
    [SerializeField] InputField floornum_input;    
    //共有スペース面積
    [SerializeField] InputField commonarea_size_input;  
    //建蔽率
    [SerializeField] InputField BuildingCoverageRatio_input;
    
    //駐車場
    int parkingnum;
    double defaltparkingsize = 5f * 2.5f;
    double parkingsize;
    //部屋タイプ
    string roomtype = "";
    //ごみ箱面積
    double dustboxsize;
    //緑地面積
    double greenspasesize;
    //自転車置き場面積
    double bicycleparkingsize;
    double defaltbicycleparkingsize=0.5*2;
    //まだ残っている面積
    double allsitesize;

    public void StartCalc() {
        float sitesize = float.Parse(sitesize_input.text);
        int housenum = int.Parse(housenum_input.text);
        int floornum = int.Parse(floornum_input.text);
        int commonarea_size = int.Parse(commonarea_size_input.text);
        double BuildingCoverageRatio = double.Parse(BuildingCoverageRatio_input.text) / 100;
        allsitesize = sitesize;
        double floorsize = sitesize * BuildingCoverageRatio;

        Debug.Log("--------------検証開始--------------");
        Debug.Log(
            "敷居：" + sitesize + "㎡" +
            "建蔽率：" + BuildingCoverageRatio * 100 + "%" +
            "希望戸数" + housenum + "戸");

        
        
        if (housenum % floornum != 0) {
            Debug.Log("1階層の戸数" + housenum / floornum);
            return;
        }


        double housesize = (floorsize - commonarea_size) / (int)(housenum / floornum);

        if (housesize < 30) {
            roomtype = "1R";
        }
        else {
            roomtype = "FR";
        }

        //ごみ箱計算
        dustboxsize = calc_dustboxsize(roomtype,housenum);

        //自転車置き場計算
        bicycleparkingsize = calc_bicycleparkingsize(roomtype);

        //緑地計算
        greenspasesize = calc_greenspasesize(sitesize);

        //駐車場計算
        parkingnum = calc_parkingnum(roomtype, housesize, housenum);
        parkingsize = parkingnum * defaltparkingsize;


        //面積からプランが作成可能かどうか判断する
        Debug.Log(floorsize + parkingsize + dustboxsize + bicycleparkingsize + greenspasesize);
        allsitesize = allsitesize - (floorsize + parkingsize+ dustboxsize + bicycleparkingsize + greenspasesize);
        if (allsitesize > 0) {
            Debug.Log("プラン作成可能");
        }
        else {
            Debug.Log("プラン作成不可能：最低限必要な駐車場のスペースがありません" + allsitesize);
            return;
        }

        Debug.Log("部屋タイプ：" + roomtype);
        Debug.Log("一戸当たりの広さ：" + housesize);
        Debug.Log("駐車場台数：" + parkingnum);
        Debug.Log("駐車場面積：" + parkingsize);
        Debug.Log("ゴミ捨て場面積：" + dustboxsize);
        Debug.Log("駐輪場面積：" + bicycleparkingsize);
        Debug.Log("緑地面積" + greenspasesize);
    }


    //駐車場の台数計算
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

    //ゴミ捨て場面積計算
    public double calc_dustboxsize(string roomtype,int housenum) {
        double dustboxsize = 0;

        if (5<=housenum && housenum <= 9) {
            dustboxsize = 3;//生ごみ2㎡，資源ごみ1㎡
        }else if (10 <= housenum && housenum <= 19) {
            dustboxsize = 4;//生ごみ2.5㎡，資源ごみ1.5㎡
        }else if (20<=housenum) {
            dustboxsize = 1.28 * 0.666 * (housenum/10 + housenum/15);
            return dustboxsize;
        }

        if (roomtype.Equals("1R")) {
            dustboxsize /= 2;
        }

        return dustboxsize;
    }

    //自転車置き場面積計算
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

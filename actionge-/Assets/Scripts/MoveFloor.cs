﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//動く床のスクリプト(挙動は動いては戻るを繰り返す)
public class MoveFloor : MonoBehaviour {

    public Vector3 speed = Vector3.zero;                     //床の動くスピード(1フレームで動く距離)
    public Vector3 distance = Vector3.zero;                  //動く距離の上限

    private Vector3 moved = Vector3.zero;                    //移動した距離の保持
    private List<GameObject> ride = new List<GameObject>();  //床に乗っているオブジェクト




	// Update is called once per frame
	void Update () {
	    
        //動くスピード取得
        float x = speed.x;
        float y = speed.y;
        float z = speed.z;

        //x軸に関する動き
        if (moved.x >= distance.x){
            x = 0;
        }
        else if(moved.x + speed.x > distance.x){
            x = distance.x - moved.x;
        }

        //y軸に関する動き
        if (moved.y >= distance.y)
        {
            y = 0;
        }
        else if (moved.y + speed.y > distance.y)
        {
            y = distance.y - moved.y;
        }

        //z軸に関する動き
        if (moved.z >= distance.z)
        {
            z = 0;
        }
        else if (moved.z + speed.z > distance.z)
        {
            z = distance.z - moved.z;
        }

        //反映
        transform.Translate(x, y, z);

        //動いた距離を保存
        moved.x += Mathf.Abs(speed.x);
        moved.y += Mathf.Abs(speed.y);
        moved.z += Mathf.Abs(speed.z);

        //床の上のオブジェクトを床と連動して動かす
        foreach (GameObject g in ride){
            Vector3 v = g.transform.position;
            g.transform.position = new Vector3(v.x + x, v.y + y, v.z + z);
        }

        //折り返す処理
        if(moved.x >= distance.x && moved.y >= distance.y && moved.z >= distance.z){
            speed *= -1;
            moved = Vector3.zero;
        }

	}

    //上に乗ったオブジェクトの保存
    void OnTriggerEnter(Collider other){
        ride.Add(other.gameObject);
    }

    //床から離れたら削除
    void OnTriggerExit(Collider other){
        ride.Remove(other.gameObject);
    }
}

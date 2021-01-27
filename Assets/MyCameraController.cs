using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    private GameObject unitychan;

    private float difference; //カメラとunityちゃんの距離

    // Start is called before the first frame update
    void Start()
    {
        this.unitychan = GameObject.Find("unitychan");

        //カメラとunityちゃんの初期の距離
        this.difference = unitychan.transform.position.z - this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //unityちゃんに合わせてカメラ移動
        this.transform.position = new Vector3(
            0,
            this.transform.position.y,
            this.unitychan.transform.position.z - difference
        );
    }
}

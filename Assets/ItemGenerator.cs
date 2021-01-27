using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public GameObject carPrefab;
    public GameObject coinPrefab;
    public GameObject conePrefab;
    private GameObject unitychan;

    private int spawnNum = 0; //何回スポーンさせたか
    private int spawnInterval = 15; //距離15間隔にアイテム生成
    private int spawnDistance = 80; //unityちゃんのz前方50にアイテム生成
    private int goalPos = 360; //z360以降の位置ではスポーンしない
    private float posRange = 3.4f; //アイテムを出すx方向の範囲


    void Start()
    {
        this.unitychan = GameObject.Find("unitychan");
    }

    void Update()
    {
        this.transform.position = new Vector3(
            this.transform.position.x,
            this.transform.position.y,
            this.unitychan.transform.position.z
        );

        if ( spawnNum*spawnInterval < this.unitychan.transform.position.z )
        {
            int spawnPositionZ = spawnNum*spawnInterval + spawnDistance;
            if ( spawnPositionZ < goalPos )
            {
                ItemSpawn( spawnPositionZ );
            }
            spawnNum++;
        }
    }

    void ItemSpawn(int spawnPositionZ)
    {
        int num = Random.Range(1, 11);
        if (num <= 2) //20%の確率でコーンを横一直線に生成
        {
            for (float j = -1; j <= 1; j += 0.4f)
            {
                GameObject cone = Instantiate(conePrefab);
                cone.transform.position = new Vector3 (4 * j, cone.transform.position.y, spawnPositionZ);
            }
        }else{
            for (int  j = -1; j<=1; j++){ //3レーンにおいて1レーン毎に生成
                int item = Random.Range(1, 11);
                int offsetZ = Random.Range(-5, 6);
                //60%コイン配置、30%車配置、10%何も無し
                if ( 1 <= item && item <= 6)
                {
                    GameObject coin = Instantiate(coinPrefab);
                    coin.transform.position = new Vector3(posRange*j, coin.transform.position.y, spawnPositionZ+offsetZ);
                }else if ( 7 <= item && item <= 9 )
                {
                    GameObject car = Instantiate(carPrefab);
                    car.transform.position = new Vector3(posRange*j, car.transform.position.y, spawnPositionZ+offsetZ);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag" || other.gameObject.tag == "CoinTag")
        {
            Destroy(other.gameObject);
        }
    }
}

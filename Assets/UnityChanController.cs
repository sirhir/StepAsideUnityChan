using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour
{

    private Animator myAnimator;
    private Rigidbody myRigidbody;

    private float velocityZ = 16f; //前進速度

    //横移動関連
    private float velocityX = 10f; //横移動速度
    private float movableRange = 3.4f; //左右の移動可能範囲

    //上下移動関連
    private float velocityY = 10f; //上下移動速度

    //ゲーム終了関連
    private float coefficient = 0.99f; //動きの減速係数、ゲームオーバー時徐々に止まるように
    private bool isEnd = false;
    private GameObject stateText;

    //得点関連
    private GameObject scoreText;
    private int score = 0;

    //UIボタン押下の判定
    private bool isLButtonDown = false;
    private bool isRButtonDown = false;
    private bool isJButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        this.myAnimator = GetComponent<Animator>();
        this.myAnimator.SetFloat("Speed",1);
        this.myRigidbody = GetComponent<Rigidbody>();
        this.stateText = GameObject.Find("GameResultText");
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isEnd) //ゲーム終了時徐々に減速し停止
        {
            this.velocityZ *= this.coefficient;
            this.velocityX *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }


        float inputVelocityX = 0; //横移動速度、入力とunityちゃんの位置に応じて変更
        if ( ( Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown ) && this.transform.position.x > -this.movableRange )
        {
            inputVelocityX -= velocityX;
        }
        if ( ( Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown ) && this.transform.position.x < this.movableRange )
        {
            inputVelocityX += velocityX;
        }

        float inputVelocityY = 0; //上下移動速度、ジャンプすると上昇(下降は重力がやってくれる、現在のY軸速度を維持するのを忘れずに)
        if ( ( Input.GetKeyDown(KeyCode.Space) || this.isJButtonDown ) && this.transform.position.y < 0.5f )
        {
            this.myAnimator.SetBool("Jump", true);
            inputVelocityY = this.velocityY;
        }else{
            inputVelocityY = this.myRigidbody.velocity.y;
        }
        if ( this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump") )
        {
            this.myAnimator.SetBool ("Jump", false);
        }

        this.myRigidbody.velocity = new Vector3(inputVelocityX, inputVelocityY, this.velocityZ);
    }


    void OnTriggerEnter(Collider other)
    {
        //障害物に衝突した場合
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }

        //ゴール地点に到達した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            this.stateText.GetComponent<Text>().text = "CLEAR!!";
        }

        //コインに衝突した場合
        if (other.gameObject.tag == "CoinTag")
        {
            this.score += 10;
            this.scoreText.GetComponent<Text>().text = "Score " + this.score + "pt";
            GetComponent<ParticleSystem>().Play();
            Destroy (other.gameObject);
        }      
    }


    //UIボタンイベント関連
    public void GetMyJumpButtonDown()
    {
            this.isJButtonDown = true;
    }
    public void GetMyJumpButtonUp()
    {
            this.isJButtonDown = false;
    }
    public void GetMyLeftButtonDown()
    {
            this.isLButtonDown = true;
    }
    public void GetMyLeftButtonUp() 
    {
            this.isLButtonDown = false;
    }
    public void GetMyRightButtonDown()
    {
            this.isRButtonDown = true;
    }
    public void GetMyRightButtonUp()
    {
            this.isRButtonDown = false;
    }

}

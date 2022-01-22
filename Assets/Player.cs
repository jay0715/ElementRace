using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Player : Photon.MonoBehaviour
{
    public PhotonView photonView;
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public Text PlayerNameText;
    public bool m_FacingLeft = true;
    public bool IsWin = false;
    public RuntimeAnimatorController woodChar;
    public RuntimeAnimatorController fireChar;
    public RuntimeAnimatorController metalChar;
    public RuntimeAnimatorController WaterChar;
    //[SerializeField]
    // private LayerMask playerLayerMask;
    public float MoveSpeed = 5;
    public int timer = 3;
    Vector2 movement;
    public int characterChoice = 0;
    public int numOfPlayer;
    public Text winner;

    private void Awake()
    {
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
            PlayerNameText.color = Color.black;
        }
        else
        {
            PlayerNameText.text = photonView.owner.NickName;
            PlayerNameText.color = Color.green;
        }
    }
    private void Update()
    {
        if (photonView.isMine)
        {
            CheckInput();
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }
    private void CheckInput()
    {
        //var move = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        // transform.position += move * MoveSpeed * Time.deltaTime;
        // var moveV = new Vector3(Input.GetAxisRaw("Vertical"), 0);
        //transform.position += moveV * MoveSpeed * Time.deltaTime;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.A))
        {
            photonView.RPC("FlipxFalse", PhotonTargets.AllBuffered);
          //  m_FacingLeft = true;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            photonView.RPC("FlipxTrue", PhotonTargets.AllBuffered);
           // m_FacingLeft = false;
        }
      //  if (Input.GetKeyDown(KeyCode.W))
      //  {
      //      photonView.RPC("FlipyTrue", PhotonTargets.AllBuffered);
      //  }

        if (Input.GetKeyDown(KeyCode.S))
        {
            photonView.RPC("FlipyFalse", PhotonTargets.AllBuffered);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            photonView.RPC("RunTrue", PhotonTargets.AllBuffered);      
        }
        else
        {
            photonView.RPC("RunFalse", PhotonTargets.AllBuffered);
        }
        if(Input.GetKey(KeyCode.F) && IsWin == true)
        {

            // animTrophy.SetBool("IsPressed", true);
            StartCoroutine(WinScene(0));
        }
       

        }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Win")
        {
            Debug.Log("Win");
            // photonView.RPC("Win", PhotonTargets.AllBuffered);
            IsWin = true;

        }
        
        //Water adv
        if (collision.gameObject.tag == "SpeedUp_Water")
        {
            if (characterChoice == 4)
            {
                Debug.Log("Water Speed Up");
                MoveSpeed = 7;
                //anim.SetBool("IsSpeed_Up", true);
                photonView.RPC("Speed_UpTrue", PhotonTargets.AllBuffered);
                StartCoroutine(SpeedUpDone(3));//Not need when the floor can triger 
            }
            else
            {
                Debug.Log("Speed Down");
                MoveSpeed = 3;
                // anim.SetBool("IsSpeed_Down", true);
                photonView.RPC("Speed_DownTrue", PhotonTargets.AllBuffered);
                StartCoroutine(SpeedDownDone(3));
            }
        }
        /* Need when the floor can triger
        else {
            Debug.Log("Back");
            MoveSpeed = 5;
            //anim.SetBool("IsSpeed_Up", false);
            photonView.RPC("Speed_UpFalse", PhotonTargets.AllBuffered);
        }
           */ 
        
            //Wood adv
            if (collision.gameObject.tag == "SpeedUp_Wood")
            {
                if (characterChoice == 2)
                {
                    Debug.Log("Wood Speed Up");
                    MoveSpeed = 7;
                    //anim.SetBool("IsSpeed_Up", true);
                    photonView.RPC("Speed_UpTrue", PhotonTargets.AllBuffered);
                    StartCoroutine(SpeedUpDone(3));//Not need when the floor is triger 
            }
                else
                {
                    Debug.Log("Cant move");
                    MoveSpeed = 0;
                // anim.SetBool("IsSpeed_Down", true);
                    photonView.RPC("Speed_DownTrue", PhotonTargets.AllBuffered);
                StartCoroutine(SpeedDownDone(3));
                }
            }
        /*
        else
        {
            MoveSpeed = 5;
            anim.SetBool("IsSpeed_Up", false);
        }
        */

        //Metal adv
        if (collision.gameObject.tag == "SpeedUp_Metal")
        {
            if (characterChoice == 1)
            {
                Debug.Log("Wood Speed Up");
                MoveSpeed = 7;
                //anim.SetBool("IsSpeed_Up", true);
                photonView.RPC("Speed_UpTrue", PhotonTargets.AllBuffered);
                StartCoroutine(SpeedUpDone(3));
            }
            else
            {
                Debug.Log("Damage");

                // anim.SetBool("IsSpeed_Down", true);
                photonView.RPC("damage", PhotonTargets.AllBuffered);
                //StartCoroutine(SpeedDownDone(3));
            }
        }

        //Fire adv
        if (collision.gameObject.tag == "SpeedUp_Fire")
        {
            /*if (characterChoice == 3)
            {
                Debug.Log("Fire Speed Up");
                MoveSpeed = 7;
                //anim.SetBool("IsSpeed_Up", true);
                photonView.RPC("Speed_UpTrue", PhotonTargets.AllBuffered);
                StartCoroutine(SpeedUpDone(3));
            }
            else
            {*/
                Debug.Log("damage");
                // anim.SetBool("IsSpeed_Down", true);
                //Player controller in a wrong direction  
                
                
                //Player controller in a wrong direction
                photonView.RPC("damageFire", PhotonTargets.AllBuffered);
               // StartCoroutine(controllerFix(3));

            //}
        }
        
    }

    


    //Speed_UP end
    IEnumerator SpeedUpDone(int delay)
    {
        yield return new WaitForSeconds(delay);
        MoveSpeed = 5;
        //anim.SetBool("IsSpeed_Up", false);
        photonView.RPC("Speed_UpFalse", PhotonTargets.AllBuffered);
        Debug.Log("speed_Up Done");
    }
    //Speed_down end
    IEnumerator SpeedDownDone(int delay)
    {
        yield return new WaitForSeconds(delay);
        MoveSpeed = 5;
        //  anim.SetBool("IsSpeed_Down", false);
        photonView.RPC("Speed_DownFalse", PhotonTargets.AllBuffered);
        Debug.Log("speed_Down Done");
    }
    
    //Win Scene
    IEnumerator WinScene(int delay)
    {
        yield return new WaitForSeconds(delay);
        photonView.RPC("Win", PhotonTargets.AllBuffered);
    }

    //IsRunning
    [PunRPC]
    private void RunTrue()
    {
        anim.SetBool("IsRunning", true);
    }
    //!IsRunning
    [PunRPC]
    private void RunFalse()
    {
        anim.SetBool("IsRunning", false);
    }
    //IsSpeed_Up
    [PunRPC]
    private void Speed_UpTrue()
    {
        anim.SetBool("IsSpeed_Up", true);
    }
    //!IsSpeed_up
    [PunRPC]
    private void Speed_UpFalse()
    {
        anim.SetBool("IsSpeed_Up", false);
    }
    //Speed_Down
    [PunRPC]
    private void Speed_DownTrue()
    {
        anim.SetBool("IsSpeed_Down", true);
    }
    //!Speed_Down
    [PunRPC]
    private void Speed_DownFalse()
    {
        anim.SetBool("IsSpeed_Down", false);
    }

 
    [PunRPC]
    private void FlipxTrue()
    {
        sr.flipX = true; 
    }

    [PunRPC]
    private void FlipxFalse()
    {
        sr.flipX = false;
    }

   // [PunRPC]
   // private void FlipyTrue()
  //  {
   //    sr.flipY = true;
   // }

    [PunRPC]
    private void FlipyFalse()
    {
        sr.flipY = false;
    }
    /*
    [PunRPC]
    private void End()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }
    */

    [PunRPC]
    private void damage()
    {
        var health = GetComponent<Health>();
            health.TakeDamage(25);
    }

    [PunRPC]
    private void damageFire()
    {
        var health = GetComponent<Health>();
        health.TakeDamage(100);
    }
    /*
    [PunRPC]
    private void kill() {
        photonView.RPC("damage", PhotonTargets.AllBuffered);
    }*/

    [PunRPC]
    private void Win()
    {
            PhotonNetwork.LoadLevel("EndScene");
            Debug.Log("end method");
    }
    /*
    [PunRPC]
    public void LoadWood()
    {
        anim.runtimeAnimatorController = woodChar;
        Debug.Log("Wood Select");
        characterChoice = 2;
    }

    [PunRPC]
    public void LoadFire()
    {
        anim.runtimeAnimatorController = fireChar;
        Debug.Log("Fire Select");
        characterChoice = 3;
    }

    [PunRPC]
    public void LoadMetal()
    {
        anim.runtimeAnimatorController = metalChar;
        Debug.Log("Metal Select");
        characterChoice = 1;
    }

    [PunRPC]
    public void LoadWater()
    {
        anim.runtimeAnimatorController = WaterChar;
        Debug.Log("Water Select");
        characterChoice = 4;
    }

    */
}

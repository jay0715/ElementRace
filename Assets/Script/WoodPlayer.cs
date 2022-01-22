using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WoodPlayer : Photon.MonoBehaviour
{
    [Header("OnlineServer")]
    public PhotonView photonView;

    [Header("Player")]
    public Rigidbody2D rb;
    public Animator anim;
    public GameObject PlayerCamera;
    public SpriteRenderer sr;
    public float MoveSpeed = 5;
    Vector2 movement;


    [Header("PlayerStates")]
    public bool IsWin = false;
    public bool IsUpState = false;
    public bool IsDownState = false;
    bool isBurn = false;


    [Header("UI")]
    public Text PlayerNameText;
    public GameObject SpeedUpState;
    public GameObject SpeedDownState;

    [Header("SoundEffects")]
    private AudioSource audioSrc;
    public AudioClip buff;
    public AudioClip debuff;
    public AudioClip win;
    public int timer = 3;

    private void Awake()
    {
        if (photonView.isMine)
        {
            PlayerCamera.SetActive(true);
            PlayerNameText.text = PhotonNetwork.playerName;
            PlayerNameText.color = Color.black;
            audioSrc = GetComponent<AudioSource>();
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
        if (isBurn == true)
        {
            photonView.RPC("damageForFlameWall", PhotonTargets.AllBuffered);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * MoveSpeed * Time.fixedDeltaTime);
    }
    private void CheckInput()
    {

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Flip left
        if (Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            photonView.RPC("FlipxFalse", PhotonTargets.AllBuffered);
        }

        //Flip right
        if (Input.GetKeyDown(KeyCode.D) || (Input.GetKeyDown(KeyCode.RightArrow)))
        {
            photonView.RPC("FlipxTrue", PhotonTargets.AllBuffered);
        }


        //Flip Down
        if (Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)))
        {
            photonView.RPC("FlipyFalse", PhotonTargets.AllBuffered);

        }

        //Switching Idle to run
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.UpArrow))
        {
            photonView.RPC("RunTrue", PhotonTargets.AllBuffered);         
        }
        else
        {
            photonView.RPC("RunFalse", PhotonTargets.AllBuffered);
            audioSrc.Pause();   
        }

        //Winning condition
        if(IsWin == true)
        {
            StartCoroutine(WinScene(0));
            audioSrc.PlayOneShot(win, 0.7f);
        }
       

        }

    #region Interations
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Win")
        {
            IsWin = true;
        }

        //Water Object Conditions 
        if (collision.gameObject.tag == "SpeedUp_Water")
        {
            MoveSpeed = 3;
            photonView.RPC("Speed_DownTrue", PhotonTargets.AllBuffered);
            photonView.RPC("SDS", PhotonTargets.AllBuffered);
            audioSrc.PlayOneShot(debuff, 0.5f);
            photonView.RPC("DebuffSound", PhotonTargets.OthersBuffered);
            StartCoroutine(SpeedDownDone(3));
        }

        //Wood Object Conditions 
        if (collision.gameObject.tag == "SpeedUp_Wood")
        {
            MoveSpeed = 7;
            photonView.RPC("Speed_UpTrue", PhotonTargets.AllBuffered);
            photonView.RPC("SUS", PhotonTargets.AllBuffered);
            audioSrc.PlayOneShot(buff, 0.5f);
            photonView.RPC("BuffSound", PhotonTargets.OthersBuffered);
            StartCoroutine(SpeedUpDone(3));//Not need when the floor is triger 
        }


        //Metal Objects Conditions
        if (collision.gameObject.tag == "SpeedUp_Metal")
        {
            MoveSpeed = 1;
            photonView.RPC("Speed_DownTrue", PhotonTargets.AllBuffered);
            photonView.RPC("SDS", PhotonTargets.AllBuffered);
            audioSrc.PlayOneShot(debuff, 0.5f);
            photonView.RPC("DebuffSound", PhotonTargets.OthersBuffered);
            StartCoroutine(SpeedDownDone(1));
        }

        //Fire Objects Condition
        if (collision.gameObject.tag == "SpeedUp_Fire")
        {
            anim.SetBool("IsShake", true);
            photonView.RPC("damage", PhotonTargets.AllBuffered);
            StartCoroutine(CamShakeDone(1));

        }


        //FlameWall Condition
        if (collision.gameObject.tag == "FlameWall")
        {
            isBurn = true;
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "FlameWall")
        {
            isBurn = false;
        }
    }


    //Speed_UP end
    IEnumerator SpeedUpDone(int delay)
    {
        yield return new WaitForSeconds(delay);
        MoveSpeed = 5;
        photonView.RPC("Speed_UpFalse", PhotonTargets.AllBuffered);
        photonView.RPC("SUD", PhotonTargets.AllBuffered);
    }

    //Speed_down end
    IEnumerator SpeedDownDone(int delay)
    {
        yield return new WaitForSeconds(delay);
        MoveSpeed = 5;
        photonView.RPC("Speed_DownFalse", PhotonTargets.AllBuffered);
        photonView.RPC("SDD", PhotonTargets.AllBuffered);
    }

    //Camera shake done
    IEnumerator CamShakeDone(int delay)
    {
        yield return new WaitForSeconds(delay);
        anim.SetBool("IsShake", false);
    }

    //Win Scene
    IEnumerator WinScene(int delay)
    {
        yield return new WaitForSeconds(delay);
        photonView.RPC("Win", PhotonTargets.AllBuffered);
    }
    #endregion

    #region synchronization
    //Debuff_Sound
    [PunRPC]
    private void DebuffSound()
    {
        audioSrc.PlayOneShot(debuff, 0.2f);
    }
    //Buff_Sound
    [PunRPC]
    private void BuffSound()
    {
        audioSrc.PlayOneShot(buff, 0.2f);
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

    [PunRPC]
    private void FlipyFalse()
    {
        sr.flipY = false;
    }
 
    [PunRPC]
    private void damage()
    {
        var health = GetComponent<Health>();
        health.TakeDamage(20);
    }
       [PunRPC]
    private void damageForFlameWall()
    {
        var health = GetComponent<Health>();
        health.TakeDamage(1);
    }


    [PunRPC]
    private void Win()
    {
            PhotonNetwork.LoadLevel("EndScene");
            Debug.Log("end method");
    }

    #region UIEffects
    [PunRPC]
    private void SUS()
    {
        if(IsDownState == true)
        {
            SpeedDownState.SetActive(false);
            SpeedUpState.SetActive(true);
            IsUpState = true;
        }
        else
        {
            SpeedUpState.SetActive(true);
            IsUpState = true;
        }      
    }

    [PunRPC]
    private void SUD()
    {
        SpeedUpState.SetActive(false);
        IsUpState = false;
    }

    [PunRPC]
    private void SDS()
    {
        if(IsUpState == true)
        {
            SpeedUpState.SetActive(false);
            SpeedDownState.SetActive(true);
            IsDownState = true;
        }
        else
        {
            SpeedDownState.SetActive(true);
            IsDownState = true;
        }
    }

    [PunRPC]
    private void SDD()
    {
        SpeedDownState.SetActive(false);
        IsDownState = false;
    }
    #endregion

    private void footstep()
    {
        audioSrc.Play();
        audioSrc.volume = 0.05f;
    }
    #endregion
}

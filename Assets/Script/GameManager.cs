using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.Tilemaps;

public class GameManager : Photon.MonoBehaviour
{
    [Header("CharPrefab")]
    public GameObject WoodCharPrefab;
    public GameObject WaterCharPrefab;
    public GameObject MetalCharPrefab;

    [Header("GameSetting")]
    public GameObject GameCanvas;
    public GameObject SceneCamera;
    //CharSelected?
    bool isSelected = false;
    public int charNum;
    public Animator CamAni;
    public GameObject FlameWall;
    public float speed;
    public GameObject TileMap;

    [Header("CharacterSelectionPanel")]
    [SerializeField] private GameObject StartButton;
    public GameObject PlayerNum;
    public GameObject NumGrid;
    public GameObject RoomName;
    public GameObject NameGrid;
    public GameObject CountDown;
    public GameObject Title;

    [Header ("SpawnPosition")]
    public GameObject SpawnPointForFlameWall;
    public GameObject SpawnPointUpdate;
    public GameObject SpawnPointReset;

    [Header("SoundEffects")]
    public AudioSource StartB;
    public AudioSource ClickB;


    [Header ("Counter")]
    bool startTimer = false;
    double timerIncrementValue;
    double decTimer;
    double startTime;
    [SerializeField]double EndTimer = 0;
    ExitGames.Client.Photon.Hashtable CustomeValue;


    private void Awake()
    {
        GameCanvas.SetActive(true);
        StartButton.SetActive(false);
        SpawnPointUpdate.transform.position = new Vector2(SpawnPointReset.transform.position.x, SpawnPointReset.transform.position.y);

        if (PhotonNetwork.player.IsMasterClient)
        {
            CustomeValue = new ExitGames.Client.Photon.Hashtable();
            startTime = PhotonNetwork.time;
            startTimer = true;
            CustomeValue.Add("StartTime", startTime);
            PhotonNetwork.room.SetCustomProperties(CustomeValue);
        }
        else
        {
            startTime = PhotonNetwork.time;
            startTimer = true;
        }

    }

    private void Update()
    {
        //Check number of Players in the room and get the name 
        if (GameCanvas == true)
        {
            GameObject obj = Instantiate(PlayerNum, new Vector2(0, 0), Quaternion.identity);
            obj.transform.SetParent(NumGrid.transform, false);
            obj.GetComponent<Text>().text = PhotonNetwork.room.playerCount + " ";
            GameObject objName = Instantiate(RoomName, new Vector2(100, 0), Quaternion.identity);
            objName.transform.SetParent(NameGrid.transform, false);
            objName.GetComponent<Text>().text = PhotonNetwork.room.Name + " ";
        }           

   
        if (!startTimer) return;

        timerIncrementValue = PhotonNetwork.time - startTime;
        //Decrese Timer
        double roundTime = 30.0;
        decTimer = roundTime - timerIncrementValue;
     
        if (CountDown != null)
        {
            CountDown.GetComponent<Text>().text = decTimer.ToString();
        }
        if (decTimer <= EndTimer)
        {     
            Destroy(Title);
            Destroy(CountDown);
            Destroy(TileMap);
            FlameWall.transform.Translate(Vector2.right * speed * Time.deltaTime);
            SpawnPointUpdate.transform.Translate(Vector2.right * speed * Time.deltaTime);
            PhotonNetwork.room.IsOpen = false;
        }
    }

    public void SpawnPlayer()
    {   
        if (isSelected == true)
        {
            CamAni.SetBool("IsMoving", true);
            if (charNum == 1) 
            {
                StartCoroutine(CameraViewForWood(5));
                GameCanvas.SetActive(false);         
            }          
            else if (charNum == 2)
            {
                StartCoroutine(CameraViewForMetal(5));
                GameCanvas.SetActive(false);
            }
            else if (charNum == 3)
            {              
                StartCoroutine(CameraViewForWater(5));
                GameCanvas.SetActive(false);
            }
        }
    }
     
    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        PhotonNetwork.DestroyAll();
        PhotonNetwork.LoadLevel("MainGame");
    }
    #region CharacterSelection
    //Wood Character
    public void LoadWood()
    {
        StartButton.SetActive(true);
        isSelected = true;
        charNum = 1;
    }
    //MetalCharacter
    public void LoadMetal()
    {
        StartButton.SetActive(true);
        isSelected = true;
        charNum = 2;
    }
    //WaterCharacter
    public void LoadWater()
    {
        StartButton.SetActive(true);    
        isSelected = true;
        charNum = 3;
    }

    IEnumerator CameraViewForWood(int delay)
    {
        yield return new WaitForSeconds(delay);
        CamAni.SetBool("IsMoving", false);
        SceneCamera.SetActive(false);
        PhotonNetwork.Instantiate(WoodCharPrefab.name, new Vector2(SpawnPointUpdate.transform.position.x, SpawnPointUpdate.transform.position.y), Quaternion.identity, 0);
        FlameWall.SetActive(true);

    }

    IEnumerator CameraViewForWater(int delay)
    {
        yield return new WaitForSeconds(delay);
        CamAni.SetBool("IsMoving", false);
        SceneCamera.SetActive(false);
        PhotonNetwork.Instantiate(WaterCharPrefab.name, new Vector2(SpawnPointUpdate.transform.position.x, SpawnPointUpdate.transform.position.y - 2), Quaternion.identity, 0);
        FlameWall.SetActive(true);
    }

    IEnumerator CameraViewForMetal(int delay)
    {
        yield return new WaitForSeconds(delay);
        CamAni.SetBool("IsMoving", false);
        SceneCamera.SetActive(false);
        PhotonNetwork.Instantiate(MetalCharPrefab.name, new Vector2(SpawnPointUpdate.transform.position.x, SpawnPointUpdate.transform.position.y + 2), Quaternion.identity, 0);
        FlameWall.SetActive(true);
    }

    #endregion

    //GameSound
    public void GamestartSound()
    {
        StartB.Play();
        StartB.volume = 0.05f;
    }

    public void ClickSound()
    {
        ClickB.Play();
        ClickB.volume = 0.05f;
    }



}

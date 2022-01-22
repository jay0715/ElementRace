using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    //Connect to Photon Server
    [SerializeField] private string VersioName = "0.1";
    [SerializeField] private GameObject UsernameMenu;
    [SerializeField] private GameObject ConnectPanel;

    [SerializeField] private InputField UsernameInput;
    [SerializeField] public InputField CreateGameInput;
    [SerializeField] private InputField JoinGameInput;

    [SerializeField] private GameObject StartButton;
    public AudioSource ClickB;
    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings(VersioName);

    }

    private void Start()
    {
        UsernameMenu.SetActive(true);

    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void ChangeUserNameInput()
    {
        if(UsernameInput.text.Length >= 1 && UsernameInput.text.Length <= 10)
        {
            StartButton.SetActive(true);
        }
        else
        {
            StartButton.SetActive(false);
        }
    }

    public void SetUserName()
    {
        UsernameMenu.SetActive(false);
        PhotonNetwork.playerName = UsernameInput.text;
    }

    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(CreateGameInput.text, new RoomOptions() { maxPlayers = 5 }, null);    
    }

    public void JoinGame()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.maxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text, roomOptions, TypedLobby.Default);
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("MainGame");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void BackToMenu()
    {
        UsernameMenu.SetActive(true);
    }

    public void ClickSound()
    {
        ClickB.Play();
        ClickB.volume = 0.05f;
    }
}

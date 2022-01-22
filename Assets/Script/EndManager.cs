using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndManager : MonoBehaviour
{
    [SerializeField] private GameObject MenuButton;
    [SerializeField] private GameObject QuitButtom;
    public GameObject WinGrid;
    public GameObject PlayerWin;
    public AudioSource ClickB;
    // Start is called before the first frame update

    private void Update()
    {
        GameObject obj = Instantiate(PlayerWin, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(WinGrid.transform, false);

        obj.GetComponent<Text>().text = "Player : "+PhotonNetwork.playerName + " " ;
    }
    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void ClickSound()
    {
        ClickB.Play();
    }
}

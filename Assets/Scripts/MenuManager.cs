using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text searching;
    [SerializeField] private GameObject notQueueMenu;
    [SerializeField] private GameObject queueMenu;
    
    private bool find;
    private Hashtable hash = new Hashtable();
    private void Update()
    {
        EnableDisableInQueue();
        
        if (find)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                SceneManager.LoadScene(1);
            }
        }
    }

    void EnableDisableInQueue()
    {
        searching.enabled = find;
        queueMenu.SetActive(find);
        notQueueMenu.SetActive(!find);
    }

    public void QuitQueue()
    {
        PhotonNetwork.Disconnect();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
    public void SearchGame()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log("On connected To Master");
        
        PhotonNetwork.JoinLobby();
    }
    
    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("On Joined Lobby");
        
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Creation Room");
        
        RoomOptions room = new RoomOptions();
        room.IsOpen = true;
        room.MaxPlayers = 2;
        room.IsVisible = true;
        PhotonNetwork.CreateRoom(PhotonNetwork.AuthValues.UserId, room, null);    
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("On joined Room");
        
        base.OnJoinedRoom();

        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            hash["PlayerNumber"] = 1;
        }
        else
        {
            hash["PlayerNumber"] = 2;
        }
        
        hash["RoundNumber"] = 1;
        PhotonNetwork.LocalPlayer.CustomProperties = hash;

        find = true;
        
        Debug.Log("Loading");
    }
}

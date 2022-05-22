using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class RoundManager : MonoBehaviourPunCallbacks
{
    public static RoundManager instance;
    
    [SerializeField] private GameObject playerPref;
    [SerializeField] private PhotonView playerView;
    [SerializeField] private int localPlayerTurn;
    [SerializeField] private int currentPlayerNumberTurnNumberTurn;
    
    
    private int roundState; 
    private GameObject playerInstance;

    public int LocalPlayerTurn
    {
        get => localPlayerTurn;
    }

    public int CurrentPlayerNumberTurn
    {
        get => currentPlayerNumberTurnNumberTurn;
    }
    
    public int StateRound
    {
        get => roundState;
        set
        {
            roundState = value;
        }
    }

    private void Awake()
    {
        instance = this;
        currentPlayerNumberTurnNumberTurn = 1;
        if (PhotonNetwork.IsMasterClient)
        {
            localPlayerTurn = 1;
        }
        else
        {
            localPlayerTurn = 2;
        }
        
    }

    void Start()
    {
        roundState = 0;
        SpawnNewPlayer();
    }
    
    public void SpawnNewPlayer()
    {
        if (localPlayerTurn==1)
        {
            playerInstance =
                PhotonNetwork.Instantiate(playerPref.name, Vector3.zero, Quaternion.identity, 0);
        }
        else
        {
            playerInstance =
                PhotonNetwork.Instantiate(playerPref.name, Vector3.zero, new Quaternion(0,180,0,0), 0);
        }
        
        playerInstance.GetComponent<PlayerSetup>().enabled = true;
        
    }
    
    public void EndRound()
    {
        PlacementManager.instance.ReInitMonster();
        BattlePhaseManager.instance.ClearUnits();

        playerView.RPC("RPC_EndTurn", RpcTarget.AllViaServer);
    }

    public void BattlePhase()
    {
        DiceManager.instance.DeleteAllResources(DiceManager.instance.DiceChoosen);
        roundState = 3;
    }
    
    public void Action()
    {
        switch (roundState)
        {
            case 4:
                BattlePhaseManager.instance.Attack();
                break;
            case 5:
                EffectManager.instance.Action();
                break;
        }
    }

    public void CancelAction()
    {
        switch (roundState)
        {
            case 4:
                BattlePhaseManager.instance.CancelSelection();
                break;
            case 5:
            case 6:
            case 7:
                EffectManager.instance.CancelSelection(1);
                break;
          
        }
    }
    
    [PunRPC]
    private void RPC_EndTurn()
    {
        roundState = 0;
        if (currentPlayerNumberTurnNumberTurn==1)
        {
            if (localPlayerTurn==1)
            {
                UiManager.instance.EnableDisableShader(false);
            }
            else
            {
                UiManager.instance.EnableDisableShader(true);
            }
            currentPlayerNumberTurnNumberTurn = 2;
        }
        else
        {
            if (localPlayerTurn==1)
            {
                UiManager.instance.EnableDisableShader(true);
            }
            else
            {
                UiManager.instance.EnableDisableShader(false);
            }
            currentPlayerNumberTurnNumberTurn = 1;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected (DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(1);
        DiceManager.instance = null;
        DeckManager.instance = null;
        UiManager.instance = null;
        EffectManager.instance = null;
        instance = null;
    }
}

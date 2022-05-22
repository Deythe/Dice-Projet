using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class AleatInvokeMonster : MonoBehaviour, IEffects
{
    [SerializeField] private PhotonView view;
    [SerializeField] private GameObject cardUnit;
    [SerializeField] private int usingPhase = 3;
    [SerializeField] private List<Vector2> boardPosition = new List<Vector2>();
    [SerializeField] private int numberPoupoul=1;
    private int random;
    private bool used;
    private bool here;
    private int i,j;
    

    public void OnCast(int phase)
    {
        if (view.AmOwner)
        {
            if (phase == usingPhase)
            {
                for (j = 0; j < numberPoupoul; j++)
                {
                    InitArrayOfPosition();
                    Action();
                    boardPosition.Clear();
                }
                
                used = true;
                EffectManager.instance.CancelSelection(1);
            }
        }
    }
    
    [PunRPC]
    private void RPC_Action(float x, float z)
    {
        PhotonNetwork.Instantiate(cardUnit.name, new Vector3(x,0.55f,z), PlayerSetup.instance.transform.rotation, 0);
    }

    private void Action()
    {
        for (i = boardPosition.Count; i > 0; i--)
        {
            here = false;
            random = Random.Range(0, boardPosition.Count);

            foreach (var place in PlacementManager.instance.GetBoard())
            {
                if (place.emplacement.Contains(boardPosition[random]))
                {
                    here = true;
                }
            }

            if (!here)
            {
                view.RPC("RPC_Action", RpcTarget.Others, boardPosition[random].x, boardPosition[random].y);
                return;
            }
            
            boardPosition.RemoveAt(random);
        }
    }
    void InitArrayOfPosition()
    {
        for (float x = -3.5f ; x <= 3.5f; x++)
        {
            for (float y = -4.5f ; y <= 4.5; y++)
            {
                boardPosition.Add(new Vector2(x, y));
            }
        }
    }

    public int GetPhaseActivation()
    {
        return usingPhase;
    }

    public bool GetUsed()
    {
        return used;
    }
    
    public void SetUsed(bool b)
    {
        used = b;
    }
}
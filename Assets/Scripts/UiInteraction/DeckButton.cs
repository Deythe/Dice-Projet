using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckButton : MonoBehaviour
{
    [SerializeField] private DeckScriptable deck;
    public void Action()
    {
        SoundManager.instance.PlaySFXSound(0, 0.07f);
        FireBaseManager.instance.User.currentDeck = deck.indexCrea;
        FireBaseManager.instance.User.currentDiceDeck = deck.diceDeck.diceDeck;
        MenuManager.instance.PlayButton.interactable = true;
        //MenuManager.instance.p_shader.enabled = true;
    }
}

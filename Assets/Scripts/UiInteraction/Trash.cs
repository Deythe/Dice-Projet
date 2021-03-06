using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Trash : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] private Image image;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (Input.touchCount > 0)
        {
            if(RoundManager.instance.StateRound==2 && !PlacementManager.instance.IsWaiting  && PlacementManager.instance.SpecialInvocation)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    image.color = new Color(1, 0, 0, 0.5f);
                }
            }
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                if(RoundManager.instance.StateRound==2 && !PlacementManager.instance.IsWaiting && PlacementManager.instance.SpecialInvocation)
                {
                    UiManager.instance.p_textFeedBack.enabled = false;
                    PlacementManager.instance.CancelSelection();
                }
            }
        }
        image.color = new Color(1, 1, 1, 0);
    }
}

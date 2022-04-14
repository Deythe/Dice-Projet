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
            if(RoundManager.instance.GetStateRound()==2)
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
                switch (RoundManager.instance.GetStateRound())
                {
                    case 2:
                        PlacementManager.instance.CancelSelection();
                        break;
                }
            }
        }
        image.color = new Color(1, 1, 1, 0);
    }
}
using UnityEngine;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private Vector3 original;

    public void OnBeginDrag(PointerEventData eventData)
    {
        original = transform.position; //紀錄原始座標
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.position = original;//歸位
    }

}

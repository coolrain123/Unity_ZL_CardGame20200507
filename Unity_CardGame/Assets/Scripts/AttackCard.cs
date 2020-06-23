using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UILineRenderer))]
public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public UILineRenderer line;

    private void Start()
    {
        line = GetComponent<UILineRenderer>();
        line.material = Resources.Load<Material>("線條材質");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
   


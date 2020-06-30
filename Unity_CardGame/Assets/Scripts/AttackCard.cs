using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.EventSystems;
using System.Collections;


public class AttackCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler , IPointerEnterHandler  ,IPointerExitHandler
{
    private UILineRenderer line;
    private Transform arrow;

    private Vector2 posBegin, posDrag;

    private static bool canAtk;
    private static Transform parent;
    private static Transform target;

    private void Start()
    {
       
        line = GameObject.Find("線條").GetComponent<UILineRenderer>();
        arrow = GameObject.Find("箭頭").transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        line.enabled = true;
        //介面座標轉螢幕座標
        //介面.x = 螢幕x-螢幕寬/2 
        posBegin.x = eventData.position.x - Screen.width / 2;
        //介面.y = 螢幕y-螢幕高/2 
        posBegin.y = eventData.position.y - Screen.height / 2;

        line.Points[0] = posBegin;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //介面座標轉螢幕座標
        //介面.x = 螢幕x-螢幕寬/2 
        posDrag.x = eventData.position.x - Screen.width / 2;
        //介面.y = 螢幕y-螢幕高/2 
        posDrag.y = eventData.position.y - Screen.height / 2;

        line.Points[1] = posDrag;

        line.Resoloution = (posDrag - posBegin).magnitude / 50;

        arrow.GetComponent<RectTransform>().anchoredPosition = posDrag;

        arrow.up = posDrag - posBegin;//設定箭頭方向
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        line.enabled = false;
        arrow.position = Vector2.one * 1000;

        if (canAtk && transform.parent != parent)
        {
            print("攻擊");
            StartCoroutine(Attack());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        print("滑入卡片");
        canAtk = true;
        parent = transform.parent;
        target = transform;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canAtk = false;
        parent = null;
        target = null;
    }

    private IEnumerator Attack()
    {
        transform.parent.SetAsLastSibling(); 
        Vector3 original = transform.position;

        while(transform.position.y != target.position.y)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 0.9f * Time.deltaTime * 30);
            yield return new WaitForSeconds(0.1f);
        }

        transform.position = original;
    }
}
   


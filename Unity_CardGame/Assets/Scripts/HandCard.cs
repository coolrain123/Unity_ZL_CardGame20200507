using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HandCard : MonoBehaviour, IBeginDragHandler,IDragHandler,IEndDragHandler
{
    private Vector3 original;
    private RectTransform rect;
    private Transform scene;

    private bool inScene;
    private int crystalCost;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        crystalCost = int.Parse(transform.Find("消耗").GetComponent<Text>().text);
        
        scene = GameObject.Find("我方場地").transform;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        original = transform.position; //紀錄原始座標
       
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (inScene) return;
        transform.position = eventData.position;
       
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (rect.anchoredPosition.y > 60)
        {
            checkCrystal();
        }
        else
        {
            transform.position = original;//歸位       
        }
    }
    private void checkCrystal()
    {
        if (crystalCost <= BattleManager.instance.crystal)
        {
            inScene = true;
            transform.SetParent(scene);
            BattleManager.instance.crystal -= crystalCost;  //扣水晶            
            BattleManager.instance.UpdateCrystal();         //更新水晶    
            BattleManager.instance.handcardCount--;         //手牌數量--
        }
        else
        {
            transform.position = original;
        }
       
    }

}

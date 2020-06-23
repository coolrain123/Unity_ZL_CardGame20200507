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

    public string sceneName;//場地名稱
    public float pos;       //拖拉進場判定位置

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        crystalCost = int.Parse(transform.Find("消耗").GetComponent<Text>().text);
        
        scene = GameObject.Find(sceneName).transform;
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
        bool con;
        if (sceneName.Contains("NPC")) con = rect.anchoredPosition.y <= pos;
        else con = rect.anchoredPosition.y > 60;

        if (con)
        {
            checkCrystal();
        }
        else
        {
            transform.position = original;//歸位       
        }
    }

    public BattleManager battle;
    private void checkCrystal()
    {
        if (crystalCost <= battle.crystal)
        {
            inScene = true;
            transform.SetParent(scene);
            battle.crystal -= crystalCost;  //扣水晶            
            battle.UpdateCrystal();         //更新水晶    
            battle.handcardCount--;         //手牌數量--

            gameObject.AddComponent<AttackCard>();
            if (sceneName.Contains("NPC")) transform.Find("卡背").gameObject.SetActive(false);
        }
        else
        {
            transform.position = original;
        }
       
    }

}

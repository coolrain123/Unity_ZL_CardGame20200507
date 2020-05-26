using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public List<CardData> deck= new List<CardData>();   
    public static DeckManager instance;

    [Header("牌組卡牌資訊")]
    public GameObject DeckObject;
    [Header("牌組內容")]
    public Transform contentDeck;
    [Header("卡牌數量")]
    public Text TextCountDeck;
    [Header("開始遊戲按鈕")]
    public Button btnStart;

    private void Awake()
    {
        instance = this;

        btnStart.interactable = false;
    }

    public void AddCard(int index)
    {
        if (deck.Count < 30)
        {
            //選取的卡牌
            CardData card = GetCard.instance.cards[index - 1];
            //尋找要增加卡牌在清單的資料
            // " => " 黏巴達 (Lambda C#7)
            // 相同卡牌 = 牌組.尋找全部(卡牌 => 卡牌.相同(目前點選的卡牌資訊))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(card));
            

            if (sameCard.Count < 2)
            {
                deck.Add(GetCard.instance.cards[index - 1]);
               
                      
                Transform temp;                
                
                if (sameCard.Count < 1)
                {
                    temp = Instantiate(DeckObject, contentDeck).transform;
                    //添加 牌組物件腳本
                    temp.gameObject.AddComponent<DeckObject>().index = card.index;
                    temp.name = "套牌" + card.name;
                }
                else
                {
                    temp = GameObject.Find("套牌" + card.name).transform;
                }
                
                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
                temp.Find("卡片名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();
                TextCountDeck.text = "卡片數量: " + deck.Count + " / 30";
            }
           
            
        }
        if (deck.Count == 30)
        btnStart.interactable = true;

    }

    public void DeleteCard(int index)
    {
        CardData card = GetCard.instance.cards[index - 1];

        List<CardData> sameCard = deck.FindAll(c => c.Equals(card));
        
        //刪除卡牌
        deck.Remove(card);

        Transform temp = GameObject.Find("套牌" + card.name).transform;
        //相同卡>1
        //更新牌組物件 數量
        if (sameCard.Count > 1)
        {
            temp.Find("數量").GetComponent<Text>().text = (sameCard.Count - 1).ToString();
        }
        //相同卡<1
        else
        {
         //刪除牌組物件
            Destroy(temp.gameObject);
        }
        TextCountDeck.text = "卡片數量: " + deck.Count + " / 30";
        btnStart.interactable = false;
      
    }
}

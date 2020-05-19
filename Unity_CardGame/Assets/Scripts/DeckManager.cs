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
    public GameObject TextCountDeck;

    private void Awake()
    {
        instance = this;
    }

    public void AddCard(int index)
    {
        if (deck.Count < 30)
        {
            //尋找要增加卡牌在清單的資料
            // " => " 黏巴達 (Lambda C#7)
            // 相同卡牌 = 牌組.尋找全部(卡牌 => 卡牌.相同(目前點選的卡牌資訊))
            List<CardData> sameCard = deck.FindAll(c => c.Equals(GetCard.instance.cards[index - 1]));
            

            if (sameCard.Count < 2)
            {
                deck.Add(GetCard.instance.cards[index - 1]);
               
                CardData card = GetCard.instance.cards[index - 1];               
                Transform temp;                
                
                if (sameCard.Count < 1)
                {
                    temp = Instantiate(DeckObject, contentDeck).transform;
                    temp.name = "套牌" + card.name;
                }
                else
                {
                    temp = GameObject.Find("套牌" + card.name).transform;
                }
                TextCountDeck.GetComponent<Text>().text = "卡片數量: " + deck.Count + " / 30";

                temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
                temp.Find("卡片名稱").GetComponent<Text>().text = card.name;
                temp.Find("數量").GetComponent<Text>().text = (sameCard.Count + 1).ToString();

            }
           
            
        }
       
      
    }

    public void DeleteCard(int index)
    {
        print("刪除牌組卡牌，編號為:" + index);
    }
}

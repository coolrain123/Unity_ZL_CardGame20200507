using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeckManager : MonoBehaviour
{

    public List<CardData> deck= new List<CardData>();   

    public List<GameObject> deckGameObject= new List<GameObject>();   
    //牌組管理器實體物件
    public static DeckManager instance;

    [Header("卡牌物件")]
    public GameObject DeckObject;
    [Header("牌組內容")]
    public Transform contentDeck;
    [Header("卡牌數量")]
    public Text TextCountDeck;
    [Header("開始遊戲按鈕")]
    public Button btnStart;
    [Header("洗牌後牌組")]
    public Transform transShuffle;

    
    /// <summary>
    /// 開始遊戲
    /// </summary>
    private void StartBattle()
    {      
        Shuffle();
        BattleManager.instance.StartBattle();
       
    }
    private void Awake()
    {
        instance = this;

        btnStart.interactable = false;

        btnStart.onClick.AddListener(StartBattle);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            choose30Cards();
        }
    }

    private void choose30Cards()
    {
        for (int i = 0; i < 2; i++)
        {
            for (int j = 1; j <= 15; j++)
            {
                AddCard(j);
            }
        }
    }
    public void Shuffle()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            //儲存目前卡牌
            CardData original = deck[i];

            //取得隨機 0 - 30
            int r = Random.Range(0, deck.Count);

            // 目前 = 隨機卡牌
            deck[i] = deck[r];

            // 隨機卡牌 = 目前
            deck[r] = original;
        }
        CreateCard();
    }

        /// <summary>
    /// 建立卡牌物件放在 洗牌後牌組
    /// </summary>
    private void CreateCard()
    {
        for (int i = 0; i < deck.Count; i++)
        {
            Transform temp = Instantiate(GetCard.instance.cardObject, transShuffle).transform;

            CardData card = deck[i];

            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name;
            temp.Find("描述").GetComponent<Text>().text = card.description;

            temp.Find("遮色片").Find("卡片圖").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

            deckGameObject.Add(temp.gameObject);
        }
    }
   
   
}

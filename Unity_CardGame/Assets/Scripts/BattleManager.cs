using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public List<CardData> BattleDeck = new List<CardData>();

    public List<GameObject> handGameObject = new List<GameObject>();

    public static BattleManager instance;

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;
    [Header("畫布")]
    public Transform canvas;
    [Header("手牌區域")]
    public Transform handArea;
    [Header("水晶"), Tooltip("水晶圖片，用來顯示的 10 張")]
    public GameObject[] crystalObject;
    [Header("水晶數量介面")]
    public Text textCrystal;
    [Header("擲金幣畫面")]
    public GameObject coinView;



    private bool firstAtk;

    private bool myTurn;
    private int crystalTotal;

    /// <summary>
    /// 水晶數量
    /// </summary>
    public int crystal ;

    private void Start()
    {
        instance = this;
    }

    public void StartBattle()
    {
        gameView.SetActive(true);
        ThrowCoin();
    }

    /// <summary>
    /// 我方結束
    /// </summary>
    public void EndTurn()
    {
        myTurn = false;
    }

    /// <summary>
    /// 對方結束 水晶+1
    /// </summary>
    public void StartTurn()
    {
        myTurn = true;
        crystalTotal++;
        crystalTotal = Mathf.Clamp(crystalTotal, 1, 10);//夾住水晶數1~10
        crystal = crystalTotal;
       
        Crystal();
        StartCoroutine(GetCard(1));
    }

    private void ThrowCoin()
    {
        coin.AddForce(0, Random.Range(300, 500), 0);
        coin.AddTorque(Random.Range(100, 200), 0, Random.Range(100, 200));
        Invoke("coinCheck", 3);
    }

    /// <summary>
    /// 檢查硬幣正反面
    /// rotation.x  為0  為正面
    /// rotation.x  為-1 為反面
    /// </summary>
    private void coinCheck()
    {

        print(coin.transform.GetChild(0).position.y);

        firstAtk = coin.transform.GetChild(0).position.y < -1f ? false : true;
        print("先攻?"+ firstAtk );

        int card = 3;

        if (firstAtk)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }

        Crystal();

        StartCoroutine(GetCard(card));
    }


    /// <summary>
    /// 處理水晶數量
    /// </summary>
    private void Crystal()
    {
        // 顯示目前有幾顆水晶
        for (int i = 0; i < crystal; i++)
        {
            crystalObject[i].SetActive(true);
        }

        textCrystal.text = crystal + " / 10";
    }

    /// <summary>
    /// 更新水晶介面與圖片
    /// </summary>
    public void UpdateCrystal()
    {
        for (int i = 0; i < crystalObject.Length; i++)
        {
            if (i < crystal) continue;  // 如果 迴圈編號 < 目前水晶數量 就繼續 (跳過此次)

            crystalObject[i].SetActive(false);
        }

        textCrystal.text = crystal + " / 10";
    }

    private IEnumerator GetCard(int count)
    {
        for (int i = 0; i < count; i++)
        {
            BattleDeck.Add(DeckManager.instance.deck[0]);

            DeckManager.instance.deck.RemoveAt(0);

            handGameObject.Add(DeckManager.instance.deckGameObject[0]);

            DeckManager.instance.deckGameObject.RemoveAt(0);

            yield return StartCoroutine(MoveCard());
        }           
        
    }

    /// <summary>
    /// 抽出卡牌並移動到手牌區
    /// </summary>
    /// <returns></returns>

    private int handcardCount;
    private IEnumerator MoveCard()
    {
        RectTransform card = handGameObject[handGameObject.Count - 1].GetComponent<RectTransform>();

        //進手牌前
        card.SetParent(canvas);
        card.anchorMin = Vector2.one * 0.5f;
        card.anchorMax = Vector2.one * 0.5f;

        while (card.anchoredPosition.x > 501)
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 0), 0.5f * Time.deltaTime * 50);

            yield return null;
        }

        yield return new WaitForSeconds(0.35f);

        if(handcardCount == 10)//大於10張
        {
            print("爆手牌");
        }

        else
        {
            //進入手牌
            card.localScale = Vector3.one * 0.5f;

            while (card.anchoredPosition.y > -274)
            {
                card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -275), 0.5f * Time.deltaTime * 50);

                yield return null;
            }

            card.SetParent(handArea);
            card.gameObject.AddComponent<HandCard>();
            handcardCount++;
        }
       
    }
}

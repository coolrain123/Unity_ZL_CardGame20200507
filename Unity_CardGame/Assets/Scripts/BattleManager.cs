using System.Collections.Generic;
using System.Collections;
using UnityEngine;

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
    public Transform handArea;

    private bool firstAtk;
    /// <summary>
    /// 水晶數量
    /// </summary>
    public int crystal = 1;

    private void Start()
    {
        instance = this;
    }

    public void StartBattle()
    {
        gameView.SetActive(true);
        ThrowCoin();
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

       StartCoroutine(GetCard(3));
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
    private IEnumerator MoveCard()
    {
        RectTransform card = handGameObject[handGameObject.Count - 1].GetComponent<RectTransform>();

        card.SetParent(canvas);
        card.anchorMin = Vector2.one * 0.5f;
        card.anchorMax = Vector2.one * 0.5f;

        while (card.anchoredPosition.x > 501)
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(500, 0), 0.5f * Time.deltaTime * 50);

            yield return null;
        }

        yield return new WaitForSeconds(0.35f);

        card.localScale = Vector3.one * 0.5f;

        while (card.anchoredPosition.y > -274)
        {
            card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, new Vector2(0, -275), 0.5f * Time.deltaTime * 50);
           
            yield return null;
        }

        card.SetParent(handArea);
        card.gameObject.AddComponent<HandCard>();
    }
}

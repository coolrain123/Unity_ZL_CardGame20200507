using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public List<CardData> BattleDeck = new List<CardData>();

    [Header("金幣")]
    public Rigidbody coin;
    [Header("遊戲畫面")]
    public GameObject gameView;

    private bool firstAtk;
    public static BattleManager instance;

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
        print(coin.rotation.x);

        firstAtk = coin.rotation.x < 0 ? false : true;
        print("先後攻"+ firstAtk );

        GetCard();
    }
    private void GetCard()
    {
        BattleDeck.Add(DeckManager.instance.deck[0]);

        DeckManager.instance.deck.RemoveAt(0);
    }
}

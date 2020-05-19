using UnityEngine;
using UnityEngine.UI;

public class BookCard : MonoBehaviour
{
    /// <summary>
    /// 卡牌圖鑑編號
    /// </summary>
    public int index;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChooseBookCard);
    }

    private void ChooseBookCard()
    {
        print("選取卡片的圖鑑編號: " + index);
        DeckManager.instance.AddCard(index);
    }
}

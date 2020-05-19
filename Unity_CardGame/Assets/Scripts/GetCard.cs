using System.Collections;
using UnityEngine;
using UnityEngine.Networking;//網路連線API
using UnityEngine.UI;

public class GetCard : MonoBehaviour
{
    public CardData[] cards;

    [Header("卡片物件")]
    public GameObject cardObject;
    [Header("卡片物件")]
    public Transform contentCard;

    private CanvasGroup loadingPanel;
    private Image loading;
    public static GetCard instance;

    private void Awake()
    {
        instance = this;

        loadingPanel = GameObject.Find("載入畫面").GetComponent<CanvasGroup>();
        loading = GameObject.Find("進度條").GetComponent<Image>();

    }
    private void Start()
    {
        StartCoroutine(GetCardData());
    }

    public IEnumerator GetCardData()
    {
        loadingPanel.alpha = 1;
        loadingPanel.blocksRaycasts = true;

        //引用 (網路要求 www = 網路要求.Post("網址",""))
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxa_hlodN_iqO-I1Cj8DDTB0J68rnxm7v9AXhyRaVTUzkaQC-w/exec", ""))
        {
            //等待 網路要求時間
            // yield return www.SendWebRequest();
            www.SendWebRequest();

            while (www.downloadProgress<1)
            {
                yield return null;
                loading.fillAmount = www.downloadProgress;
            }

            if (www.isNetworkError || www.isHttpError)
            {
                print("網路錯誤" + www.error);
            }
            else
            {                
                cards = JsonHelper.FromJson<CardData>(www.downloadHandler.text);
                cresteCard();
            }
        }
        yield return new WaitForSeconds(0.5f);
        loadingPanel.alpha = 0;
        loadingPanel.blocksRaycasts = false;

    }

    private void cresteCard()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            Transform temp =  Instantiate(cardObject, contentCard).transform;

            CardData card = cards[i];

            temp.Find("消耗").GetComponent<Text>().text = card.cost.ToString();
            temp.Find("攻擊").GetComponent<Text>().text = card.attack.ToString();
            temp.Find("血量").GetComponent<Text>().text = card.hp.ToString();
            temp.Find("名稱").GetComponent<Text>().text = card.name.ToString();
            temp.Find("描述").GetComponent<Text>().text = card.description.ToString();

            temp.Find("遮色片").Find("卡片圖").GetComponent<Image>().sprite = Resources.Load<Sprite>(card.file);

            //將<卡牌圖鑑>.編號 = 卡片.編號;
            temp.gameObject.AddComponent<BookCard>().index = card.index;
        }
    }
    

    /// <summary>
    /// 讀取 Excel 資料並轉換為陣列
    /// </summary>
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
    
}

/// <summary>
/// 卡片資料
/// </summary>
/// 序列化:使資訊可顯示在屬性面板上 
[System.Serializable]
public class CardData
{
    public int index;
    public string name;
    public string description;
    public int cost;
    public float attack;
    public float hp;
    public string file;
}
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;//網路連線API

public class GetCard : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GetCardData());
    }

    public IEnumerator GetCardData()
    {
        //引用 (網路要求 www = 網路要求.Post("網址",""))
        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbxa_hlodN_iqO-I1Cj8DDTB0J68rnxm7v9AXhyRaVTUzkaQC-w/exec", ""))
        {
            //等待 網路要求時間
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                print("網路錯誤" + www.error);
            }
            else
            {
                print(www.downloadHandler.text);
            }
        }
    }
}


using UnityEngine;


public class NPCDeckManager : DeckManager
{

    public static NPCDeckManager instanceNPC;


    protected override void Awake()
    {
        instanceNPC = this;
        btnStart.onClick.AddListener(choose30Cards);
    }

    protected override void Update()
    {
        
    }

    protected override void choose30Cards()
    {
        base.choose30Cards();

        Invoke("Shuffle", 1);
    }
}

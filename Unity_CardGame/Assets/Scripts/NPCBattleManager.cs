
using UnityEngine;

public class NPCBattleManager : BattleManager
{
    public static NPCBattleManager instenceNPC;

    private void Start()
    {
        instenceNPC = this;
        sceneName = "NPC方場地";
        pos = -180;
    }

    protected override void coinCheck()
    {
        firstAtk = !instance.firstAtk;  //等同玩家相反
        int card = 3;

        if (firstAtk)
        {
            crystalTotal = 1;
            crystal = 1;
            card = 4;
        }
        Crystal();
        StartCoroutine(GetCard(card, NPCDeckManager.instanceNPC, 200, 275));
    }

    
}

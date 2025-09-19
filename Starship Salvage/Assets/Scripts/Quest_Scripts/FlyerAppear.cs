using UnityEngine;

public class FlyerAppear : MonoBehaviour
{
    public GameObject CoLuFlyer;
    public GameObject RaLuFlyer;
    public GameObject LuLuFlyer;
    private NPC npc;
    public bool hasFlyerAppeared;

    public void FlyerAppears()
    {
        if (npc != null && npc.hasTalked == true)
        {
            CoLuFlyer.SetActive(true);
            RaLuFlyer.SetActive(true);
            LuLuFlyer.SetActive(true);

            hasFlyerAppeared = true;
        }
    }

    
}

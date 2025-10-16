using UnityEngine;

public class FlyerAppear : MonoBehaviour
{
    public GameObject CoLuFlyer;
    public GameObject RaLuFlyer;
    public GameObject LuLuFlyer;
    public NPC npc;
    public bool hasFlyerAppeared;

    public void FlyerAppears()
    {
        Debug.Log("making appear");
        if (npc != null && npc.hasTalked == true && !hasFlyerAppeared)
        {
            CoLuFlyer.SetActive(true);
            RaLuFlyer.SetActive(true);
            LuLuFlyer.SetActive(true);

            hasFlyerAppeared = true;
        }
    }

    
}

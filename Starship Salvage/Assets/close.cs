using UnityEngine;

public class close : MonoBehaviour
{
    public GameObject CloseTarget;

    public void OnClose()
    {
        CloseTarget.SetActive(false);
        //maybe play close sound?
    }
}

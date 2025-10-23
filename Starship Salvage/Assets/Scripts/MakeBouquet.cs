using UnityEngine;

public class MakeBouquet : MonoBehaviour
{
    public GameObject BouquetItem;
    public GameObject Minigame;
    public GameObject DisableTable;
    public Hotbar inventory;
    public GameObject ZinniaExclamation;
    public void OnMakeBouquet()
    {
        BouquetItem.SetActive(true);
        ZinniaExclamation.SetActive(true);
        Minigame.SetActive(false);
        DisableTable.SetActive(false);

        inventory.RemoveItemByID("MLF");
        inventory.RemoveItemByID("CLF");
        inventory.RemoveItemByID("LLF");
        inventory.RemoveItemByID("RLF");


    }
}

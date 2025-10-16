using UnityEngine;

public class MakeBouquet : MonoBehaviour
{
    public GameObject BouquetItem;
    public GameObject Minigame;
    public GameObject DisableTable;
    public Hotbar inventory;
    public void OnMakeBouquet()
    {
        BouquetItem.SetActive(true);
        Minigame.SetActive(false);
        DisableTable.SetActive(false);

        inventory.RemoveItemByID("MLF");
        inventory.RemoveItemByID("CLF");
        inventory.RemoveItemByID("LLF");
        inventory.RemoveItemByID("RLF");


    }
}

using TMPro;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.WSA;
using UnityEngine.XR;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Objectives : MonoBehaviour
{
    public TextMeshProUGUI ObjectiveBox;
    public Image Map;
    public Hotbar Inventory;

    [Header("Map Sprites")]
    public Sprite ZorbShip;
    public Sprite Zinnia;
    public Sprite Rami;

    [Header("Flyer map Sprites")]
    [Header("3")]
    public Sprite FlyCoLuRaLuLuLu;
    [Header("2")]
    public Sprite FlyCoLuRaLu;
    public Sprite FlyCoLuLuLu;
    public Sprite FlyLuLuRaLu;
    [Header("1")]
    public Sprite FlyCoLu;
    public Sprite FlyRaLu;
    public Sprite FlyLuLu;


    [Header("Flower map Sprites")]
    [Header("4")]
    public Sprite CoLuMinLuRaLuLuLu;
    [Header("3")]
    public Sprite CoLuRaLuLuLu;
    public Sprite MinLuRaLuLuLu;
    public Sprite CoLuMinLuLuLu;
    public Sprite CoLuRaLuMinLu;
    [Header("2")]
    public Sprite CoLuMinLu;
    public Sprite CoLuLuLu;
    public Sprite CoLuRaLu;
    public Sprite MinLuRaLu;
    public Sprite MinLuLuLu;
    public Sprite RaLuLuLu;
    [Header("1")]
    public Sprite CoLu;
    public Sprite MinLu;
    public Sprite RaLu;
    public Sprite LuLu;

    private bool FlyerCoLu;
    private bool FlyerMinLu;
    private bool FlyerRaLu;
    private bool FlyerLuLu;

    private bool FlowerCoLu;
    private bool FlowerMinLu;
    private bool FlowerRaLu;
    private bool FlowerLuLu;
    public void GetObjective(string ObjectiveName)
    {
        if (ObjectiveName == "FLYER")
        {
            // Deliver Flyers(0 / 3) -trigger trough zorb quest in action, cross off as flyer quests complete -FLYER
            
            //update map
            //logic to increase counter
            FlyerCoLu = Inventory.hasItem("3");
            FlyerRaLu = Inventory.hasItem("2");
            FlyerLuLu = Inventory.hasItem("4");

            if (FlyerCoLu && FlyerRaLu && FlyerLuLu)
            {
                Map.sprite = FlyCoLuRaLuLuLu;
                ObjectiveBox.text = "Deliver Festival Flyers (0/3)";
            } else if (FlyerCoLu && FlyerRaLu)
            {
                Map.sprite = FlyCoLuRaLu;
                ObjectiveBox.text = "Deliver Festival Flyers (1/3)";
            } else if (FlyerCoLu && FlyerLuLu)
            {
                Map.sprite = FlyCoLuLuLu;
                ObjectiveBox.text = "Deliver Festival Flyers (1/3)";
            } else if (FlyerLuLu && FlyerRaLu)
            {
                Map.sprite = FlyLuLuRaLu;
                ObjectiveBox.text = "Deliver Festival Flyers (1/3)";
            }
            else if (FlyerCoLu)
            {
                Map.sprite = FlyCoLu;
                ObjectiveBox.text = "Deliver Festival Flyers (2/3)";
            }
            else if (FlyerRaLu)
            {
                Map.sprite = FlyRaLu;
                ObjectiveBox.text = "Deliver Festival Flyers (2/3)";
            } else if (FlyerLuLu)
            {
                Map.sprite = FlyLuLu;
                ObjectiveBox.text = "Deliver Festival Flyers (2/3)";
            } else
            {
                GetObjective("DELIVERED");
            }



        }
        else if (ObjectiveName == "DELIVERED")
        {
            // Return to Zorb - trigger trough 3 / 3 flyers delivered -DELIVERED
            ObjectiveBox.text = "Return to Zorb in Min Lu";
            Map.sprite = ZorbShip;

        }
        else if (ObjectiveName == "SPANNER")
        {
            // Repair Ship -trigger through pickup of spanner -SPANNER
            ObjectiveBox.text = "Repair the ship";
            Map.sprite = ZorbShip;


        }
        else if (ObjectiveName == "ZINNIA")
        {
            // Talk to Zinnia in Co Lu -trigger through day 2 - ZINNIA
            ObjectiveBox.text = "Talk to Zinnia in Co Lu ";
            Map.sprite = Zinnia;


        }
        else if (ObjectiveName == "FLOWER")
        {
            // Collect Flowers(0 / 4) -trigger through zinnia quest in action, cross off as flowers appear in inventory - FLOWER
            ObjectiveBox.text = "Collect Region Flowers (0/4)";
            //update map
            FlowerCoLu = Inventory.hasItem("3");
            FlowerRaLu = Inventory.hasItem("2");
            FlowerLuLu = Inventory.hasItem("4");
            FlowerMinLu = Inventory.hasItem("5");

            if (FlowerCoLu && FlowerMinLu && FlowerRaLu && FlowerLuLu)
            {
                GetObjective("FOUND");
            }
            else if (FlowerMinLu && FlowerRaLu && FlowerLuLu)
            {
                Map.sprite = CoLu;
                ObjectiveBox.text = "Find Region Flowers (3/4)";
            }
            else if (FlowerCoLu && FlowerRaLu && FlowerLuLu)
            {
                Map.sprite = MinLu;
                ObjectiveBox.text = "Find Region Flowers (3/4)";
            }
            else if (FlowerCoLu && FlowerMinLu && FlowerLuLu)
            {
                Map.sprite = RaLu;
                ObjectiveBox.text = "Find Region Flowers (3/4)";
            }
            else if (FlowerCoLu && FlowerMinLu && FlowerRaLu)
            {
                Map.sprite = LuLu;
                ObjectiveBox.text = "Find Region Flowers (3/4)";
            }
            else if ( FlowerRaLu && FlowerLuLu)
            {
                Map.sprite = CoLuMinLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerMinLu && FlowerRaLu)
            {
                Map.sprite = CoLuLuLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerCoLu && FlowerMinLu)
            {
                Map.sprite = RaLuLuLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerMinLu &&FlowerLuLu)
            {
                Map.sprite = CoLuRaLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerCoLu && FlowerRaLu)
            {
                Map.sprite = MinLuLuLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerCoLu && FlowerLuLu)
            {
                Map.sprite = MinLuRaLu;
                ObjectiveBox.text = "Find Region Flowers (2/4)";
            }
            else if (FlowerCoLu)
            {
                Map.sprite = MinLuRaLuLuLu;
                ObjectiveBox.text = "Find Region Flowers (1/4)";
            }
             else if (FlowerMinLu)
            {
                Map.sprite = CoLuRaLuLuLu;

                ObjectiveBox.text = "Find Region Flowers (1/4)";
            }
            else if (FlowerRaLu)
            {
                Map.sprite = CoLuMinLuLuLu;
                ObjectiveBox.text = "Find Region Flowers (1/4)";
            }
            else if (FlowerLuLu)
            {
                Map.sprite = CoLuRaLuMinLu;

                ObjectiveBox.text = "Find Region Flowers (1/4)";
            }
            else
            {
                Map.sprite = FlyCoLuRaLu;
                ObjectiveBox.text = "Find Region Flowers (0/4)";
            }
     

        }
        else if (ObjectiveName == "FOUND")
        {
            // Return to Zinnia - all 4 flowers in inventory - FOUND
            ObjectiveBox.text = "Return to Zinnia in Co Lu";
            Map.sprite = Zinnia;

        }
        else if (ObjectiveName == "ARRANGE")
        {
            // Make Bouquet -zinnia bouquet dialogue done - ARRANGE
            ObjectiveBox.text = "Make the flower bouquet";
            Map.sprite = Zinnia;


        }
        else if (ObjectiveName == "BOUQUET")
        {
            // Give Bouquet to Zinnia -bouquet made - BOUQUET
            ObjectiveBox.text = "Talk to Zinnia";
            Map.sprite = Zinnia;

        }
        else if (ObjectiveName == "GEARS")
        {
            //  Repair Ship -pick up of gears - GEARS
            ObjectiveBox.text = "Repair the ship";
            Map.sprite = ZorbShip;

        }
        else if (ObjectiveName == "RAMI")
        {
            //  Talk to Chef Rami in Ra Lu -day 3 - RAMI
            ObjectiveBox.text = "Talk to Chef Rami in Ra Lu";
            Map.sprite = Rami;

        }
        else if (ObjectiveName == "COOK")
        {
            //  your hand at cooking in Astro Bistro -rami quest in action - COOK
            ObjectiveBox.text = "Try your hand at cooking in Astro Bistro";
            Map.sprite = Rami;

        }
        else if (ObjectiveName == "TASTE")
        {
            //  Give Chef Rami a taste - completion of minigame -TASTE
            ObjectiveBox.text = "Talk to Chef Rami";
            Map.sprite = Rami;

        }
        else if (ObjectiveName == "METAL")
        {
            // Repair ship -pick up xxx -METAL
            ObjectiveBox.text = "Repair the ship";
            Map.sprite = ZorbShip;


        }
        else if (ObjectiveName == "ZORB")
        {
            //  Talk to Zorb - day 4 - ZORB
            ObjectiveBox.text = "Talk to Zorb";
            Map.sprite = ZorbShip;


        }
        else if (ObjectiveName == "HOME")
        {
            //  Head back home - talked to zorb -HOME
            ObjectiveBox.text = "Talk to Zorb when you're ready to head home";
            Map.sprite = ZorbShip;

        }
        else
        {
            Debug.Log("Objective doesnt exist");
            
        }

    }
}

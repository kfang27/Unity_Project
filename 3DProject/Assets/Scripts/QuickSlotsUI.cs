using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    // Image class comes from UnityEngine.UI
    public Image leftWeaponIcon;
    public Image rightWeaponIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft, WeaponItem weapon)
    {
        if (isLeft == false)
        {
            if (weapon.itemIcon != null)
            {
                rightWeaponIcon.sprite = weapon.itemIcon;
                //rightWeaponIcon.preserveAspect = true;
                rightWeaponIcon.enabled = true;
            }
            else
            {
                rightWeaponIcon.sprite = null;
                rightWeaponIcon.enabled = false;
            }
        }
        else
        {
            if (weapon.itemIcon != null)
            {
                leftWeaponIcon.sprite = weapon.itemIcon;
                //leftWeaponIcon.preserveAspect = true;
                leftWeaponIcon.enabled = true;
            }
            else
            {
                leftWeaponIcon.sprite = null;
                leftWeaponIcon.enabled = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterials : MonoBehaviour
{
    [SerializeField]
    Material playerHandsL;
    [SerializeField]
    float emmissiveTogglePHL;
    [SerializeField]
    float emmissiveStrengthPHL;
    [SerializeField] 
    [ColorUsage(true, true)]
    Color hsvColorPHL;

    [SerializeField]
    Material playerHandsR;
    [SerializeField]
    float emmissiveTogglePHR;
    [SerializeField]
    float emmissiveStrengthPHR;
    [SerializeField]
    [ColorUsage(true, true)]
    Color hsvColorPHR;

    [SerializeField]
    Material playerRingL;
    [SerializeField]
    float emmissiveTogglePRL;
    [SerializeField]
    float emmissiveStrengthPRL;
    [SerializeField]
    [ColorUsage(true, true)]
    Color hsvColorPRL;

    [SerializeField]
    Material playerRingR;
    [SerializeField]
    float emmissiveTogglePRR;
    [SerializeField]
    float emmissiveStrengthPRR;
    [SerializeField]
    [ColorUsage(true, true)]
    Color hsvColorPRR;

    // Update is called once per frame
    void Update()
    {
        playerHandsL.SetFloat("_Emissive_Toggle_On", emmissiveTogglePHL);  
        playerHandsL.SetFloat("_EmissiveStrenght", emmissiveStrengthPHL);  
        playerHandsL.SetColor("_EmissiveColour", hsvColorPHL);

        playerHandsR.SetFloat("_Emissive_Toggle_On", emmissiveTogglePHR);
        playerHandsR.SetFloat("_EmissiveStrenght", emmissiveStrengthPHR);
        playerHandsR.SetColor("_EmissiveColour", hsvColorPHR);

        playerRingL.SetFloat("_Emissive_Toggle_On", emmissiveTogglePRL);
        playerRingL.SetFloat("_EmissiveStrenght", emmissiveStrengthPRL);
        playerRingL.SetColor("_EmissiveColour", hsvColorPRL);

        playerRingR.SetFloat("_Emissive_Toggle_On", emmissiveTogglePRR);
        playerRingR.SetFloat("_EmissiveStrenght", emmissiveStrengthPRR);
        playerRingR.SetColor("_EmissiveColour", hsvColorPRR);
    }
}

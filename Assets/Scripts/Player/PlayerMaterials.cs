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

    [SerializeField]
    Material playerFeathers;

    [SerializeField]
    Material playerClothing;

    public Material GetPlayerFeather() { return playerFeathers; }
    public Material GetPlayerClothing() { return playerClothing; }
    public Material GetPlayerHandsL() { return playerHandsL; }
    public Material GetPlayerHandsR() { return playerHandsR; }
    public Material GetPlayerRingL() { return playerRingL; }
    public Material GetPlayerRingR() { return playerRingR; }

    public void SetEmmissiveTogglePHL(float tempETPHL) { emmissiveTogglePHL = tempETPHL; }
    public void SetEmmissiveStrengthPHL(float tempESPHL) { emmissiveStrengthPHL = tempESPHL; }
    public void SetHsvColorPHL(Color tempHsvColorPHL) { hsvColorPHL = tempHsvColorPHL; }


    public void SetEmmissiveTogglePHR(float tempETPHR) { emmissiveTogglePHR = tempETPHR; }
    public void SetEmmissiveStrengthPHR(float tempESPHR) { emmissiveStrengthPHR = tempESPHR; }
    public void SetHsvColorPHR(Color tempHsvColorPHR) { hsvColorPHL = tempHsvColorPHR; }


    public void SetEmmissiveTogglePRL(float tempETPRL) { emmissiveTogglePRL = tempETPRL; }
    public void SetEmmissiveStrengthPRL(float tempESPRL) { emmissiveStrengthPRL = tempESPRL; }
    public void SetHsvColorPRL(Color tempHsvColorPRL) { hsvColorPHL = tempHsvColorPRL; }


    public void SetEmmissiveTogglePRR(float tempETPRR) { emmissiveTogglePHR = tempETPRR; }
    public void SetEmmissiveStrengthPRR(float tempESPRR) { emmissiveStrengthPHR = tempESPRR; }
    public void SetHsvColorPRR(Color tempHsvColorPRR) { hsvColorPHL = tempHsvColorPRR; }

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

    public void ResetVar()
    {
        emmissiveTogglePHL = 0.0f;
        emmissiveStrengthPHL = 0.0f;
         hsvColorPHL = new Color(0.0f, 0.0f, 0.0f);

        emmissiveTogglePHR = 0.0f;
        emmissiveStrengthPHR = 0.0f;
         hsvColorPHR = new Color(0.0f, 0.0f, 0.0f);

        emmissiveTogglePRL = 0.0f;
        emmissiveStrengthPRL = 0.0f;
        hsvColorPRL = new Color(0.0f, 0.0f, 0.0f);

        emmissiveTogglePRR = 0.0f;
        emmissiveStrengthPRR = 0.0f;
        hsvColorPRR = new Color(0.0f, 0.0f, 0.0f);
    }
}

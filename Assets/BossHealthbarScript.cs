using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthbarScript : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth = 0.0f;
    public List<BaseEnemyClass> enemies = new List<BaseEnemyClass>();
    public Image healthbar;
    public TextMeshProUGUI bossName;
    private void Update()
    {
        float currentHealth = 0;
        //maxHealth = 0;
        foreach (BaseEnemyClass enemy in enemies)
        {
            if (enemy)
            {
                currentHealth += enemy.GetHealth();
                //maxHealth += enemy.maxHealth;
            }

        }
        healthbar.fillAmount = currentHealth / maxHealth;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthbarScript : MonoBehaviour
{
    float maxHealth;
    float currentHealth = 0.0f;
	List<BaseEnemyClass> enemies = new List<BaseEnemyClass>();
	[SerializeField]
	Image healthbar;
	[SerializeField]
	TextMeshProUGUI bossName;
    
    private void Update()
    {
        currentHealth = 0;
        foreach (BaseEnemyClass enemy in enemies)
        {
            if (enemy)
            {
                currentHealth += enemy.GetHealth();
            }

        }
        healthbar.fillAmount = currentHealth / maxHealth;
    }
    
	public void AddEnemy(BaseEnemyClass enemy)
	{
		enemies.Add(enemy);
	}
	
	public void RemoveEnemy(BaseEnemyClass enemy)
	{
		enemies.Remove(enemy);
	}
	
	public void SetName(string name)
	{
		bossName.text = name;
	}
	
	public void SetMaxHealth(float health)
	{
		maxHealth = health;
	}
	
	public int GetBossCount()
	{
		return enemies.Count;
	}
}

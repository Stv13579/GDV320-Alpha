using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SAIM Data")]
public class SAIMData : ScriptableObject
{
    [SerializeField]
	List<GameObject> enemyTypes;
    
	[SerializeField]
	List<EnemySquad> enemySquads;

    GameObject player;

    //The amount of spawns available for the entire room in the entire duration.
    [SerializeField]
    int totalSpawns;


    //How long into the room spawning can still occur. After this time is reached, there will be no more spawning.
    [SerializeField]
    float spawnDuration;

	//The set difficulty. 1 for easy, 5 med, 10 hard.
	[SerializeField]
    int difficulty;

	//Based off of the set difficulty, will change dynamically as the player plays. Starts at 10
	[SerializeField]
    int adjustedDifficulty;


    //The total amount of time the spawn timer has to reach before there is another legal spawn event.
	//Adjusted by player performance and difficulty.
	[SerializeField]
    float totalSpawnTimer;

	//The max amount to spawn in each spawn event
	[SerializeField]
	int spawnMax;
	[SerializeField]
    int spawnMin;

	//If there are less enemies than this number, can spawn.
	[SerializeField]
	int enemyMinimum;
	
	[SerializeField]
	float difficultyAdjustTimerTotal_DAMAGE;
	[SerializeField]
    float difficultyAdjustTimerTotal_KILLS;

	//The amount of damage the player takes within a time period to adjust difficulty
	[SerializeField]
    int playerDamageThreshold;

	[SerializeField]
    int enemyKillThreshold;

    public void ResetDifficulty()
    {
        adjustedDifficulty = 1;
    }    
    
	//A lot of getters and setters
    
	public int GetDifficulty()
	{
		return difficulty;
	}
	
	public void SetDifficulty(int newDifficulty)
	{
		difficulty = newDifficulty;
	}
	
	public int GetAdjustedDifficulty()
	{
		return adjustedDifficulty;
	}
	
	public void SetAdjustedDifficulty(int newDifficulty)
	{
		adjustedDifficulty = newDifficulty;
	}
	
	public void SetPlayer(GameObject playerObj)
	{
		player = playerObj;
	}
	
	public float GetAdjustTimerDamage()
	{
		return difficultyAdjustTimerTotal_DAMAGE;
	}
	
	public float GetAdjustTimerKills()
	{
		return difficultyAdjustTimerTotal_KILLS;
	}
	
	public int GetMinSpawns()
	{
		return spawnMin;
	}
	
	public void SetMinSpawns(int newMin)
	{
		spawnMin = newMin;
	}
	
	public int GetMaxSpawns()
	{
		return spawnMax;
	}
	
	public void SetMaxSpawns(int newMax)
	{
		spawnMax = newMax;
	}
	
	
	public int GetTotalSpawns()
	{
		return totalSpawns;
	}
	
	public float GetSpawnDuration()
	{
		return spawnDuration;
	}
	
	public float GetTotalSpawnTimer()
	{
		return totalSpawnTimer;
	}
	
	public int GetEnemyMinimum()
	{
		return enemyMinimum;
	}
	
	public List<GameObject> GetEnemyList()
	{
		return enemyTypes;
	}
	
	public List<EnemySquad> GetSquadsList()
	{
		return enemySquads;
	}
	
	public int GetPlayerDamageThreshold()
	{
		return playerDamageThreshold;
	}
	
	public int GetEnemyKillThreshold()
	{
		return enemyKillThreshold;
	}
}

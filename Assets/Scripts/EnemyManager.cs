using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	// Start is called before the first frame update
	private static EnemyManager _instance;
	[SerializeField]
	private GameObject[] enemyPrefabList= null;
	[SerializeField]
	private int[] enemyCostList = null;
	[SerializeField]
	private Transform leftSpawnTransform = null;
	[SerializeField]
	private Transform rightSpawnTransform = null;
	[SerializeField]
	private float offsetMaxX=0;
	[SerializeField]
	private float offsetMaxYZ = 0;
	[SerializeField]
	private Tree tree = null;
	[SerializeField]
	private float timeBetweenSpawns;
	private float timeSinceSpawn = 0;

	public bool IsSpawning { get { return toSpawnList.Count > 0; } }

	private List<BasicEnemy> enemyList = new List<BasicEnemy>();
	public List<BasicEnemy> EnemyList { get { return enemyList; } }
	private List<BasicEnemy> toSpawnList = new List<BasicEnemy>();
	
	public static EnemyManager Instance
	{
		get
		{
			if (_instance == null) Debug.LogError("No EnemyManager");

			return _instance;
		}
	}
	void Start()
    {
		_instance = this;
		if(enemyPrefabList == null || enemyPrefabList.Length == 0) Debug.LogError("EnemyManager doesn't have Enemy Prefabs set.");
		else if(enemyCostList == null) Debug.LogError("EnemyManager doesn't have Enemy costs set.");
		else if(enemyPrefabList.Length<Enum.GetNames(typeof(EnemyType)).Length || enemyPrefabList.Length> enemyCostList.Length) Debug.LogError("EnemyManager doesn't have correct amount of prefabs/costs set.");
		if (leftSpawnTransform == null || rightSpawnTransform == null) Debug.LogError("EnemyManager doesn't have spawntransforms set.");
		if (tree == null) Debug.LogError("EnemyManager doesn't have tree set.");


		//TEST
		GenerateWave(30,80);
		//REMOVE LATER
	}

    // Update is called once per frame
    void Update()
    {
        if(IsSpawning)
		{
			timeSinceSpawn += Time.deltaTime;
			if(timeSinceSpawn > timeBetweenSpawns) 
			{
				timeSinceSpawn = 0;
				BasicEnemy enemyScript = toSpawnList[0];
				enemyScript.gameObject.SetActive(true);
				enemyList.Add(enemyScript);
				toSpawnList.RemoveAt(0);
			}
		}
    }
	public void SpawnEnemy(EnemyType type, bool spawnLeft = true, bool spawnInstantly = false)
	{
		GameObject enemy = Instantiate(enemyPrefabList[(int)type]);
		enemy.transform.position = (spawnLeft) ? leftSpawnTransform.position : rightSpawnTransform.position;
		float offsetYZ = UnityEngine.Random.Range(-offsetMaxYZ, offsetMaxYZ);
		float offsetX = UnityEngine.Random.Range(-offsetMaxX, offsetMaxX);
		enemy.transform.Translate(offsetX, offsetYZ, offsetYZ);
		BasicEnemy enemyScript = enemy.GetComponent<BasicEnemy>();
		enemyScript.SetTree(tree);
		if(spawnInstantly) enemyList.Add(enemyScript);
		else
		{
			toSpawnList.Add(enemyScript);
			enemyScript.gameObject.SetActive(false);
		}
	}
	public void RemoveEnemy(BasicEnemy enemy)
	{
		enemyList.Remove(enemy);
		Destroy(enemy.gameObject);
	}
	public void GenerateWave(int leftBudget, int rightBudget)
	{
		int typeNumber = 0;
		while(leftBudget> enemyCostList[0])
		{
			typeNumber = UnityEngine.Random.Range(0, enemyPrefabList.Length);
			if (enemyCostList[typeNumber]<leftBudget)
			{
				SpawnEnemy((EnemyType)typeNumber, true);
				leftBudget -= enemyCostList[typeNumber];
			}
		}
		while (rightBudget > enemyCostList[0])
		{
			typeNumber = UnityEngine.Random.Range(0, enemyPrefabList.Length);
			if (enemyCostList[typeNumber] < rightBudget)
			{
				SpawnEnemy((EnemyType)typeNumber, false);
				rightBudget -= enemyCostList[typeNumber];
			}
		}
		if(leftBudget+rightBudget > enemyCostList[0]) SpawnEnemy(0, leftBudget>rightBudget);
	}

	public BasicEnemy CheckForEnemy(Vector3 position, float range)
	{
		foreach(var enemy in enemyList)
		{
			if (Vector3.Distance(new Vector3(position.x, position.y, 0), new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0)) <= range) return enemy;
		}
		return null;
	}
}

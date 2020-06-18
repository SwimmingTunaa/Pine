using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Region : ScriptableObject
{
    public string tag;
    public ObjectPools panels;
    public ObjectPools projectiles;
    public List<ObstaclePoolConfig> obstaclePoolsLevel = new List<ObstaclePoolConfig>();
    public List<ObstaclePoolConfig> obstaclePoolsLevelInstances;
    
    public void Initialise()
    {
        panels.Initialise();
        projectiles.Initialise();
        //creates the starting Obstacles
        obstaclePoolsLevelInstances = new List<ObstaclePoolConfig>();

        for (int i = 0; i < 4; i++)
        {
            if(i < obstaclePoolsLevel.Count)
            {   
                //create an instance for editing purposes
                ObstaclePoolConfig instance = Instantiate(obstaclePoolsLevel[i]);
                obstaclePoolsLevelInstances.Add(instance);
                
                //creates whatever pool that has been allocated
                obstaclePoolsLevelInstances[i].Initialise();
                //add pools to a list for easier configuration
                obstaclePoolsLevelInstances[i].CreateList();
                for (int z = 0; z < obstaclePoolsLevelInstances[i].poolList.Count; z++)
                {
                    //checks to see if the level had empty pools
                    if(obstaclePoolsLevelInstances[i].poolList[z] == null)
                    //replace empty pool with the previous level's pool
                    if(obstaclePoolsLevelInstances[i - 1].poolList[z] != null)
                        obstaclePoolsLevelInstances[i].poolList[z] = obstaclePoolsLevelInstances[i - 1].poolList[z];
                }
            }
            else
            {
                obstaclePoolsLevelInstances.Add(obstaclePoolsLevelInstances[i - 1]);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(menuName = "Config/Obstacle Pool Config")]
public class ObstaclePoolConfig: ScriptableObject
{
    public ObstaclePool top;
    public ObstaclePool mid;
    public ObstaclePool bot;
    [HideInInspector] public List<ObstaclePool> poolList = new List<ObstaclePool>();

    public void CreateList()
    {
        if(top != null)
            poolList.Add(top);
        if(mid != null)
            poolList.Add(mid);
        if(bot != null)
            poolList.Add(bot);
    }

    public void Initialise()
    {
        if(top != null)
            top.Initialise();
        if(mid != null)
            mid.Initialise();
        if(bot != null)
            bot.Initialise();
    }
}

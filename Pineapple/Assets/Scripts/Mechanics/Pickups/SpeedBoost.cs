using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : PickUpsBase
{
    //TODO:
    //Stop Obstacle Spawners
    //turn off player collisions
    //make player fly up (to avoid floor evelation when landing) and then fly foward
    //player stay at constant height
    //last n seconds
    //player cant get hurt/die
    //turn everything back on
    public float distanceToTravel;
    public float boostSpeed = 20;
    public float minHeightThreshold = 3;
    public LayerMask groundCheck;
    public float transitionSpeed;

    Vector3 targetPos;
    GameObject _player;
    bool _active;
    bool boost;
    Vector2 currentPos;
   
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && triggerAmount > 0)
        {
            DoAction(other.gameObject);
        }
    }

    public override void Update()
    {
        if(_active)
        {
            if(_player.transform.position.y <= targetPos.y && !boost)
            {
                MovePlayerTowardsPos(targetPos, transitionSpeed);
            }
        
            if(boost)
            {
                MovePlayerTowardsPos(targetPos, boostSpeed);
                if(_player.transform.position.x >= targetPos.x)
                    {
                        boost = false;
                        _active = false;
                    }
            }  
            
            
        }
    }

    public override void DoAction(GameObject player)
    {
        base.DoAction(player);
       StartCoroutine(Activate(player, true));        
    }

    IEnumerator Activate(GameObject player, bool toggle)
    {
         _player = player;

        //turn off the master spawner
        MasterSpawner.Instance.gameObject.SetActive(false);
        //turn off player collision
        player.GetComponent<Collider2D>().enabled = false;
        //turn on kinematic
        player.GetComponent<Rigidbody2D>().isKinematic = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        //turn off player controller
        player.GetComponent<PlayerController>().pausePlayer = true;

        //check players distance from ground and make sure its above a minimum height
        RaycastHit2D hit2D = Physics2D.Raycast(player.transform.position, -player.transform.up, 8f, groundCheck);
        currentPos = new Vector3(player.transform.position.x, hit2D.point.y + minHeightThreshold); 
        if(Vector2.Distance(player.transform.position, hit2D.point) < minHeightThreshold)
        {
            targetPos = currentPos; 
            _active = true;
        }
        else targetPos =  currentPos + Vector2.right * distanceToTravel;   

        Time.timeScale = 0;  
        yield return new WaitForSecondsRT(.75f);
        Time.timeScale = 1;
        boost = true;
        targetPos =  currentPos + (Vector2.right * distanceToTravel);
        
    }

    void MovePlayerTowardsPos(Vector3 targetPos,float speed)
    {
        _player.transform.position = Vector3.MoveTowards(_player.transform.position, targetPos, speed * Time.unscaledDeltaTime);
    }
}

using UnityEngine;
using UnityEngine.Rendering;

public class MovingPlatform : MonoBehaviour
{
    public GameObject[] waypoints;
    public float speed;
    private int waypointIndex;

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        if (Vector3.Distance(transform.position, waypoints[waypointIndex].transform.position) < 0.1f)
        {
            waypointIndex++;

            if (waypointIndex >= waypoints.Length) 
            {
                waypointIndex = 0;
            }
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex].transform.position, speed* Time.deltaTime);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
        //if (collision.gameObject.CompareTag("Player"))
        //{
            //collision.gameObject.transform.SetParent(transform);
        //}
    //}

    //private void OnCollisionExit(Collision collision)
    //{
        //if(collision.gameObject.CompareTag("Player"))
        //{
            //collision.gameObject.transform.SetParent(null);
        //}
    //}
}


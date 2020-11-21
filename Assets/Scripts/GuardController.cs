using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardController : MonoBehaviour
{
    public Transform pathHolder;
    public float guardSpeed;
    public float guardWaitTime;
    public float turnSpeed = 90;

    void Start(){
        int len = pathHolder.childCount;
        Vector3[] waypoints = new Vector3[len];
        for(int i = 0; i < len; ++i){
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));
    }


    IEnumerator FollowPath(Vector3[] waypoints)
    {

        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, guardSpeed * Time.deltaTime);

            if(transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(guardWaitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            // yield for one frame at end of each iteration of while loop
            // this is so the guard doesn't teleport to next position but moves there frame by frame 
            yield return null;
        }
        
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        // Angle guard needs on y-axis to face look target 
        Vector3 dirLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirLookTarget.z, dirLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) > 0.05f)
        {
            //Debug.Log(transform.eulerAngles.y);
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null; 
        }

    }
    
    void OnDrawGizmos(){
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach(Transform waypoint in pathHolder){
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
}

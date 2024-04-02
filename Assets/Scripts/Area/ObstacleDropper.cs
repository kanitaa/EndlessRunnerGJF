using System.Collections;
using UnityEngine;

public class ObstacleDropper : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DropObstacle());
    }

    private IEnumerator DropObstacle()
    {
        //The range of x positions where obstacles can spawn.
        int minXPosition = GameManager.Instance.MinPathValue;
        int maxXPosition = GameManager.Instance.MaxPathValue;

        while (true)
        {
            //Get even number for x position because lane width is 2 units.
            int randomXPosition = Random.Range(minXPosition / 2, maxXPosition / 2 + 1) * 2;

            //Get a random obstacle.
            int randomIndex = Random.Range(0, ObjectPoolManager.Instance.ObstaclePoolsDictionary.Count);
            GameObject obstacle = ObjectPoolManager.Instance.ObstaclePoolsDictionary[randomIndex].Get();

            //Set it's position to the random x.
            Vector3 obstaclePosition = transform.position;
            obstaclePosition.x = randomXPosition;
            obstacle.transform.position = obstaclePosition;

            //Wait a bit before spawning the next obstacle.
            yield return new WaitForSeconds(2);
        }
    }
}

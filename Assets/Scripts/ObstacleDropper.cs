using System.Collections;
using UnityEngine;

public class ObstacleDropper : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstacles; 

    private void Start()
    {
        StartCoroutine(DropObstacle());

    }
  

    private IEnumerator DropObstacle()
    {
        // Define the step
        int step = 2;

        // Calculate the number of steps needed to cover the range
        int numSteps = (GameManager.Instance.MaxPathValue - GameManager.Instance.MinPathValue) / step + 1;

        // Initialize the current position
        int currentPositionIndex = 0;

        while (true)
        {
            // Calculate the current x position
            int currentXPosition = GameManager.Instance.MinPathValue + currentPositionIndex * step;

            // Ensure the current position is within the range
            if (currentXPosition <= GameManager.Instance.MaxPathValue)
            {
                int random = Random.Range(0, ObjectPoolManager.Instance.PoolsDictionary.Count);
                GameObject obstacle = ObjectPoolManager.Instance.PoolsDictionary[random].Get();
           
                Vector3 obstaclePosition = transform.position;
                obstaclePosition.x = currentXPosition;
                obstacle.transform.position = obstaclePosition;
               
            }

            // Move to the next position index
            currentPositionIndex = (currentPositionIndex + 1) % numSteps;

            yield return new WaitForSeconds(2);
        }
    }
}

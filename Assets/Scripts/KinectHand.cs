using UnityEngine;
using UnityEngine.SceneManagement;

public class KinectHand : MonoBehaviour
{
    public PlayerPoints playerPoints;

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "play")
        {
            SceneManager.LoadScene("Game");
        }
        else
        {
            playerPoints.Points += 0.25f;
        }
    }
}

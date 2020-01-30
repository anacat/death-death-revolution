using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{
    public bool exit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (exit)
            {
                Application.Quit();
            }
            else
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}

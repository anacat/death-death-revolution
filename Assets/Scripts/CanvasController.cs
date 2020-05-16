using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject win;
    public GameObject lose;

    public PlayerPoints playerPoints;
    public Image leftSlider;
    public Image rightSlider;

    public GameObject kinectView;

    private float elapsedTime;
    private float timeSinceStart;
    private bool _lost;
    private float _lastPoints;
    private float _pointTimer;

    private CoordinateMapperView _kinectThing;

    private void Start()
    {
        playerPoints.Points = 0;

        _kinectThing = FindObjectOfType<CoordinateMapperView>();
    }

    private bool addedRigidBody;

    private void Update()
    {
        if (_kinectThing.bodyFound)
        {
            leftSlider.fillAmount = playerPoints.points / 100;
            rightSlider.fillAmount = playerPoints.points / 100;

            timeSinceStart += Time.deltaTime;

            if (playerPoints.Points >= 100)
            {
                win.SetActive(true);
                leftSlider.gameObject.SetActive(false);
                rightSlider.gameObject.SetActive(false);
            }
            else if (playerPoints.Points <= 0 && timeSinceStart > 10f)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime >= 3f)
                {
                    _lost = true;

                    lose.SetActive(true);
                    leftSlider.gameObject.SetActive(false);
                    rightSlider.gameObject.SetActive(false);

                    if (elapsedTime >= 5f && kinectView.GetComponent<Rigidbody>() == null)
                    {
                        kinectView.AddComponent<Rigidbody>();
                    }
                }
            }
            else
            {
                if (_lastPoints == playerPoints.Points)
                {
                    _pointTimer += Time.deltaTime;
                }
                else
                {
                    _pointTimer = 0f;
                }

                if (_pointTimer > 2f)
                {
                    playerPoints.Points -= 0.5f;
                }
            }

            if (playerPoints.Points > 0 && !_lost)
            {
                elapsedTime = 0;
            }

            _lastPoints = playerPoints.Points;
        }
    }
}

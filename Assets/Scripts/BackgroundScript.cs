using UnityEngine;

public class BackgroundScript : MonoBehaviour
{
    public PlayerPoints playerPoints;

    private Material _material;

    /*private void Awake()
    {
        _material = GetComponent<Renderer>().material;
    }

    public void Update()
    {
        if (playerPoints.win)
        {
            _material.SetInt("_State", 1);
        }
        else if (playerPoints.lost)
        {
            _material.SetInt("_State", 2);
        }
    }*/
}

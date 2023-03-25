using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    // Screen height divded by pixels per unit sprites for consistent camera size
    const int DEFAULT_ZOOM = 6;

    public int Zoom
    {
        set
        {
            Camera.main.orthographicSize = Screen.height / 16 / value;
        }
    }

    public Transform Target { get; set; }

    // Use this for initialization
    void Start()
    {
        Target = Actors.PlayerAvatarControl.GetPlayerInstance().transform;
        Zoom = DEFAULT_ZOOM;
    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            transform.position = Target.transform.position + new Vector3(0.0f, 0.0f, -10.0f);
        }
    }

}
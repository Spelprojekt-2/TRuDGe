using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Camera cam;
    [SerializeField] private float thresh = 50f;
    [SerializeField] private float markiplier = 3f;

    private ParticleSystem ps;
    private Color col;
    private bool check = false;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();

        RacerData data = transform.root.GetComponentInChildren<RacerData>();
        int layer;
        int playerIndex = data.index;
        switch (playerIndex)
        {
            case 0:
                layer = LayerMask.NameToLayer("Player1fx");
                ps.gameObject.layer = layer;
                cam.cullingMask |= 1 << layer;
                break;
            case 1: ps.gameObject.layer = LayerMask.NameToLayer("Player2fx"); 
                layer = LayerMask.NameToLayer("Player2fx");
                ps.gameObject.layer = layer;
                cam.cullingMask |= 1 << layer;
                break;
            case 2: ps.gameObject.layer = LayerMask.NameToLayer("Player3fx"); 
                layer = LayerMask.NameToLayer("Player3fx");
                ps.gameObject.layer = layer;
                cam.cullingMask |= 1 << layer;
                break;
            case 3: ps.gameObject.layer = LayerMask.NameToLayer("Player4fx"); 
                layer = LayerMask.NameToLayer("Player4fx");
                ps.gameObject.layer = layer;
                cam.cullingMask |= 1 << layer;
                break;
        }
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;
        if ((speed > thresh) && !check)
        {
            ps.Play();
            check = true;
        }
        float alpha = (speed - thresh)*markiplier;
        if (alpha < 0) alpha = 0;
        col = new Color32(255,255,255,(byte)alpha);
        ps.startColor = col;
    }
}

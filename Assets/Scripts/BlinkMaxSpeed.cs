
using TMPro;
using UnityEngine;

public class BlinkMaxSpeed : MonoBehaviour
{
    public Color startColor = Color.green;
    public Color endColor = Color.black;
    [Range(0, 10)]
    public float speed = 1f;

    private TextMeshProUGUI textMesh;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        float rate = Time.time * speed;
        textMesh.color = Color.Lerp(startColor, endColor, Mathf.PingPong(rate, 1));
    }
}

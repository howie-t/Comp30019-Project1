using UnityEngine;
using System.Collections;

public class PointLight : MonoBehaviour {

    public Color color;

    void Start()
    {
        this.color = new Color(0.435f, 0.388f, 0.188f);
        this.transform.position = new Vector3(75.0f,0.0f,32.0f);
    }

    void Update()
    {
        transform.RotateAround(new Vector3(32.0f,0.0f,32.0f), Vector3.forward, 20 * Time.deltaTime);
    }
    public Vector3 GetWorldPosition()
    {
        return this.transform.position;
    }
}

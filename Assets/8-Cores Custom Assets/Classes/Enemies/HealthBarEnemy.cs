using UnityEngine;

[System.Serializable]
public class HealthBarEnemy : MonoBehaviour
{
    [HideInInspector]
    public GameObject enemy;

    [HideInInspector]
    public Camera cam;

    [HideInInspector]
    public float healthbarValue;

    [HideInInspector]
    public float healthbarWidth;

    [HideInInspector]
    public Vector3 healthbarSize;

    //Used to move healthbar y position.
    public float healthbarYPositionOffset = 1f;

    private void Start()
    {
        enemy = this.transform.parent.gameObject;
        cam = Camera.main;
    }

    private void Update()
    {
        healthbarValue = enemy.GetComponent<BaseEnemy>().health;

        //TO CHECK, value may vary.
        healthbarWidth = healthbarValue;

        healthbarSize = new Vector3(healthbarWidth * 0.001f, 0.4f, 0.01f);

        this.transform.localScale = healthbarSize;

        //healthbarSize = Vector3.zero;

        MoveAndRotateHealthBar(enemy.transform, cam.transform);
    }

    private void MoveAndRotateHealthBar(Transform enemy, Transform camera)
    {
        this.transform.position = enemy.position + new Vector3(0, healthbarYPositionOffset, 0);
        
        transform.LookAt(camera.position);
        transform.rotation = Quaternion.LookRotation(camera.position - transform.position);
        transform.Rotate(new Vector3(-180,  0, 180));
        //transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.y, transform.rotation.z));

        //this.transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}




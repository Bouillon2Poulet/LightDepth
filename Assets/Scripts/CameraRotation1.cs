using UnityEngine;

public class RotationAroundWorldCenter : MonoBehaviour
{
    public float rotationSpeed = 30.0f;  // Vitesse de rotation en degrés par seconde
    public float radius = 10.0f;        // Rayon de l'orbite
    public GameObject target;

    private void Update()
    {
        // Calculer l'angle en radians en fonction du temps
        float angle = Time.time * rotationSpeed * Mathf.Deg2Rad;

        // Calculer les nouvelles positions x et z en fonction de l'angle et du rayon
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;

        // Mettre à jour la position de l'objet sur le plan (x, z)
        transform.position = target.transform.position + new Vector3(x, transform.position.y, z);
        transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position, new Vector3(0, 1, 0));
    }
}

using UnityEngine;

public class CratePickup : MonoBehaviour
{
    public GameObject weaponPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform socket = other.transform.Find("PlayerModel/WeaponSocket");

            if (socket != null)
            {
                // Clear old weapon
                foreach (Transform child in socket)
                {
                    Destroy(child.gameObject);
                }

                // Spawn and scale
                GameObject weapon = Instantiate(weaponPrefab, socket);
                weapon.transform.localScale = Vector3.one;

                // Grip allignment
                Transform grip = weapon.transform.Find("WeaponGrip");

                if (grip != null)
                {
                    weapon.transform.position = socket.position - grip.localPosition;
                    weapon.transform.rotation = socket.rotation * Quaternion.Inverse(grip.localRotation);
                }
                else
                {
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = Quaternion.identity;
                }
            }

            Destroy(gameObject);
        }
    }
}
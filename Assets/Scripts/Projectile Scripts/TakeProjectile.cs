using DG.Tweening;
using UnityEngine;

namespace Projectile_Scripts
{
    public class TakeProjectile : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.transform.DOKill();
                Destroy(other.gameObject);
            }
        }
    }
}
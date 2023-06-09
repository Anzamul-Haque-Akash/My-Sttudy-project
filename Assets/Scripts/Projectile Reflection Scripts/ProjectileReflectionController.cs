using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Projectile_Reflection_Scripts
{
    public class ProjectileReflectionController : MonoBehaviour
    {
        [Title("Projectile Data")] 
        [SerializeField] private float m_Speed;
        
        [Title("Projectile Prefab")] 
        [SerializeField] private GameObject m_Projectile;
        
        private InitialAimRaycustController _initialAimRaycustController;

        private void Awake()
        {
            _initialAimRaycustController = GetComponent<InitialAimRaycustController>();
        }

        private void Start()
        {
            _initialAimRaycustController.OnRayHit += SpawnProjectile;
        }

        private void SpawnProjectile(RaycastHit hit)
        {
            GameObject projectile = Instantiate(m_Projectile, transform.position, Quaternion.identity);
            Vector3 initialPosition = projectile.transform.position;
            
            projectile.transform.DORotate(new Vector3(360f, 0f, 0f), 0.6f, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart);
                
            ThrowProjectile(projectile, initialPosition, hit);
        }

        private void ThrowProjectile(GameObject projectile, Vector3 initialPosition , RaycastHit hit)
        {
            float distance = Vector3.Distance(projectile.transform.position, hit.point);
            float duration = distance / m_Speed; 
            
            projectile.transform.DOMove(hit.point, duration).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Vector3 projectileDirection = (hit.point - initialPosition).normalized;

                Vector3 reflection = projectileDirection - 2f * Vector3.Dot(hit.normal, projectileDirection) * hit.normal;
                
                
                Ray ray = new Ray(hit.point, reflection);
                RaycastHit refelctionHit;

                if (Physics.Raycast(ray, out refelctionHit))
                {
                    ThrowProjectile(projectile, projectile.transform.position, refelctionHit);
                }
            });
        }
    }
}
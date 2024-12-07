using UnityEngine;

namespace SpaceShooter
{
    public class ColisionDamageApplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundary";

        [SerializeField] private float m_VelosityDamageModifier;

        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.transform.CompareTag(IgnoreTag) || collision.transform.CompareTag("Wall"))

                return;

            var destructable = transform.root.GetComponent<Destructible>();

            if (destructable != null)
            {
                destructable.ApplyDamage((int)m_DamageConstant + 
                                                      (int)(m_VelosityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}

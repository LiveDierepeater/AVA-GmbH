using UnityEngine;
using Karts;

namespace Environment
{
    public class TrashBin : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            // Return if colliding Object is a Unit.
            if (other.transform.GetComponent<CarComponent>() is not null)
                Destroy(other.gameObject);
        }
    }
}

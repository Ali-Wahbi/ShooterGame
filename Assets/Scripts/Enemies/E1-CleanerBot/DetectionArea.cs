using UnityEngine;
using UnityEngine.Events;
public class DetectionArea : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider2D> OnDetectEnter;
    [SerializeField] UnityEvent OnDetectExit;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnDetectEnter.Invoke(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            OnDetectExit.Invoke();
        }
    }
}

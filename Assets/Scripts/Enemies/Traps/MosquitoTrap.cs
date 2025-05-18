using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

using Exentsions;
public class MosquitoTrap : MonoBehaviour
{
    [SerializeField] SpriteRenderer Radar;
    [SerializeField] Ease RadarEase;
    [SerializeField] float RadarDelay = 0.5f, RadarDuration = 1f;
    bool isDestroyed = false;
    float RadarValue
    {
        set
        {
            Radar.material.SetFloat("_value", value);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        AnimateRadar();
    }


    // <summary>
    /// animation for the radar using the shader material attached
    /// </summary>
    void AnimateRadar()
    {

        DOTween.To(() => 0f, x => RadarValue = x, 1f, RadarDuration).SetEase(RadarEase)
        .SetDelay(RadarDelay)
        .SetLoops(-1, LoopType.Restart)
        .OnComplete(() => CheckDestroyed());
    }

    void CheckDestroyed()
    {
        if (isDestroyed) Radar.enabled = false;
    }
    /// <summary>
    /// Change the trajectory of the buller
    /// </summary>
    /// <param name="bullet">the player's bullet that entered the area</param>
    void DissperseBullet(Transform bullet)
    {
        float rotationZ = bullet.eulerAngles.z;

        float rotationOffset = 160f;
        float newRotation = Random.Range(rotationZ - rotationOffset, rotationZ + rotationOffset);
        Debug.Log("Bullet new rotation: " + newRotation + " Zdegrees");
        bullet.eulerAngles = bullet.eulerAngles.With(z: newRotation);
    }

    // called by the EnemyStats component
    public void OnDefeated()
    {
        isDestroyed = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }

    // set in the editor by the detection area child
    /// <summary>
    /// detect the bullet that entered the radar area
    /// </summary>
    /// <param name="collider">the player's bullet collider that entered the area</param>
    public void onBulletEntered(Collider2D collider)
    {
        if (isDestroyed) return;
        if (collider.CompareTag("PlayerBullet"))
        {
            Debug.Log("Player bullet etnered");
            DissperseBullet(collider.transform);

        }
    }

}

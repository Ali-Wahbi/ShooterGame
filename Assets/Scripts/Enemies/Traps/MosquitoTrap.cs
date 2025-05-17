using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

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
    /// animatio for the radar using the shader material attached
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

    public void OnDefeated()
    {
        isDestroyed = true;
    }

    public void onBulletEntered(Collider2D collider)
    {
        if (isDestroyed) return;
        if (collider.CompareTag("PlayerBullet"))
        {
            Debug.Log("Player bullet etered");


        }
    }

}

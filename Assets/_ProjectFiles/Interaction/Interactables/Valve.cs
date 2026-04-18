using System;
using DG.Tweening;
using UnityEngine;

public class Valve : MonoBehaviour, IInteractable
{
    public float duration = 3f;

    [Header("Valve Rotation")]
    public Vector3 valveRotation;

    [Header("Door Rotation")]
    public Transform door;
    public Vector3 doorRotation;

    private float _progress = 0f;
    private Tween _tween;

    public InteractionType GetInteractionType() => InteractionType.Hold;
    public InteractionName GetInteractionName() => InteractionName.Rotate;

    public void OnInteractStart(PlayerInteract player)
    {
        _tween?.Kill();

        _tween = DOTween.To(
                () => _progress,
                x =>
                {
                    _progress = x;
                    Apply();
                },
                1f, duration * (1f - _progress))
            .SetEase(Ease.Linear);
    }

    public void OnInteract(PlayerInteract player)
    {
        
    }

    public void OnInteractEnd(PlayerInteract player)
    {
        _tween?.Kill();

        _tween = DOTween.To(
                () => _progress,
                x =>
                {
                    _progress = x;
                    Apply();
                },
                0f, duration * _progress
            )
            .SetEase(Ease.OutSine);
    }

    private void Apply()
    {
        transform.localRotation = Quaternion.Euler(valveRotation * _progress);
        door.localRotation = Quaternion.Euler(doorRotation * _progress);
    }

    public void OnHoverEnter() { }
    public void OnHoverExit() { }
}
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Note : PickableItem
{
    public Transform pivot;
    public Vector3 rotation;
    public float duration = 0.8f;
    
    public override void OnInspect()
    {
        _ = DelayedOpen();
    }

    private async UniTaskVoid DelayedOpen()
    {
        await UniTask.WaitForSeconds(0.5f);
        pivot.DOLocalRotate(rotation, duration);
    }
}

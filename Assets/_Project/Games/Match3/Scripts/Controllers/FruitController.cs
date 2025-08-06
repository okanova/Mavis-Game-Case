using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class FruitController : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Collider _collider;
    
    private PoolObjectType _type;
    public PoolObjectType Type => _type;
    
    public void Intialize(FruitDatabase fruitDatabase, PoolObjectType type)
    {
        _type = type;
        _collider = GetComponent<Collider>();
        
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        
        Vector3 randomOffset = Random.insideUnitSphere;
        randomOffset.y = Mathf.Abs(randomOffset.y);
        randomOffset = randomOffset.normalized * fruitDatabase.spawnRadius;

        transform.position = fruitDatabase.spawnPosition + randomOffset;
        transform.rotation = Random.rotation;

        _rigidbody.isKinematic = false;
    }

    private void CloseFruit()
    {
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
        transform.DORotate(Vector3.zero, 0.5f);
    }

    private void OnMouseUp()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;
        
        GameManager.Instance.ChangeState(GameState.Paused);
        CloseFruit();
        FruitArgs args = new FruitArgs(this);
        EventManager.InvokeEvent(GameEvents.ChooseFruit, args);
    }
}
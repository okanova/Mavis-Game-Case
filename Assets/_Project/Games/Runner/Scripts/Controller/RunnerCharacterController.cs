using System;
using UnityEngine;
using _Project.Scripts.Core;

public class RunnerCharacterController : MonoBehaviour
{
    [SerializeField] private RunnerCharacterSettings settings;

    private int currentLane = 1; // 0 = sol, 1 = orta, 2 = sağ
    private Vector3 targetPosition;
    private float lastZ;

    private void OnEnable()
    {
        InputEvents.OnSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        InputEvents.OnSwipe -= OnSwipe;
    }

    private void Start()
    {
        targetPosition = transform.position;
        Camera.main.GetComponent<CameraFollow>().SetTarget(transform);
    }

    #region MOVEMENT

    #region FORWARD

    private void Update()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;
        
        transform.Translate(Vector3.forward * settings.forwardSpeed * Time.deltaTime);
        
        Vector3 desiredPosition = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, settings.laneSwitchSpeed * Time.deltaTime);

        if (transform.position.z - 50 > lastZ)
        {
            lastZ += 50;
            EventManager.InvokeEvent(GameEvents.GenerateMap);
        }
    }

    #endregion

    #region SIDE

    private void OnSwipe(Vector2 direction)
    {
        if (GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            bool right = direction.x > 0;
            MoveLane(right);
        }
    }

    private void MoveLane(bool goingRight)
    {
        currentLane += goingRight ? 1 : -1;
        currentLane = Mathf.Clamp(currentLane, 0, 2);

        float targetX = (currentLane - 1) * settings.laneDistance;
        targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    #endregion

    #endregion

    #region TRIGGER

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gold"))
        {
            CurrencyArgs args = new CurrencyArgs(Currencies.gold, 1);
            EventManager.InvokeEvent(GameEvents.Currency, args);
            ObjectPoolManager.ReturnObject(PoolObjectType.Gold, other.gameObject);
        }
        else if (other.CompareTag("Obstacle"))
        {
            GameManager.Instance.ChangeState(GameState.GameOver);
        }
    }

    #endregion
   
}
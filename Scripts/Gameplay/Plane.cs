using UnityEngine;

public class Plane : MonoBehaviour
{
    private Vector2 _targetPosition;
    private bool _isActive;

    private void Start()
    {
        _targetPosition = Vector2.zero;
    }
    private void OnEnable()
    {
        PlayerInput.Instance.PlayerMouseDown += OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp += OnPlayerMouseUp;
    }
    private void OnDisable()
    {
        PlayerInput.Instance.PlayerMouseDown -= OnPlayerMouseDown;
        PlayerInput.Instance.PlayerMouseUp -= OnPlayerMouseUp;
    }
    private void FixedUpdate()
    {
        if (!_isActive)
            return;
        if (GameState.Instance.CurrentState != GameState.State.InGame)
            return;

        _targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(_targetPosition.x, transform.position.y);
    }
    private void OnPlayerMouseDown()
    {
        _isActive = true;
    }
    private void OnPlayerMouseUp()
    {
        _isActive = false;
    }
}

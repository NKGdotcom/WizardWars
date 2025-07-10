using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("プレイヤーの情報")]
    [SerializeField] private PlayerData playerData;
    [Header("回転を行う閾値")]
    [SerializeField] private float minRotationMagnitude = 0.01f;

    private Vector3 mousePos;
    private Rigidbody playerRb;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if (GameStateMachine.Instance.IsPlaying())
        {
            Plane _plane = new Plane(Vector3.up, 0);
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(_ray, out float _distance))
            {
                mousePos = _ray.GetPoint(_distance);
            }

            Vector3 _targetPosition = Vector3.MoveTowards(playerRb.position, mousePos, playerData.PlayerMoveSpeed * Time.fixedDeltaTime);
            playerRb.MovePosition(_targetPosition);

            Vector3 _direction = mousePos - playerRb.position;
            if(_direction.sqrMagnitude > minRotationMagnitude)
            {
                Quaternion _mousePositionRotation = Quaternion.LookRotation(_direction);
                playerRb.rotation = Quaternion.RotateTowards(playerRb.rotation, _mousePositionRotation, playerData.PlayerRotationSpeed * Time.fixedDeltaTime);
            }
        }
    }
}

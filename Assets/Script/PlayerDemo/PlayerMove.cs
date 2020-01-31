using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの行動
/// </summary>
public class PlayerMove : MonoBehaviour
{

    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float cameraSpeed = 3.0f;

    [SerializeField]
    private new Rigidbody rigidbody;

    [SerializeField]
    private Animator animator;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        //移動
        Move();
        //カメラ操作
        CameraMove();
    }

    /// <summary>
    /// プレイヤーの行動
    /// </summary>
    private void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * speed * Time.deltaTime;
            animator.SetBool("IsDash", true);
        }
        else
        {
            animator.SetBool("IsDash", false);
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position -= transform.right * speed * Time.deltaTime;
        }
    }

    /// <summary>
    /// カメラの操作
    /// </summary>
    private void CameraMove()
    {
        float rotationX = Input.GetAxis("Mouse X") * cameraSpeed;
        //Y軸を更新します（キャラクターを回転）取得したX軸の変更をキャラクターのY軸に反映します
        transform.Rotate(0, rotationX, 0);
    }
}

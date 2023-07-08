using System.Collections;
using System.Collections.Generic;
using Touch.CustomGravity;
using Touch.PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class GravityUI : MonoBehaviour
{
    public Image Fill;
    public RectTransform Pointer;
    public float RotateSpeed = 3f;
    private float _maxGChangeCd;

    private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _maxGChangeCd = _playerController.ChangeGravityColdTime;
    }

    // Update is called once per frame
    void Update()
    {
        Fill.fillAmount = 1 -  Mathf.Clamp(_playerController.CurrentGChangeCd, 0, _maxGChangeCd) / _maxGChangeCd;
        var gDir = GlobalGravity.Instance.GravityDirection;
        var angle = Mathf.Acos(gDir.x);
        if (gDir.y < 0) angle = -angle;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
        Pointer.rotation = Quaternion.Slerp(Pointer.rotation, targetRotation, RotateSpeed * Time.unscaledDeltaTime);
    }
}

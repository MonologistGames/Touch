using System.Collections;
using System.Collections.Generic;
using Touch.PlayerController;
using UnityEngine;
using UnityEngine.UI;

public class EnergyUI : MonoBehaviour
{
    public Image Fill;
    public Image Frame;
    public Color ExhaustedColor;
    public float FadeTime = 0.5f;
    private float _currentUnchangedTime;
    private float _maxEnergy;

    #region MonoBehaviour Callbacks
    
    // Start is called before the first frame update
    void Start() 
    { 
        var playerController = FindObjectOfType<PlayerController>();
        _maxEnergy = playerController.FloatingEnergy;
        playerController.OnFloatingEnergyChanged += UpdateEnergy;
    }
    
    
    void Update()
    {
        _currentUnchangedTime += Time.deltaTime;
        if (_currentUnchangedTime > FadeTime)
        {
            Frame.color = new Color(0, 0,0, 0);
            Fill.color = new Color(0, 0,0, 0);
        }
    }
    
    #endregion
    
    private void UpdateEnergy(float currentEnergy, bool isExhausted)
    {
        Fill.color = Color.white;
        Frame.color = Color.white;
        Fill.fillAmount = currentEnergy / _maxEnergy;
        _currentUnchangedTime = 0f;
    }
}

using UnityEngine;

public class BuildingObject : MonoBehaviour
{
    public BuildingData Data => _data;
    public bool IsOnFire
    {
        get => _isOnFire;
        set
        {
            if (_isOnFire == value)
                return;

            _isOnFire = value;

            if (_isOnFire)
            {
                _fireEffect.Play();
            }
            else
            {
                _fireEffect.Stop();
            }
        }
    }

    [SerializeField] private BuildingData _data;
    private ParticleSystem _fireEffect;

    private bool _isOnFire;

    public void Init(BuildingData data)
    {
        _data = data;
        IsOnFire = false;

        _fireEffect = GetComponentInChildren<ParticleSystem>();
    }
}
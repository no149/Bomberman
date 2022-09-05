using UnityEngine;
using System.Collections.Generic;
public class WandererBadGuy : BadGuy
{
    Location _currentLocation;
    RoamingCharacterLocationMap _roamingCharacterMap;
    private Vector2 _destpos;
    private float _destangle;
    private Vector2 _initialLocation;
    private Location nextLocation;
    private Vector2 _currentVelocity;

    public void Init()
    {
        _roamingCharacterMap = new RoamingCharacterLocationMap(this);
        _roamingCharacterMap.Init();
    }
    protected override void Move()
    {
        if (_roamingCharacterMap.Ready)
        {
            var currentDistance = (_rb2D.position - _destpos).sqrMagnitude;
            if (currentDistance > 0.01 && nextLocation.Equals(Location.Default) == false)
            {

                _rb2D.velocity = _currentVelocity;

            }
            else
            {
                nextLocation = _roamingCharacterMap.GetNextLocation();
                _destpos = _roamingCharacterMap.ConvertToWorldPosition(nextLocation);
                _destangle = Mathf.Atan2(_destpos.y - _rb2D.position.y, _destpos.x - _rb2D.position.x);
                var speed = Time.fixedDeltaTime * BaseSpeed;
                var sin = Mathf.Round(Mathf.Sin(_destangle));
                var cos = Mathf.Round(Mathf.Cos(_destangle));
                _currentVelocity = new Vector2(speed * cos, speed * sin);
            }
        }
    }

    protected override void Start()
    {
        Init();
        base.Start();
        _initialLocation = _rb2D.position;
    }
}
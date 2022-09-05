public struct Location
{
    static Location _default;
    public Location(float x, float y, bool blocked)
    {
        this.x = x;
        this.y = y;
        this.blocked = blocked;
    }

    public static Location Default
    {
        get
        {
            return _default;
        }
    }
    public float x;
    public float y;
    public bool blocked;

    public override bool Equals(object obj)
    {
        if (obj is Location == false)
            return false;

        return ((Location)obj).x == x && ((Location)obj).y == y;
    }

    public override string ToString()
    {
        return string.Format("x:{0}, y:{1}, blcoked:{2}", x, y, blocked);
    }

    public override int GetHashCode()
    {
        return (int) System.Math.Pow(2,x*y);
    }
}

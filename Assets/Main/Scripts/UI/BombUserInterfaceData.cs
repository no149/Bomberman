using System.Collections.Generic;
using System.Linq;
public class BombUserInterfaceData
{

    public IEnumerable<BombTypeCount> BombTypesCount { get; set; }

    public int this[Bomb.BombType type]
    {
        get
        {
            var bombTypeCount = BombTypesCount.SingleOrDefault(bombCount => bombCount.BombType == type);
            return bombTypeCount?.Count ?? 0;
        }
        set
        {
            var bombTypeCount = BombTypesCount.SingleOrDefault(bombCount => bombCount.BombType == type);
            if (bombTypeCount == null)
                bombTypeCount = new BombTypeCount() { BombType = type };
            bombTypeCount.Count = value;
        }
    }
}
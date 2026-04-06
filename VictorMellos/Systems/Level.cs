using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VictorMellos;
public class Level
{
    public int Diff =1; // Short for difficulty (Sim, eu sei que é obvio mas difficulty é muito chato de escrever varias vezes (E, vai que era difference em vez de difficulty, eu tenho que deixar claro, não é verdade?))

    public Level(int level_Number)
    {
        Diff =
        Math.Max
        (
            1,
            level_Number + level_Number * level_Number / 5
        );


    }
}

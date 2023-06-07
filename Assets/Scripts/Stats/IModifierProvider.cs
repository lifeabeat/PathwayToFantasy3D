using System.Collections.Generic;
namespace RPG.Stats
{
    interface IModiferProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);

    }
}

using System.Collections.Generic;
namespace RPG.Stats
{
    public interface IModiferProvider
    {
        IEnumerable<float> GetAdditiveModifier(Stat stat);
        IEnumerable<float> GetPercentageModifer(Stat stat);

    }
}

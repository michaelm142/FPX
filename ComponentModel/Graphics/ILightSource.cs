using Microsoft.Xna.Framework;

namespace FPX
{
    public interface ILightSource
    {
        Color DiffuseColor { get; set; }

        Color SpecularColor { get; set; }

        float SpecularIntensity { get; set; }
        float SpecularPower { get; set; }
    }
}

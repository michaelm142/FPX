using Microsoft.Xna.Framework;

namespace ComponentModel
{
    public interface ILightSource
    {
        Color DiffuseColor { get; set; }

        Color SpecularColor { get; set; }

        float SpecularIntensity { get; set; }
        float SpecularPower { get; set; }
    }
}

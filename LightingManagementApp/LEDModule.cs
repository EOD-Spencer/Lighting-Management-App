using System.Drawing;

namespace LightingManagementApp;

public class LEDModule
{
    #region Properties
    
    /// <summary>
    /// Gets or sets the number of pixels in each row of the LED module.
    /// </summary>
    /// <remarks>Defaults to a single LED.</remarks>
    public int Width { get; set; } = 1;

    /// <summary>
    /// Gets or sets the number of pixel rows in the LED module.
    /// </summary>
    /// <remarks>Defaults to a single row.</remarks>
    private int Height { get; set; } = 1;

    /// <summary>
    /// Gets or sets whether the module supports a separate white LED.
    /// </summary>
    private bool SupportsSeparateWhite { get; set; }
    
    /// <summary>
    /// Gets or sets the color percentage to use for the LEDs.
    /// </summary>
    public float ColorPercentage { get; set; } = 1.0f;
    
    /// <summary>
    /// Gets or sets the value to use when displaying white.
    /// </summary>
    private Color WhiteValue { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Constructs a new instance of the Module class.
    /// </summary>
    public LEDModule()
    {
        
    }
    
    #endregion
    
    #region Methods
    
    /// <summary>
    /// Sets the white value using a percentage.
    /// </summary>
    /// <param name="colorPercentage">The color percentage.</param>
    /// <param name="separateWhite">if set to <c>true</c> [separate white].</param>
    public void SetWhiteValue(float colorPercentage, bool separateWhite = false)
    {
        SupportsSeparateWhite = separateWhite;
        if (SupportsSeparateWhite)
        {
            WhiteValue = Color.FromArgb(
                alpha: (int)(255 * colorPercentage), 
                red: 0,
                green: 0,
                blue: 0
            );
        }
        else
        {
            WhiteValue = Color.FromArgb(
                alpha: 0,
                red: (int)(255 * colorPercentage),
                green: (int)(255 * colorPercentage),
                blue: (int)(255 * colorPercentage)
            );
        }

        Patterns.SetAllLEDsToSingleColor(this, WhiteValue);
    }

    /// <summary>
    /// Sets the color of the entire strip.
    /// </summary>
    /// <param name="color">The color.</param>
    /// <param name="count">The count.</param>
    private void SetColor(Color color, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Image?.SetPixel(i, Height, color);
        }
        Update();
    }
    
    #endregion
}
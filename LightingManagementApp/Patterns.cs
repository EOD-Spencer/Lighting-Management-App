using System.Drawing;

namespace LightingManagementApp;

public class Patterns 
{
    /// <summary>
    /// The method used to turn all LEDs off.
    /// </summary>
    /// <param name="module">The module to apply the pattern to.</param>
    public static void TurnOffAllLEDs(LEDModule module)
    {
        var container = module.Image;
        container.Clear();
        module.Update();
    }

    /// <summary>
    /// Method used to set all LEDs to the color passed.
    /// </summary>
    /// <param name="module">The module to apply the pattern to.</param>
    /// <param name="color">The color to set the modules LEDs to.</param>
    public static void SetAllLEDsToSingleColor(LEDModule module, Color color)
    {
        var container = module.Image;
        container.Clear(color);
        module.Update();
    }

    /// <summary>
    /// Method used to fill the LED from left to right with the provided color.
    /// </summary>
    /// <param name="module">The module to apply the pattern to.</param>
    /// <param name="color">The color to fill the LED module with.</param>
    /// <param name="delay">The amount of time to wait before filling the next LED (milliseconds).</param>
    public void ColorWipe(LEDModule module, Color color, int delay = 25)
    {
        module.Image.Clear();
        for (int i = 0; i < module.Width; i++)
        {
            module.Image.SetPixel(i, module.Height, color);
            module.Update();
            Thread.Sleep(delay);
        }
    }

    /// <summary>
    /// Method used to flash the LED module.
    /// </summary>
    /// <param name="module">The module to apply the pattern to.</param>
    /// <param name="color">The color to flash the LED module.</param>
    /// <param name="numFlashes">The amount of times to flash the LED module.</param>
    /// <param name="flashDelay">The amount of time to wait between flashes.</param>
    /// <param name="token">Token used to cancel the pattern.</param>
    public void Flash(LEDModule module, Color color, int numFlashes, int flashDelay, CancellationToken token)
    {
        // Clear any previous data
        module.Image.Clear();
        
        // Continue to show the pattern unless cancellation is requested.
        while (!token.IsCancellationRequested)
        {
            for (int flashes = 0; flashes < numFlashes; flashes++)
            {
                // Set the color for each LED
                for (int pixel = 0; pixel < module.Width; pixel++)
                {
                    module.Image.SetPixel(pixel, 0, color);
                }

                // Show the color
                module.Update();
                Thread.Sleep(flashDelay);
                
                // Clear the color
                module.Image.Clear();
                module.Update();
                Thread.Sleep(flashDelay);
            }
            Thread.Sleep(flashDelay*3);
        }
    }
}
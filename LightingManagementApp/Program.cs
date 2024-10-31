namespace LightingManagementApp;

public static class Program
{
    #region Properties

    /// <summary>
    /// The text to display as the title of the console window.
    /// </summary>
    private const string WindowTitle = "Lighting Manager App";
    
    /// <summary>
    /// The identification of the Device.
    /// </summary>
    private const string DeviceId = "<replace-with-your-device-id>";
    
    /// <summary>
    /// The address of the Internet of Things (IoT) broker.
    /// </summary>
    private const string IotBrokerAddress = "<replace-with-your-iot-hub-name>.azure-devices.net";

    /// <summary>
    /// The number ID of the General-purpose I/O (GPIO) Pin assigned to the LED.
    /// </summary>
    private const int Pin = 18;
    
    /// <summary>
    /// The amount of time the LED should be turned "ON".
    /// </summary>
    /// <remarks>Time is expressed in milliseconds.</remarks>
    private const int LightTime = 1000;
    
    /// <summary>
    /// The amount of time the LED should be turned "OFF".
    /// </summary>
    /// <remarks>Time is expressed in milliseconds.</remarks>
    private const int DimTime = 2000;

    /// <summary>
    /// The BUS ID number for the I2C pins.
    /// </summary>
    private const int BusId = 1;
    
    #endregion
    
    /// <summary>
    /// Entry point for the Lighting Management Application.
    /// </summary>
    public static void Main()
    {
        // Clear any console text
        Console.Clear();
        
        // Create the main menu
        Menu mainMenu = new()
        {
            MenuTitle = "Main Menu",
            Message = "Select one of the following"
        };

        // Show the main menu.
        mainMenu.Show();
    }
}
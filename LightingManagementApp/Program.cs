using System.Device.Gpio;
using System.Device.I2c;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Azure.Devices.Client;
using System.Device.Spi;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Ws28xx;

namespace LightingManagementApp;

public static class Program
{
    #region Globals
    
    // set up IoT Hub message
    private const string DeviceId = "<replace-with-your-device-id>";
    private const string IotBrokerAddress = "<replace-with-your-iot-hub-name>.azure-devices.net";

    // LED constraints 
    private const int Pin = 18;
    private const int LightTime = 1000;
    private const int DimTime = 2000;

    // busId number for I2C pins
    private const int BusId = 1;

    #endregion
    
    public static void Main()
    {
        Console.WriteLine($"Lighting Management App!");

        InitializeDefaults();
        

        

        // Configure the LED strip
        while (ledCount == -1)
        {
            Console.Write("Enter the Number of LEDs (0 to exit): ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out ledCount))
            {
                break;
            }
        }

        if (ledCount == 0)
        {
            return;
        }

        while (true)
        {
            Console.WriteLine("Select the type of LEDs");
            Console.WriteLine("1: WS2812b");
            Console.WriteLine("2: WS2815b");
            Console.WriteLine("3: SK68012");
            Console.WriteLine("0: Exit");
            Console.Write("Type of Strip: ");
            string ledType = Console.ReadLine()!.Trim();

            switch (ledType)
            {
                case "0":
                    return;

                case "1":
                    ledStrip = new Patterns(new Ws2812b(spi, ledCount), ledCount);
                    break;

                case "2":
                    ledStrip = new Patterns(new Ws2815b(spi, ledCount), ledCount);
                    break;

                case "3":
                    ledStrip = new Patterns(new Sk6812(spi, ledCount), ledCount) { SupportsSeparateWhite = true };
                    break;

                default:
                    Console.WriteLine("Unsupported selection.");
                    break;
            }

            if (ledStrip != null)
            {
                break;
            }
        }

        // Set Initial state of the LEDs to off
        ledStrip.SwitchOffLEDs();

        while (isActive)
        {
            DrawMenu();
        }

        Console.WriteLine("Exit application.");

        

        void RequestPercentage()
        {
            Console.Write("Please enter the percentage between 0 and 100 in in whole numbers:");
            string? percentage = Console.ReadLine();
            if (int.TryParse(percentage, out int parsedValue))
            {
                if (parsedValue < 0 || parsedValue > 100)
                {
                    Console.WriteLine("Invalid entry. Value not acceptable.");
                }
                else
                {
                    colorPercentage = parsedValue / 100.0f;
                }
            }

            Console.WriteLine("Any key to return");
            Console.ReadKey();
        }

        void StartRainbow()
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            Task rainbowTask = Task.Run(() => ledStrip.Rainbow(cancellationTokenSource.Token),
                cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            rainbowTask.Wait(cancellationTokenSource.Token);
            ledStrip.SwitchOffLEDs();
        }

        void StartKnightRider()
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            Task knightRiderTask = Task.Run(() => ledStrip.KnightRider(cancellationTokenSource.Token),
                cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            knightRiderTask.Wait(cancellationTokenSource.Token);
            ledStrip.SwitchOffLEDs();
        }

        void StartChase(Color color, Color blankColor)
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            Task rainbowTask =
                Task.Run(() => ledStrip.TheatreChase(color, blankColor, cancellationTokenSource.Token),
                    cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            rainbowTask.Wait(cancellationTokenSource.Token);
            ledStrip.SwitchOffLEDs();
        }

        void StartFlash(Color color, int flashes, int flashDelay)
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            Task flashTask =
                Task.Run(() => ledStrip.Flash(color, flashes, flashDelay, cancellationTokenSource.Token),
                    cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            flashTask.Wait(cancellationTokenSource.Token);
            ledStrip.SwitchOffLEDs();
        }

        while(true){
            try
            {
                // set higher sampling and perform a synchronous measurement
                i2CBmp280.TemperatureSampling = Sampling.LowPower;
                i2CBmp280.PressureSampling = Sampling.UltraHighResolution;
                var readResult = i2CBmp280.Read();

                // led on
                led.Write(Pin, PinValue.High);
                Thread.Sleep(LightTime);

                // print out the measured data
                string temperature = readResult.Temperature?.DegreesCelsius.ToString("F");
                string pressure = readResult.Pressure?.Hectopascals.ToString("F");
                Console.WriteLine("-----------------------------------------");
                Console.WriteLine($"Temperature: {temperature}\u00B0C");
                Console.WriteLine($"Pressure: {pressure}hPa");

                // send to Iot Hub
                string message = $"{{\"Temperature\":{temperature},\"Pressure\":{pressure},\"DeviceID\":\"{DeviceId}\"}}";
                Message eventMessage = new Message(Encoding.UTF8.GetBytes(message));
                azureIoTClient.SendEventAsync(eventMessage).Wait();
                Console.WriteLine($"Data is pushed to Iot Hub: {message}");

                // blink and led off
                led.Write(Pin, PinValue.Low);
                Thread.Sleep(75);
                led.Write(Pin, PinValue.High);
                Thread.Sleep(75);
                led.Write(Pin, PinValue.Low);
                Thread.Sleep(75);
                led.Write(Pin, PinValue.High);
                Thread.Sleep(75);
                led.Write(Pin, PinValue.Low);
                Thread.Sleep(DimTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex.Message}");
            }
        }

        void InitializeDefaults()
        {
            // set up for LED and pin
            GpioController led = new();
            led.OpenPin(Pin, PinMode.Output);

            // setup for LEDs
            int ledCount = -1; // Number of LEDs (set to -1 to force user input)
            MenuId menuId = MenuId.Root; // Set the menu id to the root menu
            bool isActive = true; // Set the application to active
            float colorPercentage = 1.0f; // Set the color percentage to 100%

            // Setup Serial Peripheral Interface (SPI) to communicate with the LED strip
            SpiConnectionSettings settings = new(0, 0)
            {
                ClockFrequency = 2_400_000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8
            };

            // Create the SPI device
            SpiDevice spi = SpiDevice.Create(settings);

            // Create the LED strip object
            Patterns? ledStrip = null;
        
            // Create an X.509 certificate object.
            X509Certificate2 cert = new X509Certificate2($"{DeviceId}.pfx", "1234");
            DeviceAuthenticationWithX509Certificate auth = new DeviceAuthenticationWithX509Certificate(DeviceId, cert);
            DeviceClient azureIoTClient = DeviceClient.Create(IotBrokerAddress, auth, TransportType.Mqtt);

            if (azureIoTClient == null)
            {
                Console.WriteLine("Failed to create DeviceClient!");
            }
            else
            {
                Console.WriteLine("Successfully created DeviceClient!");
                Console.WriteLine("Press CTRL+D to stop application");
            }
        }
    }
}
﻿using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ws28xx;

namespace LightingManagementApp;

public static class Program
{
    #region Properties

    /// <summary>
    /// Gets or sets the ID of the currently displayed menu.
    /// </summary>
    private MenuId CurrentMenu { get; set; } = MenuId.Root;

    /// <summary>
    /// Gets or sets whether the program is active.
    /// </summary>
    private bool IsActive { get; set; } = true;

    #endregion

    /// <summary>
    /// Entry point for the Lighting Management Application.
    /// </summary>
    public static void Main()
    {        
        // Setup Serial Peripheral Interface (SPI) to communicate with the LED strip
        SpiConnectionSettings defaultSettings = new(0, 0)
        {
            ClockFrequency = 2_400_000,
            Mode = SpiMode.Mode0,
            DataBitLength = 8
        };

        // Create the SPI device
        SpiDevice spi = SpiDevice.Create(defaultSettings);

        int ledCount = -1; // Number of LEDs (set to -1 to force user input)

        // Create the LED strip object
        Ws28xx? ledStrip = null;

        // Configure the LED strip
        while (ledCount == -1)
        {
            Console.Write("Enter the Number of LEDs (0 to exit): ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out ledCount)) break;
        }

        if (ledCount == 0) return;

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
                    ledStrip = new Ws2812b(spi, ledCount);
                    break;

                case "2":
                    ledStrip = new Ws2815b(spi, ledCount);
                    break;

                case "3":
                    ledStrip = new Sk6812(spi, ledCount);
                    break;

                default:
                    Console.WriteLine("Unsupported selection.");
                    break;
            }

            if (ledStrip != null) break;
        }

        // Set Initial state of the LEDs to off
        ledStrip.Image.Clear(Color.Black);
        
        while (IsActive)
        {
            DrawMenu();
        }

        Console.WriteLine("Exit application.");

        void DrawMenu()
        {
            Console.Clear();
            switch (MenuId)
            {
                case MenuId.Root:
                    isActive = DrawMainMenu();
                    break;

                case MenuId.WhiteLevel:
                    DrawWhiteLevelMenu();
                    break;

                case MenuId.TheaterChase:
                    DrawTheatreChaseMenu();
                    break;

                case MenuId.Wipe:
                    DrawWipeMenu();
                    break;
                
                case MenuId.Flash:
                    DrawFlashMenu();
                    break;

                case MenuId.Special:
                    DrawSpecialMenu();
                    break;
            }
        }

        void DrawSpecialMenu()
        {
            Console.WriteLine("1. Knight Rider");
            Console.WriteLine("0: Back");
            Console.Write("Selection: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    MenuId = MenuId.Root;
                    break;

                case "1":
                    StartKnightRider();
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

        void DrawWipeMenu()
        {
            Console.WriteLine("1. Wipe black");
            Console.WriteLine("2. Wipe red");
            Console.WriteLine("3. Wipe green");
            Console.WriteLine("4. Wipe blue");
            Console.WriteLine("5. Wipe yellow");
            Console.WriteLine("6. Wipe turquoise");
            Console.WriteLine("7. Wipe purple");
            Console.WriteLine("8. Wipe cold white");
            //TODO Fix this.
            // if (ledStrip.SupportsSeparateWhite)
            // {
            //     Console.WriteLine("9: Separate white");
            // }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    MenuId = MenuId.Root;
                    break;

                case "1":
                    ledStrip.Image.Clear(Color.Black);
                    break;

                case "2":
                    ledStrip.Image.Clear(Color.Red);
                    break;

                case "3":
                    ledStrip.Image.Clear(Color.Green);
                    break;

                case "4":
                    ledStrip.Image.Clear(Color.Blue);
                    break;

                case "5":
                    ledStrip.Image.Clear(Color.Yellow);
                    break;

                case "6":
                    ledStrip.Image.Clear(Color.Turquoise);
                    break;

                case "7":
                    ledStrip.Image.Clear(Color.Purple);
                    break;

                case "8":
                    ledStrip.Image.Clear(Color.White);
                    break;

                case "9":
                    ledStrip.Image.Clear(Color.FromArgb(255, 0, 0, 0));
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

        void DrawTheatreChaseMenu()
        {
            Console.WriteLine("1: All LED off");
            Console.WriteLine("2: Chase in red");
            Console.WriteLine("3: Chase in green");
            Console.WriteLine("4: Chase in blue");
            Console.WriteLine("5: Christmas Chase");
            //TODO Fix This
            // if (ledStrip.SupportsSeparateWhite)
            // {
            //     Console.WriteLine("6: White Chase");
            // }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    MenuId = MenuId.Root;
                    break;

                case "1":
                    ledStrip.Image.Clear(Color.Black);
                    break;

                case "2":
                    StartChase(Color.Red, Color.FromArgb(0, 0, 0, 0));
                    break;

                case "3":
                    StartChase(Color.Green, Color.FromArgb(0, 0, 0, 0));
                    break;

                case "4":
                    StartChase(Color.Blue, Color.FromArgb(0, 0, 0, 0));
                    break;

                case "5":
                    StartChase(Color.Red, Color.Green);
                    break;

                case "6":
                    StartChase(Color.FromArgb(255, 0, 0, 0), Color.FromArgb(0, 0, 0, 0));
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

        bool DrawMainMenu()
        {
            Console.WriteLine("0: Back");
            Console.WriteLine("1: All LED off");
            Console.WriteLine("2: All LED to white (percentage)");
            Console.WriteLine("3: Rainbow");
            Console.WriteLine("4: TheatreChase");
            Console.WriteLine("5: Wipe");
            Console.WriteLine("6: Flash");
            Console.WriteLine("7: Special");

            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            bool keepActive = true;
            switch (choice)
            {
                case "0":
                    keepActive = false;
                    break;

                case "1":
                    ledStrip.Image.Clear(Color.Black);
                    keepActive = true;
                    break;

                case "2":
                    keepActive = true;
                    MenuId = MenuId.WhiteLevel;
                    break;

                case "3":
                    StartRainbow();
                    break;

                case "4":
                    MenuId = MenuId.TheaterChase;
                    break;

                case "5":
                    MenuId = MenuId.Wipe;
                    break;

                case "6":
                    MenuId = MenuId.Flash;
                    break;
                
                case "7":
                    MenuId = MenuId.Special;
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }

            return keepActive;
        }

        void DrawWhiteLevelMenu()
        {
            Console.WriteLine($"1: Change percentage (currently: {colorPercentage * 100}%)");
            Console.WriteLine("2: LEDs white (RGB)");
            //TODO Fix This
            // if (ledStrip.SupportsSeparateWhite)
            // {
            //     Console.WriteLine("3: Separate White");
            // }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    MenuId = MenuId.Root;
                    break;

                case "1":
                    RequestPercentage();
                    break;

                case "2":
                    //TODO Fix this
                    // ledStrip.SetWhiteValue(colorPercentage);
                    break;

                case "3":
                    //TODO Fix this
                    // ledStrip.SetWhiteValue(colorPercentage, true);
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

        void DrawFlashMenu()
        {
            Console.WriteLine("0: Back");
            Console.WriteLine("1. Flash Red");
            Console.WriteLine("2. Flash Green");
            Console.WriteLine("3. Flash Blue");
            Console.WriteLine("4. Flash Yellow");
            Console.WriteLine("5. Flash Turquoise");
            Console.WriteLine("6. Flash Purple");
            Console.WriteLine("7. Flash White");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            Console.WriteLine("How many times should the LED flash?");
            int numFlashes = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter a value for the flash delay in milliseconds (default 50):");
            int flashDelay = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case "0":
                    MenuId = MenuId.Root;
                    break;

                case "1":
                    StartFlash(Color.Red, numFlashes, flashDelay);
                    break;

                case "2":
                    StartFlash(Color.Green, numFlashes, flashDelay);
                    break;

                case "3":
                    StartFlash(Color.Blue, numFlashes, flashDelay);
                    break;

                case "4":
                    StartFlash(Color.Yellow, numFlashes, flashDelay);
                    break;

                case "5":
                    StartFlash(Color.Turquoise, numFlashes, flashDelay);
                    break;

                case "6":
                    StartFlash(Color.Purple, numFlashes, flashDelay);
                    break;

                case "7":
                    StartFlash(Color.White, numFlashes, flashDelay);
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }
        
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

            //Task rainbowTask = Task.Run(() => ledStrip.Rainbow(cancellationTokenSource.Token), cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            //rainbowTask.Wait(cancellationTokenSource.Token);
            ledStrip.Image.Clear(Color.Black);
        }

        void StartKnightRider()
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            //Task knightRiderTask = Task.Run(() => ledStrip.KnightRider(cancellationTokenSource.Token), cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            //knightRiderTask.Wait(cancellationTokenSource.Token);
            ledStrip.Image.Clear(Color.Black);
        }

        void StartChase(Color color, Color blankColor)
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            //Task rainbowTask = Task.Run(() => ledStrip.TheaterChase(color, blankColor, cancellationTokenSource.Token), cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            //rainbowTask.Wait(cancellationTokenSource.Token);
            ledStrip.Image.Clear(Color.Black);
        }
        
        void StartFlash(Color color, int flashes, int flashDelay)
        {
            Console.WriteLine("Any key to stop");
            CancellationTokenSource cancellationTokenSource = new();

            //Task flashTask = Task.Run(() => ledStrip.Flash(color, flashes, flashDelay, cancellationTokenSource.Token), cancellationTokenSource.Token);
            Console.ReadKey();
            cancellationTokenSource.Cancel();
            //flashTask.Wait(cancellationTokenSource.Token);
            ledStrip.Image.Clear(Color.Black);
        }
    }
}
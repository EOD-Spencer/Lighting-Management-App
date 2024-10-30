using System.Drawing;

namespace LightingManagementApp;

public class Menu
{
    private const int MenuWidth = 30;
    
    #region Properties

    /// <summary>
    /// Gets or sets the menu window text.
    /// </summary>
    public string? WindowTitle { set => Console.Title = value ?? ""; }

    /// <summary>
    /// Gets or sets the menu header text.
    /// </summary>
    public string? HeaderTitle { get; set; }

    /// <summary>
    /// Gets or sets the message text to display within the menu.
    /// </summary>
    private string? Message { get; set; }

    /// <summary>
    /// Gets or sets the option that the user has chosen.
    /// </summary>
    private int SelectedOption { get; set; }

    /// <summary>
    /// Gets or sets a dictionary keyed by a selections name
    /// and containing the actions to perform upon selection.
    /// </summary>
    public Dictionary<string, Action>? Options { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Method used to construct a new instance of the menu class.
    /// </summary>
    public Menu()
    {
        Console.CursorVisible = false;
    }

    /// <summary>
    /// Method used to construct a new instance of the menu class.
    /// </summary>
    /// <param name="windowTitle"></param>
    /// <param name="headerTitle"></param>
    /// <param name="message"></param>
    public Menu(string? windowTitle, string? headerTitle, string? message)
    {
        WindowTitle = windowTitle;
        HeaderTitle = headerTitle;
        Message = message;
        Console.CursorVisible = false;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Method used to build the menu based on the provided actions.
    /// </summary>
    public void Build()
    {
        // Set the properties of the menu
        SelectedOption = 0;

        // Show the menu
        do
        {
            // Clear the console
            Console.Clear();

            // Show the menu message
            // Displays the header title
            if (!string.IsNullOrEmpty(HeaderTitle))
            {
                string? message = HeaderTitle;
                int width = MenuWidth > (message.Length - 1) ? (MenuWidth + 2) : (message.Length + 2);

                message = " " + message + " ";
                message = message.PadLeft(message.Length + (width - message.Length) / 2, '-');
                message = message.PadRight(width, '-');
                Console.WriteLine($"{message, MenuWidth}");
            }

            // Displays the message
            if (!string.IsNullOrEmpty(Message))
            {
                string? message = Message;
                int width = MenuWidth > (message.Length - 1) ? (MenuWidth + 2) : (message.Length + 2);

                Console.WriteLine($"{message, MenuWidth}");
                Console.WriteLine(new string('-', width));
            }

            // Iterate over the selectable options and print them to the console.
            if (Options != null)
                for (int i = 0; i < Options.Count; i++)
                {
                    // Highlights the selected option
                    if (i == SelectedOption)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine($"{Options.ElementAt(i),MenuWidth}");

                    Console.ResetColor();
                }
        }
        while (!HandleInput());

        // Invokes the action corresponding to the selected option
        string? key = Options?.Keys.ElementAt(SelectedOption);
        if (key != null) Options?[key].Invoke();
    }

    /// <summary>
    /// Method used to process the user's input.
    /// </summary>
    /// <returns></returns>
    private bool HandleInput()
    {
        int endIndex = Options?.Count - 1 ?? 0;
        int option = SelectedOption;

        // Changes the selected option based on the key pressed.
        switch (Console.ReadKey(true).Key)
        {
            case ConsoleKey.Enter:
                Reset();
                return true;
            case ConsoleKey.UpArrow when option > 0:
                // option--;
                break;
            case ConsoleKey.UpArrow when option == 0:
                // option = endIndex;
                break;
            case ConsoleKey.DownArrow when option < endIndex:
                // option++;
                break;
            case ConsoleKey.DownArrow when option == endIndex:
                // option = 0;
                break;
            default:
                HandleInput();
                break;
        }
        return false;
    }

    /// <summary>
    /// Method used to Reset the Menu UI
    /// </summary>
    private void Reset()
    {
        WindowTitle = null;
        HeaderTitle = null;
        Message = null;
    }

    /// <summary>
    /// Method used to exit the application.
    /// </summary>
    public static void Exit() => Environment.Exit(0);

    #endregion

    #region Menu Screens

    public static void ShowConfigurationMenu(Menu menu)
    {
        // Set the properties for the menu.
        menu.HeaderTitle = "Configuration Menu";

        // Create a dictionary of the selectable menu options.
        menu.Options = new Dictionary<string, Action>()
        {
            { "Serial Peripheral Interface (SPI) Configuration", menu.ShowSpiConfigurationMenu },
            { "LED Type Configuration", menu.ShowLEDTypeSelectionMenu },
            { "White Level Configuration", menu.ShowWhiteLevelMenu },
            { "~Back", DrawMainMenu }
        };

        // Build and show the menu.
        menu.Build();
    }

    /// <summary>
    /// Method used to show the SPI Configuration menu.
    /// </summary>
    private void ShowSpiConfigurationMenu()
    {
        // Set the properties for the menu.
        HeaderTitle = "Serial Peripheral Interface (SPI) Configuration Menu";

        // Create a dictionary of the selectable menu options.

        // Build and show the menu.
        Build();
    }

    /// <summary>
    /// Method used to confirm the users' selection.
    /// </summary>
    private void ShowConfirmMenu()
    {
        // Set the properties for the menu.
        WindowTitle = "Confirm";
        Message = "Are you sure?";

        // Create a dictionary of the selectable menu options.

        // Build and show the menu.
        Build();
        switch (SelectedOption)
        {
            case 0:
                ShowMainMenu();
                break;
            case 1:
                ShowMainMenu();
                break;
        }
    }

    /// <summary>
    /// Method used to show the LED type selection menu.
    /// </summary>
    private void ShowLEDTypeSelectionMenu()
    {
        // Set the properties for the menu.
        HeaderTitle = "LED Type Selection Menu";

        // Create a dictionary of the selectable menu options.

        // Build and show the menu.
        Build();
    }

    /// <summary>
    /// Method used to show the White Level Menu.
    /// </summary>
    private void ShowWhiteLevelMenu()
    {
        // Set the properties for the menu.
        HeaderTitle = "White Level Selection Menu";

        // Create a dictionary of the selectable menu options.

        // Build and show the menu.
        Build();
    }

    private void DrawMenu()
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

                case MenuId.TheatreChase:
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

    private void DrawSpecialMenu()
        {
            Console.WriteLine("1. Knight Rider");
            Console.WriteLine("0: Back");
            Console.Write("Selection: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    menuId = MenuId.Root;
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

    private void DrawWipeMenu()
        {
            Console.WriteLine("1. Wipe black");
            Console.WriteLine("2. Wipe red");
            Console.WriteLine("3. Wipe green");
            Console.WriteLine("4. Wipe blue");
            Console.WriteLine("5. Wipe yellow");
            Console.WriteLine("6. Wipe turquoise");
            Console.WriteLine("7. Wipe purple");
            Console.WriteLine("8. Wipe cold white");
            if (module.SupportsSeparateWhite)
            {
                Console.WriteLine("9: Separate white");
            }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");

            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    menuId = MenuId.Root;
                    break;

                case "1":
                    module.ColorWipe(module.FilterColor(Color.Black));
                    break;

                case "2":
                    module.ColorWipe(module.FilterColor(Color.Red));
                    break;

                case "3":
                    module.ColorWipe(module.FilterColor(Color.Green));
                    break;

                case "4":
                    module.ColorWipe(module.FilterColor(Color.Blue));
                    break;

                case "5":
                    module.ColorWipe(module.FilterColor(Color.Yellow));
                    break;

                case "6":
                    module.ColorWipe(module.FilterColor(Color.Turquoise));
                    break;

                case "7":
                    module.ColorWipe(module.FilterColor(Color.Purple));
                    break;

                case "8":
                    module.ColorWipe(module.FilterColor(Color.White));
                    break;

                case "9":
                    module.ColorWipe(Color.FromArgb(255, 0, 0, 0));
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

    private void DrawTheatreChaseMenu()
        {
            Console.WriteLine("1: All LED off");
            Console.WriteLine("2: Chase in red");
            Console.WriteLine("3: Chase in green");
            Console.WriteLine("4: Chase in blue");
            Console.WriteLine("5: Christmas Chase");

            if (module.SupportsSeparateWhite)
            {
                Console.WriteLine("6: White Chase");
            }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    menuId = MenuId.Root;
                    break;

                case "1":
                    module.SwitchOffLEDs();
                    break;

                case "2":
                    StartChase(module.FilterColor(Color.Red), Color.FromArgb(0, 0, 0, 0));
                    break;

                case "3":
                    StartChase(module.FilterColor(Color.Green), Color.FromArgb(0, 0, 0, 0));
                    break;

                case "4":
                    StartChase(module.FilterColor(Color.Blue), Color.FromArgb(0, 0, 0, 0));
                    break;

                case "5":
                    StartChase(module.FilterColor(Color.Red), module.FilterColor(Color.Green));
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

    private bool DrawMainMenu()
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
                    module.SwitchOffLEDs();
                    keepActive = true;
                    break;

                case "2":
                    keepActive = true;
                    menuId = MenuId.WhiteLevel;
                    break;

                case "3":
                    StartRainbow();
                    break;

                case "4":
                    menuId = MenuId.TheatreChase;
                    break;

                case "5":
                    menuId = MenuId.Wipe;
                    break;

                case "6":
                    menuId = MenuId.Flash;
                    break;

                case "7":
                    menuId = MenuId.Special;
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }

            return keepActive;
        }

    private void DrawWhiteLevelMenu()
        {
            Console.WriteLine($"1: Change percentage (currently: {colorPercentage * 100}%)");
            Console.WriteLine("2: LEDs white (RGB)");
            if (module.SupportsSeparateWhite)
            {
                Console.WriteLine("3: Separate White");
            }

            Console.WriteLine("0: Back");
            Console.Write("Selection: ");
            string? choice = Console.ReadLine();
            switch (choice)
            {
                case "0":
                    menuId = MenuId.Root;
                    break;

                case "1":
                    RequestPercentage();
                    break;

                case "2":
                    module.SetWhiteValue(colorPercentage);
                    break;

                case "3":
                    module.SetWhiteValue(colorPercentage, true);
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

    private void DrawFlashMenu()
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
                    menuId = MenuId.Root;
                    break;

                case "1":
                    StartFlash(module.FilterColor(Color.Red), numFlashes, flashDelay);
                    break;

                case "2":
                    StartFlash(module.FilterColor(Color.Green), numFlashes, flashDelay);
                    break;

                case "3":
                    StartFlash(module.FilterColor(Color.Blue), numFlashes, flashDelay);
                    break;

                case "4":
                    StartFlash(module.FilterColor(Color.Yellow), numFlashes, flashDelay);
                    break;

                case "5":
                    StartFlash(module.FilterColor(Color.Turquoise), numFlashes, flashDelay);
                    break;

                case "6":
                    StartFlash(module.FilterColor(Color.Purple), numFlashes, flashDelay);
                    break;

                case "7":
                    StartFlash(module.FilterColor(Color.White), numFlashes, flashDelay);
                    break;

                default:
                    Console.WriteLine("Unknown Selection. Any key to continue.");
                    Console.ReadKey();
                    break;
            }
        }

    #endregion
}
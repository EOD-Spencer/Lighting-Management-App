using System.Collections.Immutable;

namespace LightingManagementApp;

public class Menu
{
    private const int Width = 30;
    
    #region Properties
    
    /// <summary>
    /// Sets the title of the window displaying the menu.
    /// </summary>
    private string WindowTitle { get; set; } = "Lighting Management App";
    
    /// <summary>
    /// Gets or sets the title of the menu.
    /// </summary>
    public string MenuTitle { get; set; }
    
    /// <summary>
    /// Gets or sets the selection message text.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the options available in the menu.
    /// </summary>
    public string[] Options { get; set; }
    
    /// <summary>
    /// Gets or sets the currently selected menu option.
    /// </summary>
    private int SelectedOption { get; set; }
    
    /// <summary>
    /// Gets or sets whether the console cursor is displayed.
    /// </summary>
    private bool ShowCursor { get; set; }
    
    #endregion
    
    #region Constructors
    
    /// <summary>
    /// Constructor for a new instance of the Menu class.
    /// </summary>
    public Menu()
    {
        // Set the properties of the menu.
        MenuTitle = "<MENU TITLE PLACEHOLDER>";
        Message = "<MENU MESSAGE PLACEHOLDER>";
        Options = ["Exit", "<OPTION 1 PLACEHOLDER>", "<OPTION 2 PLACEHOLDER>"];
    }

    #endregion
    
    #region Methods
    
    /// <summary>
    /// Method used to build the menu.
    /// </summary>
    public void Show()
    {
        // Clears the currently displayed console text.
        Console.Clear();
        
        // Set the title of the console window.
        Console.Title = WindowTitle;
        
        // Hide the console cursor.
        Console.CursorVisible = ShowCursor;
        
        // Format and show the menu title in the console.
        ShowMenuTitle();
        
        // Format and show the menu title in the console.
        ShowMenuBody();
    }
    
    /// <summary>
    /// Method used to format the title of the console menu.
    /// </summary>
    /// <returns>The menu title to display.</returns>
    private void ShowMenuTitle()
    {
        // Get the width of the console window.
        int consoleWidth = Console.WindowWidth;
        
        // Get the width of the menu title.
        int titleWidth = MenuTitle.Length;
        
        // Determine how many spaces on each side is needed to center the menu title.
        int padValue = (consoleWidth - titleWidth) / 2;
        
        // Add the padValue to each side of the menu title.
        string formattedString = MenuTitle.PadLeft(padValue).PadRight(consoleWidth);
        
        // Set the menu title property to the formatted string.
        MenuTitle = formattedString;
        
        // Show a separation between the title and the menu body.
        Console.WriteLine(new string('=', Console.WindowWidth));
        
        // Show the menu title.
        Console.WriteLine($"{MenuTitle}");
        
        // Show a separation between the title and the menu body.
        Console.WriteLine(new string('=', Console.WindowWidth));
    }

    private void ShowMenuBody()
    {
        // Show the selection message.
        Console.WriteLine($"{Message}:");

        // Iterate over the selectable options and print them to the console.
        for (int i = 0; i < Options.Length; i++)
        {
            // Write the index of the option followed by the option text.
            // Example: 0. Exit
            Console.WriteLine($"{i}. {Options[i]}");
            
            // If this is the currently selected option, highlight it.
            if (i == SelectedOption)
            {
                DisplayAsSelected();
            }
            
            // Reset the console color.
            Console.ResetColor();
        }
    }
    
    /// <summary>
    /// Method used to display the current console text as selected.
    /// </summary>
    private static void DisplayAsSelected()
    {
        // Set the console background color to White.
        Console.BackgroundColor = ConsoleColor.White;
        
        // Set the console foreground color to Black.
        Console.ForegroundColor = ConsoleColor.Black;
    }
    
    #endregion
}
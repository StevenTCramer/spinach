namespace SpinachExplorer;

using Spinach.Manager;

internal static partial class Program
{
  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Application Entry Point
  // /////////////////////////////////////////////////////////////////////////////////////////////

  internal static void Main(string[] args) => MainMenu();

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Constructors
  // /////////////////////////////////////////////////////////////////////////////////////////////

  static Program()
  {
    // TextSearchIndex is a facade class for all of Spinach's internal functionality. By
    // placing all indexing and lookup functionality in the facace class, we can provide
    // a nicely abstracted API to any Spinach consumers.
    TextSearchIndex = new TextSearchIndex();
    TextSearchManager = new TextSearchManager();
  }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Private Static Properties
  // /////////////////////////////////////////////////////////////////////////////////////////////

  private static TextSearchIndex TextSearchIndex { get; }

  private static TextSearchManager TextSearchManager { get; }

  // /////////////////////////////////////////////////////////////////////////////////////////////
  // Private Static Methods
  // /////////////////////////////////////////////////////////////////////////////////////////////

  private static void ClearScreen() => Console.Write("\u001b[2J\u001b[H");

  private static void Pause()
  {
    Console.WriteLine();
    Console.Write("Press <Enter> to return to Main Menu ... ");
    Console.ReadLine();
  }
}

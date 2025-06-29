using Spectre.Console;

namespace Helpers {
  public static class array {
    // add an element to an array
    public static string[] add(string[] arr, string val) {
      int n = arr.Length;
      string[] new_arr = new string[n];
      new_arr[n] = val;

      for (int i = 0; i < n-1; i++) {
        new_arr[i] = arr[i];
      }

      return new_arr;
    }

    // edit an element from an array
    public static string[] edit(string[] arr, string val, int idx) {
      arr[idx] = val;
      return arr;   
    }

    // delete an element from an array
    public static string[] delete(string[] arr, int idx) {
      int n = arr.Length - 1;
      string[] new_arr = new string[n];

      for (int i = 0; i < n; i++) {
        if (i == idx) continue;
        new_arr[i] = arr[i];
      }

      return new_arr;
    }
  }

  // other helpers maybe later
    /*
  var variable = AnsiConsole.Prompt(new ConfirmationPrompt(string));

  var variable = AnsiConsole.Prompt(new TextPrompt<T>(string));
  
  var variable = AnsiConsole.Prompt(
    new SelectionPrompt<T>()
    .Title(string)
    .PageSize(int)
    .MoreChoicesText(string)
    .AddChoices([])
  );

  var table = new Table();

  table.AddColumn(new TableColumn(string).Centered());
  table.AddRow(string[]);

  table.ShowRowSeparators();

  table.Alignment(Justify.Right);
  table.RightAligned();
  table.Centered();
  table.LeftAligned();

  table.Border(TableBorder.Rounded);
  */

  public static class console {
    public static bool rconfirm(string msg) {
      return AnsiConsole.Prompt(new ConfirmationPrompt(msg).InvalidChoiceMessage("[red]Opcion invalida[/]"));
    }

    public static string rstring(string msg) {
      return AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
          if (int.TryParse(input, out _))
            return ValidationResult.Error("[red]Valor invalido[/]");
          else if (input.Length <= 4)
            return ValidationResult.Error("[red]Valor invalido[/]");
          return ValidationResult.Success();
      }));
    }

    public static void wline(string msg) {
      AnsiConsole.MarkupLine(msg);
    }

    public static int rint(string msg) {
      return int.Parse(AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
        if (!int.TryParse(input, out int n)) 
          return ValidationResult.Error("[red]Numero invalido[/]");
        else if (n <= 0) return ValidationResult.Error("[red]Numero invalido[/]");
        return ValidationResult.Success();
      })));
    }

    // public static string rselect() {}
    // public static Table wtable() {}
  }
}
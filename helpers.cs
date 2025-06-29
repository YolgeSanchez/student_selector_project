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

  table.ShowRowSeparators();

  table.Alignment(Justify.Right);
  table.RightAligned();
  table.Centered();
  table.LeftAligned();

  table.Border(TableBorder.Rounded);
  */

  public static class console {
    // asks input for a confirmation
    public static bool read_confirm(string msg) {
      return AnsiConsole.Prompt(new ConfirmationPrompt(msg).InvalidChoiceMessage("[red]Opcion invalida[/]"));
    }

    // writes a message in console
    public static void write_line(string msg) {
      AnsiConsole.MarkupLine(msg);
    }

    // asks for a string to the user
    public static string read_string(string msg) {
      return AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
          if (int.TryParse(input, out _))
            return ValidationResult.Error("[red]Valor invalido[/]");
          else if (input.Length <= 4)
            return ValidationResult.Error("[red]Valor invalido[/]");
          return ValidationResult.Success();
      }));
    }

    // asks for an integer to the user
    public static int read_int(string msg) {
      return int.Parse(AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
        if (!int.TryParse(input, out int n)) 
          return ValidationResult.Error("[red]Numero invalido[/]");
        else if (n <= 0) return ValidationResult.Error("[red]Numero invalido[/]");
        return ValidationResult.Success();
      })));
    }

    // make a selection list for the user to choose
    public static string read_select(
        string[] opt, 
        string Title, 
        string MoreChoices = "[grey](Desplácese arriba y abajo para ver más opciones)[/]", 
        int PageSize = 10
      ) {
      return AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title(Title)
        .PageSize(PageSize)
        .MoreChoicesText(MoreChoices)
        .AddChoices(opt)
      );
    }

    // create a table with its headers but without any rows
    public static Table create_table(string[] headers) {
      var table = new Table();

      table.AddColumns(headers);

      table.ShowRowSeparators();
      table.Border(TableBorder.Rounded);

      return table;
    }

    // add multiple rows to a table
    public static Table add_rows(Table table, string[,] rows) {
      for(int rowidx = 0; rowidx < rows.GetLength(0); rowidx++) {
        string[] row = new string[rows.GetLength(1)];
        for (int colidx = 0; colidx < rows.GetLength(1); colidx++) {
          row[colidx] = rows[rowidx, colidx];
        }
        table.AddRow(row);
      }

      return table;
    }

  }
}
using Spectre.Console;

namespace Helpers {
  public static class array {
    // add an element to an array
    public static string[] add(string[] arr, string val) {
      // validate duplicates
      if (Array.IndexOf(arr, val) != -1) throw new Exception("Valor duplicado");

      // add element
      int len = arr.Length;
      string[] new_arr = new string[len + 1];
      new_arr[len] = val;

      for (int idx = 0; idx < len; idx++) {
        new_arr[idx] = arr[idx];
      }

      return new_arr;
    }

    // edit an element from an array
    public static void edit(string[] arr, string val, int idx) {
      // validate duplicates
      if (Array.IndexOf(arr, val) != -1) throw new Exception("Valor duplicado");

      // edit element
      arr[idx] = val;
    }

    // delete an element from an array
    public static string[] delete(string[] arr, int idx) {
      // validate index
      int len = arr.Length;
      if (idx >= len--) throw new Exception("Indice fuera de rango");

      // delete element
      string[] new_arr = new string[len];

      for (int oldidx = 0, newidx = 0; oldidx < len; oldidx++) {
        if (oldidx == idx) continue;
        new_arr[newidx++] = arr[oldidx];
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
        else if (n < 0) return ValidationResult.Error("[red]Numero invalido[/]");
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
using Spectre.Console;

namespace Helpers {
  public static class array {
    public static T[] add<T>(T[] arr, T val) {
      int len = arr.Length;
      T[] new_arr = new T[len + 1];
      new_arr[len] = val;

      for (int idx = 0; idx < len; idx++) {
        new_arr[idx] = arr[idx];
      }

      return new_arr;
    }

    public static T[] remove<T>(T[] arr, int idx) {
      int len = arr.Length;
      if (idx >= len) throw new Exception("[red]Indice fuera de rango[/]");

      T[] new_arr = new T[len - 1];

      for (int oldidx = 0, newidx = 0; oldidx < len; oldidx++) {
        if (oldidx == idx) continue;
        new_arr[newidx++] = arr[oldidx];
      }

      return new_arr;
    }

    public static string[,] to_2d(string[] arr) {
      int len = arr.Length;
      string[,] new_arr = new string[len, 1];

      for (int idx = 0; idx < len; idx++) new_arr[idx, 0] = arr[idx];

      return new_arr;
    }
  }

  public static class console {
    public static bool read_confirm(string msg) {
      return AnsiConsole.Prompt(new ConfirmationPrompt(msg).InvalidChoiceMessage("[red]Opcion invalida[/]"));
    }

    public static void write_line(string msg) {
      AnsiConsole.MarkupLine(msg);
    }

    public static string read_string(string msg) {
      return AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
          if (int.TryParse(input, out _))
            return ValidationResult.Error("[red]Valor invalido[/]");
          else if (input.Length <= 4)
            return ValidationResult.Error("[red]Valor invalido[/]");
          return ValidationResult.Success();
      }));
    }

    public static int read_int(string msg) {
      return int.Parse(AnsiConsole.Prompt(new TextPrompt<string>(msg).Validate(input => {
        if (!int.TryParse(input, out int n)) 
          return ValidationResult.Error("[red]Numero invalido[/]");
        else if (n < 0) return ValidationResult.Error("[red]Numero invalido[/]");
        return ValidationResult.Success();
      })));
    }

    public static string read_select(
        string[] opt, 
        string Title, 
        string MoreChoices = "[#b5b5b5](Desplácese arriba y abajo para ver más opciones)[/]", 
        int PageSize = 10
      ) {
      return AnsiConsole.Prompt(new SelectionPrompt<string>()
        .Title(Title)
        .PageSize(PageSize)
        .MoreChoicesText(MoreChoices)
        .AddChoices(opt)
      );
    }

    public static Table create_table(string[] headers) {
      var table = new Table();

      table.AddColumns(headers);

      table.ShowRowSeparators();
      table.Border(TableBorder.Square);

      return table;
    }

    public static void add_rows(Table table, string[,] data) {
      int rows = data.GetLength(0);
      int cols = data.GetLength(1);
      for(int row = 0; row < rows; row++) {
        string[] data_row = new string[cols];

        for (int col = 0; col < cols; col++) {
          data_row[col] = data[row, col];
        }

        table.AddRow(data_row);
      }
    }

  }
}
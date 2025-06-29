using System;
using Helpers;
using Spectre.Console;
using Humanizer;
using Newtonsoft.Json;

internal class Program {
  private static void Main(string[] args) {
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

    

  }
}


/* 
necesidades claras ------------------------------------

repetir hasta que el usuario desee salir
cuando quiera salir pedir confirmacion de querer salir

crear una lista donde se almacenen roles
crear una lista donde se almacenen estudiantes
crear helpers para agregar, editar y eliminar estudiantes y roles

seleccionar dos estudiantes al azar de la lista
asignar roles a esos estudiantes seleccionados
no puede salir un mismo estudiante en el mismo giro de ruleta
permitir girar la ruleta varias veces
ver los dos ultimos seleccionados del giro anterior

las entradas del usuario deben ser siempre validadas
manejo de errores ---------
  listas vacias
  opciones del menu incorrectas

guardar historial de giros anteriores en archivos txt o csv
permitir acceder al historial de giros anteriores

*/

using System;
using Helpers;
using Spectre.Console;
using Humanizer;
using Newtonsoft.Json;

internal class Program {

  private static readonly string[] menu_options = {"Ver estudiantes", "Ver roles", "Salir"};
  private static string[] students = {"Jorge Sanchez", "Joel Benites", "Giancarlo Perez", "Irvin Samboy"};
  private static string[] roles = {"Developer", "Designer", "Leader"};


  private static void Main(string[] args) {
    while(true) {
      var menu = console.read_select(menu_options, "Menu Principal");

      if (menu == "Salir") {
        bool exit = confirm_exit();
        if (exit) break;
      }  

      menu_selection(menu);

    } 
  }

  private static bool confirm_exit() {
    return console.read_confirm("Estas seguro de que quieres salir?: "); 
  }

  private static void show_table(string[] headers, string[,] rows) {
    var table = console.create_table(headers);
    console.add_rows(table, rows);
    AnsiConsole.Write(table);
  }

  private static void menu_selection(string menu) {
    switch (menu) {
      case "Ver estudiantes":
        show_table(["Estudiantes"], array.to_2d(students));
        break;
      case "Ver roles":
        show_table(["Roles"], array.to_2d(roles));
        break;
    }
  }
}


/* 
necesidades claras ------------------------------------

[x] repetir hasta que el usuario desee salir
[x] cuando quiera salir pedir confirmacion de querer salir

[] crear una lista donde se almacenen roles
[] crear una lista donde se almacenen estudiantes
[] crear helpers para agregar, editar y eliminar estudiantes y roles

[] seleccionar dos estudiantes al azar de la lista
[] asignar roles a esos estudiantes seleccionados
[] no puede salir un mismo estudiante en el mismo giro de ruleta
[] permitir girar la ruleta varias veces
[] ver los dos ultimos seleccionados del giro anterior

[] las entradas del usuario deben ser siempre validadas
[] manejo de errores ---------
  listas vacias
  [x] opciones del menu incorrectas

[] guardar historial de giros anteriores en archivos txt o csv
[] permitir acceder al historial de giros anteriores

*/

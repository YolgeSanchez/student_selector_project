using System;
using Helpers;
using Spectre.Console;
using Humanizer;
using Newtonsoft.Json;
using Clases;
using System.IO;

internal class Program {

  private static readonly string[] menu_options = {"Ruleta", "Estudiantes", "Roles", "Salir"};
  private static readonly string[] students_options = {"Ver estudiantes", "Agregar estudiante", "Editar estudiante", "Eliminar estudiante", "<- Atras"};
  private static readonly string[] edit_student_actions = {"Nombre", "Roles"};
  private static readonly string[] roles_options = {"Ver roles", "Agregar rol", "Editar rol", "Eliminar rol", "<- Atras"};
  private static readonly string[] roulette_options = {"Girar ruleta", "Girar ruleta con ultima seleccion", "<- Atras"};

  private static string[] roles = new string[0];
  private static string[] last_roulette_selection = new string[0];

  private static void Main(string[] args) {
    Student.load();
    loadRoles();

    while(true) {
      var menu = console.read_select(menu_options, "Menu Principal");

      if (menu == "Salir") {
        bool exit = confirm_exit();
        if (exit) {
          var roulette_history = JsonConvert.SerializeObject(Student.assign_registry());
          var students_registry = JsonConvert.SerializeObject(Student.students_list());
          var roles_registry = JsonConvert.SerializeObject(roles);
          
          string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

          File.WriteAllText($"registros/registro_{timeStamp}.json", roulette_history);
          File.WriteAllText("estudiantes.json", students_registry);
          File.WriteAllText("roles.json", roles_registry);

          break;
        };
      }  

      menu_selection(menu);

    } 
  }

  private static void loadRoles() {
    var json = File.ReadAllText("roles.json");
    string[] roles_list = JsonConvert.DeserializeObject<string[]>(json) ?? Array.Empty<string>();
    foreach (string role in roles_list) roles = array.add(roles, role);
  }

  private static bool confirm_exit() {
    return console.read_confirm("Estas seguro de que quieres salir?: "); 
  }

  private static void show_table(string[] headers, string[,] rows, string empty = "La lista esta vacia") {
    if (rows.GetLength(0) == 0) {
      console.write_line($"[red]{empty}[/]");
      return;
    }
    var table = console.create_table(headers);
    console.add_rows(table, rows);
    AnsiConsole.Write(table);
  }

  private static void menu_selection(string menu) {
    switch (menu) {
      case "Estudiantes":
        students_selection();
        break;
      case "Roles":
        roles_selection();
        break;
      case "Ruleta":
        roulette_selection();
        break;
    }
  }

  private static void roulette_selection() {
    while(true) {
      string menu_roulette = console.read_select(roulette_options, "Ruleta");
      if (menu_roulette == "<- Atras") break;

      switch (menu_roulette) {
        case "Girar ruleta":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles para girar la ruleta[/]");
            break;
          } else if (Student.students.Length == 0) {
            console.write_line("[red]No hay estudiantes para girar la ruleta[/]");
            break;
          }

          var selection = AnsiConsole.Prompt(new MultiSelectionPrompt<string>()
            .Title("Selecciona los roles a asignar en este giro de la ruleta: ")
            .PageSize(10)
            .MoreChoicesText("[#b5b5b5](Muevete hacia arriba y abajo para revelar mas roles)[/]")
            .InstructionsText(
              "[#b5b5b5](Presiona [blue]<space>[/] para seleccionar un rol, " + 
              "[green]<enter>[/] para aceptar)[/]")
            .AddChoices(roles));

          string[] convert = selection.ToArray();
          last_roulette_selection = convert;
          try {
            string[,] msg = Roulette.spin(convert); 
            show_table(["Estudiante", "Rol"], msg);
          } catch (Exception ex) {
            console.write_line(ex.Message);
          }
          break; 
        case "Girar ruleta con ultima seleccion":
          if (last_roulette_selection.Length == 0) {
            console.write_line("[red]No ha hecho ningun giro de ruleta todavia[/]");
            break;
          }

          bool still_exists = true;
          for (int idx = 0; idx < last_roulette_selection.Length; idx++) {
            if (Array.IndexOf(roles, last_roulette_selection[idx]) == -1) {
              still_exists = false;
              break;
            }
          }

          if (!still_exists) {
            console.write_line("[red]Los roles usados para el anterior giro fueron modificados o eliminados[/]");
            break;
          }

          try {
            string[,] msg = Roulette.spin(last_roulette_selection); 
            show_table(["Estudiante", "Rol"], msg);
          } catch (Exception ex) {
            console.write_line(ex.Message);
          }
          break;
      }
    }
  }

  private static void students_selection() {
    while(true) {
      string menu_students = console.read_select(students_options, "Estudiantes");
      if (menu_students == "<- Atras") break;

      switch (menu_students) {
        case "Ver estudiantes":
          show_table(["Estudiante", "Roles"], Student.students_list_matrix(), "No hay ningun estudiante que mostrar");
          break;
        case "Agregar estudiante":
          string student;
          string msg;

          student = console.read_string("Ingresa el nombre del estudiante que desea agregar: ");

          msg = Student.add_student(student);
          console.write_line(msg);

          break;
        case "Editar estudiante":
          if (Student.students_list().Length == 0) {
            console.write_line("[red]No hay estudiantes que editar en la lista[/]");
            break;
          }

          student = console.read_select(Student.students_list(), "Selecciona un estudiante para editar");
          int student_idx = Array.IndexOf(Student.students_list(), student);
          string action = console.read_select(edit_student_actions, "Que desea editar?: ");

          switch (action)  {
            case "Nombre":
              string new_name = console.read_string("Ingresa el nuevo nombre del estudiante: "); 
              msg = Student.edit_student(student, new_name);
              console.write_line(msg);

            break;
            case "Roles": 
              action = console.read_select(["Agregar", "Eliminar"], "Que desea hacer?: ");
              switch (action) {
                case "Agregar":

                  string[] filtered_roles = new string[0];
                  string[] student_roles = Student.students[student_idx].roles;

                  if (student_roles.Length == roles.Length) {
                    console.write_line("[red]No hay roles para agregarle a este estudiante[/]");
                    break;
                  }

                  foreach (var idx in roles) {
                    if (Array.IndexOf(student_roles, idx) != -1) continue;
                    filtered_roles = array.add(filtered_roles, idx);
                  }

                  string role = console.read_select(filtered_roles, "Seleccione el rol a agregar: ");
                  msg = Student.add_role(student, role);
                  console.write_line(msg);

                  break;
                case "Eliminar": 
                  if (Student.students[student_idx].roles.Length == 0) {
                    console.write_line("[red]No tiene roles para eliminarle a este estudiante[/]");
                    break;
                  }

                  role = console.read_select(Student.students[student_idx].roles, "Selecciona el rol a eliminar: "); 
                  msg = Student.remove_role(student, role);
                  console.write_line(msg);

                  break;
              }
            break;
          }

          break;
        case "Eliminar estudiante":
          if (Student.students_list().Length == 0) {
            console.write_line("[red]No hay estudiantes que eliminar en la lista[/]");
            break;
          }

          student = console.read_select(Student.students_list(), "Selecciona un estudiante para eliminar");
          bool confirm = console.read_confirm("Seguro de que quieres eliminar este estudiante?: ");

          if (!confirm) {
            console.write_line("[#b5b5b5]Accion cancelada[/]");
            break;
          };

          msg = Student.remove_student(student);
          console.write_line(msg);

          break;
      }
    }
  }

  private static void roles_selection() {
// {"Ver roles", "Agregar rol", "Editar rol", "Eliminar rol", "<- Atras"};
    while(true) {
      string menu_roles = console.read_select(roles_options, "Roles");      
      if (menu_roles == "<- Atras") break;

      switch (menu_roles) {
        case "Ver roles":
          show_table(["Rol"], array.to_2d(roles));

          break;
        case "Agregar rol":
          string role;

          role = console.read_string("Ingresa el rol que desea agregar: ");

          if (Array.IndexOf(roles, role) != -1) {
            console.write_line("[red]Este rol ya existe[/]");
            break; 
          }

          roles = array.add(roles, role);
          console.write_line("[green]Rol agregado correctamente[/]");
          break;
        case "Editar rol":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles para editar[/]");
            break;
          }
          role = console.read_select(roles, "Selecciona un rol para editar");
          string new_role = console.read_string("Ingresa el nuevo nombre para este rol: ");
          int idx = Array.IndexOf(roles, role);

          bool exists = Array.IndexOf(roles, new_role) != -1;
          if (exists) {
            console.write_line("[red]El rol que intenta asignar ya existe[/]");
            break;
          }

          roles[idx] = new_role;
          console.write_line("[green]Rol actualizado correctamente[/]");

          break;
        case "Eliminar rol":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles que eliminar[/]");
            break;
          }

          role = console.read_select(roles, "Selecciona un rol para eliminar");
          idx = Array.IndexOf(roles, role);
          bool confirm = console.read_confirm("Seguro de que quieres eliminar este rol?: ");

          if (!confirm){
            console.write_line("[#b5b5b5]Accion cancelada[/]");
            break;
          }

          roles = array.remove(roles, idx);
          console.write_line("[green]Rol eliminado correctamente[/]");
          break;
      }
    } 
  }
}


/* 
necesidades claras ------------------------------------

[x] repetir hasta que el usuario desee salir
[x] cuando quiera salir pedir confirmacion de querer salir

[x] crear una lista donde se almacenen roles
[x] crear una lista donde se almacenen estudiantes
[x] crear helpers para agregar, editar y eliminar estudiantes
[x] crear helpers para agregar, editar y eliminar roles

[x] seleccionar dos estudiantes al azar de la lista
[x] asignar roles a esos estudiantes seleccionados
[x] no puede salir un mismo estudiante en el mismo giro de ruleta
[x] permitir girar la ruleta varias veces
[x] ver los dos ultimos seleccionados del giro anterior

[x] las entradas del usuario deben ser siempre validadas
[x] manejo de errores ---------
  [x] listas vacias
  [x] opciones del menu incorrectas

[] guardar historial de giros anteriores en archivos txt o csv
[] permitir acceder al historial de giros anteriores

*/

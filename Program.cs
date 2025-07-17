using System;
using Helpers;
using Spectre.Console;
using Humanizer;
using Newtonsoft.Json;
using Clases;
using System.IO;

internal class Program {

  private static readonly string[] menu_options = {"Ruleta", "Estudiantes", "Roles", "Ver registros", "Salir"};
  private static readonly string[] students_options = {"Ver estudiantes", "Agregar estudiante", "Editar estudiante", "Eliminar estudiante", "<- Atras"};
  private static readonly string[] edit_student_actions = {"Nombre", "Roles"};
  private static readonly string[] roles_options = {"Ver roles", "Agregar rol", "Editar rol", "Eliminar rol", "<- Atras"};
  private static readonly string[] roulette_options = {"Girar ruleta", "Girar ruleta con ultima seleccion", "<- Atras"};

  private static string[] roles = new string[0];
  private static string[] last_roulette_selection = new string[0];
  private static string[] registries_options = {"registros", "<- Atras"};
  private static FigletFont font = FigletFont.Load("larry3d.flf");

  private static void Main(string[] args) {
    Student.load();
    loadRoles();

    AnsiConsole.Progress()
    .Start(ctx => {
      var task1 = ctx.AddTask("[green]Cargando procesos[/]");
      var task2 = ctx.AddTask("[green]Cargando archivos y datos[/]");

      while(!ctx.IsFinished) {
        task1.Increment(1.5);
        task2.Increment(0.5);
        Thread.Sleep(25);
      }
    });
    Thread.Sleep(400);
    AnsiConsole.Clear();

    while(true) {
      AnsiConsole.Write(new FigletText(font, "Bienvenido"));
      var menu = console.read_select(menu_options, "Menu Principal");

      if (menu == "Salir") {
        bool exit = confirm_exit();
        if (exit) break;
      }  

      menu_selection(menu);

    } 

    var roulette_history = JsonConvert.SerializeObject(Student.assign_registry());
    var students_registry = JsonConvert.SerializeObject(Student.students_list());
    var roles_registry = JsonConvert.SerializeObject(roles);
    
    string timeStamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");

    File.WriteAllText($"registros/registro_{timeStamp}.txt", roulette_history);
    File.WriteAllText("estudiantes.txt", students_registry);
    File.WriteAllText("roles.txt", roles_registry);
  }

  private static void loadRoles() {
    var txt = File.ReadAllText("roles.txt");
    string[] roles_list = JsonConvert.DeserializeObject<string[]>(txt) ?? Array.Empty<string>();
    foreach (string role in roles_list) roles = array.add(roles, role);
  }

  private static bool confirm_exit() {
    return console.read_confirm("Estas seguro de que quieres salir?: "); 
  }

  private static void show_table(string[] headers, string[,] rows, string empty = "La lista esta vacia") {
    if (rows.GetLength(0) == 0) {
      console.write_line($"[red]{empty}[/]");
      console.read_key();
      AnsiConsole.Clear();
      return;
    }
    var table = console.create_table(headers);
    console.add_rows(table, rows);
    AnsiConsole.Write(table);
    console.read_key();
    AnsiConsole.Clear();
  }

  private static void menu_selection(string menu) {
    switch (menu) {
      case "Estudiantes":
        AnsiConsole.Clear();
        students_selection();
        break;
      case "Roles":
        AnsiConsole.Clear();
        roles_selection();
        break;
      case "Ruleta":
        AnsiConsole.Clear();
        roulette_selection();
        break;
      case "Ver registros":
        AnsiConsole.Clear();
        string[] files = Directory.GetFiles("registros");
        if (files.Length == 0) {
          console.write_line("[red]No hay ningun registro todavia[/]");
          console.read_key();
          break;
        }
        load_registries();
        view_registries();
        break;
    }
  }

  private static void load_registries() {
    string[] files = Directory.GetFiles("registros").OrderByDescending(f => f).ToArray();
    if (files.Length == 0) return;

    registries_options = new string[0];

    foreach (string file in files) {
      string time_string = file
        .Replace("registros\\registro_", "")
        .Replace(".txt", "")
        .Replace("_", " ");

      var file_timestamp = DateTime.ParseExact(time_string, "yyyy-MM-dd HH-mm-ss", null);
      var difference = DateTime.Now - file_timestamp;
      string humanized = difference.Humanize();
      string registry_option = $"Registro de hace {humanized}";

      registries_options = array.add<string>(registries_options, registry_option);
    }

    registries_options = array.add<string>(registries_options, "<- Atras");
  }


  private static void view_registries() {
    while(true) {
      AnsiConsole.Write(new FigletText(font, "Registros"));
      string[] files = Directory.GetFiles("registros").OrderByDescending(f => f).ToArray();
      string menu_registry = console.read_select(registries_options, "Registros");

      if (menu_registry == "<- Atras") break;

      int file_idx = Array.IndexOf(registries_options, menu_registry);
      var txt = File.ReadAllText(files[file_idx]);
      Student[] spins = JsonConvert.DeserializeObject<Student[]>(txt) ?? Array.Empty<Student>();

      show_table(["Estudiante", "Rol"], Student.students_list_matrix(spins), "En este registro no se asigno o giro la ruleta de roles en ningun momento");
    }
  }

  private static void roulette_selection() {
    while(true) {
      AnsiConsole.Write(new FigletText(font, "Ruleta"));
      string menu_roulette = console.read_select(roulette_options, "Ruleta");
      if (menu_roulette == "<- Atras") break;

      switch (menu_roulette) {
        case "Girar ruleta":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles para girar la ruleta[/]");
            console.read_key();
            break;
          } else if (Student.students.Length == 0) {
            console.write_line("[red]No hay estudiantes para girar la ruleta[/]");
            console.read_key();
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
            console.read_key();
          }
          break; 
        case "Girar ruleta con ultima seleccion":
          if (last_roulette_selection.Length == 0) {
            console.write_line("[red]No ha hecho ningun giro de ruleta todavia[/]");
            console.read_key();
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
            console.read_key();
            break;
          }

          try {
            string[,] msg = Roulette.spin(last_roulette_selection); 
            show_table(["Estudiante", "Rol"], msg);
          } catch (Exception ex) {
            console.write_line(ex.Message);
            console.read_key();
          }
          break;
      }
    }

    AnsiConsole.Clear();
  }

  private static void students_selection() {
    while(true) {
      AnsiConsole.Write(new FigletText(font, "Estudiantes"));
      string menu_students = console.read_select(students_options, "Estudiantes");
      if (menu_students == "<- Atras") break;

      switch (menu_students) {
        case "Ver estudiantes":
          console.delay("Cargando estudiantes");
          show_table(["Estudiante", "Roles"], Student.students_list_matrix(Student.students), "No hay ningun estudiante que mostrar");
          break;
        case "Agregar estudiante":
          string student;
          string msg;

          student = console.read_string("Ingresa el nombre del estudiante que desea agregar: ");

          msg = Student.add_student(student);
          console.write_line(msg);
          console.read_key();

          break;
        case "Editar estudiante":
          if (Student.students_list().Length == 0) {
            console.write_line("[red]No hay estudiantes que editar en la lista[/]");
            console.read_key();
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
              console.read_key();

            break;
            case "Roles": 
              action = console.read_select(["Agregar", "Eliminar"], "Que desea hacer?: ");
              switch (action) {
                case "Agregar":

                  string[] filtered_roles = new string[0];
                  string[] student_roles = Student.students[student_idx].roles;

                  if (student_roles.Length == roles.Length) {
                    console.write_line("[red]No hay roles para agregarle a este estudiante[/]");
                    console.read_key();
                    break;
                  }

                  foreach (var idx in roles) {
                    if (Array.IndexOf(student_roles, idx) != -1) continue;
                    filtered_roles = array.add(filtered_roles, idx);
                  }

                  string role = console.read_select(filtered_roles, "Seleccione el rol a agregar: ");
                  msg = Student.add_role(student, role);
                  console.write_line(msg);
                  console.read_key();

                  break;
                case "Eliminar": 
                  if (Student.students[student_idx].roles.Length == 0) {
                    console.write_line("[red]No tiene roles para eliminarle a este estudiante[/]");
                    console.read_key();
                    break;
                  }

                  role = console.read_select(Student.students[student_idx].roles, "Selecciona el rol a eliminar: "); 
                  msg = Student.remove_role(student, role);
                  console.write_line(msg);
                  console.read_key();

                  break;
              }
            break;
          }

          break;
        case "Eliminar estudiante":
          if (Student.students_list().Length == 0) {
            console.write_line("[red]No hay estudiantes que eliminar en la lista[/]");
            console.read_key();
            break;
          }

          student = console.read_select(Student.students_list(), "Selecciona un estudiante para eliminar");
          bool confirm = console.read_confirm("Seguro de que quieres eliminar este estudiante?: ");

          if (!confirm) {
            console.write_line("[#b5b5b5]Accion cancelada[/]");
            console.read_key();
            break;
          };

          msg = Student.remove_student(student);
          console.write_line(msg);
          console.read_key();

          break;
      }

      AnsiConsole.Clear();
    }
  }

  private static void roles_selection() {
    while(true) {
      AnsiConsole.Write(new FigletText(font, "Roles"));
      string menu_roles = console.read_select(roles_options, "Roles");      
      if (menu_roles == "<- Atras") break;

      switch (menu_roles) {
        case "Ver roles":
          show_table(["Rol"], array.to_2d(roles), "No hay ningun rol que mostrar");

          break;
        case "Agregar rol":
          string role;

          role = console.read_string("Ingresa el rol que desea agregar: ");

          if (Array.IndexOf(roles, role) != -1) {
            console.write_line("[red]Este rol ya existe[/]");
            console.read_key();
            break; 
          }

          roles = array.add(roles, role);
          console.write_line("[green]Rol agregado correctamente[/]");
          console.read_key();
          break;
        case "Editar rol":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles para editar[/]");
            console.read_key();
            break;
          }
          role = console.read_select(roles, "Selecciona un rol para editar");
          string new_role = console.read_string("Ingresa el nuevo nombre para este rol: ");
          int idx = Array.IndexOf(roles, role);

          bool exists = Array.IndexOf(roles, new_role) != -1;
          if (exists) {
            console.write_line("[red]El rol que intenta asignar ya existe[/]");
            console.read_key();
            break;
          }

          roles[idx] = new_role;
          console.write_line("[green]Rol actualizado correctamente[/]");
          console.read_key();

          break;
        case "Eliminar rol":
          if (roles.Length == 0) {
            console.write_line("[red]No hay roles que eliminar[/]");
            console.read_key();
            break;
          }

          role = console.read_select(roles, "Selecciona un rol para eliminar");
          idx = Array.IndexOf(roles, role);
          bool confirm = console.read_confirm("Seguro de que quieres eliminar este rol?: ");

          if (!confirm){
            console.write_line("[#b5b5b5]Accion cancelada[/]");
            console.read_key();
            break;
          }

          roles = array.remove(roles, idx);
          console.write_line("[green]Rol eliminado correctamente[/]");
          console.read_key();
          break;
      }

      AnsiConsole.Clear();
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

[x] guardar historial de giros anteriores en archivos txt o csv
[x] permitir acceder al historial de giros anteriores

*/

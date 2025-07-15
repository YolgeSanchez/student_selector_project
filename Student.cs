using Spectre.Console;
using Helpers;
using Newtonsoft;
using Newtonsoft.Json;

namespace Clases {
  public class Student {
    public static Student[] students = new Student[0];
    public string name;
    public string[] roles = new string[0];

    public Student(string name) {
      this.name = name;
    }

    public static void load() {
      var json = File.ReadAllText("estudiantes.json");
      string[] students_list = JsonConvert.DeserializeObject<string[]>(json) ?? Array.Empty<string>();
      foreach (string student in students_list) Student.add_student(student);
    }

    public static string[] students_list() {
      string[] names = new string[students.Length];
      for (int idx = 0; idx < students.Length; idx++) names[idx] = students[idx].name;

      return names;
    }

    public static string[,] students_list_matrix() {
      string[,] students_matrix = new string[students.Length, 2];

      for (int row = 0; row < students.Length; row++) {
        string name = students[row].name;
        string roles = string.Join(", ", students[row].roles);
        roles = string.IsNullOrEmpty(roles) ? "[#b5b5b5]<sin roles>[/]" : roles;

        students_matrix[row, 0] = name;
        students_matrix[row, 1] = roles;
      }

      return students_matrix;
    }

    public static string add_role(string student_name, string rol) {
      int idx = Array.IndexOf(students_list(), student_name);
      string[] roles = students[idx].roles;
      
      try {
        students[idx].roles = array.add(roles, rol);
      } catch {
        return "[red]Este estudiante ya posee ese rol[/]";
      }

      return "[green]Rol agregado correctamente[/]";
    }

    public static string remove_role(string student_name, string rol) {
      int idx = Array.IndexOf(students_list(), student_name);
      string[] roles = students[idx].roles;
      int rol_idx = Array.IndexOf(roles, rol);

      students[idx].roles = array.remove(roles, rol_idx);

      return "[green]Rol eliminado correctamente[/]";
    }

    public static bool find_role(string student_name, string rol) {
      int idx = Array.IndexOf(students_list(), student_name);
      string[] roles = students[idx].roles;

      return Array.IndexOf(roles, rol) != 1;
    }

    public static string add_student(string name) {
      if (Array.IndexOf(students_list(), name) != -1) return "[red]Este estudiante ya existe[/]";

      Student student = new Student(name);
      students = array.add<Student>(students, student);

      return "[green]Estudiante agregado correctamente[/]";
    }

    public static string remove_student(string name) {
      int idx = Array.IndexOf(students_list(), name);

      students = array.remove<Student>(students, idx);

      return "[green]Estudiante eliminado correctamente[/]";
    }

    public static string edit_student(string name, string new_name) {
      if (Array.IndexOf(students_list(), new_name) != -1) return "[red]El nombre que intenta asignar ya existe[/]";

      int idx = Array.IndexOf(students_list(), name);
      students[idx].name = new_name;

      return "[green]Estudiante actualizado correctamente[/]";
    }

    public static Student[] assign_registry() {
      Student[] assigned = new Student[0];

      foreach(Student student in students) {
        if (student.roles.Length == 0) continue;
        assigned = array.add<Student>(assigned, student);
      }

      return assigned;
    }
  }
}
using Spectre.Console;
using Helpers;

namespace Student {
  public class Student {
    public static Student[] students = new Student[0];
    string name;
    string[] roles = new string[0];

    public Student(string name) => this.name = name;

    public static string[] students_list() {
      string[] names = new string[students.Length];
      for (int idx = 0; idx < students.Length; idx++) names[idx] = students[idx].name;

      return names;
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
      if (Array.IndexOf(students_list(), new_name) != 1) return "[red]El nombre que intenta asignar ya existe[/]";

      int idx = Array.IndexOf(students_list(), name);
      students[idx].name = new_name;

      return "[green]Estudiante actualizado correctamente[/]";
    }
  }
}
using Helpers;
using Clases;
using Spectre.Console;

namespace Clases {
  public static class Roulette {
    public static string[,] spin(string[] roles) {
      Random rnd = new Random();  

      string[,] result = new string[roles.Length, 2];
      string[] students = Student.students_list();
      int rs_idx = 0;
      bool valid = true;

      foreach (string role in roles) {
        int[] seen = new int[0]; 
        valid = false;

        while (seen.Length != students.Length) {
          int idx = rnd.Next(students.Length);

          if (Array.IndexOf(seen, idx) != -1) continue; 

          bool alreadyAssigned = false;
          for (int row = 0; row < rs_idx; row++) {
            if (result[row,0] == students[idx]) {
              alreadyAssigned = true;
              break;
            }
          }

          if (alreadyAssigned || Array.IndexOf(Student.students[idx].roles, role) != -1) {
            seen = array.add(seen, idx);
            continue;
          }

          visualRoulette(students, role, idx);
          
          result[rs_idx, 0] = students[idx];
          result[rs_idx, 1] = role;
          rs_idx++;
          valid = true;
          break;
        }

        if (!valid) throw new Exception("[red]No es posible hacer este giro[/]");
      }

      for (int row = 0; row < result.GetLength(0); row++) {
        Student.add_role(result[row, 0], result[row, 1]);
      }

      return result;
    }

    private static void visualRoulette(string[] students, string role, int final_idx) {
      int spins = new Random().Next(30, 40);
      int curr = 0;

      for (int spin = 0; spin < spins; spin++) {
        AnsiConsole.Clear();
        console.write_line($"[bold yellow]Seleccionando {role}...[/]\n");

        int highlight_idx = curr % students.Length;

        for (int student_idx = 0; student_idx < students.Length; student_idx++) {
          if (student_idx == highlight_idx) console.write_line($"[bold green]-> {students[student_idx]}[/]");
          else console.write_line($"   {students[student_idx]}");
        }

        curr++;
        Thread.Sleep(80 + spin * 3);
      }

      AnsiConsole.Clear();
      console.write_line("[bold yellow]¡Seleccionado![/]\n");

      for (int student_idx = 0; student_idx < students.Length; student_idx++) {
        if (student_idx == final_idx) console.write_line($"[bold green]-> {students[student_idx]}[/]");
        else console.write_line($"   {students[student_idx]}");
      }
      Thread.Sleep(2000);
    }
  }
}
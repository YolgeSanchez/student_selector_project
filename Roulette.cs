using Helpers;
using Clases;

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

          if (alreadyAssigned) {
            seen = array.add(seen, idx);
            continue;
          } else if (Array.IndexOf(Data.students[idx].roles, role) != -1) {
            seen = array.add(seen, idx);
            continue;
          }
          
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
  }
}
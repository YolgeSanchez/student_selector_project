using Helpers;
using Clases;

namespace Clases {
  public static class Roulette {
    public static string[,] spin() {
      Random rnd = new Random();  
      // en el caso de que solo tenga 2 roles
      int a = 0, b = 0;
      do {
        a = rnd.Next(Data.students.Length);
        b = rnd.Next(Data.students.Length);
      } while (a == b);

      string name_a = Data.students[a].name;
      string name_b = Data.students[b].name;

      Student.add_role(name_a, Data.roles[0]);
      Student.add_role(name_b, Data.roles[1]);

      string[,] selection = {{name_a, Data.roles[0]}, {name_b, Data.roles[1]}};
      return selection;
    }
  }
}

/* 
que necesito?
tener un registro de los estudiantes y los roles que se les han sido asignados hoy
poder girar la ruleta y obtener 2 estudiantes aleatorios
*/
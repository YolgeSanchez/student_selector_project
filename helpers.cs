namespace Helpers {
  public static class array {
    // add an element to an array
    public static string[] add(string[] arr, string val) {
      int n = arr.Length;
      string[] new_arr = new string[n];
      new_arr[n] = val;

      for (int i = 0; i < n-1; i++) {
        new_arr[i] = arr[i];
      }

      return new_arr;
    }

    // edit an element from an array
    public static string[] edit(string[] arr, string val, int idx) {
      arr[idx] = val;
      return arr;   
    }

    // delete an element from an array
    public static string[] delete(string[] arr, int idx) {
      int n = arr.Length - 1;
      string[] new_arr = new string[n];

      for (int i = 0; i < n; i++) {
        if (i == idx) continue;
        new_arr[i] = arr[i];
      }

      return new_arr;
    }
  }

  // other helpers maybe later
}
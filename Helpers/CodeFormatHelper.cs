namespace Helpers;

public static class CodeFormatHelper
{
   public static List<string> HighlightedCommands = new List<string>() 
   {
      "DbWrite", 
      "System.out.print",  // Java print command
      "System.out.println", // Java println command
      "disp",  // MATLAB disp command
      "fprintf",  // MATLAB fprintf command
      "Console.Write",  // C# Write command
      "Console.WriteLine"  // C# WriteLine command
   };
   
   public static string GetInformationText(string platform)
   {
      return platform.ToLower() switch
      {
         "java" => "For Java, use 'System.out.println', 'System.out.print' or 'DbWrite' to output text.",
         "matlab" => "For MATLAB, use 'disp', 'fprintf' or 'DbWrite' to output text.",
         "csharp" => "For C#, use 'Console.WriteLine', 'Console.Write'' or 'DbWrite' to output text.",
         _ => "Use 'DbWrite' for database operations, available across all platforms."
      };
   }
}
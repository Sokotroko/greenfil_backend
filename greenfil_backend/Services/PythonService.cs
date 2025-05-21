using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Greenfil.Backend.Services;

public class PythonService
{
    public async Task<string> GenerateStlFromImage(string imagePath)
    {
        string scriptPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            "PythonScripts",
            "image_to_stl.py"
        );

        if (!File.Exists(scriptPath))
            throw new FileNotFoundException($"Script Python no encontrado: {scriptPath}");

        string outputDir = Path.Combine(Directory.GetCurrentDirectory(), "modelos_3d");
        Directory.CreateDirectory(outputDir);

        string outputName = Path.GetFileNameWithoutExtension(imagePath) + ".stl";

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = $"\"{scriptPath}\" \"{imagePath}\" \"{outputDir}\" \"{outputName}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                WorkingDirectory = Directory.GetCurrentDirectory()
            }
        };

        process.Start();
        
        string[] outputLines = (await process.StandardOutput.ReadToEndAsync())
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        string stlPath = outputLines.LastOrDefault()?.Trim() ?? "";

        string error = await process.StandardError.ReadToEndAsync();
        if (!string.IsNullOrWhiteSpace(error) && error.ToLower().Contains("error"))
            throw new Exception($"Python error: {error}");

        if (!File.Exists(stlPath))
            throw new FileNotFoundException($"STL no generado en: {stlPath}");

        return stlPath;
    }
}
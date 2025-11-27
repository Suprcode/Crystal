using System.Text.RegularExpressions;

//string localizationFile = @"C:\Users\Tang\Downloads\MapInfoExport已翻译.txt";
//string originalFile = @"C:\Users\Tang\Downloads\MapInfoExport.txt";

//var original=File.ReadAllLines(originalFile);
//var localization=File.ReadAllLines(localizationFile);

//for (int i = 0; i < original.Length; i++)
//{
//    var line = original[i];
//    if (line.StartsWith("["))
//    {
//        var match = Regex.Match(line, @"^\[(\S+)\s(\S+)\]");
//        var mapFileName=match.Groups[1].Value;
//        var mapName=match.Groups[2].Value;

//        var localizationLine = localization.FirstOrDefault(x => x.StartsWith("["+mapFileName));
//        if (localizationLine != null)
//        {
//            var locMatch = Regex.Match(localizationLine, @"^\[(\S+)\s(.+)\]");

//            Console.WriteLine($"替换前：{original[i]}");
//            original[i]= Regex.Replace(line, @"^\[(\S+)\s(\S+)\]", $"[{mapFileName} {locMatch.Groups[2].Value}]");
//            Console.WriteLine($"替换后：{original[i]}");
//        }
//    }
//}

//File.WriteAllLines(originalFile, original);


string originalFile = @"C:\Users\Tang\Downloads\ItemInfo Output.csv";
string localizationFile = @"C:\Users\Tang\Downloads\ItemInfo Output已翻译.csv";

var original = File.ReadAllLines(originalFile).Skip(1).ToArray();
var localization = File.ReadAllLines(localizationFile).Skip(1).ToArray();

Dictionary<string, string> itemDic = new();

foreach (var it in original)
{
    var row = it.Split(',');
    var index = row[0];
    var name = row[1];

    var localizationlocRow = localization.FirstOrDefault(x => x.StartsWith(index + ","));
    if (localizationlocRow != null)
    {
        var locRow = localizationlocRow.Split(',');
        var locName = locRow[1];
        itemDic.TryAdd(name, locName);
    }
}
var dropFiles = Directory.GetFiles(@"C:\Users\Tang\Documents\Mir2\Crystal_ZhCN\Build\Server\Debug\Envir\Drops", "*.txt", SearchOption.AllDirectories);
int complateCount = 0;
foreach (var file in dropFiles)
{
    var lines = File.ReadAllLines(file);
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        var match = Regex.Match(line, @"^\d+/\d+\s+(\S+)");
        if (match.Success)
        {
            string itemName = match.Groups[1].Value;
            if (itemDic.TryGetValue(itemName, out var locName))
            {
                lines[i] = line.Replace(itemName, locName);
            }


        }
        else
        {

        }
    }
    File.WriteAllLines(file, lines);
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write($"处理掉落表文件进度：{++complateCount}/{dropFiles.Length}");
    


}
Console.WriteLine();
var recipeFiles = Directory.GetFiles(@"C:\Users\Tang\Documents\Mir2\Crystal_ZhCN\Build\Server\Debug\Envir\Recipe", "*.txt", SearchOption.AllDirectories);
complateCount = 0;
foreach (var recipe in recipeFiles)
{
    var lines = File.ReadAllLines(recipe);
    bool isIngredientsOrTools = false;
    for (int i = 0; i < lines.Length; i++)
    {
        var line = lines[i];
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("["))
        {
            continue;
        }
        else
        {
            var match = Regex.Match(line, @"^([^\s|[]+)(?=\s?)");
            if(match.Success)
            {
                string itemName = match.Groups[1].Value;
                if (itemDic.TryGetValue(itemName, out var locName))
                {
                    lines[i] = line.Replace(itemName, locName);
                }
            }
        }
    }
    File.WriteAllLines(recipe, lines);

    var oldName = Path.GetFileNameWithoutExtension(recipe);
    if(itemDic.TryGetValue(oldName, out var locRecipeName))
    {
        var newName = locRecipeName;
        File.Move(recipe,recipe.Replace(oldName,newName));
    }
    Console.SetCursorPosition(0, Console.CursorTop);
    Console.Write($"处理配方文件进度：{++complateCount}/{recipeFiles.Length}");
    
}
Console.WriteLine();
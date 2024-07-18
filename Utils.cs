namespace FormSorter;

public class Utils
{
    public static HashSet<string> eirrmu_hotels = File.ReadAllLines("eirrmu_hotels.txt").ToHashSet();
    public static bool IsForEirrmu(string file)
    {
        // 1
        if (IsError(file))
        {
            return false;
        }

        // 2
        if (IsScala(file))
        {
            return true;
        }
        
        // 3
        var markId = "9449";
        var malahitId = "9437";
        var ummsId = new Xml().GetUmmsId(file);
        if (ummsId == markId)
        {
            return true;
        }
        if (ummsId == malahitId)
        {
            if (IsRegForm(file))
            {
                return true;
            }
        }

        // 4
        var hotelId = Xml.GetHotelId(file);
        return eirrmu_hotels.Contains(hotelId);
    }


    public static void CreateFolders(string resutlFolder)
    {
        var eirrmuFolder = Path.Combine(resutlFolder, "eirrmu");
        var ppoFolder = Path.Combine(resutlFolder, "ppo");
        if (!Directory.Exists(resutlFolder))
        {
            Directory.CreateDirectory(resutlFolder);
        }
        if (!Directory.Exists(eirrmuFolder))
        {
            Directory.CreateDirectory(eirrmuFolder);
        }
        if (!Directory.Exists(ppoFolder))
        {
            Directory.CreateDirectory(ppoFolder);
        }
    }

    public static void CopyFiles(string sourceFolder, string targetFolder)
    {
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            if (Path.GetExtension(file).ToLower() != ".xml")
            {
                continue;
            }

            if (IsForEirrmu(file))
            {
                string targetFile = Path.Combine(targetFolder, "eirrmu", Path.GetFileName(file));
                if (!File.Exists(targetFile))
                {
                    File.Copy(file, targetFile);
                }
            }
            else
            {
                string targetFile = Path.Combine(targetFolder, "ppo", Path.GetFileName(file));
                if (!File.Exists(targetFile))
                {
                    new Xml().CutTag(file, targetFile, "Signature"); 
                }
            }
        }

        foreach (var subDir in Directory.GetDirectories(sourceFolder))
        {
            if (IsSortedFolder(subDir))
                continue;
            CopyFiles(subDir, targetFolder);
        }
    }



    public static void MoveFiles(string sourceFolder, string targetFolder)
    {
        foreach (var file in Directory.GetFiles(sourceFolder))
        {
            if (Path.GetExtension(file).ToLower() != ".xml")
            {
                continue;
            }

            var tmp = Path.GetDirectoryName(file).Split('\\');
            string dir = tmp[tmp.Length - 1];
            string targetDir = Path.Combine(targetFolder, dir);
            if (!Directory.Exists(targetDir)) { Directory.CreateDirectory(targetDir); }
            string targetFile = Path.Combine(targetFolder, dir, Path.GetFileName(file));
            File.Move(file, targetFile, true);
        }

        foreach (var subDir in Directory.GetDirectories(sourceFolder))
        {
            MoveFiles(subDir, targetFolder);
        }
    }

    

    public static bool IsRegForm(string file)
    {
        return file.ToLower().Contains("form5");
    }

    public static bool IsScala(string file)
    {
        return file.ToLower().Contains("skala");
    }

    public static bool IsSortedFolder(string file)
    {
        return file.ToLower().Contains("sorted");
    }

    public static bool IsError(string file)
    {
        return file.ToLower().Contains("error");
    }
}

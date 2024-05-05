using luval.glhp.Nodes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace luval.glhp
{
    public static class Transform
    {
        public static void ToPlaceVisitCsvFile(string account, string sourceDirectory, string outputFile)
        {
            ToPlaceVisitCsvFile(account, sourceDirectory, outputFile, NullLogger.Instance);
        }


        public static void LoadAllVisitInfosToCsv(string account, string sourceDirectory, string outputFile, ILogger logger)
        {
            var infos = LoadAllFilesToVisitInfo(account, sourceDirectory, logger);
            using (var resultFile = new StreamWriter(outputFile))
            {
                logger.LogInformation($"Writing {infos.Count} records into file");
                resultFile.WriteLine(VisitInfo.ToCsvHeader());
                foreach (var info in infos)
                {
                    resultFile.WriteLine(info.ToCsv());
                }
                resultFile.Close();
                logger.LogInformation($"Done loading to file {outputFile}");
            }
        }

        public static List<VisitInfo> LoadAllFilesToVisitInfo(string account, string sourceDirectory, ILogger logger)
        {
            var dirInfo = new DirectoryInfo(sourceDirectory);
            if (!dirInfo.Exists) throw new DirectoryNotFoundException($"{sourceDirectory} not valid");

            var result = new List<VisitInfo>();
            var files = dirInfo.GetFiles("*.json", SearchOption.AllDirectories).OrderBy(x => x.Name).ToList();

            var fileCount = files.Count;
            var count = 1;

            foreach (var file in files)
            {
                logger.LogInformation($"Parsing {count} of {fileCount} file {file.Name}");

                var history = LoadFile(file);
                var infos = history.GetVisitInfos();
                infos.ForEach(i => { i.Account = account; });
                result.AddRange(infos);
                count++;
            }

            logger.LogInformation($"Done loading information, {result.Count} records loaded");

            return result;
        }

        public static void ToPlaceVisitCsvFile(string account, string sourceDirectory, string outputFile, ILogger logger)
        {
            var dirInfo = new DirectoryInfo(sourceDirectory);
            if (!dirInfo.Exists) throw new DirectoryNotFoundException($"{sourceDirectory} not valid");

            var files = dirInfo.GetFiles("*.json", SearchOption.AllDirectories).OrderBy(x => x.Name).ToList();

            using (var resultFile = new StreamWriter(outputFile))
            {
                var isFirt = true;
                var fileCount = files.Count;
                var count = 1;
                foreach (var file in files)
                {
                    logger.LogInformation($"Parsing {count} of {fileCount} file {file.Name}");

                    var history = LoadFile(file);
                    var places = history.GetPlaceVisits();
                    if (isFirt)
                    {
                        resultFile.WriteLine(ChildVisit.CsvHeader());
                        isFirt = false;
                    }
                    foreach (var place in places)
                    {
                        resultFile.WriteLine(place.ToCsv().Replace("@account", account));
                    }
                    count++;
                }
                resultFile.Close();
                logger.LogInformation($"Completed");
            }
        }

        public static LocationHistory LoadFile(FileInfo file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            if (!file.Exists) throw new FileNotFoundException($"file {file.Name} not found");
            return JsonConvert.DeserializeObject<LocationHistory>(File.ReadAllText(file.FullName));
        }
    }
}

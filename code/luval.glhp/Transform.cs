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

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using NemTracker.Dtos.Reports;
using NemTracker.Tools.Features;

namespace NemTracker.Features.Tools
{
    public class P5ReportProcessor
    {
        private readonly string _tempStoragePath;
        private static readonly string _nemwebHost = "http://nemweb.com.au/";
        private static readonly string _reportPath = "Reports/Current/P5_Reports/";
        private List<string> _files = new List<string>();
        
        public P5ReportProcessor()
        {
            _tempStoragePath = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/nemTracker/";

            Console.WriteLine(_tempStoragePath);
            
            if (!Directory.Exists(Environment.SpecialFolder.LocalApplicationData.ToString()))
            {
                Directory.CreateDirectory(Environment.SpecialFolder.LocalApplicationData.ToString());
            }
            
            if (!Directory.Exists(_tempStoragePath))
            {
                Directory.CreateDirectory(_tempStoragePath);
            }
            
            if (!Directory.Exists(P5MinPath()))
            {
                Directory.CreateDirectory(P5MinPath());
            }
        }

        public static List<ReportDto> CheckNewInstructions()
        {
            
            var files = ListFiles();

            var instructions = (from file in files

                let instructionStamps = file
                    .Replace("/Reports/Current/P5_Reports/PUBLIC_P5MIN_", "")
                    .Replace(".zip", "")
                    .Split("_")
                
                select new InstructionFile() {
                    Path = file, 
                    PeriodStamp = long.Parse(instructionStamps[0]), 
                    GenerationStamp =  long.Parse(instructionStamps[1])}).ToList();

            var now = DateTime.Now;
            
            var nowAEMO 
                = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("Australia/Brisbane"));
            
            var instructionStamp = nowAEMO.NextNemInterval();

            var currentReports = instructions
                .Select(instruction => instruction
                .GetDto(instructionStamp)).ToList();

            return currentReports;

        }

        public List<RegionSolutionDto> ProcessLines(ReportDto file)
        {
            DownloadExtractInstruction(file);
            
            var results = new List<RegionSolutionDto>();
            var fileName = P5MinPath() + "PUBLIC_P5MIN_" + file.IntervalDateTime.ToString("yyyyMMddHHmm")  
                           + "_" +  file.PublishDateTime.ToString("yyyyMMddHHmmss");
            
            using var reader = new StreamReader(fileName + ".csv");

            var regionSolutions = new List<RegionSolutionDto>();
            
            while (reader.ReadLine() is { } line)
            {
                var lineSplit = line.Split(",");
                
                if (lineSplit[0].Contains("D"))
                {
                    var aemoType = (ReportTypeEnum) Int16.Parse(lineSplit[3]);
                    
                    switch (aemoType)
                    {
                        case ReportTypeEnum.RegionSolution:
                            var dto = lineSplit.ProcessRegionSolutionLine();
                            regionSolutions.Add(dto);
                            break;
                    }
                }
                
            }

            return regionSolutions;

        }

        /*
         * Helpers for file download and management
         */
        private static List<string> ListFiles()
        {
            var html = string.Empty;
            var url = _nemwebHost + _reportPath;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            var response = (HttpWebResponse) request.GetResponse();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return new List<string>();
            }
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
            
            var lines = html
                .Split("<A HREF=\"/Reports/Current/P5_Reports/DUPLICATE/\">DUPLICATE</A><br>")[1]
                .Split("<br></pre><hr></body></html>")[0].Split("<br>");

             var files = lines.Select(line => line.Split("<A HREF=\"")[1]
                    .Split("\">PUBLIC_P5MIN_")[0])
                .ToList();

             return files;
        }

        private void DownloadExtractInstruction(ReportDto file)
        {
            var fileName = P5MinPath() + file.IntervalDateTime.ToString("yyyyMMddHHmm") 
                                       + "_" + file.PublishDateTime.ToString("yyyyMMddHHmmss");

            var zipExists = File.Exists(fileName + ".zip");
            
            if (!zipExists)
            {
                using var httpClient = new WebClient();
                httpClient.DownloadFile(
                    _nemwebHost + file.Path,
                    fileName + ".zip"
                );
            }
            
            if (!File.Exists(P5MinPath() + "PUBLIC_P5MIN_" + file.IntervalDateTime.ToString("yyyyMMddHHmm")
                             + "_" + file.PublishDateTime.ToString("yyyyMMddHHmmss") + ".csv"))
            {
                ZipFile.ExtractToDirectory(fileName + ".zip",
                    P5MinPath());
            }
            
        }
        
        private class InstructionFile
        {
            public string Path;
            public long PeriodStamp;
            public long GenerationStamp;

            public ReportDto GetDto(long nextNemInterval)
            {
                var dto = new ReportDto
                {
                    PublishDateTime = GenerationStamp.GetDateTime("yyyyMMddHHmmss"),
                    IntervalDateTime = PeriodStamp.GetDateTime("yyyyMMddHHmm"),
                    Path = Path,
                    Processed = false,
                    IntervalProcessType = (IntervalProcessTypeEnum) 0,
                    ReportType = ReportTypeEnum.RegionSolution
                };

                dto.IntervalProcessType = PeriodStamp == nextNemInterval 
                        ? IntervalProcessTypeEnum.Realtime : IntervalProcessTypeEnum.Historical;
                
                return dto;
            }
            
        }

        private string P5MinPath()
        {
            return _tempStoragePath + "P5_Min/";
        }
        
    }
}
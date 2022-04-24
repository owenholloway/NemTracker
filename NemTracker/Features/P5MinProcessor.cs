using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using NemTracker.Dtos.P5Minute;
using NemTracker.Tools.Features;

namespace NemTracker.Features
{
    public class P5MinProcessor
    {
        private readonly string _tempStoragePath;
        private readonly string _nemwebHost = "http://nemweb.com.au/";
        private readonly string _reportPath = "Reports/Current/P5_Reports/";
        private List<string> _files = new List<string>();
        private P5MinuteDataDto _data = new P5MinuteDataDto();
        
        public P5MinProcessor()
        {
            _tempStoragePath = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/nemTracker/";

            if (!Directory.Exists(_tempStoragePath))
            {
                Directory.CreateDirectory(_tempStoragePath);
            }
            
            if (!Directory.Exists(P5MinPath()))
            {
                Directory.CreateDirectory(P5MinPath());
            }
        }

        public P5MinuteDataDto ProcessInstructions()
        {
            var file = CheckNewInstructions();
            
            if (file is not null)
            {
                ProcessLines(file);
            }

            return _data;
        }

        private InstructionFile CheckNewInstructions()
        {
            
            _files = ListFiles();

            var instructions = (from file in _files

                let instructionStamps = file
                    .Replace("/Reports/Current/P5_Reports/PUBLIC_P5MIN_", "")
                    .Replace(".zip", "")
                    .Split("_")
                
                select new InstructionFile() {
                    Path = file, 
                    PeriodStamp = long.Parse(instructionStamps[0]), 
                    GenerationStamp =  long.Parse(instructionStamps[1])}).ToList();
            
            
            
            //TODO report persistence
            
            var now = DateTime.Now;
            
            var nowAEMO 
                = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("Australia/Brisbane"));
            
            var instructionStamp = nowAEMO.NextNemInterval();

            var currentReports = instructions
                .Select(instruction => instruction
                .GetDto(instructionStamp)).ToList();

            return null;
        }

        private void ProcessLines(InstructionFile file)
        {
            var results = new List<RegionSolutionDto>();
            var fileName = P5MinPath() + "PUBLIC_P5MIN_" + file.PeriodStamp + "_" + file.GenerationStamp;
            
            using var reader = new StreamReader(fileName + ".csv");

            string line;

            while (!File.Exists(P5MinPath() + "PUBLIC_P5MIN_" + file.PeriodStamp 
                                + "_" + file.GenerationStamp + ".csv"))
            {
                Thread.Sleep(15);
            }
            
            Thread.Sleep(15);
            
            while ((line = reader.ReadLine()) != null)
            {
                var lineSplit = line.Split(",");
                
                if (lineSplit[0].Contains("D"))
                {
                    var aemoType = (AemoTypeEnum) Int16.Parse(lineSplit[3]);
                    
                    switch (aemoType)
                    {
                        case AemoTypeEnum.RegionSolution:
                            var dto = lineSplit.ProcessRegionSolutionLine();
                            _data.RegionSolutionDtos.Add(dto);
                            break;
                    }
                }
                
            }
            
        }

        /*
         * Helpers for file download and management
         */
        private List<string> ListFiles()
        {
            var html = string.Empty;
            var url = _nemwebHost + _reportPath;

            var request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (var response = (HttpWebResponse)request.GetResponse())
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

        private void DownloadExtractInstruction(InstructionFile file)
        {
            var fileName = P5MinPath() + file.PeriodStamp + "_" + file.GenerationStamp;

            var zipExists = File.Exists(fileName + ".zip");
            
            if (!zipExists)
            {
                using var httpClient = new WebClient();
                httpClient.DownloadFile(
                    _nemwebHost + file.Path,
                    fileName + ".zip"
                );
            }
            
            if (!File.Exists(P5MinPath() + "PUBLIC_P5MIN_" + file.PeriodStamp 
                             + "_" + file.GenerationStamp + ".csv"))
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
                    IntervalProcessType = (IntervalProcessTypeEnum) 0
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
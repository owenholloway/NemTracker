using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using NemTracker.Dtos.Aemo;
using NemTracker.Dtos.P5Minute;
using NemTracker.Dtos.Stations;
using NemTracker.Tools.Features;

namespace NemTracker.Features
{
    public class P5MinProcessor
    {
        private readonly string _tempStoragePath;
        private readonly string _nemwebHost = "http://nemweb.com.au/";
        private readonly string _reportPath = "Reports/Current/P5_Reports/";
        private string _nextInstructionNumber = null;
        private List<string> _files = new List<string>();
        private readonly string _dateTimeFormat = "yyyy/MM/dd HH:mm:ss";
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
            
            ListFiles();

            var instructions = (from file in _files
                
                let instructionStamps = file
                    .Replace("/Reports/Current/P5_Reports/PUBLIC_P5MIN_", "")
                    .Replace(".zip", "")
                    .Split("_")
                
                select new InstructionFile() {
                    Path = file, 
                    PeriodStamp = instructionStamps[0], 
                    GenerationStamp = instructionStamps[1]}).ToList();

            if (_nextInstructionNumber is not null) return null;
            //TODO report persistence
            var now = DateTime.Now;
            var nowAEMO 
                = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("Australia/Brisbane"));
            var instructionStamp = nowAEMO.NextNemInterval();

            foreach (var instructionFile in from instructionFile in instructions let fileStamp 
                    = Int64.Parse(instructionFile.PeriodStamp) 
                where instructionStamp == fileStamp select instructionFile)
            {
                DownloadExtractInstruction(instructionFile);
                return instructionFile;
            }

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
                            var dto = ProcessRegionSolutionLine(lineSplit);
                            _data.RegionSolutionDtos.Add(dto);
                            break;
                    }
                }
                
            }
            
        }

        private RegionSolutionDto ProcessRegionSolutionLine(string[] line)
        {
            var dto = new RegionSolutionDto();
            
            dto.RunTime = GetDateTime(line[4]);
            dto.Interval = GetDateTime(line[6]);
            dto.Region = line[7].Trim().GetRegion();
            dto.Rrp = line[8].DoubleValue();
            dto.Rop = line[9].DoubleValue();
            dto.ExcessGeneration = line[10].DoubleValue();
            dto.Raise6SecRrp = line[11].DoubleValue();
            dto.Raise6SecRop = line[12].DoubleValue();
            dto.Raise60SecRrp = line[13].DoubleValue();
            dto.Raise60SecRop = line[14].DoubleValue();
            dto.Raise5MinRrp = line[15].DoubleValue();
            dto.Raise5MinRop = line[16].DoubleValue();
            dto.RaiseRegRrp = line[17].DoubleValue();
            dto.RaiseRegRop = line[18].DoubleValue();
            dto.Lower6SecRrp = line[19].DoubleValue();
            dto.Lower6SecRop = line[20].DoubleValue();
            dto.Lower60SecRrp = line[21].DoubleValue();
            dto.Lower60SecRop = line[22].DoubleValue();
            dto.Lower5MinRrp = line[23].DoubleValue();
            dto.Lower5MinRop = line[24].DoubleValue();
            dto.LowerRegRrp = line[25].DoubleValue();
            dto.LowerRegRop = line[26].DoubleValue();
            dto.TotalDemand = line[27].DoubleValue();
            dto.AvailableGeneration = line[28].DoubleValue();
            dto.AvailableLoad = line[29].DoubleValue();
            dto.DemandForecast = line[30].DoubleValue();
            dto.DispatchableGeneration = line[31].DoubleValue();
            dto.DispatchableLoad = line[32].DoubleValue();
            dto.NetInterchange = line[33].DoubleValue();
            dto.Lower5MinDispatch = line[34].DoubleValue();
            dto.Lower5MinImport = line[35].DoubleValue();
            dto.Lower5MinLocalDispatch = line[36].DoubleValue();
            dto.Lower5MinLocalReq = line[37].DoubleValue();
            dto.Lower5MinReq = line[38].DoubleValue();
            dto.Lower60SecDispatch = line[39].DoubleValue();
            dto.Lower60SecImport = line[40].DoubleValue();
            dto.Lower60SecLocalDispatch = line[41].DoubleValue();
            dto.Lower60SecLocalReq = line[42].DoubleValue();
            dto.Lower60SecReq = line[43].DoubleValue();
            dto.Lower6SecDispatch = line[44].DoubleValue();
            dto.Lower6SecImport = line[45].DoubleValue();
            dto.Lower6SecLocalDispatch = line[46].DoubleValue();
            dto.Lower6SecLocalReq = line[47].DoubleValue();
            dto.Lower6SecReq = line[48].DoubleValue();
            dto.Raise5MinDispatch = line[49].DoubleValue();
            dto.Raise5MinImport = line[50].DoubleValue();
            dto.Raise5MinLocalDispatch = line[51].DoubleValue();
            dto.Raise5MinLocalReq = line[52].DoubleValue();
            dto.Raise5MinReq = line[53].DoubleValue();
            dto.Raise60SecDispatch = line[54].DoubleValue();
            dto.Raise60SecImport = line[55].DoubleValue();
            dto.Raise60SecLocalDispatch = line[56].DoubleValue();
            dto.Raise60SecLocalReq = line[57].DoubleValue();
            dto.Raise60SecReq = line[58].DoubleValue();
            dto.Raise6SecDispatch = line[59].DoubleValue();
            dto.Raise6SecImport = line[60].DoubleValue();
            dto.Raise6SecLocalDispatch = line[61].DoubleValue();
            dto.Raise6SecLocalReq = line[62].DoubleValue();
            dto.Raise6SecReq = line[63].DoubleValue();
            dto.AggregateDispatchError = line[64].DoubleValue();
            dto.InitialSupply = line[65].DoubleValue();
            dto.ClearedSupply = line[66].DoubleValue();
            dto.LowerRegImport = line[67].DoubleValue();
            dto.LowerRegDispatch = line[68].DoubleValue();
            dto.LowerRegLocalDispatch = line[69].DoubleValue();
            dto.LowerRegLocalReq = line[70].DoubleValue();
            dto.LowerRegReq = line[71].DoubleValue();
            dto.RaiseRegImport = line[72].DoubleValue();
            dto.RaiseRegDispatch = line[73].DoubleValue();
            dto.RaiseRegLocalDispatch = line[74].DoubleValue();
            dto.RaiseRegLocalReq = line[75].DoubleValue();
            dto.RaiseRegReq = line[76].DoubleValue();
            dto.Raise5MinLocalViolation = line[77].DoubleValue();
            dto.RaiseRegLocalViolation = line[78].DoubleValue();
            dto.Raise60SecLocalViolation = line[79].DoubleValue();
            dto.Raise6SecLocalViolation = line[80].DoubleValue();
            dto.Lower5MinLocalViolation = line[81].DoubleValue();
            dto.LowerRegLocalViolation = line[82].DoubleValue();
            dto.Lower60SecLocalViolation = line[83].DoubleValue();
            dto.Lower6SecLocalViolation = line[84].DoubleValue();
            dto.Raise5MinViolation = line[85].DoubleValue();
            dto.RaiseRegViolation = line[86].DoubleValue();
            dto.Raise60SecViolation = line[87].DoubleValue();
            dto.Raise6SecViolation = line[88].DoubleValue();
            dto.Lower5MinViolation = line[89].DoubleValue();
            dto.LowerRegViolation = line[90].DoubleValue();
            dto.Lower60SecViolation = line[91].DoubleValue();
            dto.Lower6SecViolation = line[92].DoubleValue();
            dto.LastChanged = GetDateTime(line[93]);
            dto.TotalIntermittentGeneration = line[94].DoubleValue();
            dto.DemandAndNonSchedgen = line[95].DoubleValue();
            dto.Uigf = line[96].DoubleValue();
            dto.SemiScheduleClearedMw = line[97].DoubleValue();
            dto.SemiScheduleComplianceMw = line[98].DoubleValue();
            dto.SsSolarUigf = line[99].DoubleValue();
            dto.SsWindUigf = line[100].DoubleValue();
            dto.SsSolarClearedMw = line[101].DoubleValue();
            dto.SsWindClearedMw = line[102].DoubleValue();
            dto.SsSolarComplianceMw = line[103].DoubleValue();
            dto.SsWindComplianceMw = line[104].DoubleValue();
            dto.WdrInitialMw = line[105].DoubleValue();
            dto.WdrAvailable = line[106].DoubleValue();
            dto.WdrDispatched = line[107].DoubleValue();
            
            return dto;

        }


        /*
         * Helpers for file download and management
         */
        private void ListFiles()
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

            _files = lines.Select(line => line.Split("<A HREF=\"")[1]
                    .Split("\">PUBLIC_P5MIN_")[0])
                .ToList();
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
            public string PeriodStamp;
            public string GenerationStamp;
        }

        private string P5MinPath()
        {
            return _tempStoragePath + "P5_Min/";
        }

        private DateTime GetDateTime(string value)
        {
            return DateTime.ParseExact(value.Replace("\"",""), 
                _dateTimeFormat, new CultureInfo("En-AU"));
        }
        
    }
}
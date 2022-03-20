using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NemTracker.Dtos.P5Minute;
using NemTracker.Dtos.Stations;

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

                Console.WriteLine(lineSplit[0]);
                
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
            dto.Region = GetRegion(line[7]);
            dto.Rrp = CSVDoubleValue(line[8]);
            dto.Rop = CSVDoubleValue(line[9]);
            dto.ExcessGeneration = CSVDoubleValue(line[10]);
            dto.Raise6SecRrp = CSVDoubleValue(line[11]);
            dto.Raise6SecRop = CSVDoubleValue(line[12]);
            dto.Raise60SecRrp = CSVDoubleValue(line[13]);
            dto.Raise60SecRop = CSVDoubleValue(line[14]);
            dto.Raise5MinRrp = CSVDoubleValue(line[15]);
            dto.Raise5MinRop = CSVDoubleValue(line[16]);
            dto.RaiseRegRrp = CSVDoubleValue(line[17]);
            dto.RaiseRegRop = CSVDoubleValue(line[18]);
            dto.Lower6SecRrp = CSVDoubleValue(line[19]);
            dto.Lower6SecRop = CSVDoubleValue(line[20]);
            dto.Lower60SecRrp = CSVDoubleValue(line[21]);
            dto.Lower60SecRop = CSVDoubleValue(line[22]);
            dto.Lower5MinRrp = CSVDoubleValue(line[23]);
            dto.Lower5MinRop = CSVDoubleValue(line[24]);
            dto.LowerRegRrp = CSVDoubleValue(line[25]);
            dto.LowerRegRop = CSVDoubleValue(line[26]);
            dto.TotalDemand = CSVDoubleValue(line[27]);
            dto.AvailableGeneration = CSVDoubleValue(line[28]);
            dto.AvailableLoad = CSVDoubleValue(line[29]);
            dto.DemandForecast = CSVDoubleValue(line[30]);
            dto.DispatchableGeneration = CSVDoubleValue(line[31]);
            dto.DispatchableLoad = CSVDoubleValue(line[32]);
            dto.NetInterchange = CSVDoubleValue(line[33]);
            dto.Lower5MinDispatch = CSVDoubleValue(line[34]);
            dto.Lower5MinImport = CSVDoubleValue(line[35]);
            dto.Lower5MinLocalDispatch = CSVDoubleValue(line[36]);
            dto.Lower5MinLocalReq = CSVDoubleValue(line[37]);
            dto.Lower5MinReq = CSVDoubleValue(line[38]);
            dto.Lower60SecDispatch = CSVDoubleValue(line[39]);
            dto.Lower60SecImport = CSVDoubleValue(line[40]);
            dto.Lower60SecLocalDispatch = CSVDoubleValue(line[41]);
            dto.Lower60SecLocalReq = CSVDoubleValue(line[42]);
            dto.Lower60SecReq = CSVDoubleValue(line[43]);
            dto.Lower6SecDispatch = CSVDoubleValue(line[44]);
            dto.Lower6SecImport = CSVDoubleValue(line[45]);
            dto.Lower6SecLocalDispatch = CSVDoubleValue(line[46]);
            dto.Lower6SecLocalReq = CSVDoubleValue(line[47]);
            dto.Lower6SecReq = CSVDoubleValue(line[48]);
            dto.Raise5MinDispatch = CSVDoubleValue(line[49]);
            dto.Raise5MinImport = CSVDoubleValue(line[50]);
            dto.Raise5MinLocalDispatch = CSVDoubleValue(line[51]);
            dto.Raise5MinLocalReq = CSVDoubleValue(line[52]);
            dto.Raise5MinReq = CSVDoubleValue(line[53]);
            dto.Raise60SecDispatch = CSVDoubleValue(line[54]);
            dto.Raise60SecImport = CSVDoubleValue(line[55]);
            dto.Raise60SecLocalDispatch = CSVDoubleValue(line[56]);
            dto.Raise60SecLocalReq = CSVDoubleValue(line[57]);
            dto.Raise60SecReq = CSVDoubleValue(line[58]);
            dto.Raise6SecDispatch = CSVDoubleValue(line[59]);
            dto.Raise6SecImport = CSVDoubleValue(line[60]);
            dto.Raise6SecLocalDispatch = CSVDoubleValue(line[61]);
            dto.Raise6SecLocalReq = CSVDoubleValue(line[62]);
            dto.Raise6SecReq = CSVDoubleValue(line[63]);
            dto.AggregateDispatchError = CSVDoubleValue(line[64]);
            dto.InitialSupply = CSVDoubleValue(line[65]);
            dto.ClearedSupply = CSVDoubleValue(line[66]);
            dto.LowerRegImport = CSVDoubleValue(line[67]);
            dto.LowerRegDispatch = CSVDoubleValue(line[68]);
            dto.LowerRegLocalDispatch = CSVDoubleValue(line[69]);
            dto.LowerRegLocalReq = CSVDoubleValue(line[70]);
            dto.LowerRegReq = CSVDoubleValue(line[71]);
            dto.RaiseRegImport = CSVDoubleValue(line[72]);
            dto.RaiseRegDispatch = CSVDoubleValue(line[73]);
            dto.RaiseRegLocalDispatch = CSVDoubleValue(line[74]);
            dto.RaiseRegLocalReq = CSVDoubleValue(line[75]);
            dto.RaiseRegReq = CSVDoubleValue(line[76]);
            dto.Raise5MinLocalViolation = CSVDoubleValue(line[77]);
            dto.RaiseRegLocalViolation = CSVDoubleValue(line[78]);
            dto.Raise60SecLocalViolation = CSVDoubleValue(line[79]);
            dto.Raise6SecLocalViolation = CSVDoubleValue(line[80]);
            dto.Lower5MinLocalViolation = CSVDoubleValue(line[81]);
            dto.LowerRegLocalViolation = CSVDoubleValue(line[82]);
            dto.Lower60SecLocalViolation = CSVDoubleValue(line[83]);
            dto.Lower6SecLocalViolation = CSVDoubleValue(line[84]);
            dto.Raise5MinViolation = CSVDoubleValue(line[85]);
            dto.RaiseRegViolation = CSVDoubleValue(line[86]);
            dto.Raise60SecViolation = CSVDoubleValue(line[87]);
            dto.Raise6SecViolation = CSVDoubleValue(line[88]);
            dto.Lower5MinViolation = CSVDoubleValue(line[89]);
            dto.LowerRegViolation = CSVDoubleValue(line[90]);
            dto.Lower60SecViolation = CSVDoubleValue(line[91]);
            dto.Lower6SecViolation = CSVDoubleValue(line[92]);
            dto.LastChanged = GetDateTime(line[93]);
            dto.TotalIntermittentGeneration = CSVDoubleValue(line[94]);
            dto.DemandAndNonSchedgen = CSVDoubleValue(line[95]);
            dto.Uigf = CSVDoubleValue(line[96]);
            dto.SemiScheduleClearedMw = CSVDoubleValue(line[97]);
            dto.SemiScheduleComplianceMw = CSVDoubleValue(line[98]);
            dto.SsSolarUigf = CSVDoubleValue(line[99]);
            dto.SsWindUigf = CSVDoubleValue(line[100]);
            dto.SsSolarClearedMw = CSVDoubleValue(line[101]);
            dto.SsWindClearedMw = CSVDoubleValue(line[102]);
            dto.SsSolarComplianceMw = CSVDoubleValue(line[103]);
            dto.SsWindComplianceMw = CSVDoubleValue(line[104]);
            dto.WdrInitialMw = CSVDoubleValue(line[105]);
            dto.WdrAvailable = CSVDoubleValue(line[106]);
            dto.WdrDispatched = CSVDoubleValue(line[107]);
            
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

        private double CSVDoubleValue(string value)
        {
            if (value.Length == 0)
            {
                return 0.0;
            }
            
            return Double.Parse(value);
        }

        private RegionEnum GetRegion(string value)
        {
            if (value.Contains(GetEnumDescription(RegionEnum.NSW1)))
            {
                return RegionEnum.NSW1;
            }
            
            if (value.Contains(GetEnumDescription(RegionEnum.VIC1)))
            {
                return RegionEnum.VIC1;
            }
            
            if (value.Contains(GetEnumDescription(RegionEnum.QLD1)))
            {
                return RegionEnum.QLD1;
            }
            
            if (value.Contains(GetEnumDescription(RegionEnum.SA1)))
            {
                return RegionEnum.SA1;
            }
            
            if (value.Contains(GetEnumDescription(RegionEnum.TAS1)))
            {
                return RegionEnum.TAS1;
            }

            return RegionEnum.UNDF;
        }
        
        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }

        private DateTime GetDateTime(string value)
        {
            return DateTime.ParseExact(value.Replace("\"",""), 
                _dateTimeFormat, new CultureInfo("En-AU"));
        }
        
    }
}
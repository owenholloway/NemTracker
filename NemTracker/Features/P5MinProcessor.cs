using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using NemTracker.Exceptions;

namespace NemTracker.Features
{
    public class P5MinProcessor
    {
        private readonly string _tempStoragePath;
        private readonly string _nemwebHost = "http://nemweb.com.au/";
        private readonly string _reportPath = "Reports/Current/P5_Reports/";
        private string _nextInstructionNumber = null;
        private List<string> _files = new List<string>();
        
        public P5MinProcessor()
        {
            _tempStoragePath = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/nemTracker/";

            if (!Directory.Exists(_tempStoragePath))
            {
                Directory.CreateDirectory(_tempStoragePath);
            }
        }

        public void ProcessInstructions()
        {
            CheckNewInstructions();
        }

        private void CheckNewInstructions()
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

            if (_nextInstructionNumber is null)
            {
                //TODO report persistence
                var now = DateTime.Now;
                var nowAEMO 
                    = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.FindSystemTimeZoneById("Australia/Brisbane"));
                var instructionStamp = nowAEMO.NextNemInterval();

                foreach (var instructionFile in instructions)
                {
                    var fileStamp = Int64.Parse(instructionFile.PeriodStamp);
                    
                    if (instructionStamp == fileStamp)
                    {
                        DownloadExtractInstruction(instructionFile.Path);
                    }
                }

            }
            else
            {
                
            }
        }

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

        private void DownloadExtractInstruction(string path)
        {

        }
        
        private class InstructionFile
        {
            public string Path;
            public string PeriodStamp;
            public string GenerationStamp;
        }
        
    }
}
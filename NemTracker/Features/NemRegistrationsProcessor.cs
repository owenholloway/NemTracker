using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using ExcelDataReader;
using NemTracker.Dtos;

namespace NemTracker.Features
{
    public class NemRegistrationsProcessor
    {
        private List<StationDto> _stations = new List<StationDto>();
        private string _tempStoragePath;

        public NemRegistrationsProcessor()
        {
            _tempStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/nemTracker/";

            if (!System.IO.Directory.Exists(_tempStoragePath))
            {
                System.IO.Directory.CreateDirectory(_tempStoragePath);
            }
            
        }
        
        
        public List<StationDto> GetStations()
        {
            //DownloadNewXls();

            var fileStream = File.Open(NemDataDocument(), FileMode.Open, FileAccess.Read);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xlsReader = ExcelReaderFactory.CreateBinaryReader(fileStream);

            var data = xlsReader.AsDataSet();
            
            foreach (DataTable dataTable in data.Tables)
            {
                Console.WriteLine(dataTable.TableName);
            }
            
            return new List<StationDto>();
        }

        
        private void DownloadNewXls()
        {
            using var httpClient = new WebClient();
            httpClient.DownloadFile("https://www.aemo.com.au/-/media/Files/Electricity/NEM/" +
                                    "Participant_Information/NEM-Registration-and-Exemption-List.xls", 
                NemDataDocument());
        }

        
        private string NemDataDocument()
        {
            return _tempStoragePath + "NEM-Registration-and-Exemption-List.xls";
        }
        
    }
}
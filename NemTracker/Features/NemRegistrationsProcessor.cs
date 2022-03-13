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
            
            var stations = new List<StationDto>();

            var fileStream = File.Open(NemDataDocument(), FileMode.Open, FileAccess.Read);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xlsReader = ExcelReaderFactory.CreateBinaryReader(fileStream);

            var data = xlsReader.AsDataSet();
            
            foreach (DataTable dataTable in data.Tables)
            {
                //Console.WriteLine(dataTable.TableName);
                if (!dataTable.TableName.Contains("Generators and Scheduled Loads")) continue;
                
                foreach (DataRow dataTableRow in dataTable.Rows)
                {
                    if (dataTableRow.ItemArray[0].ToString().Contains("Participant") &&
                        dataTableRow.ItemArray[8].ToString().Contains("Technology Type - Primary"))
                    {
                        continue;
                    }
                    
                    var stationDto = new StationDto();
                    
                    stationDto.StationName = dataTableRow.ItemArray[1].ToString().Trim();
                    
                    var physicalUnitNo = dataTableRow.ItemArray[10].ToString().Trim().Split("-");
                    
                    switch (physicalUnitNo.Length)
                    {
                        case 1:
                        {
                            stationDto.PhysicalUnitMin = int.Parse(physicalUnitNo[0]);
                            stationDto.PhysicalUnitMax = int.Parse(physicalUnitNo[0]);
                            break;
                        }
                        
                        case 2:
                        {
                            stationDto.PhysicalUnitMin = Int32.Parse(physicalUnitNo[0]);
                            stationDto.PhysicalUnitMax = Int32.Parse(physicalUnitNo[1]);
                            break;
                        }
                        
                    }
                    
                    stations.Add(stationDto);
                    
                    Console.WriteLine();
                }
                
            }
            
            return stations;
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
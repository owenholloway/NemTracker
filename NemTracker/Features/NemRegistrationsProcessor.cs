using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using NemTracker.Dtos;
using NemTracker.Dtos.Stations;
using static System.Int32;

// ReSharper disable PossibleNullReferenceException
// We have to work with memory is a generally unsafe way to process the file 
// this will be caught be error catching instead of checking value safely.
// Bad practice in general but I will make an exception for this.

namespace NemTracker.Features
{
    public class NemRegistrationsProcessor
    {
        private List<StationDto> _stations = new List<StationDto>();
        private readonly string _tempStoragePath;

        public NemRegistrationsProcessor()
        {
            _tempStoragePath = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/nemTracker/";

            if (!Directory.Exists(_tempStoragePath))
            {
                Directory.CreateDirectory(_tempStoragePath);
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
                    var dataTableItems = dataTableRow.ItemArray;
                    
                    if (dataTableItems[0].ToString().Contains("Participant") &&
                        dataTableItems[8].ToString().Contains("Technology Type - Primary"))
                    {
                        continue;
                    }
                    
                    var stationDto = new StationDto();
                  
                    stationDto.StationName = dataTableItems[1].ToString().Trim();

                    stationDto.DispatchTypeEnum 
                        = GetDispatchType(dataTableItems[3].ToString());

                    stationDto.TechnologyType 
                        = GetTechnologyType(dataTableItems[8].ToString());

                    stationDto.TechnologyTypeDescriptor 
                        = GetTechnologyTypeDescriptor(dataTableItems[9].ToString());

                    var units = dataTableItems[10].ToString();
                    var physicalUnitNo = units.Trim().Split("-");
                    
                    switch (physicalUnitNo.Length)
                    {
                        case 1:
                        {
                            var unitMin = -1;
                            TryParse(physicalUnitNo[0], out unitMin);
                            stationDto.PhysicalUnitMin = unitMin;
                            stationDto.PhysicalUnitMin = unitMin;
                            stationDto.PhysicalUnitMax = unitMin;
                            break;
                        }
                        
                        case 2:
                        {
                            var unitMin = -1;
                            TryParse(physicalUnitNo[0], out unitMin);
                            stationDto.PhysicalUnitMin = unitMin;
                            var unitMax = -1;
                            TryParse(physicalUnitNo[0], out unitMax);
                            stationDto.PhysicalUnitMax = unitMax;
                            break;
                        }
                        
                    }

                    stationDto.DUID = dataTableItems[13].ToString().Trim();
                    
                    stations.Add(stationDto);
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

        /*
         * Data Processing Helper Functions
         */

        private DispatchTypeEnum GetDispatchType(string rawValue)
        {
            foreach (var dispatchType in (DispatchTypeEnum[]) Enum.GetValues(typeof(DispatchTypeEnum)))
            {
                if (rawValue.Contains(GetEnumDescription(dispatchType)))
                {
                    return dispatchType;
                }
            }

            return DispatchTypeEnum.Undefined;
        }
        
        private TechnologyTypeEnum GetTechnologyType(string rawValue)
        {
            foreach (var technologyType in (TechnologyTypeEnum[]) Enum.GetValues(typeof(TechnologyTypeEnum)))
            {
                if (rawValue.Contains(GetEnumDescription(technologyType)))
                {
                    return technologyType;
                }
            }

            return TechnologyTypeEnum.Undefined;
        }
        
        private TechnologyTypeDescriptorEnum GetTechnologyTypeDescriptor(string rawValue)
        {
            foreach (var technologyType in (TechnologyTypeDescriptorEnum[]) Enum.GetValues(typeof(TechnologyTypeDescriptorEnum)))
            {
                if (rawValue.Contains(GetEnumDescription(technologyType)))
                {
                    return technologyType;
                }
            }

            return TechnologyTypeDescriptorEnum.Undefined;
        }
        
        /*
         * General Helper Functions
         */
        private string NemDataDocument()
        {
            return _tempStoragePath + "NEM-Registration-and-Exemption-List.xls";
        }
        
        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
        
    }
}
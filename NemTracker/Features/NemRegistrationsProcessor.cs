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
using NemTracker.Dtos.Aemo;
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


        public List<ParticipantDto> GetParticipants()
        {
            var participants = new List<ParticipantDto>();
            
                        var fileStream = File.Open(NemDataDocument(), FileMode.Open, FileAccess.Read);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var xlsReader = ExcelReaderFactory.CreateBinaryReader(fileStream);

            var data = xlsReader.AsDataSet();
            
            foreach (DataTable dataTable in data.Tables)
            {
                //Console.WriteLine(dataTable.TableName);
                if (!dataTable.TableName.Contains("Registered Participants")) continue;

                foreach (DataRow dataTableRow in dataTable.Rows)
                {
                    if (dataTableRow[1].ToString().Contains("ABN / ACN") 
                        || dataTableRow[1].ToString().Contains("Totals"))
                    {
                        continue;
                    }

                    var dto = new ParticipantDto();

                    dto.Name = dataTableRow[0].ToString().Trim();
                    dto.ABN = dataTableRow[1].ToString().Trim();

                    dto.DemandResponseServiceProviderAncillaryServiceLoad 
                        = dataTableRow[2].ToString().Contains('1');
                    dto.DemandResponseServiceProviderWholesaleDemandResponseUnit 
                        = dataTableRow[3].ToString().Contains('1');
                    dto.GeneratorMarketScheduled
                        = dataTableRow[4].ToString().Contains('1');
                    dto.GeneratorMarketNonScheduled
                        = dataTableRow[5].ToString().Contains('1');
                    dto.GeneratorMarketSemiScheduled 
                        = dataTableRow[6].ToString().Contains('1');
                    dto.GeneratorNonMarketScheduled 
                        = dataTableRow[7].ToString().Contains('1');
                    dto.GeneratorNonMarketNonScheduled 
                        = dataTableRow[8].ToString().Contains('1');
                    dto.GeneratorNonMarketSemiScheduled 
                        = dataTableRow[9].ToString().Contains('1');
                    dto.MarketSmallGenerationAggregator 
                        = dataTableRow[10].ToString().Contains('1');
                    dto.MarketCustomer 
                        = dataTableRow[11].ToString().Contains('1');
                    dto.MeteringCoordinator 
                        = dataTableRow[12].ToString().Contains('1');
                    dto.MarketNSP 
                        = dataTableRow[13].ToString().Contains('1');
                    dto.NSPTransmission 
                        = dataTableRow[14].ToString().Contains('1');
                    dto.NSPDistribution 
                        = dataTableRow[15].ToString().Contains('1');
                    dto.NSPOther 
                        = dataTableRow[16].ToString().Contains('1');
                    dto.SpecialParticipantSystemOperator 
                        = dataTableRow[17].ToString().Contains('1');
                    dto.SpecialParticipantDistributionOperator 
                        = dataTableRow[18].ToString().Contains('1');
                    dto.Trader 
                        = dataTableRow[19].ToString().Contains('1');
                    dto.Intending 
                        = dataTableRow[20].ToString().Contains('1');
                    dto.Reallocator 
                        = dataTableRow[21].ToString().Contains('1');
                    
                    participants.Add(dto);
                    
                }

            }
            
            fileStream.Close();

            return participants;
        }

        public List<StationDto> GetStations()
        {

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

                    stationDto.ParticipantName = dataTableItems[0].ToString().Trim();
                    
                    stationDto.StationName = dataTableItems[1].ToString().Trim();

                    stationDto.Region = GetRegion(dataTableItems[2].ToString().Trim());
                    
                    stationDto.DispatchType 
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
            
            fileStream.Close();
            
            return stations;
        }

        
        public void DownloadNewXls()
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

        private static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
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
        
    }
}
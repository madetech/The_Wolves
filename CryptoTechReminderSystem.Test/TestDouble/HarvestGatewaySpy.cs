using System;
using System.Collections.Generic;
using CryptoTechReminderSystem.DomainObject;
using CryptoTechReminderSystem.Gateway;

namespace CryptoTechReminderSystem.Test.TestDouble
{
    public class HarvestGatewaySpy : IHarvestDeveloperRetriever, ITimeSheetRetriever
    {
        public bool IsRetrieveDevelopersCalled;
        public bool IsRetrieveTimeSheetsCalled;
        public DateTimeOffset[] RetrieveTimeSheetsArguments;
            
        public IList<HarvestDeveloper> RetrieveDevelopers()
        {
            IsRetrieveDevelopersCalled = true;
              
            return new List<HarvestDeveloper>();
        }

        public IList<TimeSheet> RetrieveTimeSheets(DateTimeOffset dateFrom, DateTimeOffset dateTo)
        {
            IsRetrieveTimeSheetsCalled = true;
            
            RetrieveTimeSheetsArguments = new[]
            {
                dateFrom,
                dateTo
            };

            return new List<TimeSheet>();
        }
    }
}
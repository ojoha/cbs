using Dolittle.ReadModels;
using System;
using Concepts;

namespace Read.CaseReports
{
    public class CaseReport : IReadModel
    {
        public CaseReport(Guid dataCollectorId, Guid healthRiskId,
            string origin, string message, int numberOfMalesUnder5, int numberOfMalesAged5AndOlder,
            int numberOfFemalesUnder5, int numberOfFemalesAged5AndOlder, double longitude,
            double latitude, DateTimeOffset timestamp, string region)
        {
            this.DataCollectorId = dataCollectorId;
            this.HealthRiskId = healthRiskId;
            this.Origin = origin;
            this.Message = message;
            this.NumberOfMalesUnder5 = numberOfMalesUnder5;
            this.NumberOfMalesAged5AndOlder = numberOfMalesAged5AndOlder;
            this.NumberOfFemalesUnder5 = numberOfFemalesUnder5;
            this.NumberOfFemalesAged5AndOlder = numberOfFemalesAged5AndOlder;
            this.Longitude = longitude;
            this.Latitude = latitude;
            this.Timestamp = timestamp;
            this.Region = region;
        }
        public Guid DataCollectorId { get; }
        public Guid HealthRiskId { get; }
        public string Origin { get; }
        public string Message { get; }
        public int NumberOfMalesUnder5 { get; }
        public int NumberOfMalesAged5AndOlder { get; }
        public int NumberOfFemalesUnder5 { get; }
        public int NumberOfFemalesAged5AndOlder { get; }
        public double Longitude { get; }
        public double Latitude { get; }
        public DateTimeOffset Timestamp { get; }
        public string Region { get; }
    }
}

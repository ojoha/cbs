/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2017 International Federation of Red Cross. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Events;
using Events.External;
using Infrastructure.Application;
using Infrastructure.Events;
using Read;
using Read.HealthRiskFeatures;
using System;

namespace Web.Features.CaseReportReceptionFeatures
{
    //TODO: Is the web project the right places for this? Processing the TextMessageReceived event is kinda like a command, and an event is only emited if its a valid message
    public class TextMessageRecievedEventProcessor : Infrastructure.Events.IEventProcessor
    {
        public static readonly Feature Feature = "CaseReportReception";
        private readonly IEventEmitter _eventEmitter;
        private readonly IDataCollectors _dataCollectors;
        private readonly IHealthRisks _healthRisks;

        public TextMessageRecievedEventProcessor(
            IEventEmitter eventEmitter,
            IDataCollectors dataCollectors,
            IHealthRisks healthRisks)
        {
            _eventEmitter = eventEmitter;
            _dataCollectors = dataCollectors;
            _healthRisks = healthRisks;
        }

        //TODO: Add a test that ensure that the right count is put in the right property
        public void Process(TextMessageReceived @event)
        {
            //TODO: Handle if parsing fails and send TextMessageParseFailed event  
            var caseReportContent = TextMessageContentParser.Parse(@event.Message);
            
            if(caseReportContent.GetType() == typeof(SingleCaseReportContent))
            {
                var singlecaseReport = caseReportContent as SingleCaseReportContent;
                _eventEmitter.Emit(Feature, new CaseReportReceived
                {
                    Id = Guid.NewGuid(),
                    DataCollectorId = _dataCollectors.GetByMobilePhoneNumber(@event.OriginNumber)?.Id,
                    HealthRiskId = _healthRisks.GetByReadableId(caseReportContent.HealthRiskId).Id,
                    NumberOfFemalesUnder5 = 
                    singlecaseReport.Age <= 5 && singlecaseReport.Sex == Sex.Female ? 1 : 0,
                    NumberOfFemalesOver5 =
                    singlecaseReport.Age > 5 && singlecaseReport.Sex == Sex.Female ? 1 : 0,
                    NumberOfMalesUnder5 =
                    singlecaseReport.Age <= 5 && singlecaseReport.Sex == Sex.Male ? 1 : 0,
                    NumberOfMalesOver5 =
                    singlecaseReport.Age > 5 && singlecaseReport.Sex == Sex.Male ? 1 : 0,
                    Latitude = @event.Latitude,
                    Longitude = @event.Longitude,
                    Timestamp = @event.Sent
                });
            }
            else
            {
                var report = caseReportContent as MultipleCaseReportContent;
                _eventEmitter.Emit(Feature, new CaseReportReceived
                {
                    Id = Guid.NewGuid(),
                    DataCollectorId = _dataCollectors.GetByMobilePhoneNumber(@event.OriginNumber)?.Id,
                    HealthRiskId = _healthRisks.GetByReadableId(caseReportContent.HealthRiskId).Id,
                    NumberOfFemalesUnder5 = report.FemalesUnder5,                    
                    NumberOfFemalesOver5 = report.FemalesOver5,
                    NumberOfMalesUnder5 = report.MalesUnder5,
                    NumberOfMalesOver5 = report.MalesOver5,
                    Latitude = @event.Latitude,
                    Longitude = @event.Longitude,
                    Timestamp = @event.Sent
                });
            }
           
        }
    }    
}

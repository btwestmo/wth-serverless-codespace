const { app, output } = require('@azure/functions'); 

const cosmosOutput = output.cosmosDB({ 
    databaseName: 'LicensePlates', 
    containerName: 'NeedsManualReview', 
    createIfNotExists: true, 
    connection: 'cosmosDBConnectionString', 
}); 

app.eventGrid('queuePlateForManualCheckup', { 
    return: cosmosOutput, 
    handler: (event, context) => { 
        context.log('Event grid function processed event:', event); 
        return { 
            fileName: event.data['fileName'], 
            licensePlateText: '', 
            timeStamp: event.data['timeStamp'], 
            resolved: false,
            id: event.id
        };
    },
});
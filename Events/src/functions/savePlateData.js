const { app, output } = require('@azure/functions'); 

const cosmosOutput = output.cosmosDB({ 
    databaseName: 'LicensePlates', 
    containerName: 'Processed', 
    createIfNotExists: true, 
    connection: 'cosmosDBConnectionString', 
}); 

app.eventGrid('savePlateData', { 
    return: cosmosOutput, 
    handler: (event, context) => { 
        context.log('Event grid function processed event:', event); 
        return { 
            fileName: event.data['fileName'], 
            licensePlateText: event.data['licensePlateText'], 
            timeStamp: event.data['timeStamp'], 
            exported: false,
            id: event.id 
        }; 
    }, 
}); 
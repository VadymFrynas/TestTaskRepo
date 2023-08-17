# Order Management API

This repository contains a simple RESTful Web API built using ASP.NET Core 6.0 for managing customer orders. The API utilizes Azure Service Bus to notify when a new order is created.

## Setup and Run
1. Clone the repository
Navigate to the project directory:
Configure the appsettings.json:
Put your Azure service bus connection string as value for "AzureServiceBusConnectionString", and you also need to create  a queue for message receive. After creation put name of the queue as value for "QueueName".
In the end your json will be looks like this 
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "AzureServiceBusConnectionString": "your service bus connection string",
  "QueueName": "created queue name"
}
```
## API Endpoints
+ POST /api/customers: Create a new customer.
+ GET /api/customers: Retrieve a list of all customers.
+ GET /api/customers/{customerId}: Retrieve details of a specific customer.
+ POST /api/orders: Create a new order for a customer.
+ GET /api/orders: Retrieve a list of all orders.
+ GET /api/orders/{orderId}: Retrieve details of a specific order.
+ PATCH /api/orders/{orderId}: Give an opportunity to update order.

## Architectural Decisions
+ Database: InMemoryDatabase is used as the database for storing customer and order data.
+ Entity Framework Code-First: Entity Framework is used with a code-first approach to define the database schema using C# classes.
+ Service Bus Integration: Azure Service Bus is used to send messages when new orders are created, and a consumer service updates the customer orders count accordingly.
+ DTOs: Data Transfer Objects (DTOs) are used to define the shape of data exchanged between the API and clients.
+ Dependency Injection: The application uses dependency injection for managing services and database context.

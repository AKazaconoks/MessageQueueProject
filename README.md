# Message Queue Project

This is an ASP.NET Core Web API application that provides a simple client and notification management system with a message queue. The application allows the user to manage clients, notifications, and integrates with MassTransit for messaging capabilities.

## Features

- Client management: create, read, update, and delete clients
- Notification management: create, read, and retrieve notifications with filtering options
- Message Queue integration: using MassTransit and RabbitMQ for handling notification messages
- User authentication using JWT (JSON Web Tokens)

## Technologies Used

- ASP.NET Core: for building the Web API application
- Entity Framework Core: for database operations
- SQL Server: as the database management system
- MassTransit: as the message queue library
- RabbitMQ: as the message broker
- JWT (JSON Web Tokens): for user authentication

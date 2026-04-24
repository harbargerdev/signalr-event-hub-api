# SignalR Event Hub API

## Overview
This repository contains a prototype .NET 10 Web API that integrates a SignalR Hub. The purpose of this project is to continue learning and validate the viability of using SignalR with a desktop application. The API is designed to create child tasks that produce random events and generate an event for the executing user's name.

## Features
- **SignalR Hub Integration**: Real-time communication using SignalR.
- **Random Event Generation**: Child tasks produce random events.
- **User-Specific Events**: Generates an event for the executing user's name.
- **Lightweight RESTful API**: Provides endpoints for interacting with the SignalR Hub.

## Prerequisites
To run this project, ensure you have the following installed:
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

## Setup Instructions

### Clone the Repository
```bash
git clone https://github.com/your-username/signalr-event-hub-api.git
cd signalr-event-hub-api
```

### Build and Run the API
1. Restore dependencies:
   ```bash
   dotnet restore
   ```
2. Build the project:
   ```bash
   dotnet build
   ```
3. Run the API:
   ```bash
   dotnet run --project SignalR.Events.Api
   ```

### Run with Docker
1. Build the Docker image:
   ```bash
   docker build -t signalr-event-hub-api .
   ```
2. Run the Docker container:
   ```bash
   docker run -p 5000:5000 signalr-event-hub-api
   ```

## Usage

### API Endpoints
- **GET** `/events`: Retrieve a list of events.
- **POST** `/events`: Trigger a new random event.
- **GET** `/events/user`: Retrieve the executing user's event.

### SignalR Hub
The SignalR Hub is available at `/eventhub`. Clients can connect to this hub to receive real-time updates about events.

### Example HTTP Request
Use the provided `SignalR.Events.Api.http` file to test the API endpoints with tools like [HTTPie](https://httpie.io/) or [Postman](https://www.postman.com/).

## Project Structure
- **Controllers**: Contains the `EventsController` for handling API requests.
- **Hubs**: Contains the `EventHub` for SignalR communication.
- **Models**: Defines the `EventDetails` and `EventNotification` models.
- **Services**: Includes workers like `CurrentUserEventWorker` and `RandomUserEventWorker` for event generation.

## Contributing
Contributions are welcome! To contribute:
1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Submit a pull request with a detailed description of your changes.

## License
This project is licensed under the [MIT License](LICENSE).

## Acknowledgments
- [Microsoft SignalR](https://learn.microsoft.com/en-us/aspnet/core/signalr/) for real-time communication.
- The .NET community for providing excellent resources and support.

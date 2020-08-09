# Chatroom
Chatroom Application

## Getting Started
Here you will find some instructions to get you a copy of the project up and running on your local machine for development and testing purposes, as well as a general overview of the project and its functionality.

## Features
* Allows registered users to log in and talk with other users in a chatroom.
* Allow users to post messages as commands into the chatroom with the following format /stock=stock_code. When a user types a command, internally a call to a separate API to retrieve the qoutes based on the Stock Symbol. The API internally makes use of Stooq API to get quotes. 
* Chat messages are ordered by their timestamps and keeps a cache of the last 50 messages.
* The project includes some unit tests of the code.
* Bonus:
>* Invalid commands sent to the bot or exceptions raised are handled.
>* .NET identity is used for users authentication
>* Scripts are provided to start the Hosts

### Project Structure

These projects are included in the solution:

* **Chatroom.App** (web): The web application for chatroom. It is configured to authenticate through the Identityserver. (Will be deployed at http://localhost:5002)
* **Chatroom.Bot** (web api): Separate API that exposes a method that will internally get the Stock data by internally calling Stooq API. This API also authenticates through IdentityServer. (Will be deployed at http://localhost:5000)
* **Chatroom.Core** (library): Core classes.
* **Chatroom.Tests** (test): Unit tests of the classes.
* **IdentityServerAspNetIdentity** (web): External authentication server. Users are created on a Internal SQLLite database (AspIdUsers.db). You can download https://sqlitestudio.pl/ to check data inside the database. (Will be deployed at http://localhost:5001)

## Installation, Configuration and Deployment

### Prerequisites

In order to get the application running, you need to have installed dot net core version 3.0. Once installed you can go to the folder and execute the StartAll.bat

### Configuration

No special requirements for configuration. Deploy port values can be updated in configuration files if required.

## Deployment

To deploy the application you can do one of the following:
1. Start command line and  execute StartAll.bat. This script will start the 3 Hosts (IdentityServer, MVC app and Stock API)
2. In Visual studio, set the solution to start multiple project and select: IdentityServerAspNetIdentity, Chatroom.App and Chatroom.Bot

## Technologies and frameworks

* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-3.1identity ) - Used for the MVC application (adding MVC middleware) and for the Web API for Stock quotes
* [Identity Server](https://identityserver.io/) - Identity management, authorization, and API security. 
* [CsvHelper](https://joshclose.github.io/CsvHelper/) - CVS processing library
* [SignalR](https://docs.microsoft.com/en-us/aspnet/core/signalr/introduction?view=aspnetcore-3.1) - Added to MVC application as a middleware to handle asynchrounous notifications

## Known limitations and improvement options

* Improve UI. Maybe adding some features like show Connected users.
* Add unit tests to improve coverage closer to 100%
* SignalR framework is based on WebSockets which allows a limited number of concurrent connections.  For escalability, Azure SignalR service can be used.
* Automate UI tests using selenium framework
* Include Unit Tests for Javascript files

## External resources

* [Stooq.com](https://stooq.com) - Stock quotes service

## Usage - Flow
* Once the 3 hosts are running you can go to the Chat app (http://localhost:5002). 
* The home page does not require authentication so you will be prompt with the Chatroom app home page.
* If you try to access to Chat section, you will be redirected to Identity Server for proper user authentication. You can use one of the following default users created:
>* bob - Pass123$
>* alice - Pass123$
>* patrick - Pass123$
>* john - Pass123$
* Once succesfully authenticated, you will be redirected to the application and will be able to access the Chat section.
* Enter the chat section and start sharing messages

## Author

* **Álvaro Quintana** - *Developer* - [Álvaro Quintana](https://github.com/aquintan)

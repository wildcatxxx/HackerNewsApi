# Hacker News Best Stories API
## Overview
This ASP.NET Core Web API retrieves the best `n` stories from the Hacker News API, sorted by score in descending order.

## Features
- Concurrent story fetching for optimal performance
- In-memory caching to reduce load on Hacker News API
- Rate-safe design using HttpClientFactory
- Swagger UI for easy API testing and exploration
- RESTful endpoint design

## Requirements
- .NET 10 SDK or later
- Internet connection (to access the Hacker News API)

## Installation & Setup

1. **Navigate to the project directory:**
   ```bash
   cd HackerNewsApi
   ```

2. **Restore dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the application:**
   ```bash
   dotnet run
   ```

4. **Access the API:**
   - API Base URL: `https://localhost:7XXX` or `http://localhost:5XXX` (ports may vary)
   - Swagger UI: Navigate to `https://localhost:7XXX/swagger` in your browser
   - API Endpoint: `GET /beststories?n={number}`

## Example Response

```json
[
  {
    "title": "Apple has locked my Apple ID, and I have no recourse. A plea for help",
    "uri": "https://hey.paris/posts/appleid/",
    "postedBy": "parisidau",
    "time": "2025-12-13T04:55:59+00:00",
    "score": 1621,
    "commentCount": 1000
  },
  {
    "title": "Europeans' health data sold to US firm run by ex-Israeli spies",
    "uri": "https://www.ftm.eu/articles/europe-health-data-us-firm-israel-spies",
    "postedBy": "Fnoord",
    "time": "2025-12-14T12:15:09+00:00",
    "score": 586,
    "commentCount": 363
  }
]
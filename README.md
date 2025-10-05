# 🌳 TbSense Backend

**TbSense Backend** is a comprehensive IoT-enabled plantation management system designed to monitor, analyze, and optimize tree plantation operations. The system provides real-time environmental monitoring, harvest tracking, yield prediction, and advanced analytics through a RESTful API built with .NET 9.0.

[![.NET Version](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![EF Core](https://img.shields.io/badge/EF%20Core-9.0.9-512BD4)](https://docs.microsoft.com/ef/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16+-336791?logo=postgresql)](https://www.postgresql.org/)
[![FastEndpoints](https://img.shields.io/badge/FastEndpoints-7.0.1-00D1B2)](https://fast-endpoints.com/)

---

## 📋 Table of Contents

- [Features](#-features)
- [Architecture](#-architecture)
- [Technology Stack](#-technology-stack)
- [Prerequisites](#-prerequisites)
- [Getting Started](#-getting-started)
- [Project Structure](#-project-structure)
- [API Documentation](#-api-documentation)
- [Database Schema](#-database-schema)
- [Configuration](#-configuration)
- [Development](#-development)
- [Testing](#-testing)
- [Deployment](#-deployment)
- [Contributing](#-contributing)
- [License](#-license)

---

## ✨ Features

### 🌱 Plantation Management

- **Multi-Plantation Support**: Manage multiple plantations with detailed profiles
- **Geospatial Tracking**: GPS coordinates for plantations and individual trees
- **Land Area Management**: Track land usage and density metrics
- **Growth Monitoring**: Historical tracking of plantation expansion

### 🌳 Tree Monitoring

- **IoT Sensor Integration**: Real-time environmental data collection
- **Individual Tree Tracking**: Monitor 5,387+ trees with unique identifiers
- **Health Status**: Active/inactive status tracking
- **Environmental Metrics**:
  - Air Temperature
  - Soil Temperature
  - Soil Moisture

### 📊 Advanced Analytics (51 Chart Endpoints)

- **9 Pie Charts**: Distribution analysis (tree activity, harvest distribution, environmental zones)
- **14 Bar Charts**: Comparative analysis (plantation ranking, top performers, weekly trends)
- **13 Histograms**: Statistical distributions (yield, temperature, moisture, age)
- **15 Area Charts**: Cumulative trends (growth tracking, stress monitoring, adoption rates)

### 🌾 Harvest Management

- **Harvest Recording**: Track 470+ harvest events with detailed metrics
- **Quality Grading**: Grade A/B/C classification
- **Yield Analysis**: Total and average yield calculations
- **Temporal Analysis**: Monthly, weekly, and daily harvest patterns

### 🤖 Machine Learning Integration

- **Yield Prediction**: ML models for harvest forecasting
- **Model Training**: Integrated training service with external Python trainer
- **Model Versioning**: Track and manage multiple prediction models
- **Performance Metrics**: Model accuracy and evaluation tracking

### 📈 Dashboard System

- **Global Dashboard**: Portfolio-wide analytics and KPIs
- **Plantation Dashboard**: Individual plantation deep-dive analysis
- **Tree Dashboard**: Single tree health and history monitoring
- **Data Explorer**: Advanced filtering and data export capabilities

### 🔐 Data Management

- **Pagination Support**: Efficient data retrieval with filtering and sorting
- **Advanced Filtering**: Multi-field filtering with operators
- **CSV/Excel Export**: Data export for external analysis
- **Audit Trail**: Complete history of all operations

---

## 🏗️ Architecture

TbSense Backend follows **Clean Architecture** principles with clear separation of concerns:

### System Architecture Overview

```mermaid
graph TB
    subgraph "Client Layer"
        WEB[Web Frontend]
        MOBILE[Mobile App]
        API_CLIENT[External API Clients]
    end

    subgraph "Presentation Layer"
        FASTENDPOINTS[FastEndpoints API]
        SWAGGER[Swagger/OpenAPI]
        MIDDLEWARE[Exception Middleware]
    end

    subgraph "Application Layer"
        SERVICES[Business Services]
        VALIDATORS[FluentValidation]
        HELPERS[Chart Helpers]
        DASHBOARD_SVC[Dashboard Service]
        PLANTATION_SVC[Plantation Service]
        TREE_SVC[Tree Service]
        ML_SVC[ML Service]
    end

    subgraph "Infrastructure Layer"
        REPOSITORIES[EF Core Repositories]
        DBCONTEXT[TbSenseBackendDbContext]
        TRAINER_CLIENT[Trainer Service Client]
        STORAGE_CLIENT[Storage Service Client]
    end

    subgraph "Domain Layer"
        ENTITIES[Domain Entities]
        INTERFACES[Service Interfaces]
        MODELS[Domain Models]
    end

    subgraph "External Services"
        POSTGRES[(PostgreSQL Database)]
        MINIO[MinIO/S3 Storage]
        TRAINER[Python ML Trainer]
    end

    WEB --> FASTENDPOINTS
    MOBILE --> FASTENDPOINTS
    API_CLIENT --> FASTENDPOINTS

    FASTENDPOINTS --> MIDDLEWARE
    FASTENDPOINTS --> SWAGGER
    MIDDLEWARE --> SERVICES

    SERVICES --> VALIDATORS
    SERVICES --> HELPERS
    DASHBOARD_SVC --> REPOSITORIES
    PLANTATION_SVC --> REPOSITORIES
    TREE_SVC --> REPOSITORIES
    ML_SVC --> TRAINER_CLIENT
    ML_SVC --> STORAGE_CLIENT

    REPOSITORIES --> DBCONTEXT
    REPOSITORIES --> INTERFACES
    SERVICES --> INTERFACES

    DBCONTEXT --> ENTITIES
    INTERFACES --> MODELS

    DBCONTEXT --> POSTGRES
    STORAGE_CLIENT --> MINIO
    TRAINER_CLIENT --> TRAINER

    style FASTENDPOINTS fill:#4CAF50
    style SERVICES fill:#2196F3
    style REPOSITORIES fill:#FF9800
    style ENTITIES fill:#9C27B0
    style POSTGRES fill:#336791
    style MINIO fill:#C72E49
```

### Clean Architecture Layers

```mermaid
graph LR
    subgraph "Clean Architecture"
        direction TB

        subgraph "Core"
            DOMAIN[Domain Layer<br/>Entities, Interfaces]
        end

        subgraph "Application"
            APP[Application Layer<br/>Services, Validators]
        end

        subgraph "Infrastructure"
            INFRA[Infrastructure Layer<br/>Repositories, External Services]
        end

        subgraph "Presentation"
            PRES[Presentation Layer<br/>API Endpoints, Controllers]
        end

        PRES --> APP
        APP --> DOMAIN
        INFRA --> DOMAIN
        PRES --> INFRA
        APP --> INFRA
    end

    style DOMAIN fill:#9C27B0,color:#fff
    style APP fill:#2196F3,color:#fff
    style INFRA fill:#FF9800,color:#fff
    style PRES fill:#4CAF50,color:#fff
```

### Project Dependencies

```mermaid
graph TD
    SERVER[TbSense.Backend.Server]
    ENDPOINTS[TbSense.Backend.Endpoints]
    BACKEND[TbSense.Backend]
    ABSTRACTION[TbSense.Backend.Abstraction]
    DOMAIN[TbSense.Backend.Domain]
    EFCORE[TbSense.Backend.EfCore]
    POSTGRES[TbSense.Backend.EfCore.Postgresql]
    TRAINER[TbSense.Backend.Trainer]
    TRAINER_ABS[TbSense.Backend.Trainer.Abstraction]
    STORAGE[TbSense.Backend.Storage]
    STORAGE_ABS[TbSense.Backend.Storage.Abstraction]
    MIGRATOR[TbSense.Backend.Migrator]

    SERVER --> ENDPOINTS
    SERVER --> BACKEND
    SERVER --> EFCORE
    SERVER --> POSTGRES
    SERVER --> TRAINER
    SERVER --> STORAGE

    ENDPOINTS --> ABSTRACTION
    ENDPOINTS --> DOMAIN

    BACKEND --> ABSTRACTION
    BACKEND --> DOMAIN

    ABSTRACTION --> DOMAIN

    EFCORE --> ABSTRACTION
    EFCORE --> DOMAIN

    POSTGRES --> EFCORE

    TRAINER --> TRAINER_ABS
    TRAINER --> ABSTRACTION

    STORAGE --> STORAGE_ABS
    STORAGE --> ABSTRACTION

    MIGRATOR --> EFCORE
    MIGRATOR --> POSTGRES

    style SERVER fill:#4CAF50,color:#fff
    style DOMAIN fill:#9C27B0,color:#fff
    style ABSTRACTION fill:#2196F3,color:#fff
```

### Request Flow

```mermaid
sequenceDiagram
    participant Client
    participant Endpoint
    participant Validator
    participant Service
    participant Repository
    participant DbContext
    participant Database

    Client->>Endpoint: HTTP Request
    Endpoint->>Validator: Validate Request

    alt Validation Failed
        Validator-->>Endpoint: Validation Error
        Endpoint-->>Client: 400 Bad Request
    else Validation Success
        Validator-->>Endpoint: Valid
        Endpoint->>Service: Call Business Logic
        Service->>Repository: Query Data
        Repository->>DbContext: LINQ Query
        DbContext->>Database: SQL Query
        Database-->>DbContext: Result Set
        DbContext-->>Repository: Entities
        Repository-->>Service: Domain Objects
        Service->>Service: Apply Business Rules
        Service-->>Endpoint: Response Model
        Endpoint-->>Client: 200 OK + JSON
    end
```

### Dashboard Analytics Flow

```mermaid
graph LR
    subgraph "Client Request"
        REQUEST[GET /api/dashboard/area-chart/cumulative-yield]
    end

    subgraph "Endpoint Layer"
        ENDPOINT[GetCumulativeYieldAreaChartEndpoint]
    end

    subgraph "Service Layer"
        DASHBOARD_SERVICE[DashboardService]
        HELPER[AreaChartHelper]
    end

    subgraph "Repository Layer"
        DASHBOARD_REPO[DashboardRepository]
    end

    subgraph "Database Layer"
        QUERY[EF Core Query]
        DB[(PostgreSQL)]
    end

    subgraph "Response"
        MODEL[CumulativeYieldResponse]
    end

    REQUEST --> ENDPOINT
    ENDPOINT --> DASHBOARD_SERVICE
    DASHBOARD_SERVICE --> DASHBOARD_REPO
    DASHBOARD_REPO --> QUERY
    QUERY --> DB
    DB --> QUERY
    QUERY --> DASHBOARD_REPO
    DASHBOARD_REPO --> DASHBOARD_SERVICE
    DASHBOARD_SERVICE --> HELPER
    HELPER --> DASHBOARD_SERVICE
    DASHBOARD_SERVICE --> MODEL
    MODEL --> ENDPOINT
    ENDPOINT --> REQUEST

    style ENDPOINT fill:#4CAF50
    style DASHBOARD_SERVICE fill:#2196F3
    style DASHBOARD_REPO fill:#FF9800
    style DB fill:#336791
    style MODEL fill:#9C27B0
```

### ML Training Workflow

```mermaid
sequenceDiagram
    participant Client
    participant API
    participant MLService
    participant TrainerClient
    participant PythonTrainer
    participant StorageService
    participant MinIO
    participant Database

    Client->>API: POST /api/models/train
    API->>MLService: TrainModelAsync()
    MLService->>Database: Create Model Record (Status: Training)
    MLService->>TrainerClient: SendTrainingRequest()
    TrainerClient->>PythonTrainer: HTTP POST /train

    PythonTrainer->>PythonTrainer: Train ML Model
    PythonTrainer->>PythonTrainer: Validate Model
    PythonTrainer-->>TrainerClient: Training Complete + Metrics

    TrainerClient->>StorageService: UploadModel()
    StorageService->>MinIO: Store Model File
    MinIO-->>StorageService: File URL

    StorageService-->>MLService: Model Stored
    MLService->>Database: Update Model (Status: Completed)
    MLService-->>API: Training Complete
    API-->>Client: 200 OK + Model Metadata
```

### Data Flow - Tree Metrics Collection

```mermaid
graph TB
    subgraph "IoT Devices"
        SENSOR1[Temperature Sensor]
        SENSOR2[Moisture Sensor]
        IOT_GATEWAY[IoT Gateway]
    end

    subgraph "API Layer"
        METRIC_ENDPOINT[POST /api/tree-metrics]
        VALIDATION[FluentValidation]
    end

    subgraph "Processing"
        METRIC_SERVICE[Tree Metric Service]
        AGGREGATION[Data Aggregation]
        ANALYTICS[Real-time Analytics]
    end

    subgraph "Storage"
        METRIC_REPO[Metrics Repository]
        DB[(PostgreSQL<br/>134,675+ readings)]
    end

    subgraph "Dashboards"
        TREE_DASH[Tree Dashboard]
        PLANT_DASH[Plantation Dashboard]
        GLOBAL_DASH[Global Dashboard]
    end

    SENSOR1 --> IOT_GATEWAY
    SENSOR2 --> IOT_GATEWAY
    IOT_GATEWAY --> METRIC_ENDPOINT

    METRIC_ENDPOINT --> VALIDATION
    VALIDATION --> METRIC_SERVICE
    METRIC_SERVICE --> AGGREGATION
    METRIC_SERVICE --> METRIC_REPO
    METRIC_REPO --> DB

    AGGREGATION --> ANALYTICS
    ANALYTICS --> TREE_DASH
    ANALYTICS --> PLANT_DASH
    ANALYTICS --> GLOBAL_DASH

    style IOT_GATEWAY fill:#00BCD4
    style METRIC_SERVICE fill:#2196F3
    style DB fill:#336791
    style TREE_DASH fill:#4CAF50
```

### Chart Generation Architecture

```mermaid
graph TB
    subgraph "Chart Request"
        REQUEST[Chart Endpoint Request]
        PARAMS[Query Parameters<br/>startDate, endDate, interval]
    end

    subgraph "Service Layer"
        SVC[Dashboard Service]
        HELPER[Chart Helper]
    end

    subgraph "Helper Functions"
        GROUP[GroupByInterval<br/>daily/weekly/monthly]
        CALC[Calculate Cumulative Values]
        STATS[Calculate Statistics<br/>avg, min, max]
    end

    subgraph "Data Processing"
        RAW[Raw Time-Series Data]
        GROUPED[Grouped Data Points]
        CUMULATIVE[Cumulative Values]
        FORMATTED[Chart Response Model]
    end

    REQUEST --> PARAMS
    PARAMS --> SVC
    SVC --> HELPER
    HELPER --> GROUP
    HELPER --> CALC
    HELPER --> STATS

    SVC --> RAW
    RAW --> GROUP
    GROUP --> GROUPED
    GROUPED --> CALC
    CALC --> CUMULATIVE
    CUMULATIVE --> STATS
    STATS --> FORMATTED
    FORMATTED --> REQUEST

    style HELPER fill:#FF9800
    style CALC fill:#2196F3
    style FORMATTED fill:#4CAF50
```

### Design Patterns Used

```mermaid
mindmap
    root((Design Patterns))
        Repository Pattern
            Data Access Abstraction
            IRepository Interface
            Generic Repository
            Specific Repositories
        Dependency Injection
            Constructor Injection
            Property Injection
            Service Lifetime Management
            IoC Container
        Factory Pattern
            DbContext Factory
            Model Factory
            Response Factory
        Strategy Pattern
            Chart Calculation Strategies
            Simple Area Chart
            Stacked Area Chart
            Histogram Binning
        Builder Pattern
            Query Builder
            Filter Builder
            Response Builder
        CQRS lite
            Read Services
            Write Services
            Command/Query Separation
```

### Database Entity Relationships

```mermaid
erDiagram
    PLANTATION ||--o{ TREE : contains
    PLANTATION ||--o{ PLANTATION_COORDINATE : "has boundary"
    PLANTATION ||--o{ PLANTATION_HARVEST : produces
    PLANTATION ||--o{ YIELD_PREDICTION : "has predictions"
    TREE ||--o{ TREE_METRIC : "has readings"
    TREE ||--o{ PLANTATION_HARVEST : "contributes to"
    MODEL ||--o{ YIELD_PREDICTION : generates

    PLANTATION {
        uuid id PK
        string name
        string description
        float land_area_hectares
        datetime planted_date
        datetime created_at
        datetime updated_at
    }

    TREE {
        uuid id PK
        uuid plantation_id FK
        double longitude
        double latitude
        datetime created_at
        datetime updated_at
    }

    TREE_METRIC {
        uuid id PK
        uuid tree_id FK
        double air_temperature
        double soil_temperature
        double soil_moisture
        datetimeoffset recorded_at
        datetime created_at
    }

    PLANTATION_HARVEST {
        uuid id PK
        uuid plantation_id FK
        uuid tree_id FK
        datetime harvested_at
        double amount
        string quality
        string notes
        datetime created_at
    }

    PLANTATION_COORDINATE {
        uuid id PK
        uuid plantation_id FK
        double longitude
        double latitude
        int order
        datetime created_at
    }

    YIELD_PREDICTION {
        uuid id PK
        uuid plantation_id FK
        uuid model_id FK
        double predicted_yield
        datetime prediction_date
        double confidence_score
        datetime created_at
    }

    MODEL {
        uuid id PK
        string name
        string version
        string file_path
        datetime training_date
        double accuracy
        string status
        datetime created_at
    }
```

### Technology Stack Integration

```mermaid
graph TB
    subgraph "Frontend/Clients"
        WEB[Web Dashboard<br/>React/Vue/Angular]
        MOBILE[Mobile App<br/>Flutter/React Native]
        IOT[IoT Devices<br/>ESP32/Raspberry Pi]
    end

    subgraph ".NET 9.0 Backend"
        FASTENDPOINTS[FastEndpoints 7.0.1<br/>HTTP API]
        EFCORE[EF Core 9.0.9<br/>ORM]
        FLUENT[FluentValidation 12.0<br/>Input Validation]
        FRAMEWORK[MasLazu Framework<br/>Base Architecture]
    end

    subgraph "Data Storage"
        POSTGRES[(PostgreSQL 16+<br/>Relational Data)]
        MINIO[(MinIO/S3<br/>Object Storage)]
    end

    subgraph "External Services"
        PYTHON[Python Trainer<br/>ML Service]
        WEATHER[Weather API<br/>Future Integration]
    end

    WEB --> FASTENDPOINTS
    MOBILE --> FASTENDPOINTS
    IOT --> FASTENDPOINTS

    FASTENDPOINTS --> FLUENT
    FASTENDPOINTS --> FRAMEWORK
    FLUENT --> EFCORE
    FRAMEWORK --> EFCORE

    EFCORE --> POSTGRES
    FRAMEWORK --> MINIO
    FASTENDPOINTS --> PYTHON

    style FASTENDPOINTS fill:#4CAF50
    style EFCORE fill:#512BD4
    style POSTGRES fill:#336791
    style MINIO fill:#C72E49
    style PYTHON fill:#3776AB
```

### Deployment Architecture

```mermaid
graph TB
    subgraph "Production Environment"
        subgraph "Container Orchestration"
            LB[Load Balancer<br/>Nginx/HAProxy]

            subgraph "API Instances"
                API1[TbSense API<br/>Instance 1]
                API2[TbSense API<br/>Instance 2]
                API3[TbSense API<br/>Instance 3]
            end
        end

        subgraph "Data Layer"
            PG_PRIMARY[(PostgreSQL<br/>Primary)]
            PG_REPLICA[(PostgreSQL<br/>Read Replica)]
        end

        subgraph "Storage Layer"
            MINIO_CLUSTER[MinIO Cluster<br/>Distributed Storage]
        end

        subgraph "ML Services"
            TRAINER_SVC[Python Trainer<br/>Service Pool]
        end

        subgraph "Monitoring"
            PROMETHEUS[Prometheus<br/>Metrics]
            GRAFANA[Grafana<br/>Dashboards]
            LOGS[ELK Stack<br/>Logs]
        end
    end

    LB --> API1
    LB --> API2
    LB --> API3

    API1 --> PG_PRIMARY
    API2 --> PG_PRIMARY
    API3 --> PG_PRIMARY

    API1 --> PG_REPLICA
    API2 --> PG_REPLICA
    API3 --> PG_REPLICA

    API1 --> MINIO_CLUSTER
    API2 --> MINIO_CLUSTER
    API3 --> MINIO_CLUSTER

    API1 --> TRAINER_SVC
    API2 --> TRAINER_SVC
    API3 --> TRAINER_SVC

    API1 --> PROMETHEUS
    API2 --> PROMETHEUS
    API3 --> PROMETHEUS

    PROMETHEUS --> GRAFANA
    API1 --> LOGS
    API2 --> LOGS
    API3 --> LOGS

    style LB fill:#FF6B6B
    style API1 fill:#4CAF50
    style API2 fill:#4CAF50
    style API3 fill:#4CAF50
    style PG_PRIMARY fill:#336791
    style PROMETHEUS fill:#E6522C
```

### Design Patterns Summary

- **Repository Pattern**: Data access abstraction
- **Dependency Injection**: IoC container for loose coupling
- **CQRS-lite**: Read/Write separation in services
- **Factory Pattern**: DbContext factory for migrations
- **Strategy Pattern**: Multiple chart calculation strategies
- **Builder Pattern**: Complex query and response construction
- **Singleton Pattern**: DbContext configuration
- **Chain of Responsibility**: Middleware pipeline

---

## 🛠️ Technology Stack

### Backend Framework

- **.NET 9.0**: Latest .NET platform
- **C# 13**: Modern C# with nullable reference types
- **FastEndpoints 7.0.1**: High-performance endpoint routing
- **MasLazu.AspNet.Framework**: Custom framework utilities

### Database & ORM

- **PostgreSQL 16+**: Primary relational database
- **Entity Framework Core 9.0.9**: ORM for data access
- **Npgsql**: PostgreSQL provider for .NET

### Storage & ML

- **FluentStorage 6.0.0**: Abstraction for cloud storage
- **AWS S3 / MinIO**: Object storage for ML models
- **External Python Service**: ML model training

### Validation & Documentation

- **FluentValidation 12.0.0**: Request validation
- **FastEndpoints.Swagger 7.0.1**: OpenAPI/Swagger documentation
- **Microsoft.AspNetCore.OpenApi 9.0.9**: OpenAPI support

### Testing

- **xUnit 2.9.3**: Testing framework
- **Coverlet**: Code coverage analysis
- **Unit Tests**: Business logic validation
- **Integration Tests**: API endpoint testing
- **E2E Tests**: End-to-end workflow testing

---

## 📋 Prerequisites

### Required Software

- **.NET 9.0 SDK** or later ([Download](https://dotnet.microsoft.com/download))
- **PostgreSQL 16+** ([Download](https://www.postgresql.org/download/))
- **Docker** (optional, for containerized services) ([Download](https://www.docker.com/))

### Optional Services

- **MinIO** or **AWS S3** (for model storage)
- **Python 3.10+** (for ML trainer service)

### Development Tools

- **Visual Studio 2022** / **VS Code** / **Rider**
- **Git** for version control
- **Postman** / **Thunder Client** (API testing)

---

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/MasLazu/tbsense-backend.git
cd tbsense-backend
```

### 2. Configure Database

Create a PostgreSQL database:

```sql
CREATE DATABASE tbsense_db;
```

Update connection string in `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=tbsense_db;Username=postgres;Password=yourpassword"
  }
}
```

### 3. Run Migrations

```bash
# Navigate to migrator project
cd src/TbSense.Backend.Migrator

# Run migrations
dotnet run
```

Or use Entity Framework CLI:

```bash
# From solution root
dotnet ef database update --project src/TbSense.Backend.EfCore.Postgresql --startup-project src/TbSense.Backend.Migrator
```

### 4. Configure External Services (Optional)

**MinIO for Model Storage:**

```bash
# Run MinIO with Docker
docker run -p 9000:9000 -p 9001:9001 \
  -e MINIO_ROOT_USER=minioadmin \
  -e MINIO_ROOT_PASSWORD=minioadmin \
  quay.io/minio/minio server /data --console-address ":9001"
```

Update `appsettings.Development.json`:

```json
{
  "MinIO": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin",
    "BucketName": "models",
    "UseSSL": false
  }
}
```

**Python Trainer Service:**

```json
{
  "TrainerService": {
    "BaseUrl": "http://localhost:5001",
    "TimeoutMinutes": 30
  }
}
```

### 5. Run the Application

```bash
# Navigate to server project
cd src/TbSense.Backend.Server

# Run in development mode
dotnet run

# Or with hot reload
dotnet watch run
```

The API will be available at:

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `http://localhost:5000/swagger`

### 6. Seed Sample Data (Optional)

```bash
# Run seeding script (if available)
dotnet run --project src/TbSense.Backend.Migrator -- --seed

# Or use SQL scripts in /sql directory
psql -U postgres -d tbsense_db -f sql/seed_data.sql
```

---

## 📁 Project Structure

```
tbsense-backend/
├── src/
│   ├── TbSense.Backend/                    # Core business logic
│   │   ├── Services/                       # Business services
│   │   ├── Utils/                          # Utility classes
│   │   ├── Validators/                     # FluentValidation validators
│   │   └── Extensions/                     # Service extensions
│   │
│   ├── TbSense.Backend.Abstraction/        # Interfaces and contracts
│   │   ├── Interfaces/                     # Service interfaces
│   │   └── Models/                         # DTOs and response models
│   │       ├── Dashboard/                  # Dashboard models
│   │       ├── PieCharts/                  # Pie chart models
│   │       ├── BarCharts/                  # Bar chart models
│   │       ├── Histograms/                 # Histogram models
│   │       └── AreaCharts/                 # Area chart models
│   │
│   ├── TbSense.Backend.Domain/             # Domain entities
│   │   └── Entities/                       # EF Core entities
│   │       ├── Plantation.cs               # Plantation entity
│   │       ├── Tree.cs                     # Tree entity
│   │       ├── TreeMetric.cs               # Sensor readings
│   │       ├── PlantationHarvest.cs        # Harvest records
│   │       ├── PlantationCoordinate.cs     # GPS coordinates
│   │       ├── PlantationYieldPrediction.cs # ML predictions
│   │       └── Model.cs                    # ML model metadata
│   │
│   ├── TbSense.Backend.EfCore/             # Data access layer
│   │   ├── Data/                           # DbContext
│   │   ├── Configurations/                 # Entity configurations
│   │   └── Extensions/                     # EF Core extensions
│   │
│   ├── TbSense.Backend.EfCore.Postgresql/  # PostgreSQL provider
│   │   └── Extensions/                     # PostgreSQL setup
│   │
│   ├── TbSense.Backend.Endpoints/          # API endpoints
│   │   ├── Endpoints/
│   │   │   ├── Dashboard/                  # Global dashboard (51 charts)
│   │   │   ├── PlantationDashboard/        # Plantation analytics
│   │   │   ├── TreeDashboard/              # Tree monitoring
│   │   │   ├── Plantations/                # CRUD operations
│   │   │   ├── Trees/                      # Tree management
│   │   │   ├── PlantationHarvests/         # Harvest records
│   │   │   ├── TreeMetrics/                # Sensor data
│   │   │   ├── PlantationCoordinates/      # GPS data
│   │   │   ├── PlantationYieldPredictions/ # ML predictions
│   │   │   └── Models/                     # ML model management
│   │   ├── EndpointGroups/                 # Endpoint grouping
│   │   └── Models/                         # Request/Response DTOs
│   │
│   ├── TbSense.Backend.Migrator/           # Database migrations
│   │   ├── Migrations/                     # EF Core migrations
│   │   └── Program.cs                      # Migration runner
│   │
│   ├── TbSense.Backend.Server/             # Web API host
│   │   ├── Program.cs                      # Application entry point
│   │   ├── appsettings.json                # Configuration
│   │   └── Extensions/                     # Server extensions
│   │
│   ├── TbSense.Backend.Trainer/            # ML training integration
│   │   └── Services/                       # Trainer service client
│   │
│   ├── TbSense.Backend.Trainer.Abstraction/ # Trainer contracts
│   │   ├── Interfaces/                     # Trainer interfaces
│   │   └── Models/                         # Training DTOs
│   │
│   ├── TbSense.Backend.Storage/            # File storage
│   │   └── Services/                       # Storage service
│   │
│   └── TbSense.Backend.Storage.Abstraction/ # Storage contracts
│       └── Interfaces/                     # Storage interfaces
│
├── test/
│   ├── TbSense.Backend.UnitTests/          # Unit tests
│   ├── TbSense.Backend.IntegrationTests/   # Integration tests
│   └── TbSense.Backend.E2ETests/           # End-to-end tests
│
├── Directory.Packages.props                # Centralized package management
├── TbSense.Backend.sln                     # Solution file
└── README.md                               # This file
```

---

## 📚 API Documentation

### Swagger UI

Access interactive API documentation at:

```
http://localhost:5000/swagger
```

### API Endpoints Overview

#### 🌍 **Global Dashboard** (`/api/dashboard`)

**Summary Endpoints:**

- `GET /plantations-summary` - Portfolio overview
- `GET /trees-summary` - Tree statistics
- `GET /land-area-summary` - Land metrics
- `GET /harvest-summary` - Harvest KPIs
- `GET /environmental-averages` - Climate data

**Pie Charts (9 endpoints):**

- `GET /pie-chart/plantations-by-trees`
- `GET /pie-chart/plantations-by-land-area`
- `GET /pie-chart/harvest-by-plantation`
- `GET /pie-chart/tree-activity-status`

**Bar Charts (14 endpoints):**

- `GET /bar-chart/top-plantations-by-yield?limit=10`
- `GET /bar-chart/top-plantations-by-avg-harvest?limit=10`
- `GET /bar-chart/tree-count-by-plantation?limit=10`
- `GET /bar-chart/tree-activity-by-plantation`
- `GET /bar-chart/harvest-frequency-by-plantation`

**Histograms (13 endpoints):**

- `GET /histogram/yield-distribution?binCount=15`
- `GET /histogram/plantation-size-distribution?binCount=10`
- `GET /histogram/tree-density-distribution?binCount=12`
- `GET /histogram/harvest-frequency-distribution`
- `GET /histogram/avg-harvest-size-distribution`

**Area Charts (15 endpoints):**

- `GET /area-chart/cumulative-yield?interval=daily`
- `GET /area-chart/cumulative-harvest-count?interval=weekly`
- `GET /area-chart/plantation-growth?interval=monthly`
- `GET /area-chart/tree-population-growth?interval=weekly`
- `GET /area-chart/cumulative-active-trees?interval=daily`
- `GET /area-chart/stacked-yield-by-plantation?limit=10&interval=monthly`

**Timeseries:**

- `GET /timeseries/environmental?interval=hourly`
- `GET /timeseries/harvest?interval=daily`
- `GET /timeseries/plantation-growth?interval=monthly`

#### 🌱 **Plantation Dashboard** (`/api/plantation-dashboard`)

**Summary:**

- `GET /basic-summary?plantationId={guid}`
- `GET /trees-summary?plantationId={guid}`
- `GET /harvest-summary?plantationId={guid}`
- `GET /ranking?plantationId={guid}`

**Charts (20 endpoints):**

- Pie Charts: 4 endpoints
- Bar Charts: 5 endpoints
- Histograms: 6 endpoints
- Area Charts: 5 endpoints

#### 🌳 **Tree Dashboard** (`/api/tree-dashboard`)

**Summary:**

- `GET /basic-info?treeId={guid}`
- `GET /current-metrics?treeId={guid}`
- `GET /environmental-averages?treeId={guid}`

**Charts (15 endpoints):**

- Pie Charts: 2 endpoints
- Bar Charts: 4 endpoints
- Histograms: 3 endpoints
- Area Charts: 4 endpoints
- Timeseries: 1 endpoint

#### 📋 **CRUD Endpoints**

**Plantations:**

- `GET /api/plantations/paginated` - List with filters
- `GET /api/plantations/{id}` - Get by ID
- `POST /api/plantations` - Create
- `PUT /api/plantations/{id}` - Update
- `DELETE /api/plantations/{id}` - Delete

**Trees:**

- `GET /api/trees/paginated`
- `GET /api/trees/{id}`
- `POST /api/trees`
- `PUT /api/trees/{id}`
- `DELETE /api/trees/{id}`

**Harvests:**

- `GET /api/plantation-harvests/paginated`
- `GET /api/plantation-harvests/{id}`
- `POST /api/plantation-harvests`
- `PUT /api/plantation-harvests/{id}`
- `DELETE /api/plantation-harvests/{id}`

**Tree Metrics:**

- `GET /api/tree-metrics/paginated`
- `GET /api/tree-metrics/{id}`
- `POST /api/tree-metrics`
- `PUT /api/tree-metrics/{id}`
- `DELETE /api/tree-metrics/{id}`

**ML Models:**

- `GET /api/models/paginated`
- `GET /api/models/{id}`
- `POST /api/models` - Create model metadata
- `POST /api/models/train` - Trigger training
- `POST /api/models/complete-training` - Mark as complete
- `GET /api/models/{id}/download` - Download model file
- `PUT /api/models/{id}`
- `DELETE /api/models/{id}`

### Query Parameters

**Pagination:**

```
?page=1&pageSize=20
```

**Filtering:**

```
?filterField=name&filterValue=North&filterOperator=contains
```

**Sorting:**

```
?sortField=createdAt&sortOrder=desc
```

**Date Filtering:**

```
?startDate=2024-01-01&endDate=2024-12-31
```

---

## 🗄️ Database Schema

### Core Entities

**Plantations**

- `Id` (UUID, PK)
- `Name` (string)
- `Description` (string, nullable)
- `LandAreaHectares` (float)
- `PlantedDate` (DateTime)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**Trees**

- `Id` (UUID, PK)
- `PlantationId` (UUID, FK)
- `Longitude` (double)
- `Latitude` (double)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**TreeMetrics**

- `Id` (UUID, PK)
- `TreeId` (UUID, FK)
- `AirTemperature` (double)
- `SoilTemperature` (double)
- `SoilMoisture` (double)
- `RecordedAt` (DateTimeOffset)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**PlantationHarvests**

- `Id` (UUID, PK)
- `PlantationId` (UUID, FK)
- `TreeId` (UUID, FK, nullable)
- `HarvestedAt` (DateTime)
- `Amount` (double)
- `Quality` (string, nullable)
- `Notes` (string, nullable)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**PlantationCoordinates**

- `Id` (UUID, PK)
- `PlantationId` (UUID, FK)
- `Longitude` (double)
- `Latitude` (double)
- `Order` (int)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**PlantationYieldPredictions**

- `Id` (UUID, PK)
- `PlantationId` (UUID, FK)
- `ModelId` (UUID, FK)
- `PredictedYield` (double)
- `PredictionDate` (DateTime)
- `ConfidenceScore` (double, nullable)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

**Models**

- `Id` (UUID, PK)
- `Name` (string)
- `Version` (string)
- `Description` (string, nullable)
- `FilePath` (string)
- `TrainingDate` (DateTime)
- `Accuracy` (double, nullable)
- `Status` (string)
- `CreatedAt`, `UpdatedAt`, `DeletedAt`

### Relationships

```
Plantation 1──────* Tree
Plantation 1──────* PlantationCoordinate
Plantation 1──────* PlantationHarvest
Plantation 1──────* PlantationYieldPrediction
Tree 1──────* TreeMetric
Tree 1──────* PlantationHarvest
Model 1──────* PlantationYieldPrediction
```

### Indexes

```sql
-- Performance indexes
CREATE INDEX idx_tree_plantation ON trees(plantation_id);
CREATE INDEX idx_tree_metrics_tree ON tree_metrics(tree_id);
CREATE INDEX idx_tree_metrics_recorded_at ON tree_metrics(recorded_at);
CREATE INDEX idx_harvests_plantation ON plantation_harvests(plantation_id);
CREATE INDEX idx_harvests_date ON plantation_harvests(harvested_at);
CREATE INDEX idx_coordinates_plantation ON plantation_coordinates(plantation_id);
```

---

## ⚙️ Configuration

### Environment Variables

Create `.env` file in server project:

```bash
# Database
ConnectionStrings__DefaultConnection="Host=localhost;Database=tbsense_db;Username=postgres;Password=yourpassword"

# MinIO/S3
MinIO__Endpoint=localhost:9000
MinIO__AccessKey=minioadmin
MinIO__SecretKey=minioadmin
MinIO__BucketName=models
MinIO__UseSSL=false

# Trainer Service
TrainerService__BaseUrl=http://localhost:5001
TrainerService__TimeoutMinutes=30

# Logging
Logging__LogLevel__Default=Information
Logging__LogLevel__Microsoft.AspNetCore=Warning
```

### appsettings.json Structure

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=tbsense_db;Username=postgres;Password=postgres"
  },
  "MinIO": {
    "Endpoint": "localhost:9000",
    "AccessKey": "minioadmin",
    "SecretKey": "minioadmin",
    "BucketName": "models",
    "UseSSL": false
  },
  "TrainerService": {
    "BaseUrl": "http://localhost:5001",
    "TimeoutMinutes": 30
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:3000", "http://localhost:5173"]
  }
}
```

---

## 💻 Development

### Building the Project

```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build src/TbSense.Backend.Server

# Build for production
dotnet build -c Release
```

### Running in Development

```bash
# Run with hot reload
cd src/TbSense.Backend.Server
dotnet watch run

# Run with specific environment
ASPNETCORE_ENVIRONMENT=Development dotnet run

# Run with custom port
dotnet run --urls "http://localhost:5000;https://localhost:5001"
```

### Code Style & Formatting

```bash
# Format code
dotnet format

# Check formatting
dotnet format --verify-no-changes
```

### Database Migrations

```bash
# Add new migration
dotnet ef migrations add MigrationName \
  --project src/TbSense.Backend.EfCore.Postgresql \
  --startup-project src/TbSense.Backend.Migrator

# Update database
dotnet ef database update \
  --project src/TbSense.Backend.EfCore.Postgresql \
  --startup-project src/TbSense.Backend.Migrator

# Rollback migration
dotnet ef database update PreviousMigrationName \
  --project src/TbSense.Backend.EfCore.Postgresql \
  --startup-project src/TbSense.Backend.Migrator

# Remove last migration
dotnet ef migrations remove \
  --project src/TbSense.Backend.EfCore.Postgresql
```

---

## 🧪 Testing

### Run All Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test /p:CollectCoverage=true

# Run specific test project
dotnet test test/TbSense.Backend.UnitTests

# Run tests with filter
dotnet test --filter "FullyQualifiedName~DashboardService"
```

### Unit Tests

```bash
cd test/TbSense.Backend.UnitTests
dotnet test --logger "console;verbosity=detailed"
```

### Integration Tests

```bash
cd test/TbSense.Backend.IntegrationTests
dotnet test
```

### E2E Tests

```bash
cd test/TbSense.Backend.E2ETests
dotnet test
```

### Test Coverage Report

```bash
# Generate coverage report
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover

# View report (requires ReportGenerator)
reportgenerator -reports:coverage.opencover.xml -targetdir:coverage-report
```

---

## 🚢 Deployment

### Docker Deployment

**Create Dockerfile:**

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["src/TbSense.Backend.Server/TbSense.Backend.Server.csproj", "src/TbSense.Backend.Server/"]
# Copy other project files...
RUN dotnet restore "src/TbSense.Backend.Server/TbSense.Backend.Server.csproj"
COPY . .
WORKDIR "/src/src/TbSense.Backend.Server"
RUN dotnet build "TbSense.Backend.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TbSense.Backend.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TbSense.Backend.Server.dll"]
```

**Docker Compose:**

```yaml
version: "3.8"

services:
  tbsense-api:
    build: .
    ports:
      - "5000:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=tbsense_db;Username=postgres;Password=secure_password
      - MinIO__Endpoint=minio:9000
    depends_on:
      - postgres
      - minio
    networks:
      - tbsense-network

  postgres:
    image: postgres:16-alpine
    environment:
      - POSTGRES_DB=tbsense_db
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=secure_password
    volumes:
      - postgres-data:/var/lib/postgresql/data
    networks:
      - tbsense-network

  minio:
    image: quay.io/minio/minio
    command: server /data --console-address ":9001"
    ports:
      - "9000:9000"
      - "9001:9001"
    environment:
      - MINIO_ROOT_USER=minioadmin
      - MINIO_ROOT_PASSWORD=minioadmin
    volumes:
      - minio-data:/data
    networks:
      - tbsense-network

volumes:
  postgres-data:
  minio-data:

networks:
  tbsense-network:
    driver: bridge
```

**Deploy:**

```bash
docker-compose up -d
```

### Production Build

```bash
# Publish for deployment
dotnet publish src/TbSense.Backend.Server -c Release -o ./publish

# Run published app
cd publish
dotnet TbSense.Backend.Server.dll
```

### Cloud Deployment

**Azure App Service:**

```bash
az webapp up --name tbsense-api --resource-group TbSense-RG --runtime "DOTNET|9.0"
```

**AWS Elastic Beanstalk:**

```bash
dotnet aws deploy
```

---

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

### Getting Started

1. Fork the repository
2. Create a feature branch: `git checkout -b feature/amazing-feature`
3. Make your changes
4. Run tests: `dotnet test`
5. Commit: `git commit -m 'Add amazing feature'`
6. Push: `git push origin feature/amazing-feature`
7. Open a Pull Request

### Code Standards

- Follow C# coding conventions
- Write unit tests for new features
- Update documentation
- Keep commits atomic and meaningful
- Use descriptive PR titles

### Commit Message Format

```
<type>(<scope>): <subject>

<body>

<footer>
```

Types: `feat`, `fix`, `docs`, `style`, `refactor`, `test`, `chore`

Example:

```
feat(dashboard): add cumulative yield area chart

- Implement cumulative calculation helper
- Add area chart endpoint
- Create response model with statistics

Closes #123
```

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](LICENSE) file for details.

---

## 👥 Authors

- **MasLazu** - _Initial work_ - [GitHub](https://github.com/MasLazu)

---

## 🙏 Acknowledgments

- Built with [FastEndpoints](https://fast-endpoints.com/)
- Uses [MasLazu.AspNet.Framework](https://github.com/MasLazu)
- Database powered by [PostgreSQL](https://www.postgresql.org/)
- Storage handled by [FluentStorage](https://github.com/robinrodricks/FluentStorage)

---

## 📞 Support

For support and questions:

- **GitHub Issues**: [Report issues](https://github.com/MasLazu/tbsense-backend/issues)
- **Documentation**: [Wiki](https://github.com/MasLazu/tbsense-backend/wiki)
- **Email**: support@tbsense.com

---

## 🗺️ Roadmap

### Version 2.0

- [ ] Real-time WebSocket updates
- [ ] Mobile app integration
- [ ] Weather API integration
- [ ] Advanced ML models (disease detection)
- [ ] Multi-tenant support
- [ ] GraphQL API
- [ ] Automated alerting system
- [ ] Export to Excel/PDF reports

### Version 2.1

- [ ] Drone integration for aerial monitoring
- [ ] Blockchain for harvest traceability
- [ ] Advanced GIS features
- [ ] Predictive maintenance
- [ ] Cost analysis module

---

## 📊 Project Stats

- **Total Endpoints**: 100+
- **Chart Endpoints**: 51
- **Database Tables**: 8
- **Test Coverage**: Target 80%+
- **API Response Time**: < 200ms (avg)
- **Supported Trees**: 5,387+
- **Supported Plantations**: 13+
- **Metrics Tracked**: 134,675+

---

<div align="center">

**Built with ❤️ using .NET 9.0**

[⬆ Back to Top](#-tbsense-backend)

</div>

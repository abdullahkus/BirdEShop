# BirdEShop - Modular Monolith Architecture & Documentation

BirdEShop is an enterprise-grade e-commerce application built with **.NET 10** and **ASP.NET Core Minimal APIs**, designed according to **Modular Monolith**, **Vertical Slice Architecture**, **CQRS**, and **Domain-Driven Design (DDD)** principles.

---

## 🏛️ Architectural Overview

BirdEShop is designed as a **Modular Monolith** where each domain module is logically isolated with its own database schema, data models, and business logic, yet hosted within a single deployable runtime:

* **Catalog Module:** Product and category management, paginated listings, and high-throughput read operations with PostgreSQL schema isolation (`catalog`).
* **Basket Module:** Shopping cart management with high-velocity read/write support backed by distributed caching (**Redis**) and cross-module price synchronization.
* **Ordering Module:** Order management, checkout process, rich domain business logic, and transactional boundaries with schema isolation (`ordering`).
* **Bootstrapper (`Api`):** Composition root responsible for dependency injection registration, middleware configuration, authentication/authorization, and exposing Minimal API endpoints via Carter.
* **Shared Kernel (`Shared`, `Shared.Contracts`, `Shared.Messaging`):** Cross-cutting concerns including MediatR pipeline behaviors, CQRS abstractions, EF Core interceptors, custom exception handlers, and asynchronous messaging contracts.

---

## 🚀 Quick Start

### 1. Prerequisites
* [.NET 10 SDK](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) or Docker Engine with Docker Compose

### 2. Start Infrastructure Services
The development environment uses Docker to orchestrate all supporting infrastructure (PostgreSQL, Redis, RabbitMQ, Seq, and Keycloak).

From the root directory, start the containers:
```bash
docker compose -f src/compose.yaml up -d
```
*(Or navigate into `src/` and run `docker compose up -d`)*

### 3. Run the Application
Start the Bootstrapper API project:
```bash
dotnet run --project src/Bootstrapper/Api/Api.csproj
```
The application will launch on `http://localhost:5000` (HTTP) and `https://localhost:5050` (HTTPS).

---

## 🔌 Infrastructure Services & Ports

| Service | Port / URL | Description / Default Credentials |
| :--- | :--- | :--- |
| **Bootstrapper API** | `http://localhost:5000` / `https://localhost:5050` | ASP.NET Core Minimal API |
| **PostgreSQL** | `localhost:5432` | DB: `BirdEShopDb`, User: `postgres`, Password: `postgres` |
| **Redis** | `localhost:6379` | Distributed Cache |
| **RabbitMQ Management** | `http://localhost:15672` | User: `guest`, Password: `guest` (AMQP on `5672`) |
| **Keycloak (Identity)** | `http://localhost:9090` | Admin: `admin` / `admin`, Realm: `myrealm` |
| **Seq (Log Dashboard)** | `http://localhost:9091` | Web UI (Ingestion port: `5341`) |

---

## 🧰 Technology Stack

* **Runtime & Framework:** .NET 10 & ASP.NET Core Minimal APIs
* **Architecture Patterns:** Modular Monolith, Vertical Slice Architecture, CQRS, Domain-Driven Design (DDD)
* **Data Access & ORM:** Entity Framework Core 10, PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL`)
* **CQRS & Mediation:** MediatR (Command/Query separation with Logging & Validation Pipeline Behaviors)
* **Validation:** FluentValidation (Fail-Fast request validation)
* **Routing:** Carter (Modular Minimal API endpoint definitions)
* **Mapping:** Mapster (High-performance object mapping)
* **Dependency Injection:** Scrutor (Assembly scanning & Decorator pattern)
* **Caching:** Redis (Distributed Caching via StackExchange.Redis)
* **Asynchronous Messaging:** MassTransit & RabbitMQ (Event-driven inter-module communication)
* **Authentication & Authorization:** Keycloak (`Keycloak.AuthServices.Authentication`)
* **Logging & Observability:** Serilog & Seq (Structured centralized logging)
* **DevOps & Containers:** Docker & Docker Compose
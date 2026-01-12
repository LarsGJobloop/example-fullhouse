# FullHouse

A mini project for hosting a strictly governed C# API using:

- Hetzner as Datacenter provider
- IaC for infrastructure definition
- Cloud Init for server configuration
- SystemD for server runtime
- Docker Compose for appliction composition
- Traefik as domain ingress
- Zitadel as Identity solution
- GitHub as Identity Provider
- OpenFGA for policy modelling
- .NET for application development
- PostgreSQL for data modelling
- MinIO for object storage


## Milestones


### v0.1 - New Dawn

IaC module deployable and recreatable.

  - Hetzner Service Setup
  - Server configured using Cloud Init
  - Reconciliation script running through SystemD units

### v0.2 - Breaking Ground

Service implemented for a resource.

  - CRUD API for document resources
  - PostgreSQL migration applying
  - CLI for working the API
  - OCI and CLI published to GitHub Container and Package Registry

### v1.0 - 

Governance overlay implmented.

  - Zitadel setup, IdP plugged in, and Traefik configured
  - OpenFGA setup and enforcing
  - Application middelware setup and enforcing
  - Resources model annotated and ReBAC models up
  - CLI update with authentication

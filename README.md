# AgroSolutions - Ingestion Service

Serviço responsável pela coleta, processamento e ingestão de dados da plataforma AgroSolutions.

Este microsserviço atua como a porta de entrada para dados provenientes de sensores IoT, telemetria de maquinário e arquivos externos, garantindo que a informação chegue íntegra e padronizada ao ecossistema

---

## Arquitetura

Este serviço segue os princípios de:

- Clean Architecture
- DDD (Domain-Driven Design)
- SOLID
- Separação por camadas:
  - API/Trigger
  - Application
  - Domain
  - Infrastructure

---

## Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Web API
- RabbitMQ
- Entity Framework Core
- SQL Server
- Docker
- GitHub Actions (CI/CD)

---

## Responsabilidades do Serviço

- Leitura de Dados
- Despacho de Eventos
- Logs estruturados

---

## Regras de Negócio

- Validação de Range
- Frequência de Dados
- Integridade Cadastral

---

##  Como Executar Localmente

### 1 - Clonar repositório

- bash
  
git clone [https://github.com/FIAP-Pos-Arquitetura-Sistemas/AgroSolutions-Users](https://github.com/FIAP-Pos-Arquitetura-Sistemas/AgroSolutionsIngestion-Service)
cd AgroSolutions-IngestionService

### Executando com Docker
docker build -t agrosolutions-ingestionservice .
docker run -p 8080:80 agrosolutions-ingestionservice

---

##  CI/CD

Este projeto utiliza GitHub Actions para:
- Build automático
- Criação de imagem Docker
- Publicação no Docker Hub

Pipeline localizado em:
.github/workflows/

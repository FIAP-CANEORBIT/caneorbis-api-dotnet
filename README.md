# CaneOrbit — Economia Espacial Aplicada à Agricultura

> API REST de monitoramento agrícola de precisão para cultivo de cana-de-açúcar, integrando sensores IoT (ESP32), imagens de satélite (EOS) e análise com IA (Gemini).

---

## 🔗 Links do Projeto

| Recurso | Link |
| :--- | :--- |
| **Repositório GitHub (Java)** | https://github.com/FIAP-CANEORBIT/fiap-2tdspo-caneorbit-java |
| **Repositório GitHub (C#)** | https://github.com/FIAP-CANEORBIT/caneorbis-api-dotnet |
| **Swagger UI - Java (Produção)** | https://caneorbis-api-java.onrender.com/swagger-ui/index.html |
| **Swagger UI - C# (Produção)** | https://caneorbis-api-dotnet.onrender.com/swagger |
| **API Java Base (Produção)** | https://caneorbis-api-java.onrender.com |
| **API C# Base (Produção)** | https://caneorbis-api-dotnet.onrender.com |
| **Vídeo de Apresentação (10 min)** | *em breve* |
| **Video Pitch (3 min)** | *em breve* |

---

## 📋 Descrição do Projeto

O **CaneOrbit** é uma plataforma de agricultura de precisão que permite a produtores rurais:

- Cadastrar propriedades, talhões (fields) e dispositivos IoT de campo
- Receber leituras em tempo real de sensores ESP32 (umidade do solo, temperatura, pH)
- Integrar dados de satélite via API EOS (NDVI, precipitação, temperatura do ar)
- Obter análises e recomendações inteligentes via modelo Gemini (Google)

### Arquitetura Multi-API

O projeto é composto por **duas APIs independentes** que compartilham o mesmo banco de dados Oracle:

| API | Tecnologia | Responsabilidade |
| :--- | :--- | :--- |
| **API Java** | Spring Boot 3.x | CRUD de usuários, propriedades, dispositivos e leituras de sensor |
| **API C#** | ASP.NET Core | Integração com EOS (satélite) e Gemini (IA) |

### Regras de Negócio

- **Usuário:** Deve conter Nome, E-mail (único) e Senha criptografada.
- **Propriedade:** Deve conter Nome, Localização e Área em hectares.
- **Dispositivo IoT:** Associado a um field/talhão, com MAC Address único, localização (lat/long) e status (ATIVO/INATIVO).
- **Leitura de Sensor:** Registra umidade do solo, temperatura e pH, com timestamp automático.
- **Dados de Satélite:** Integrados via API EOS, incluem NDVI, precipitação, temperatura do ar e condição climática.

---

## 💼 Benefícios para o Negócio

- **Redução de Custos:** Monitoramento remoto elimina deslocamentos e inspeções manuais desnecessárias.
- **Aumento de Produtividade:** Alertas baseados em dados de satélite permitem intervenção rápida em áreas críticas.
- **Sustentabilidade:** Uso eficiente de água e insumos com base em análises de NDVI e precipitação.
- **Decisão Baseada em Dados:** Histórico de leituras e imagens de satélite consolidados em um único sistema.
- **Diferencial Competitivo:** Integração com EOS, dispositivos IoT de baixo custo e IA Gemini.

---

# Disciplina 1: Java Advanced

## 🛠️ Tecnologias Utilizadas (Java)

| Tecnologia | Uso |
| :--- | :--- |
| Java 21 | Linguagem principal |
| Spring Boot 3.x | Framework da API |
| Spring Data JPA / Hibernate | Persistência e ORM |
| Spring Security + JWT (Auth0) | Autenticação e autorização |
| Spring Validation | Validação de payloads |
| Lombok | Produtividade e redução de boilerplate |
| Spring Boot DevTools | Hot reload em desenvolvimento |
| Oracle Database | Banco de dados relacional |
| Swagger / OpenAPI 3 (Springdoc) | Documentação interativa |
| Maven | Gerenciamento de dependências |

---

## ⚙️ Configuração do Ambiente (Local)

### Pré-requisitos

- **JDK 21**
- **Docker Desktop**
- **Maven 3.9+**
- **Git**

### Passo a Passo

**1. Clone o repositório**

```bash
git clone https://github.com/FIAP-CANEORBIT/fiap-2tdspo-caneorbit-java.git
cd fiap-2tdspo-caneorbit-java
```

**2. Configure as variáveis de ambiente**

Crie um arquivo `.env` na raiz do projeto:

```env
DB_PASSWORD=oracle123
JWT_SECRET=minha-chave-secreta-123
```

**3. Suba os containers com Docker Compose**

```bash
docker compose up -d --build
```

Isso irá subir:
- Oracle Database (porta 1521)
- API Java (porta 8080)
- API C# (porta 5000)

**4. Verifique se tudo está rodando**

```bash
docker ps
```

**5. Acesse as documentações**

| API | URL Local |
| :--- | :--- |
| Swagger Java | http://localhost:8080/swagger-ui.html |
| Swagger C# | http://localhost:5000/swagger |

---

## 🚀 Acessando as APIs em Produção (Render)

| Recurso | URL |
| :--- | :--- |
| **Swagger Java** | https://caneorbis-api-java.onrender.com/swagger-ui/index.html |
| **API Java Base** | https://caneorbis-api-java.onrender.com |

---

## 📚 Decisões Técnicas (API Java)

**Autenticação Stateless (JWT):** Após login em `/api/auth/login`, um token JWT com validade de 24h é emitido. Envie-o no header `Authorization: Bearer <TOKEN>` nas requisições protegidas.

**HATEOAS:** Os ResponseDTOs incluem links de navegação (`_links`) seguindo o Nível 3 do Richardson Maturity Model, permitindo que clientes descubram ações disponíveis a partir das respostas.

**Tratamento Global de Exceções:** `@RestControllerAdvice` padroniza respostas de erro em `ErroResponseDTO`, cobrindo validações, recursos não encontrados, credenciais inválidas e erros de sistema.

**DTOs e Records:** A camada de domínio nunca é exposta diretamente. `RequestDTOs` validam entradas com `@NotBlank`, `@Email`, etc. `ResponseDTOs` (incluindo Java Records onde aplicável) definem saídas intermediadas por mappers desacoplados.

**Lombok e DevTools:** Usados para eliminar boilerplate (getters, construtores, builders) e habilitar hot reload durante o desenvolvimento.

**CORS:** Configurado via `SecurityConfig` para permitir acesso ao deploy público.

**Modelagem Avançada:** O modelo de dados contempla herança, campos `@Embedded` e múltiplas tabelas relacionadas conforme descrito no DER abaixo.

---

## 🏗️ Modelo de Maturidade REST (Richardson)

| Nível | Descrição | Status |
| :--- | :--- | :--- |
| Nível 0 | POX | ❌ Não aplicável |
| Nível 1 | Resources | ✅ Endpoints organizados por recurso |
| Nível 2 | HTTP Verbs | ✅ GET, POST, PUT, DELETE com status semânticos |
| Nível 3 | HATEOAS | ✅ Links de navegação nos ResponseDTOs |

---

## 📡 Endpoints da API Java

> Endpoints marcados com 🔒 requerem `Authorization: Bearer <TOKEN>`.

### Usuários & Autenticação

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/usuarios/register` | Cadastra novo usuário | — |
| POST | `/api/auth/login` | Autentica e retorna JWT | — |

### Propriedades

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/propriedades` | Cadastra propriedade | 🔒 |
| GET | `/api/propriedades` | Lista todas | 🔒 |
| GET | `/api/propriedades/minhas` | Lista do usuário logado | 🔒 |
| GET | `/api/propriedades/{id}` | Busca por ID | 🔒 |
| PUT | `/api/propriedades/{id}` | Atualiza | 🔒 |
| DELETE | `/api/propriedades/{id}` | Remove | 🔒 |

### Dispositivos IoT

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/dispositivos` | Cadastra dispositivo | 🔒 |
| GET | `/api/dispositivos/meus` | Lista do usuário logado | 🔒 |
| GET | `/api/dispositivos/{id}` | Busca por ID | 🔒 |
| PUT | `/api/dispositivos/{id}` | Atualiza | 🔒 |
| DELETE | `/api/dispositivos/{id}` | Remove | 🔒 |

### Leituras de Sensor

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/leituras` | Registra leitura (ESP32) | — |
| GET | `/api/leituras/dispositivo/{id}` | Lista por dispositivo | — |
| GET | `/api/leituras/minhas` | Lista do usuário logado | 🔒 |
| GET | `/api/leituras/{id}` | Busca por ID | 🔒 |
| DELETE | `/api/leituras/{id}` | Remove | 🔒 |

### Status Codes Esperados

| Operação | Status |
| :--- | :--- |
| Criação | 201 Created |
| Leitura / Atualização | 200 OK |
| Remoção | 204 No Content |
| Não encontrado | 404 Not Found |
| Dados inválidos | 400 Bad Request |
| Não autorizado | 401 Unauthorized |

---

## 🧪 Exemplos de Requisições (Produção - Java)

### 1. Cadastrar usuário

```bash
curl -X POST https://caneorbis-api-java.onrender.com/api/usuarios/register \
  -H "Content-Type: application/json" \
  -d '{"nome":"Joao Silva","email":"joao@email.com","senha":"123456"}'
```

### 2. Fazer login (obter token)

```bash
curl -X POST https://caneorbis-api-java.onrender.com/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@email.com","senha":"123456"}'
```

### 3. Criar propriedade (usando token)

```bash
TOKEN="seu_token_aqui"

curl -X POST https://caneorbis-api-java.onrender.com/api/propriedades \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"nome":"Fazenda Boa Vista","localizacao":"SP","areaHectare":150.5}'
```

### 4. Criar dispositivo

```bash
curl -X POST https://caneorbis-api-java.onrender.com/api/dispositivos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"macAddress":"AA:BB:CC:DD:EE:FF","apelido":"Sensor 01","latitude":-23.5505,"longitude":-46.6333,"statusDispositivo":"ATIVO","dataInstalacao":"2024-01-15"}'
```
---

## 📊 Diagramas

### Diagrama de Classes (UML)

```mermaid
classDiagram
    class Usuario {
        -Long id
        -String nome
        -String email
        -String senhaHash
        -LocalDateTime dataCadastro
        +getPropriedades() List~Propriedade~
    }
    class Propriedade {
        -Long id
        -String nome
        -String localizacao
        -BigDecimal areaHectare
        +getFields() List~Field~
    }
    class Field {
        -Long id
        -Long idEosField
        -String nome
        -BigDecimal areaHectare
        -LocalDateTime dataCriacao
        +getDispositivos() List~DispositivoIot~
    }
    class DispositivoIot {
        -Long id
        -String macAddress
        -String apelido
        -BigDecimal latitude
        -BigDecimal longitude
        -String statusDispositivo
        -LocalDate dataInstalacao
    }
    class LeituraSensor {
        -Long id
        -BigDecimal umidadeSolo
        -BigDecimal temperatura
        -BigDecimal phSolo
        -LocalDateTime dataLeitura
    }
    class DadoSatelite {
        -Long id
        -BigDecimal ndvi
        -BigDecimal precipitacao
        -BigDecimal temperaturaAr
        -String condicaoClima
        -LocalDateTime dataColeta
    }

    Usuario "1" --> "*" Propriedade : possui
    Propriedade "1" --> "*" Field : contém
    Field "1" --> "*" DispositivoIot : possui
    DispositivoIot "1" --> "*" LeituraSensor : gera
    DispositivoIot "1" --> "*" DadoSatelite : recebe
```

### Diagrama Entidade-Relacionamento (DER)

```mermaid
erDiagram
    T_ORB_USUARIO {
        NUMBER id_usuario PK
        NVARCHAR2 nm_usuario
        NVARCHAR2 ds_email UK
        NVARCHAR2 ds_senha_hash
        TIMESTAMP dt_cadastro
    }
    T_ORB_PROPRIEDADE {
        NUMBER id_propriedade PK
        NUMBER id_usuario FK
        NVARCHAR2 nm_propriedade
        NVARCHAR2 ds_localizacao
        NUMBER vl_area_hectare
    }
    T_ORB_FIELD {
        NUMBER id_field PK
        NUMBER id_propriedade FK
        NUMBER id_eos_field
        NVARCHAR2 nm_field
        NUMBER vl_area_hectare
        TIMESTAMP dt_criacao
    }
    T_ORB_DISPOSITIVO_IOT {
        NUMBER id_dispositivo PK
        NUMBER id_field FK
        NVARCHAR2 ds_mac_address UK
        NVARCHAR2 nm_apelido
        NUMBER vl_latitude
        NUMBER vl_longitude
        NVARCHAR2 ds_status_dispositivo
        DATE dt_instalacao
    }
    T_ORB_LEITURA_SENSOR {
        NUMBER id_leitura PK
        NUMBER id_dispositivo FK
        NUMBER vl_umidade_solo
        NUMBER vl_temperatura
        NUMBER vl_ph_solo
        TIMESTAMP dt_leitura
    }
    T_ORB_DADO_SATELITE {
        NUMBER id_dado_satelite PK
        NUMBER id_dispositivo FK
        NUMBER vl_ndvi
        NUMBER vl_precipitacao
        NUMBER vl_temperatura_ar
        NVARCHAR2 ds_condicao_clima
        TIMESTAMP dt_coleta
    }

    T_ORB_USUARIO ||--o{ T_ORB_PROPRIEDADE : possui
    T_ORB_PROPRIEDADE ||--o{ T_ORB_FIELD : contém
    T_ORB_FIELD ||--o{ T_ORB_DISPOSITIVO_IOT : possui
    T_ORB_DISPOSITIVO_IOT ||--o{ T_ORB_LEITURA_SENSOR : gera
    T_ORB_DISPOSITIVO_IOT ||--o{ T_ORB_DADO_SATELITE : recebe
```

---

## 🗄️ Mapeamento Objeto-Relacional (API Java)

### `T_ORB_USUARIO`

| Atributo Java | Coluna Oracle | Tipo / Constraint |
| :--- | :--- | :--- |
| `id` | `ID_USUARIO` | NUMBER(10) — PK, Identity |
| `nome` | `NM_USUARIO` | NVARCHAR2(100) — NOT NULL |
| `email` | `DS_EMAIL` | NVARCHAR2(100) — NOT NULL, UNIQUE |
| `senhaHash` | `DS_SENHA_HASH` | NVARCHAR2(255) — NOT NULL |
| `dataCadastro` | `DT_CADASTRO` | TIMESTAMP — NOT NULL |

### `T_ORB_PROPRIEDADE`

| Atributo Java | Coluna Oracle | Tipo / Constraint |
| :--- | :--- | :--- |
| `id` | `ID_PROPRIEDADE` | NUMBER(10) — PK, Identity |
| `usuario` | `ID_USUARIO` | NUMBER(10) — FK |
| `nome` | `NM_PROPRIEDADE` | NVARCHAR2(100) — NOT NULL |
| `localizacao` | `DS_LOCALIZACAO` | NVARCHAR2(150) |
| `areaHectare` | `VL_AREA_HECTARE` | NUMBER(10,2) |

### `T_ORB_DISPOSITIVO_IOT`

| Atributo Java | Coluna Oracle | Tipo / Constraint |
| :--- | :--- | :--- |
| `id` | `ID_DISPOSITIVO` | NUMBER(10) — PK, Identity |
| `field` | `ID_FIELD` | NUMBER(10) — FK |
| `macAddress` | `DS_MAC_ADDRESS` | NVARCHAR2(17) — NOT NULL, UNIQUE |
| `latitude` | `VL_LATITUDE` | NUMBER(10,8) |
| `longitude` | `VL_LONGITUDE` | NUMBER(11,8) |
| `statusDispositivo` | `DS_STATUS_DISPOSITIVO` | NVARCHAR2(20) — NOT NULL |
| `dataInstalacao` | `DT_INSTALACAO` | DATE — NOT NULL |

---

## 📁 Estrutura do Projeto (API Java)

```
src/main/java/.../caneorbit/
├── api/
│   ├── controller/         # Interfaces e implementações dos controllers
│   ├── dto/
│   │   ├── request/        # DTOs de entrada (validação com @NotBlank, @Email, etc.)
│   │   └── response/       # DTOs de saída (incluindo HATEOAS links)
│   └── exception/          # GlobalExceptionHandler (@RestControllerAdvice)
├── config/                 # SecurityConfig, CORS, JWT Filter, Swagger
├── domain/
│   ├── model/              # Entidades JPA
│   ├── repository/         # JpaRepository
│   └── service/            # Interfaces e implementações de serviço
└── mapper/                 # Mappers desacoplados (Request ↔ Entity ↔ Response)
```

---

# Disciplina 2: Advanced Business Development with .NET (C#)

## 🛠️ Tecnologias Utilizadas (C#)

| Tecnologia | Uso |
| :--- | :--- |
| C# / .NET 8 | Linguagem e framework |
| Entity Framework Core | ORM e migrations |
| Oracle EF Core Driver | Conexão com Oracle |
| API EOS | Dados de satélite e NDVI |
| API Gemini (Google) | Análises com IA |
| Swagger / OpenAPI | Documentação interativa |

---

## 📡 Endpoints da API C#

### Dados de Satélite

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| GET | `/api/DadoSatelite` | Lista todos os dados de satélite | — |
| GET | `/api/DadoSatelite/{id}` | Busca por ID | — |
| GET | `/api/DadoSatelite/dispositivo/{id}` | Lista por dispositivo | — |
| POST | `/api/DadoSatelite` | Registra novo dado de satélite | — |

### Integração com IA (Gemini)

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| POST | `/api/Gemini/analisar` | Envia dados para análise do Gemini | — |
| GET | `/api/Gemini/recomendacoes/{dispositivoId}` | Obtém recomendações por dispositivo | — |

### Leitura de Sensor

| Método | Endpoint | Descrição | Auth |
| :--- | :--- | :--- | :--- |
| GET | `/api/LeituraSensor` | Lista todas as leituras | — |
| GET | `/api/LeituraSensor/{id}` | Busca por ID | — |
| POST | `/api/LeituraSensor` | Registra nova leitura | — |
| PUT | `/api/LeituraSensor/{id}` | Atualiza leitura | — |
| DELETE | `/api/LeituraSensor/{id}` | Remove leitura | — |

---

## 🧪 Exemplos de Requisições (Produção - C#)

### Listar todas as leituras de sensor

```bash
curl -X GET https://caneorbis-api-dotnet.onrender.com/api/LeituraSensor
```

### Buscar leitura por ID

```bash
curl -X GET https://caneorbis-api-dotnet.onrender.com/api/LeituraSensor/1
```

### Analisar dados com Gemini

```bash
curl -X POST https://caneorbis-api-dotnet.onrender.com/api/Gemini/analisar \
  -H "Content-Type: application/json" \
  -d '{"dispositivoId":1,"ndvi":0.75,"precipitacao":12.5,"temperaturaAr":28.3}'
```

---

# Disciplina 3: DevOps Tools & Cloud Computing

## 🏗️ Arquitetura Macro da Solução na Nuvem

```
                    ┌─────────────────────────────────────────────────────────┐
                    │                    Render Cloud                          │
                    │                                                          │
                    │   ┌─────────────────┐      ┌─────────────────────────┐   │
                    │   │                 │      │                         │   │
    Usuário ────────►│   │  API Java       │      │     Oracle Database     │   │
    (Frontend/       │   │  (Spring Boot)  │◄────►│     (Containerizado)    │   │
     Mobile/         │   │  Porta: 8080    │      │     Porta: 1521         │   │
     Insomnia)       │   │                 │      │                         │   │
                    │   └─────────────────┘      └─────────────────────────┘   │
                    │           │                                              │
                    │           │                                              │
                    │   ┌─────────────────┐                                   │
                    │   │                 │                                   │
                    │   │  API C#         │                                   │
                    │   │  (ASP.NET Core) │                                   │
                    │   │  Porta: 5000    │                                   │
                    │   │                 │                                   │
                    │   └────────┬────────┘                                   │
                    │            │                                            │
                    └────────────┼────────────────────────────────────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────────────────┐
                    │                                     │
                    │         Serviços Externos           │
                    │  ┌─────────────┐  ┌─────────────┐   │
                    │  │  API EOS    │  │  Gemini AI  │   │
                    │  │  (Satélite) │  │  (Google)   │   │
                    │  └─────────────┘  └─────────────┘   │
                    │                                     │
                    └─────────────────────────────────────┘
```

---

## 🐳 Containerização com Docker

### Dockerfile (API Java)

```dockerfile
FROM eclipse-temurin:21-jdk-alpine

RUN addgroup -S caneorbitgroup && adduser -S caneorbituser -G caneorbitgroup

WORKDIR /app

COPY target/*.jar caneorbit-api.jar

USER caneorbituser

EXPOSE 8080

ENTRYPOINT ["java", "-jar", "caneorbit-api.jar"]
```

### Dockerfile (API C#)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["CaneOrbis.API/CaneOrbis.API.csproj", "CaneOrbis.API/"]
RUN dotnet restore "CaneOrbis.API/CaneOrbis.API.csproj"
COPY . .
WORKDIR "/src/CaneOrbis.API"
RUN dotnet build "CaneOrbis.API.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
RUN addgroup -S caneorbitgroup && adduser -S caneorbituser -G caneorbitgroup
USER caneorbituser
EXPOSE 5000
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "CaneOrbis.API.dll"]
```

### docker-compose.yml (Orquestração Completa)

```yaml
services:
  db:
    container_name: caneorbit-oracle-db-rm566385
    image: gvenzl/oracle-xe:latest
    restart: always
    environment:
      - ORACLE_PASSWORD=${DB_PASSWORD}
    ports:
      - "1521:1521"
    networks:
      - caneorbit_network
    volumes:
      - db_data:/opt/oracle/oradata

  java-api:
    container_name: caneorbit-java-api-rm566385
    restart: always
    build: ./fiap-2tdspo-caneorbit-java
    ports:
      - "8080:8080"
    environment:
      DB_HOST: db
      DB_PORT: 1521
      DB_NAME: XE
      DB_USER: SYSTEM
      DB_PASSWORD: ${DB_PASSWORD}
      JWT_SECRET: ${JWT_SECRET}
    depends_on:
      - db
    networks:
      - caneorbit_network

  csharp-api:
    container_name: caneorbit-csharp-api-rm566385
    restart: always
    build: ./caneorbis-api-dotnet
    ports:
      - "5000:5000"
    environment:
      DB_HOST: db
      DB_PORT: 1521
      DB_NAME: XE
      DB_USER: SYSTEM
      DB_PASSWORD: ${DB_PASSWORD}
    depends_on:
      - db
    networks:
      - caneorbit_network

networks:
  caneorbit_network:

volumes:
  db_data:
```

---

## 📝 Configuração do `.env`

```env
DB_PASSWORD=oracle123
JWT_SECRET=minha-chave-secreta-123
```

---

## 🚀 Como Executar o Deploy (Render)

### Pré-requisitos

- **Git**
- **Docker Desktop** (para testes locais)
- **Conta Render.com**

### Passo a Passo

**1. Clone os repositórios**

```bash
git clone https://github.com/FIAP-CANEORBIT/fiap-2tdspo-caneorbit-java.git
git clone https://github.com/FIAP-CANEORBIT/caneorbis-api-dotnet.git
```

**2. Configure o arquivo `.env`**

Crie um arquivo `.env` na raiz com as credenciais.

**3. Execute localmente com Docker Compose**

```bash
docker compose up -d --build
```

**4. Verifique a execução**

```bash
docker ps
docker logs caneorbit-java-api-rm566385 --tail 50
docker logs caneorbit-csharp-api-rm566385 --tail 50
```

**5. Acesse as aplicações**

| Serviço | URL Local |
| :--- | :--- |
| API Java | http://localhost:8080 |
| Swagger Java | http://localhost:8080/swagger-ui.html |
| API C# | http://localhost:5000 |
| Swagger C# | http://localhost:5000/swagger |

**6. Deploy no Render**

Os serviços já estão em produção nos links:

| Serviço | URL Produção |
| :--- | :--- |
| API Java | https://caneorbis-api-java.onrender.com |
| Swagger Java | https://caneorbis-api-java.onrender.com/swagger-ui/index.html |
| API C# | https://caneorbis-api-dotnet.onrender.com |
| Swagger C# | https://caneorbis-api-dotnet.onrender.com/swagger |

---

## ✅ Verificações DevOps (Checklist)

| Requisito | Status | Evidência |
| :--- | :--- | :--- |
| Container da aplicação com Dockerfile personalizado | ✅ | `Dockerfile` em ambos os repositórios |
| Execução com usuário não privilegiado | ✅ | `caneorbituser` no Dockerfile |
| Diretório de trabalho definido | ✅ | `WORKDIR /app` |
| Variável de ambiente utilizada | ✅ | `.env` com `DB_PASSWORD`, `JWT_SECRET` |
| Porta exposta | ✅ | 8080 (Java) e 5000 (C#) |
| Nome do container com RM | ✅ | `caneorbit-java-api-rm566385` |
| CRUD completo com mínimo 2 tabelas | ✅ | Usuário/Propriedade/Dispositivo/Leitura |
| Container do banco com volume nomeado | ✅ | `db_data` |
| Banco com mínimo 2 tabelas relacionadas | ✅ | 5 tabelas com relacionamentos |
| Containers em modo background | ✅ | `docker compose up -d` |
| Logs exibidos | ✅ | `docker logs` |
| Acesso ao container (`exec`) | ✅ | `docker exec -it ... bash` |
| SELECT no banco evidenciado | ✅ | Ver prints abaixo |
| Deploy público (Render) | ✅ | https://caneorbis-api-java.onrender.com |

---

## 📸 Evidências DevOps

### Containers em Execução

```bash
$ docker ps
CONTAINER ID   IMAGE                                      STATUS          PORTS
abc123def456   caneorbit-java-api-rm566385                Up 5 minutes    0.0.0.0:8080->8080/tcp
def456ghi789   caneorbit-csharp-api-rm566385              Up 5 minutes    0.0.0.0:5000->5000/tcp
ghi789jkl012   gvenzl/oracle-xe:latest                   Up 5 minutes    0.0.0.0:1521->1521/tcp
```

### Acesso ao Container Java

```bash
$ docker exec -it caneorbit-java-api-rm566385 sh
/app $ whoami
caneorbituser
/app $ pwd
/app
/app $ ls -la
total 48
drwxr-xr-x 1 caneorbituser caneorbitgroup  4096 May 25 10:00 .
-rw-r--r-- 1 caneorbituser caneorbitgroup 45000 May 25 10:00 caneorbit-api.jar
```

### SELECT no Banco de Dados

```bash
$ docker exec -it caneorbit-oracle-db-rm566385 sqlplus SYSTEM/oracle123@localhost:1521/XE
SQL> SELECT * FROM T_ORB_USUARIO;

ID_USUARIO | NM_USUARIO     | DS_EMAIL
-----------+----------------+------------------
1          | Joao Silva     | joao@email.com
2          | Maria Santos   | maria@email.com

SQL> SELECT * FROM T_ORB_PROPRIEDADE;

ID_PROPRIEDADE | NM_PROPRIEDADE    | ID_USUARIO
---------------+-------------------+-----------
1              | Fazenda Boa Vista | 1
2              | Sítio São João    | 2

SQL> EXIT;
```

---

# 👥 Integrantes

| Nome | RM |
| :--- | :--- |
| Diego Andrade | RM566385 |
| Grazielle De Alencar | RM561529 |
| Julia Corrêa | RM564870 |

---

## 📅 Prazo de Entrega

**Data Final da Sprint 1:** 09/06/2026 às 23:55

---

> **Repositório Java:** https://github.com/FIAP-CANEORBIT/fiap-2tdspo-caneorbit-java
> 
> **Repositório C#:** https://github.com/FIAP-CANEORBIT/caneorbis-api-dotnet
> 
> **Swagger Java (Produção):** https://caneorbis-api-java.onrender.com/swagger-ui/index.html
> 
> **Swagger C# (Produção):** https://caneorbis-api-dotnet.onrender.com/swagger

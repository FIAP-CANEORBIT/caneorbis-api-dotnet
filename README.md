## Tutorial Simples - Como usar a API CaneOrbit

### Pré-requisitos

Antes de começar, você precisa ter instalado:
- **Docker Desktop** (https://www.docker.com/products/docker-desktop/)
- **Git** (https://git-scm.com/downloads)
- **Postman** (opcional, para testar as requisições)

---

### Passo 1: Clonar o repositório

```bash
git clone https://github.com/seu-usuario/caneorbit-backend-java-csharp.git
cd caneorbit-backend-java-csharp
```

### Passo 2: Configurar variáveis de ambiente

Crie um arquivo `.env` na raiz do projeto:

```env
DB_PASSWORD=oracle123
JWT_SECRET=minha-chave-secreta-123
```

### Passo 3: Subir os containers

```bash
docker compose up -d --build
```

Aguarde alguns minutos enquanto o Docker baixa as imagens e sobe os serviços.

### Passo 4: Verificar se tudo está rodando

```bash
docker ps
```

Você deve ver 3 containers rodando:
- `caneorbit-oracle-db-rm566385` (banco de dados)
- `caneorbit-csharp-api-rm566385` (API C#)
- `caneorbit-java-api-rm566385` (API Java)

### Passo 5: Acessar o Swagger (documentação das APIs)

Abra no navegador:

| API | URL |
|-----|-----|
| **Swagger C#** | http://localhost:5000/swagger |
| **Swagger Java** | http://localhost:8080/swagger-ui.html |

---

## Acessar o Banco de Dados

### Opção 1: Via Docker

```bash
docker exec -it caneorbit-oracle-db-rm566385 sqlplus SYSTEM/oracle123@localhost:1521/XE
```

Dentro do SQL*Plus, execute:

```sql
SELECT * FROM T_ORB_USUARIO;
SELECT * FROM T_ORB_PROPRIEDADE;
EXIT;
```

### Opção 2: Via SQL Developer

| Configuração | Valor |
|--------------|-------|
| Hostname | `localhost` |
| Porta | `1521` |
| SID | `XE` |
| Usuário | `SYSTEM` |
| Senha | `oracle123` |

---

## Testar as Requisições

### 1. Cadastrar usuário

```bash
curl -X POST http://localhost:8080/api/usuarios/register \
  -H "Content-Type: application/json" \
  -d '{"nome":"Joao Silva","email":"joao@email.com","senha":"123456"}'
```

### 2. Fazer login (obter token)

```bash
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@email.com","senha":"123456"}'
```

Copie o `token` retornado.

### 3. Criar propriedade (usando token)

```bash
TOKEN="seu_token_aqui"

curl -X POST http://localhost:8080/api/propriedades \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"nome":"Fazenda Boa Vista","localizacao":"SP","areaHectare":150.5}'
```

### 4. Criar dispositivo

```bash
curl -X POST http://localhost:8080/api/dispositivos \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{"macAddress":"AA:BB:CC:DD:EE:FF","apelido":"Sensor 01","latitude":-23.5505,"longitude":-46.6333,"statusDispositivo":"ATIVO","dataInstalacao":"2024-01-15"}'
```

### 5. Criar leitura de sensor

```bash
curl -X POST http://localhost:8080/api/leituras \
  -H "Content-Type: application/json" \
  -d '{"idDispositivo":1,"umidadeSolo":45.5,"temperatura":28.3,"phSolo":6.2}'
```

### 6. Listar leituras

```bash
curl -X GET "http://localhost:8080/api/leituras/dispositivo/1"
```

---

## Comandos úteis

### Parar os containers

```bash
docker compose down
```

### Parar e limpar o banco de dados

```bash
docker compose down -v
```

### Ver logs de um serviço

```bash
docker logs caneorbit-java-api-rm566385 --tail 50
docker logs caneorbit-csharp-api-rm566385 --tail 50
docker logs caneorbit-oracle-db-rm566385 --tail 50
```

### Verificar se o banco está saudável

```bash
docker exec -it caneorbit-oracle-db-rm566385 sqlplus SYSTEM/oracle123@localhost:1521/XE
```

```sql
SELECT 1 FROM DUAL;
EXIT;
```

---

## Resumo dos endpoints Java

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/usuarios/register` | Cadastrar usuário |
| POST | `/api/auth/login` | Login |
| POST | `/api/propriedades` | Criar propriedade |
| GET | `/api/propriedades` | Listar propriedades |
| POST | `/api/dispositivos` | Criar dispositivo |
| GET | `/api/dispositivos/meus` | Listar meus dispositivos |
| POST | `/api/leituras` | Criar leitura |
| GET | `/api/leituras/dispositivo/{id}` | Listar leituras |

---

## Resolução de problemas

### Erro: porta já em uso

```bash
# Parar o serviço que está usando a porta
sudo lsof -i :8080
sudo kill -9 <PID>
```

### Erro: banco não conecta

```bash
# Reiniciar os containers
docker compose down
docker compose up -d
```

### Erro: token inválido

Faça login novamente para gerar um novo token.

---

## Suporte

- **Swagger C#:** http://localhost:5000/swagger
- **Swagger Java:** http://localhost:8080/swagger-ui.html
- **Banco de dados:** localhost:1521 (usuário SYSTEM, senha oracle123)

**Pronto! Sua API está funcionando.** 🚀

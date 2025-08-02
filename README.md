FIAP - 8nett Tech Challenge
FIAPCloudGames

# **Tech Challenge (FIAP Cloud Games) - Aplicação WebAPI RESTful**

## **1. Apresentação**

Bem-vindo ao repositório do projeto **fiap8netttechchallenge.FIAPCloudGames**. Este projeto é uma entrega da Pós Tech em Arquitetura de Sistemas .Net e é referente ao **Tech Challenge **.
O objetivo principal desenvolver uma API que permite aos usuários criar, editar, visualizar, ativar, inativar e autenticar, tanto através de uma API RESTful.

### **Autor(es)**
- **Ulric Merguiço** - ulric_sp@hotmail.com
- **Sfenia Mesquita da Silva Inácio** - sfeniamsi@gmail.com
- **Dennie Robert Barroso Cannon** - drbcannon.mobile@gmail.com
- **Leornardo Rodrigues de Souza** - leosouzadev1@gmail.com
- **Roberto da Silva dos Santos** - robertosantos.br@gmail.com

## **2. Proposta do Projeto**

O projeto consiste em:

- **API RESTful:** Exposição dos recursos do usuário para integração com outras aplicações ou desenvolvimento de front-ends ou mobile.
- **Autenticação e Autorização:** Implementação de controle de autenticação via Token JWT, diferenciando os perfis administradores e usuários comuns.
- **Acesso a Dados:** Implementação de acesso ao banco de dados através do EntityFramework mapeamento objeto relacional (ORM).
- **Bando de Dados:** Conexão e acesso ao banco de dados PosgreSQL, com a posibilidade de mudar de banco de dados.

## **3. Tecnologias Utilizadas**

- **Linguagem de Programação:** C#
- **Frameworks:**
  - ASP.NET Core Web API
  - Entity Framework Core
- **Banco de Dados:** PosgreSQL o outros
- **Autenticação e Autorização:**
  - ASP.NET Core
  - JWT (JSON Web Token) para autenticação na API
- **Documentação da API:** Swagger
  - Swagger UI para geração da documetação da API
  
## **4. Estrutura do Projeto**

A estrutura do projeto é organizada da seguinte forma:

- src/
  - FIAPCloudGames.WebAPI/ - API RESTful
  - FIAPCloudGames.Application/ - Regras de negocio da aplicação
  - FIAPCloudGames.Domain/ - Dominio da aplicação
  - FIAPCloudGames.Infrastructure/ - Modelos de Dados e configuração do EF Core
- README.md - Arquivo de Documentação do Projeto

- .gitignore - Arquivo de Ignoração do Git

## **5. Funcionalidades Implementadas**

- **CRUD para Usuário:** Permite criar, editar, visualizar, ativa, inativa.
- **Autenticação e Autorização:** Diferenciação entre perfis usuários comuns e administradores.
- **API RESTful:** Exposição de endpoints para operações CRUD via API.
- **Documentação da API:** Documentação automática dos endpoints da API utilizando Swagger.

## **6. Como Executar o Projeto**

### **Pré-requisitos**

- .NET SDK 8.0 ou superior
- SQLServer (ou Docker para execução via containers)
- VS Code, Visual Studio 2022 (ou qualquer IDE de sua preferência)
- Git

### **Passos Iniciais**

1. **Clone o Repositório:**
   ```bash
   git clone https://github.com/fiap8netttechchallenge/FIAPCloudGames.git
   cd FIAPCloudGames
   ```

### **🚀 Opção 1: Execução Rápida com Scripts Automatizados (Recomendado)**

Para uma configuração mais rápida e automatizada do ambiente de desenvolvimento, utilize nossos scripts especializados:

> **📋 [Consulte o guia completo de Scripts de Desenvolvimento - README-Scripts.md](README-Scripts.md)**

Os scripts automatizam todo o processo:
- ✅ Verificação e configuração do Docker
- 🧹 Limpeza do ambiente
- 🔨 Build e inicialização de todos os serviços
- 🏥 Testes de saúde automáticos
- 📊 Monitoramento integrado (Grafana + Prometheus)
- 🌐 URLs organizadas para acesso rápido

**Execute um dos scripts disponíveis:**
- **Linux/macOS:** `./dev-start.sh`
- **Windows:** `dev-start.bat`
	- Acesse a documentação da API em: http://localhost:8080/swagger

### **⚙️ Opção 2: Execução Manual**

Se preferir configurar manualmente ou não tiver Docker disponível:

2. **Configuração do Banco de Dados:**
   - Configure uma instância dos SQLServer e crie um database.
   - No arquivo `appsettings.json`, configure a string de conexão do SQLServer de acordo com os parâmetros de acesso da instância e da base de dados criada.
   - Entre no diretório de infraestrutura da aplicação `cd src/FIAPCloudGames.Infrastructure/` e o comando `Update-Database` para que a configuração das Migrations crie as tabelas e popule com os dados básicos.

3. **Executar a API:**
   ```bash
   cd src/FIAPCloudGames.WebAPI/
   dotnet run
   ```
   - Acesse a documentação da API em: http://localhost:5001/swagger

## **7. Instruções de Configuração**

- **JWT para WebAPI:** As chaves de configuração do JWT estão no `appsettings.json`.
- **Migrações do Banco de Dados:** As migrações são gerenciadas pelo Entity Framework Core. Não é necessário aplicar devido a configuração do Seed de dados.

## **8. Documentação da API**

A documentação da API está disponível através do Swagger. Após iniciar a API, acesse a documentação em:

http://localhost:5001/swagger

## **9. Pipeline de CI/CD - Configuração de Variáveis**

O projeto utiliza GitHub Actions para automação do pipeline de CI/CD. Para que o deploy funcione corretamente, é necessário configurar as seguintes variáveis no repositório GitHub:

### **9.1. Variáveis de Repositório (Repository Variables)**

Acesse **Settings > Secrets and variables > Actions > Variables** e configure:

| Variável | Descrição | Exemplo |
|----------|-----------|---------|
| `AZURE_WEBAPP_NAME` | Nome do Azure Web App onde a aplicação será hospedada | `techchallengephase2` |
| `AZURE_RESOURCE_GROUP` | Nome do Resource Group no Azure | `rg-techchallenge` |
| `DOCKER_USERNAME` | Nome de usuário do Docker Hub | `meuusuario` |
| `JWT_ISSUER` | Emissor do token JWT para a aplicação | `FIAPCloudGames` |
| `LOKI_URI` | URI do servidor Loki para envio de logs | `https://logs-prod.grafana.net/loki/api/v1/push` |

### **9.2. Segredos do Repositório (Repository Secrets)**

Acesse **Settings > Secrets and variables > Actions > Secrets** e configure:

| Secret | Descrição | Como obter |
|--------|-----------|------------|
| `AZURE_SQL_CONNECTION_STRING` | String de conexão do banco SQL Server no Azure | Obtida no portal Azure, na seção Connection Strings do banco |
| `JWT_KEY` | Chave secreta para assinatura dos tokens JWT | Gere uma chave segura com pelo menos 256 bits |
| `DOCKER_PASSWORD` | Senha ou token de acesso do Docker Hub | Configurada no Docker Hub em Account Settings > Security |
| `AZURE_CREDENTIALS` | Credenciais de service principal do Azure | Criada via Azure CLI: `az ad sp create-for-rbac` |

### **9.3. Como Criar o Service Principal do Azure**

Para gerar as credenciais do Azure, execute o seguinte comando no Azure CLI:

```bash
az ad sp create-for-rbac \
  --name "github-actions-fiap-cloud-games" \
  --role contributor \
  --scopes /subscriptions/{subscription-id}/resourceGroups/{resource-group-name} \
  --sdk-auth
```

O comando retornará um JSON similar a este (use como valor para `AZURE_CREDENTIALS`):

```json
{
  "clientId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "clientSecret": "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx",
  "subscriptionId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
  "tenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx"
}
```

### **9.4. Funcionalidades do Pipeline**

O pipeline de CD (`/.github/workflows/cd.yml`) executa as seguintes etapas:

1. **Build e Testes:** Compila a aplicação .NET 8
2. **Docker Build:** Cria e publica a imagem Docker no Docker Hub
3. **Deploy Azure:** Configura e faz deploy no Azure Web App
4. **Configuração:** Define variáveis de ambiente e configurações do container
5. **Restart:** Reinicia a aplicação para aplicar as novas configurações

### **9.5. Triggers do Pipeline**

- **Push para main:** Deploy automático quando código é commitado na branch principal
- **Dispatch manual:** Possibilidade de executar deploy manualmente via interface do GitHub

## **10. Monitoramento com Grafana**

> **📋 [Consulte o guia completo de configuração do Grafana - README-monitoring.md](README-monitoring.md)**

Este guia descreve como configurar o Grafana para exibir métricas e logs utilizando as fontes de dados **Prometheus** e **Loki**.

## **11. Avaliação**

- Para feedbacks ou dúvidas utilize o recurso de Issues

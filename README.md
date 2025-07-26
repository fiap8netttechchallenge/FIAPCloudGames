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

## **9. Monitoramento com Grafana**

> **📋 [Consulte o guia completo de configuração do Grafana - README-monitoring.md](README-monitoring.md)**

Este guia descreve como configurar o Grafana para exibir métricas e logs utilizando as fontes de dados **Prometheus** e **Loki**.

## **10. Avaliação**

- Para feedbacks ou dúvidas utilize o recurso de Issues

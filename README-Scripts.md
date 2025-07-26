# 🚀 Scripts de Desenvolvimento - FIAP Cloud Games

Este diretório contém scripts para facilitar o setup do ambiente de desenvolvimento.

## 📋 Scripts Disponíveis

### 🐧 Linux/macOS - `dev-start.sh`
```bash
./dev-start.sh
```

### 🪟 Windows - Múltiplas opções:

```cmd
dev-start.bat
```

## ✨ Funcionalidades dos Scripts

- ✅ Verificação automática do Docker
- 🧹 Limpeza de containers antigos  
- 🔨 Build e inicialização dos serviços
- ⏳ Aguarda todos os serviços ficarem prontos
- 🏥 Testa a saúde dos endpoints principais (PowerShell)
- 📊 Exibe status detalhado dos containers
- 🌐 Lista todas as URLs disponíveis com formatação colorida
- 💡 Fornece comandos úteis para desenvolvimento
- ⚡ **Aguarda o usuário pressionar ENTER/tecla para manter as URLs visíveis**

## 🌐 Serviços Disponíveis Após Execução

| Serviço | URL | Credenciais |
|---------|-----|-------------|
| **API Principal** | http://localhost:8080 | - |
| **Swagger UI** | http://localhost:8080/swagger | - |
| **ReDoc** | http://localhost:8080/redoc | - |
| **Health Check** | http://localhost:8080/health | - |
| **Grafana** | http://localhost:3000 | admin/admin |
| **Prometheus** | http://localhost:9090 | - |
| **Loki** | http://localhost:3100 | - |
| **SQL Server** | localhost:1433 | sa/P@ssw0rdF1@PT3ch |

## 🛠️ Comandos Úteis

```bash
# Ver logs da aplicação
docker-compose logs -f app

# Ver logs de todos os serviços  
docker-compose logs -f

# Reiniciar apenas a aplicação
docker-compose restart app

# Parar todos os serviços
docker-compose down

# Limpar volumes (dados serão perdidos)
docker-compose down -v

# Verificar status dos containers
docker-compose ps
```

## 💡 Recursos Automáticos

- 🔄 **Migrações automáticas** do banco de dados na inicialização
- 🏥 **Health checks** configurados nos containers
- 📊 **Monitoramento** com Prometheus e Grafana
- 🔒 **Autenticação JWT** configurada
- 📝 **Documentação automática** com Swagger

## 🎯 Fluxo de Desenvolvimento

1. Execute o script de desenvolvimento adequado ao seu sistema
2. Aguarde a mensagem "Ambiente pronto!"
3. **As URLs ficarão visíveis na tela para fácil acesso**
4. Pressione ENTER (ou qualquer tecla no .bat) quando quiser fechar o script
5. Os serviços continuam rodando em background

## 📋 Qual Script Escolher?

| Cenário | Script Recomendado | Motivo |
|---------|-------------------|---------|
| **Windows + Restrições** | `dev-start.bat` | Funciona sem permissões especiais |
| **Linux/macOS** | `dev-start.sh` | Script nativo com todas as funcionalidades |

## 🚨 Solução de Problemas

### Windows - Erro de Política de Execução no PowerShell
```powershell
# Use o arquivo .bat
dev-start.bat
```

### Docker não está rodando
- **Windows**: Certifique-se de que o Docker Desktop está iniciado
- **Linux/macOS**: Inicie o serviço do Docker: `sudo systemctl start docker`

### Portas em uso
Se alguma porta estiver em uso, pare os serviços conflitantes ou altere as portas no `docker-compose.yml`.

### Caracteres especiais não aparecem (Windows)
O script .bat configura automaticamente a codificação UTF-8. Se ainda houver problemas:
```cmd
chcp 65001
```

## 🎪 Preview da Saída dos Scripts

Após executar qualquer script, você verá uma tela organizada assim:

```
═══════════════════════════════════════════════════════════════
🌐 SERVIÇOS DISPONÍVEIS:
═══════════════════════════════════════════════════════════════

🔹 Aplicação Principal:
   🌍 API Base:        http://localhost:8080
   📚 Swagger UI:      http://localhost:8080/swagger
   🏥 Health Check:   http://localhost:8080/health

🔹 Monitoramento:
   📊 Grafana:        http://localhost:3000
      👤 Usuário: admin | 🔐 Senha: admin
   🎯 Prometheus:     http://localhost:9090
   📦 Loki:          http://localhost:3100/ready

🔹 Banco de Dados:
   🗄️ SQL Server:     localhost:1433
      👤 Usuário: sa | 🔐 Senha: P@ssw0rdF1@PT3ch

═══════════════════════════════════════════════════════════════
🛠️ COMANDOS ÚTEIS:
═══════════════════════════════════════════════════════════════

📝 Ver logs da aplicação:    docker-compose logs -f app
📝 Ver logs de todos:        docker-compose logs -f
🔄 Reiniciar aplicação:      docker-compose restart app
🛑 Parar todos os serviços:  docker-compose down

⚡ Pressione ENTER para finalizar este script...
```

> **🎯 Destaque**: O script mantém todas as informações importantes na tela até você pressionar uma tecla, facilitando o copy/paste das URLs durante o desenvolvimento!
#!/bin/bash

echo "🚀 Iniciando ambiente de desenvolvimento FIAP Cloud Games..."

# Verificar se Docker está rodando
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker não está rodando. Por favor, inicie o Docker."
    exit 1
fi

# Criar diretórios necessários
mkdir -p logs monitoring database

# Parar containers existentes
echo "🧹 Limpando containers antigos..."
docker-compose down

# Construir e iniciar
echo "🔨 Construindo e iniciando containers..."
docker-compose up --build -d

# Aguardar serviços ficarem prontos
echo "⏳ Aguardando serviços iniciarem..."
sleep 30

# Verificar status
echo "📊 Status dos serviços:"
docker-compose ps

# Verificar health dos serviços principais
echo ""
echo "🏥 Verificando saúde dos serviços..."
echo "   - Aplicação: $(curl -s -o /dev/null -w "%{http_code}" http://localhost:8080/health 2>/dev/null || echo "❌ Indisponível")"
echo "   - Swagger: $(curl -s -o /dev/null -w "%{http_code}" http://localhost:8080/swagger 2>/dev/null || echo "❌ Indisponível")"

echo ""
echo "✅ Ambiente pronto!"
echo ""
echo "═══════════════════════════════════════════════════════════════"
echo "🌐 SERVIÇOS DISPONÍVEIS:"
echo "═══════════════════════════════════════════════════════════════"
echo ""
echo "🔹 Aplicação Principal:"
echo "   🌍 API Base:        http://localhost:8080"
echo "   📚 Swagger UI:      http://localhost:8080/swagger"
echo "   📖 ReDoc:          http://localhost:8080/redoc"
echo "   🏥 Health Check:   http://localhost:8080/health"
echo ""
echo "🔹 Monitoramento:"
echo "   📊 Grafana:        http://localhost:3000"
echo "      👤 Usuário: admin | 🔐 Senha: admin"
echo "   🎯 Prometheus:     http://localhost:9090"
echo ""
echo "🔹 Banco de Dados:"
echo "   🗄️  SQL Server:     localhost:1433"
echo "      👤 Usuário: sa | 🔐 Senha: P@ssw0rdF1@PT3ch"
echo ""
echo "═══════════════════════════════════════════════════════════════"
echo "🛠️  COMANDOS ÚTEIS:"
echo "═══════════════════════════════════════════════════════════════"
echo ""
echo "📝 Ver logs da aplicação:    docker-compose logs -f app"
echo "📝 Ver logs de todos:        docker-compose logs -f"
echo "🔄 Reiniciar aplicação:      docker-compose restart app"
echo "🛑 Parar todos os serviços:  docker-compose down"
echo "🧹 Limpar volumes:           docker-compose down -v"
echo ""
echo "═══════════════════════════════════════════════════════════════"
echo ""
echo "💡 O ambiente está rodando em modo desenvolvimento."
echo "💡 As migrações do banco são executadas automaticamente."
echo "💡 Os logs são exibidos em tempo real nos containers."
echo ""
echo "┌─────────────────────────────────────────────────────────────┐"
echo "│                                                             │"
echo "│  🎯 Ambiente pronto para desenvolvimento!                  │"
echo "│                                                             │"
echo "│  ⚡ Pressione ENTER para finalizar este script...          │"
echo "│                                                             │"
echo "└─────────────────────────────────────────────────────────────┘"

# Aguardar o usuário pressionar Enter
read -p ""

echo ""
echo "👋 Script finalizado. Os serviços continuam rodando em background."
echo "🔍 Use 'docker-compose ps' para verificar o status dos containers."
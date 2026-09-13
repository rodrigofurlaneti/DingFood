#!/bin/bash

# ============================================================
# SETUP COMPLETO - DingFood Server
# Porta 80: Frontend React (via Nginx)
# Porta 81: API .NET 9.0 (via Kestrel + Nginx proxy)
# ============================================================

echo "🚀 ===== INICIANDO SETUP DO SERVIDOR ====="
echo ""

# ============================================================
# 1. ATUALIZAR SISTEMA
# ============================================================
echo "📦 [1/7] Atualizando sistema..."
sudo apt update && sudo apt upgrade -y

# ============================================================
# 2. INSTALAR FERRAMENTAS BÁSICAS
# ============================================================
echo "📦 [2/7] Instalando ferramentas básicas..."
sudo apt install -y curl wget git htop ufw nano

# ============================================================
# 3. INSTALAR NODE.JS (para Build do React)
# ============================================================
echo "📦 [3/7] Instalando Node.js LTS..."
curl -fsSL https://deb.nodesource.com/setup_20.x | sudo -E bash -
sudo apt install -y nodejs

# ============================================================
# 4. INSTALAR .NET 9.0 SDK
# ============================================================
echo "📦 [4/7] Instalando .NET 9.0..."
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb
sudo apt update
sudo apt install -y dotnet-sdk-9.0

# ============================================================
# 5. INSTALAR NGINX (Reverse Proxy)
# ============================================================
echo "📦 [5/7] Instalando Nginx..."
sudo apt install -y nginx

# ============================================================
# 6. CONFIGURAR FIREWALL (UFW)
# ============================================================
echo "🔥 [6/7] Configurando Firewall..."
sudo ufw allow 22/tcp      # SSH
sudo ufw allow 80/tcp      # HTTP (Frontend)
sudo ufw allow 81/tcp      # API
sudo ufw --force enable

# ============================================================
# 7. CRIAR ESTRUTURA DE DIRETÓRIOS
# ============================================================
echo "📁 [7/7] Criando estrutura de diretórios..."
sudo mkdir -p /var/www/dingfood-frontend
sudo mkdir -p /opt/dingfood-api
sudo chown -R $(whoami):$(whoami) /opt/dingfood-api

# ============================================================
# 8. CONFIGURAR NGINX (Proxy Reverso)
# ============================================================
echo "⚙️ Configurando Nginx..."

# Deletar config padrão
sudo rm -f /etc/nginx/sites-enabled/default

# Criar config para DingFood
sudo tee /etc/nginx/sites-available/dingfood > /dev/null << 'NGINX_CONFIG'
# ============================================================
# Frontend React - Porta 80
# ============================================================
server {
    listen 80;
    server_name _;

    root /var/www/dingfood-frontend;
    index index.html;

    # Suporte ao React Router (SPA)
    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache de assets estáticos
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2)$ {
        expires 30d;
        add_header Cache-Control "public, immutable";
    }
}

# ============================================================
# API .NET 9.0 - Porta 81
# ============================================================
server {
    listen 81;
    server_name _;

    # Proxy para Kestrel (porta 5000)
    location / {
        proxy_pass http://127.0.0.1:5000;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection "keep-alive";
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
        proxy_read_timeout 60s;
    }
}
NGINX_CONFIG

# Ativar site
sudo ln -s /etc/nginx/sites-available/dingfood /etc/nginx/sites-enabled/

# Testar config
echo "✅ Testando configuração Nginx..."
sudo nginx -t

# Iniciar Nginx
sudo systemctl restart nginx
sudo systemctl enable nginx

# ============================================================
# 9. RESUMO
# ============================================================
echo ""
echo "🎉 ===== SERVIDOR CONFIGURADO COM SUCESSO ====="
echo ""
echo "📊 Versões instaladas:"
node -v
npm -v
dotnet --version
nginx -v
echo ""
echo "🔗 Próximos passos:"
echo "   1. Coloque o frontend React em: /var/www/dingfood-frontend/"
echo "   2. Coloque a API .NET em: /opt/dingfood-api/"
echo "   3. Inicie a API com: dotnet run --urls=http://localhost:5000"
echo ""
echo "📍 Endpoints:"
echo "   Frontend: http://191.234.174.58:80"
echo "   API:      http://191.234.174.58:81"
echo ""
echo "🔥 Firewall (UFW) status:"
sudo ufw status
echo ""
echo "✅ Pronto para deploy!"
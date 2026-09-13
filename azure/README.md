# 🚀 Setup e Deploy - DingFood

Guia completo para configurar e fazer deploy da aplicação DingFood em um servidor Azure.

---

## 📋 Índice

1. [Setup do Servidor](#setup-do-servidor)
2. [Deploy da Aplicação](#deploy-da-aplicação)
3. [Verificações](#verificações)
4. [URLs Finais](#urls-finais)

---

## Setup do Servidor

### Passo 1: Baixar e Executar Script de Setup

Execute os comandos abaixo **no Bastion** ou **via SSH**:

```bash
# Baixar o script do GitHub
wget https://raw.githubusercontent.com/rodrigofurlaneti/DingFood/main/azure/SetupDingfood.sh -O ~/setup.sh

# Dar permissão de execução
chmod +x ~/setup.sh

# Executar o script (instala Node.js, .NET 9.0, Nginx, UFW)
sudo bash ~/setup.sh
```

⏳ **Aguarde 10-15 minutos** para o script terminar completamente.

---

### Passo 2: Verificar se Tudo Funcionou

Após o script terminar, verifique se todos os componentes foram instalados:

#### Verificar Nginx

```bash
sudo systemctl status nginx
```

**Esperado:** `Active: active (running)` ✅

#### Verificar Firewall (UFW)

```bash
sudo ufw status
```

**Esperado:** Status ativo com portas 22, 80, 81 permitidas ✅

#### Verificar Versões Instaladas

```bash
# Node.js
node -v

# npm
npm -v

# .NET
dotnet --version
```

**Esperado:** Versões similares a:
- Node.js: v20.x.x
- npm: 10.x.x  
- .NET: 9.0.x

---

## Deploy da Aplicação

### Passo 1: Clone do Repositório

No Bastion, clone o repositório DingFood:

```bash
# Clonar repositório
git clone https://github.com/rodrigofurlaneti/DingFood.git ~/DingFood

# Entrar na pasta
cd ~/DingFood
```

---

### Passo 2: Deploy do Frontend React

#### 2.1 Build do Frontend (No Bastion)

```bash
# Entrar na pasta do frontend
cd ~/DingFood/frontend

# Instalar dependências
npm install

# Gerar build para produção (cria pasta dist/)
npm run build
```

#### 2.2 Copiar Arquivos para o Servidor

```bash
# Copiar arquivos do build para pasta do Nginx
sudo cp -r dist/* /var/www/dingfood-frontend/

# Definir permissões corretas
sudo chown -R www-data:www-data /var/www/dingfood-frontend/

# Voltar para raiz do projeto
cd ~/DingFood
```

✅ **Frontend disponível em:** `http://191.234.174.58:80`

---

### Passo 3: Deploy da API .NET

#### 3.1 Build da API (No Bastion)

```bash
# Entrar na pasta do backend
cd ~/DingFood/backend

# Restaurar pacotes NuGet
dotnet restore

# Publicar em Release (otimizado para produção)
dotnet publish -c Release -o ~/publish-api
```

⏳ **Pode demorar 2-3 minutos** dependendo do tamanho do projeto.

#### 3.2 Copiar Arquivos para o Servidor

```bash
# Copiar arquivos publicados para pasta da API
sudo cp -r ~/publish-api/* /opt/dingfood-api/

# Definir permissões
sudo chown -R root:root /opt/dingfood-api/

# Verificar se foi copiado
ls -la /opt/dingfood-api/
```

---

### Passo 4: Criar Serviço Systemd (Autoinicialização)

Para que a API inicie automaticamente quando o servidor reiniciar:

```bash
# Criar arquivo de serviço
sudo tee /etc/systemd/system/dingfood-api.service > /dev/null << 'EOF'
[Unit]
Description=DingFood API - .NET 9.0
After=network.target

[Service]
Type=simple
User=root
WorkingDirectory=/opt/dingfood-api
ExecStart=/usr/bin/dotnet /opt/dingfood-api/DingFood.Api.dll --urls=http://localhost:5000
Restart=always
RestartSec=10
StandardOutput=journal
StandardError=journal

[Install]
WantedBy=multi-user.target
EOF

# Recarregar configuração do systemd
sudo systemctl daemon-reload

# Ativar serviço (para iniciar automaticamente no boot)
sudo systemctl enable dingfood-api.service

# Iniciar o serviço
sudo systemctl start dingfood-api.service

# Verificar status
sudo systemctl status dingfood-api.service
```

✅ **API disponível em:** `http://191.234.174.58:81`

---

## Verificações

### Passo 1: Verificar Frontend

Confirme se o frontend foi implantado corretamente:

```bash
# Verificar se os arquivos foram copiados
ls -la /var/www/dingfood-frontend/
```

**Esperado:** Deve conter `index.html` e pasta `assets/` ✅

---

### Passo 2: Verificar API

Confirme se a API foi implantada e está rodando:

```bash
# Verificar se os arquivos foram copiados
ls -la /opt/dingfood-api/

# Verificar status do serviço
sudo systemctl status dingfood-api.service

# Ver logs em tempo real
sudo journalctl -u dingfood-api.service -f
```

**Esperado:** Serviço em status `active (running)` ✅

---

### Passo 3: Testar Conectividade

#### Testar Frontend

```bash
# Teste local
curl -I http://localhost:80

# Teste remoto
curl -I http://191.234.174.58:80
```

**Esperado:** `HTTP/1.1 200 OK` ✅

#### Testar API

```bash
# Teste local (porta 5000 - direto no Kestrel)
curl -I http://localhost:5000

# Teste remoto (porta 81 - via Nginx)
curl -I http://191.234.174.58:81
```

**Esperado:** `HTTP/1.1 200 OK` ou código apropriado do endpoint ✅

---

## URLs Finais

Após o setup e deploy completos, acesse a aplicação através de:

```
🌐 Frontend (React):  http://191.234.174.58:80
🔌 API (.NET):        http://191.234.174.58:81
```

---

## Troubleshooting

### Frontend não carrega

```bash
# Verificar permissões
ls -la /var/www/dingfood-frontend/

# Reiniciar Nginx
sudo systemctl restart nginx

# Ver logs de erro
sudo tail -f /var/log/nginx/error.log
```

### API não responde

```bash
# Verificar logs da API
sudo journalctl -u dingfood-api.service -n 50

# Reiniciar API
sudo systemctl restart dingfood-api.service

# Verificar se está escutando na porta 5000
sudo netstat -tlnp | grep 5000
```

### Nginx não inicia

```bash
# Testar configuração
sudo nginx -t

# Ver status
sudo systemctl status nginx

# Ver logs de erro
sudo systemctl status nginx -l
```

---

## 📋 Checklist de Deploy

- [ ] Script de setup executado com sucesso
- [ ] Nginx está rodando (`systemctl status nginx`)
- [ ] .NET 9.0 instalado (`dotnet --version`)
- [ ] Node.js instalado (`node -v`)
- [ ] Repositório clonado (`~/DingFood`)
- [ ] Frontend buildado (`npm run build`)
- [ ] Arquivos do frontend copiados para `/var/www/dingfood-frontend/`
- [ ] Permissões do frontend definidas
- [ ] API buildada (`dotnet publish`)
- [ ] Arquivos da API copiados para `/opt/dingfood-api/`
- [ ] Serviço systemd criado
- [ ] Serviço ativado (`systemctl enable`)
- [ ] Serviço iniciado (`systemctl start`)
- [ ] Frontend acessível em http://191.234.174.58:80 ✅
- [ ] API acessível em http://191.234.174.58:81 ✅

---

## 🔄 Atualizações Futuras

Para atualizar a aplicação quando houver mudanças no repositório:

```bash
cd ~/DingFood

# Puxar atualizações
git pull origin main

# ===== FRONTEND =====
cd frontend
npm install
npm run build
sudo cp -r dist/* /var/www/dingfood-frontend/
cd ..

# ===== BACKEND =====
cd backend
dotnet restore
dotnet publish -c Release -o ~/publish-api
sudo cp -r ~/publish-api/* /opt/dingfood-api/
cd ..

# Reiniciar API
sudo systemctl restart dingfood-api.service

echo "✅ Deploy de atualização concluído!"
```

---

## 📚 Informações Técnicas

**Versões Utilizadas:**
- Ubuntu: 22.04.5 LTS
- Node.js: v20.20.2
- npm: 10.8.2
- .NET: 9.0.203
- Nginx: 1.18.0 (Ubuntu)

**Portas:**
- SSH: 22 (acesso remoto)
- HTTP (Frontend): 80
- HTTP (API via proxy): 81
- Kestrel (API local): 5000
- MySQL: 3306

**Arquitetura:**
```
Internet → Nginx (80/81) → Frontend React / API .NET (5000) → MySQL
```

---

*Última atualização: 13/09/2026*
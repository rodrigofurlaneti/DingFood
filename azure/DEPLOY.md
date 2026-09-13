# 📋 Resumo de Deploy - DingFood

## 🎯 Objetivo

Fazer deploy da aplicação DingFood (React + .NET 9.0) em um servidor Azure com:
- **Frontend**: Porta 80 (Nginx)
- **API**: Porta 81 (Nginx reverse proxy → .NET Kestrel na 5000)

---

## 📊 Arquitetura

```
Internet
   ↓
Nginx (Porta 80 e 81)
   ├→ /var/www/dingfood-frontend/     (React - Vite)
   └→ proxy para localhost:5000         (API .NET)
        ↓
   /opt/dingfood-api/
   (DingFood.Api.dll)
        ↓
   MySQL (Porta 3306)
```

---

## 🚀 Passos de Deploy

### **Pré-requisitos**

- Servidor Ubuntu 22.04 LTS
- Node.js v20.20.2 instalado
- .NET 9.0 SDK instalado
- Nginx instalado e configurado
- Firewall UFW com portas 80, 81, 22 abertas

> Execute o script de setup: `https://raw.githubusercontent.com/rodrigofurlaneti/DingFood/main/azure/SetupDingfood.sh`

---

### **Passo 1: Clonar Repositório**

```bash
git clone https://github.com/rodrigofurlaneti/DingFood.git ~/DingFood
cd ~/DingFood
```

---

### **Passo 2: Build e Deploy do Frontend (React + Vite)**

```bash
# Entrar na pasta frontend
cd ~/DingFood/frontend

# Instalar dependências
npm install

# Build para produção (gera pasta dist/)
npm run build

# Copiar arquivos para pasta do Nginx
sudo cp -r dist/* /var/www/dingfood-frontend/

# Definir permissões
sudo chown -R www-data:www-data /var/www/dingfood-frontend/

# Voltar para raiz do projeto
cd ~/DingFood
```

✅ **Frontend disponível em: `http://191.234.174.58:80`**

---

### **Passo 3: Build e Deploy da API (.NET 9.0)**

```bash
# Entrar na pasta backend
cd ~/DingFood/backend

# Restaurar pacotes NuGet
dotnet restore

# Publicar em Release (otimizado para produção)
dotnet publish -c Release -o ~/publish-api

# Copiar arquivos para pasta da API
sudo cp -r ~/publish-api/* /opt/dingfood-api/

# Definir permissões
sudo chown -R root:root /opt/dingfood-api/

# Verificar se foi copiado
ls -la /opt/dingfood-api/
```

---

### **Passo 4: Criar Serviço Systemd (Autoinicialização)**

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

# Ativar serviço (para iniciar automaticamente)
sudo systemctl enable dingfood-api.service

# Iniciar serviço
sudo systemctl start dingfood-api.service

# Verificar status
sudo systemctl status dingfood-api.service
```

✅ **API disponível em: `http://191.234.174.58:81`**

---

## 🧪 Testes de Verificação

### **1. Verificar Frontend**

```bash
# Verificar se arquivos foram copiados
ls -la /var/www/dingfood-frontend/

# Testar acesso local
curl -I http://localhost:80

# Testar acesso remoto
curl -I http://191.234.174.58:80
```

**Esperado:** HTTP/1.1 200 OK

### **2. Verificar API**

```bash
# Verificar se arquivos foram copiados
ls -la /opt/dingfood-api/

# Verificar status do serviço
sudo systemctl status dingfood-api.service

# Ver logs em tempo real
sudo journalctl -u dingfood-api.service -f

# Testar porta 5000 (local)
curl -I http://localhost:5000/health

# Testar port 81 (remoto via Nginx)
curl -I http://191.234.174.58:81/health
```

**Esperado:** HTTP/1.1 200 OK ou similar (depende do endpoint)

### **3. Verificar Nginx**

```bash
# Status do Nginx
sudo systemctl status nginx

# Testar configuração
sudo nginx -t

# Ver logs de erro
sudo tail -f /var/log/nginx/error.log
```

**Esperado:** `configuration test is successful`

---

## 🔄 Atualizações Futuras (Quick Deploy)

Quando há alterações no GitHub:

```bash
cd ~/DingFood

# Puxar atualizações
git pull origin main

# ===== ATUALIZAR FRONTEND =====
cd frontend
npm install
npm run build
sudo cp -r dist/* /var/www/dingfood-frontend/
cd ..

# ===== ATUALIZAR API =====
cd backend
dotnet restore
dotnet publish -c Release -o ~/publish-api
sudo cp -r ~/publish-api/* /opt/dingfood-api/
cd ..

# Reiniciar API
sudo systemctl restart dingfood-api.service

echo "✅ Deploy concluído!"
```

---

## 📍 URLs Finais

```
🌐 Frontend (React):  http://191.234.174.58:80
🔌 API (.NET):        http://191.234.174.58:81
```

---

## 🆘 Troubleshooting

### **Frontend mostra erro 404 ou em branco**

```bash
# Verificar permissões
ls -la /var/www/dingfood-frontend/

# Verificar se index.html existe
file /var/www/dingfood-frontend/index.html

# Reiniciar Nginx
sudo systemctl restart nginx

# Ver logs
sudo tail -f /var/log/nginx/error.log
```

### **API não responde na porta 81**

```bash
# Verificar se está rodando na porta 5000
sudo netstat -tlnp | grep 5000

# Ver logs da API
sudo journalctl -u dingfood-api.service -n 100

# Reiniciar serviço
sudo systemctl restart dingfood-api.service
```

### **Nginx não inicia**

```bash
# Testar configuração
sudo nginx -t

# Ver erros
sudo systemctl status nginx

# Verificar se porta 80/81 está em uso
sudo netstat -tlnp | grep -E ':(80|81)'
```

### **Banco de dados não conecta**

```bash
# Verificar se MySQL está rodando
sudo systemctl status mysql

# Testar conexão
mysql -h localhost -u admin -p

# Verificar firewall
sudo ufw status
```

---

## 📋 Checklist de Deploy

- [ ] Servidor Setup executado (Node.js, .NET, Nginx)
- [ ] Git clone do repositório feito
- [ ] Frontend buildado (`npm run build`)
- [ ] Arquivos do frontend copiados para `/var/www/dingfood-frontend/`
- [ ] Permissões do frontend definidas (`www-data`)
- [ ] API buildada (`dotnet publish`)
- [ ] Arquivos da API copiados para `/opt/dingfood-api/`
- [ ] Serviço systemd criado
- [ ] Serviço ativado (`systemctl enable`)
- [ ] Serviço iniciado (`systemctl start`)
- [ ] Frontend acessível em http://191.234.174.58:80 ✅
- [ ] API acessível em http://191.234.174.58:81 ✅

---

## 📚 Referências

- [Nginx Reverse Proxy](https://nginx.org/en/docs/beginners_guide.html)
- [.NET Deployment](https://learn.microsoft.com/en-us/dotnet/core/deployment/)
- [Systemd Service Files](https://wiki.archlinux.org/title/Systemd)
- [Vite Build](https://vitejs.dev/guide/build.html)

---

**Versões Usadas:**
- Node.js: v20.20.2
- npm: v10.8.2
- .NET: 9.0.203
- Nginx: 1.18.0
- Ubuntu: 22.04.5 LTS

---

*Última atualização: 13/09/2026*
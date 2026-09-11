# CI e deploy na VM

Os workflows foram recuperados do repositório anterior e adaptados aos caminhos `DingFood`.

- `.github/workflows/ci.yml`: build e testes .NET 9, cobertura, frontend Node 22 e validação das imagens Docker. Executa em pushes/PRs para main, master e develop, além de execução manual.
- `.github/workflows/deploy.yml`: publica imagens no GHCR e executa o deploy na VM após CI bem-sucedido de um push na main deste repositório. A execução manual também é limitada à main. Usa o commit do CI no checkout, nas tags SHA e na versão da aplicação.

## Configuração do novo repositório

Em Settings → Secrets and variables → Actions, configure os mesmos valores utilizados no repositório anterior:

| Secret | Uso |
| --- | --- |
| `VM_HOST` | Host da VM |
| `VM_USER` | Usuário SSH com acesso ao Docker via sudo |
| `VM_SSH_KEY` | Chave SSH privada |
| `GHCR_PAT` | Token para baixar as imagens privadas na VM |
| `SONAR_TOKEN` | Opcional: habilita a análise SonarCloud |

`GITHUB_TOKEN` é fornecido pelo GitHub, com `packages: write` no job de publicação. As imagens agora usam pacotes próprios: `dingfood-api` e `dingfood-frontend`. Isso evita depender da permissão de escrita dos pacotes `syncbar-*` vinculados ao repositório anterior. O `GHCR_PAT` usado pela VM deve ter `read:packages` e acesso aos novos pacotes privados.

O workflow e o Compose usam `ghcr.io/rodrigofurlaneti/dingfood-api:latest` e `ghcr.io/rodrigofurlaneti/dingfood-frontend:latest`. Foram preservados o diretório `/opt/syncbarservice`, os serviços, os volumes existentes e os arquivos de HTTPS. O script HTTPS ainda utiliza o IP `9.205.156.87`. A VM deve manter o `.env` com a conexão MySQL e os segredos já usados; esses valores não são copiados pelo workflow.

Se ocorrer `permission_denied: write_package` em um pacote que já exista, confira Package settings → Manage Actions access e conceda Write ao repositório DingFood. Referência: [permissões de publicação do GitHub Packages](https://docs.github.com/en/packages/managing-github-packages-using-github-actions-workflows/publishing-and-installing-a-package-with-github-actions).

O Sonar mantém como padrão o projeto anterior `rodrigofurlaneti_SyncBar`. Para usar outro projeto, configure as variáveis `SONAR_PROJECT_KEY` e `SONAR_ORGANIZATION`. A cobertura gera Cobertura e OpenCover; o scanner C# importa OpenCover.

Não foi executado push, publicação de imagens ou deploy durante a inclusão dos arquivos. A execução real depende dos secrets, das permissões dos pacotes e da disponibilidade da VM. O daemon Docker local não estava disponível para validar as imagens nesta sessão.

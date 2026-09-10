# Dark Kitchen: empresas e integrações independentes

`businessgroup → company → branch` é a hierarquia administrativa. `CompanyId` continua sendo a fronteira operacional; os filtros não usam o grupo para compartilhar catálogo, estoque, pedidos ou integrações.

## Acesso e operação

- O cadastro existente cria uma empresa com seu grupo e um vínculo explícito em `appusercompany` para o administrador. Novos usuários também recebem o vínculo com sua empresa de origem.
- `appuser.CompanyId` permanece como empresa de origem por compatibilidade. O acesso operacional depende de `appusercompany`, e a empresa selecionada não altera a identidade do usuário.
- Administradores podem criar empresas no mesmo grupo pela página **Gerenciar empresas**. A criação concede acesso administrativo somente ao criador. Os demais usuários precisam de concessão explícita.
- Um administrador da empresa selecionada pode conceder ou revogar o acesso de outros usuários do mesmo grupo. Perfil e funcionário são definidos por empresa. O vínculo de funcionário, quando informado, precisa pertencer à empresa selecionada.
- A empresa de origem permanece o ponto de entrada do login. Para remover integralmente esse acesso, desative o usuário; a interface não permite revogar isoladamente o vínculo de origem.
- A criação da empresa inclui uma primeira filial. Configure equipe, funcionário de autoatendimento, caixa, produtos e integração para a nova operação antes de receber pedidos.

## Sessão e endpoints

`GET /api/companies/allowed` lista as empresas autorizadas. `POST /api/companies/switch`, com `{ "targetCompanyId": 20 }`, valida o vínculo e emite um JWT com `companyId`, `businessGroupId`, permissões e funcionário da empresa selecionada. A renovação em `/api/auth/refresh` aceita `companyId` e revalida a associação antes de emitir outro token.

O servidor também valida `X-Company-Id` em cada requisição autenticada e recompõe as permissões com os registros atuais. Pertencer ao grupo não concede acesso às outras empresas. Tokens de clientes não autorizam os endpoints administrativos de empresas.

`POST /api/companies/group` cria uma empresa; `GET /api/companies/group/users` lista usuários do grupo para concessão; `PUT /api/companies/access` recebe `appUserId`, `roleId`, `employeeId` opcional e `enabled` para a empresa ativa. Esses endpoints exigem o perfil Administrador.

O frontend só monta telas operacionais depois de validar empresa e filial. A troca cancela e limpa o TanStack Query, reinicia a filial e carrega um novo documento, descartando formulários, gavetas e respostas anteriores. Requisições antigas não são repetidas em outra empresa durante a renovação do token.

## Integrações

O polling processa até quatro empresas simultaneamente, com um escopo de DI e um DbContext por empresa. Os demais workers iFood também separam os escopos por empresa. Tokens são armazenados por CompanyId, e o provedor recusa credenciais de outra empresa quando há contexto ativo.

Eventos preservam o MerchantId. A resolução rejeita merchants não pertencentes ao contexto e mapeamentos ambíguos. O índice único de MerchantUuid impede que o mesmo merchant seja associado a duas filiais. Eventos com o mesmo identificador em empresas distintas mantêm deduplicação separada.

Filtros de leitura cobrem proprietários diretos e entidades filhas de catálogo, estoque, pedidos e caixa. A persistência verifica CompanyId e referências a entidades filtradas, bloqueando vínculos operacionais cruzados. As origens de pedido globais existentes continuam disponíveis como códigos de referência.

## Migração MySQL 8

O recurso SQL está em `backend/src/DingFood.Infrastructure/Persistence/Migrations/BusinessGroups.sql`, embutido na migration `202609100001_AddBusinessGroups`. A API existente executa migrations na inicialização: a primeira inicialização da versão nova aplicará essa alteração ao banco configurado.

A migração foi preparada a partir da estrutura do dump MySQL 8.4 fornecido. O dump original não foi alterado ou importado. A migração não foi executada em um servidor MySQL nesta sessão; o teste de persistência utilizou SQLite com conexões separadas.

Antes da atualização, preserve um backup verificado e pause gravações da aplicação antiga. Verifique duplicidades:

```sql
SELECT Cnpj, COUNT(*) FROM company GROUP BY Cnpj HAVING COUNT(*) > 1;
SELECT MerchantUuid, COUNT(*) FROM ifoodmerchantmapping
WHERE MerchantUuid IS NOT NULL AND TRIM(MerchantUuid) <> ''
GROUP BY MerchantUuid HAVING COUNT(*) > 1;
```

Se houver duplicidades, resolva a titularidade antes de aplicar os índices únicos. O script não escolhe arbitrariamente um proprietário nem remove registros operacionais.

Cada empresa existente recebe um grupo próprio e mantém seus IDs. Os usuários recebem os vínculos existentes. `BusinessGroupId` torna-se obrigatório após o preenchimento. O script pode ser repetido com as gravações pausadas; não redefine associações já preenchidas. Como MySQL realiza commits implícitos de DDL, uma falha pode deixar uma etapa parcialmente aplicada: mantenha a aplicação parada, corrija a causa e execute novamente. O rollback de dados exige restauração do backup, não exclusão automática de grupos ou vínculos.

## Validação

```powershell
dotnet test backend/DingFood.sln
cd frontend
node node_modules/typescript/bin/tsc -b
node node_modules/vite/bin/vite.js build
node node_modules/@playwright/test/cli.js test --config playwright.companies.config.ts
```

`DingFood.Tests` foi incluído na solução para que `dotnet test` execute também os testes unitários e de persistência. Os cenários novos cobrem permissões, revogação, JWT, escrita cruzada, credenciais concorrentes, eventos de dois merchants com persistência real e a troca de contexto no navegador. Os serviços externos são simulados nos testes; não foram enviadas publicações ou pedidos ao iFood real.

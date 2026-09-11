# Dark Kitchen: empresas, marcas e filiais

## Cadastro na interface

Abra o seletor do cabeçalho e entre em **Empresas, marcas e filiais**. Escolha a empresa antes de cadastrar:

- **Nova empresa**: cadastra outro CNPJ no grupo, com sua marca inicial e primeira filial.
- **Nova marca**: informa o nome comercial e a primeira filial dessa marca na empresa ativa.
- **Nova filial**: escolhe uma marca do grupo e cadastra outra unidade na empresa ativa. Permite vincular explicitamente a mesma marca a outro CNPJ do grupo.

Depois do cadastro, selecione empresa, marca e filial no cabeçalho. Configure equipe, caixa, catálogo e integrações da operação. O criador recebe acesso administrativo à filial; outros usuários precisam de concessão explícita em **Acesso à filial selecionada**. O funcionário informado deve pertencer exatamente àquela filial.

## Modelo e isolamento

O grupo reúne empresas e marcas. `CompanyBrand` estabelece quais marcas operam em cada CNPJ. Cada `Branch` pertence a uma empresa e a uma marca. Uma marca pode ter várias filiais e operar em vários CNPJs, mediante vínculo explícito.

Produtos, categorias, complementos, fornecedores e demais cadastros comerciais são separados por **empresa e marca**. Filiais da mesma marca e empresa usam o mesmo catálogo. Estoque operacional, pedidos, mesas, comandas, funcionários, caixa e turnos são separados por **filial**, herdando também empresa e marca. O saldo usado na venda vem de `StockItem` da filial ativa; movimentos usam o ID real desse estoque.

Todas as entidades mapeadas têm uma classificação explícita no modelo. Entidades filhas herdam o filtro do proprietário, por exemplo item → pedido → filial e movimento → estoque → filial. A inicialização do modelo falha se uma entidade nova ficar sem classificação. O [inventário completo](tenant-entity-inventory.md) é gerado pelo teste do modelo.

Identidades e vínculos organizacionais têm regras próprias: pertencer ao grupo não concede acesso às demais operações. Dicionários técnicos, como status e unidades de medida, continuam como referências do sistema. Eles não contêm os cadastros operacionais de uma cozinha.

A gravação verifica proprietário e referências, inclusive alterações e exclusões, para impedir a associação a produtos, funcionários ou outros registros fora do contexto. O mesmo CPF pode identificar cadastros de funcionário em filiais diferentes, sem compartilhar o vínculo de trabalho.

## Sessão e permissões

`AppUserCompany` concede acesso à empresa. `AppUserBranch` concede acesso à filial, com funcionário e perfil específicos. O middleware revalida esses vínculos em cada requisição e recompõe os claims de marca, filial, funcionário, perfil e permissões.

O frontend envia `X-Company-Id` e `X-Branch-Id`. Trocar empresa ou filial limpa o cache e os formulários. Respostas antigas não atualizam a nova operação. QR codes, pedidos públicos e webhooks resolvem sua operação antes de executar consultas e gravações; um contexto já vinculado não pode mudar para outra operação.

Endpoints:

- `GET /api/companies/allowed` e `POST /api/companies/switch`: empresas autorizadas e troca de empresa.
- `POST /api/companies/group`: outro CNPJ no grupo.
- `GET /api/workplaces`: somente filiais autorizadas para o usuário.
- `GET /api/workplaces/brands`: marcas disponíveis no grupo, para administradores.
- `POST /api/workplaces`: cria marca com filial ou filial de uma marca existente.
- `PUT /api/workplaces/access`: concede, atualiza ou revoga acesso à filial especificada.

Os perfis são definidos pela empresa e atribuídos por filial. Os vínculos migrados preservam os múltiplos perfis legados somente nas filiais já autorizadas. Ao editar o acesso pela nova tela, o perfil escolhido passa a valer especificamente naquela filial.

## Integrações

Os workers iFood usam contexto de empresa e marca. O cache de token inclui ambas. Merchants e eventos são resolvidos para a filial correta; a assinatura é conferida contra as configurações correspondentes. Os alertas operacionais são filtrados pela filial. Keeta e Asaas também vinculam eventos e processamentos à operação correspondente.

## Atualização do banco

A API aplica migrations na inicialização. A versão inclui:

1. `202609100001_AddBusinessGroups`: grupos e vínculos explícitos de empresa.
2. `202609100002_AddBrandsAndBranchAccess`: marcas, vínculos empresa/marca, acesso por filial e escopo dos cadastros existentes.
3. `202609100003_CompleteLegacyOperationalSchema`: compatibilidade com estruturas antigas de cartões salvos e tabelas de fechamento já previstas pelo modelo e pelos scripts SQL anteriores.

Cada empresa existente recebe uma marca inicial usando seu nome fantasia. Seus registros mantêm IDs e são vinculados a essa marca. Funcionários recebem somente sua filial de origem; administradores mantêm as filiais das empresas já autorizadas. Novos cadastros não copiam a equipe.

Reinicie a API com a versão atualizada e recarregue a interface. A aplicação de DDL no MySQL pode fazer commits implícitos; o procedimento de atualização deve preservar um backup e interromper gravações da versão antiga. A reversão de dados requer restauração, pois remover vínculos apagaria informação de propriedade.

## Validação realizada

- Solução backend: 6.606 testes unitários/persistência, 300 cenários, 14 testes de arquitetura e 3 E2E aprovados; 9 E2E dependentes de ambiente foram ignorados.
- MySQL 8.0.43 isolado: estrutura das 112 tabelas do dump, somente dados fictícios, aplicação das migrations, conferência de todas as colunas mapeadas, manutenção de acesso legado, criação de marca e empresa com retry habilitado.
- TypeScript, build Vite e Playwright: envio dos formulários de marca e filial, troca de empresa, descarte de estado anterior e seletor em desktop/celular.
- Nenhum banco da aplicação ou serviço externo recebeu dados desses testes.

O teste MySQL é opt-in (`DINGFOOD_ISOLATED_MYSQL_TEST=1`) e usa exclusivamente a instância isolada na porta 33197; na suíte normal ele fica ignorado.

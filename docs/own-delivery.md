# Delivery próprio: motoboys e precificação

Acesse **Motoboys e taxas** no menu ou `/logistica`, com perfil Administrador/Gerente e uma filial selecionada. Cadastre os motoboys e salve o modelo de pagamento. Empresa e marca são herdadas da filial; IDs de escopo não são aceitos no corpo do cadastro.

## Ativação

- Aplique a migration EF `202609110001_AddOwnDelivery` pelo procedimento de migrations do projeto. Ela acrescenta quatro tabelas e campos em `customerorder`. Não modifica `ifoodlogisticsdelivery`.
- Para usar KM, configure `Delivery__GoogleMapsApiKey` no ambiente do backend, com a Geocoding API habilitada. A chave não deve ser incluída no frontend ou no Git.
- Complete rua, número, cidade, estado e CEP da filial e do destino. A integração exige um resultado único e preciso; falha do provedor ou endereço ambíguo rejeita a criação com HTTP 422, sem salvar o pedido.
- Filiais sem configuração mantêm o comportamento anterior. Pedidos iFood e Keeta não passam pela precificação própria.

Integração baseada na [documentação oficial da Geocoding API](https://developers.google.com/maps/documentation/geocoding/guides-v3/requests-geocoding). As chamadas externas não foram executadas com credenciais reais nesta implementação.

## Regras adotadas

- KM significa raio em linha reta (Haversine), e não trajeto por ruas. O limite é inclusivo; qualquer distância maior é bloqueada antes da persistência. Não há arredondamento antes da comparação com o raio.
- A taxa por entrega é `distância × R$/KM`, arredondada para centavos, com metade para cima. Ela integra `TotalAmount`, separada da taxa de serviço, e representa também o custo por entrega do motoboy atribuído.
- Tarifas usam o instante de criação em UTC convertido para o fuso configurado, inicialmente `America/Sao_Paulo`. Dias são uma máscara de bits: domingo=1, segunda=2, terça=4, quarta=8, quinta=16, sexta=32, sábado=64. Sexta a domingo=97.
- Início inclusivo e fim exclusivo. Em regras que cruzam meia-noite, a madrugada pertence ao dia de início. Prioridades devem ser distintas; o maior número prevalece na intersecção.
- A diária não acrescenta taxa ao cliente. O valor fica congelado no pedido e é registrado uma vez por filial/motoboy/data, protegido por índice único. Na atribuição, a data corresponde à data local de criação do pedido. Diárias também podem ser registradas manualmente, mesmo sem entregas; nesse caso usam a configuração vigente. O primeiro registro da data prevalece.
- Atribuição repetida ao mesmo motoboy é idempotente; troca para outro motoboy é rejeitada para preservar o pagamento já registrado. A atribuição possui controle de concorrência.
- Pedidos antigos não são reprecificados. Valor, distância, tarifa, modelo, diária e fuso são imutáveis pelo domínio e pela persistência EF após a criação. Alterações de configuração só valem para novos pedidos.

## API

Todos os endpoints exigem autenticação e contexto de empresa/filial autorizado.

| Endpoint | Uso |
| --- | --- |
| `GET/POST /api/delivery/drivers` | Listar / cadastrar motoboys |
| `PUT /api/delivery/drivers/{id}` | Editar ou desativar |
| `GET/PUT /api/delivery/config` | Consultar / salvar configuração e regras |
| `GET /api/delivery/orders` | Últimos 200 pedidos precificados, com valores congelados |
| `PUT /api/delivery/orders/{id}/driver` | Atribuir, corpo `{ "driverId": 1 }` |
| `GET/POST /api/delivery/daily-payments` | Últimas 200 diárias / registrar `{ "driverId": 1, "workDate": "2026-09-11" }` |

Alterações em cadastros/configurações e acesso a diárias exigem Administrador/Gerente. As diárias são registros de valores a pagar; não disparam transferências ou pagamentos externos.

## Validação

- 232 testes de domínio/persistência e regressões de pedidos, isolamento de filiais e logística iFood aprovados.
- Migration executada em MySQL 8 isolado na porta 33198, importando somente as 121 tabelas do dump `Dump20260911.sql`. Teste confirma todas as colunas mapeadas, gravação/leitura das novas entidades e unicidade da diária.
- Teste Playwright cobre cadastro, payload de tarifa dinâmica, contexto de filial, atribuição e layout em desktop/celular: `node node_modules/@playwright/test/cli.js test --config playwright.delivery.config.ts` (em `frontend`).
- Build da API, TypeScript e build de produção Vite aprovados.

O teste MySQL é opt-in (`DINGFOOD_DELIVERY_MYSQL_TEST=1`), usa exclusivamente `codex_delivery_migration` em `127.0.0.1:33198` e pressupõe uma importação nova do DDL do dump. Não execute contra banco de aplicação.

# Otimização da autorização de dark kitchen

O middleware operacional agora resolve somente a filial solicitada em `X-Branch-Id`. Sem esse cabeçalho, seleciona uma única filial autorizada, ordenada por marca, nome e ID. Cabeçalhos inválidos ou filiais sem autorização retornam 403.

A resolução da filial não chama novamente a resolução completa da empresa. Confere diretamente usuário, empresa de origem, vínculo com a empresa selecionada, grupo ativo, marca vinculada e acesso à filial. Perfis legados e perfis específicos por filial são carregados em lote, com permissões ativas. O seletor de filiais usa o mesmo carregamento em lote.

Nos testes com 2 e 12 filiais, essa etapa executou 3 consultas em ambos os casos, inclusive para a listagem completa. Isso não representa o total de consultas da requisição: o middleware de empresa e o endpoint continuam executando suas próprias consultas. Não foi medido ganho percentual de latência no ambiente em uso.

Não foi adicionado cache de autorização: revogações continuam sendo verificadas no banco. Foram validados isolamento de perfis, permissões desativadas, revogação de filial e revogação da empresa. A consulta também foi executada com sucesso no MySQL 8 isolado, com dados sintéticos e rollback.

Os índices já existentes de usuário/filial, usuário/empresa, empresa/marca e perfis/permissões atendem às chaves usadas nesta alteração. Não foi criada migration de índices sem evidência de plano de execução que a justifique. A otimização elimina consultas repetidas; eventuais gargalos restantes devem ser medidos no ambiente de uso.

## Logs iFood

`IfoodTokenProvider` registrava como erro tanto integração ausente/desabilitada quanto credenciais incompletas. A integração ausente/desabilitada agora retorna sem autenticar ou gravar erro. Quando está habilitada com credenciais incompletas, o erro permanece e identifica CompanyId, BrandId e quais campos estão ausentes, sem expor credenciais. Erros de descriptografia continuam sendo registrados.

A mensagem antiga era emitida antes da chamada de autenticação externa e, sozinha, não identifica qual das condições aconteceu. Após a separação por marca, é necessário verificar a configuração da marca selecionada; este ajuste não copia credenciais entre marcas nem habilita integrações.

## Aplicação

Compilação e testes executados em Release porque a API em Debug estava em uso. Para aplicar ao processo já aberto, recompilar e reiniciar a API pelo fluxo habitual. Nenhuma migration é necessária para este ajuste.

# language: pt
Funcionalidade: Dark Kitchen com empresas independentes
  O grupo agrega empresas sem compartilhar dados ou conceder acesso implicitamente.

  Cenário: Duas marcas pertencem ao mesmo grupo
    Dado um grupo Dark Kitchen com Burger e Pizza
    Então as duas marcas pertencem ao mesmo grupo
    E cada marca preserva seu CNPJ

  Cenário: Acesso entre grupos é proibido
    Dado um grupo Dark Kitchen com Burger e Pizza
    E um usuário da Burger e uma empresa de outro grupo
    Quando tento conceder acesso à empresa de outro grupo
    Então a concessão é rejeitada

  Cenário: Credenciais são independentes por empresa
    Dado um grupo Dark Kitchen com Burger e Pizza
    Quando cada empresa configura suas credenciais iFood
    Então os identificadores e segredos permanecem separados

  Cenário: Estoques das marcas são independentes
    Dado um grupo Dark Kitchen com Burger e Pizza
    Quando uma saída de estoque ocorre na Burger
    Então o estoque da Pizza permanece intacto

  Cenário: Alternância autorizada de empresa
    Dado um grupo Dark Kitchen com Burger e Pizza
    Quando o administrador autorizado alterna para Pizza
    Então a sessão representa a empresa Pizza

  Cenário: Roteamento de pedido recebido pelo merchant
    Dado um grupo Dark Kitchen com Burger e Pizza
    Quando chega um pedido do merchant da Pizza
    Então o pedido pertence somente à filial da Pizza

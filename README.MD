# MotoDispatch

## Descrição

O projeto MotoDispatch é um sistema para gerenciamento de aluguéis de motos e gestão de entregadores. Desenvolvido
utilizando .NET 8 e banco de dados PostgreSQL, o sistema permite operações como cadastro, atualização e finalização de
aluguéis, bem como o gerenciamento de informações dos entregadores.

## Tecnologias Utilizadas

- **.NET 8**: Framework de desenvolvimento para a criação de aplicações web e APIs.
- **PostgreSQL**: Sistema gerenciador de banco de dados objeto-relacional.
- **Docker**: Plataforma de conteinerização para facilitar a implantação e execução do ambiente de desenvolvimento e
  produção.

## Como Executar

O projeto pode ser executado utilizando Docker e Docker Compose, garantindo assim, que o ambiente de desenvolvimento
seja fácil de configurar e replicar. Siga as instruções abaixo para configurar o ambiente:

### Pré-requisitos

- Docker
- Docker Compose

### Instruções de Execução

1. Clone o repositório do projeto para sua máquina local.

2. Navegue até a pasta raiz do projeto, onde está localizado o arquivo `docker-compose.yml`.

3. Execute o seguinte comando para construir e iniciar os contêineres do serviço e do banco de dados:

   ```bash
   docker-compose up --build

4. Após a inicialização dos containers, a aplicação estará acessível através do navegador ou cliente de API no seguinte
   endereço:

    ```
    http://localhost:8080/swagger/index.html
    ```


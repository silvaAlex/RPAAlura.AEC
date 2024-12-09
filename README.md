# RPAAlura.AEC

![](https://img.shields.io/badge/autor-Alex%20Junior-brightgreen)

## Escopo do Projeto

Desafio para uma vaga que consiste no desenvolvimento de um RPA simples que realiza uma busca 
automaizada no site da [Alura](https://www.alura.com.br/) e grava os resultados em um 
banco de dados (mais informações abaixo).

### Requisitos:
- Que o código seja feito em C#; 
- Utilização do framework Selenium; 
- Utilização da abordagem DDD com injeção de dependência

### Dados
Com o retorno da pesquisa salvar no banco de dados os seguintes dados: 
- Titulo; 
- Professor (Pode ser um ou todos); 
- Carga Horária; 
-  Descrição;

### Docker-Compose
- a variável `CURSO` dentro do docker-compose e responsável por buscar os cursos dentro da alura

altere para buscar outros cursos

```yml
services:
  rpaalura.aec.console:
    environment:
      SELENIUM_URL: "http://selenium:4444"
      CURSO: "Forma&ccedil;&atilde;o Modelagem e Melhorias de Processos de Neg&oacute;cios"
```


#### Como rodar
```shell
docker-compose up
```



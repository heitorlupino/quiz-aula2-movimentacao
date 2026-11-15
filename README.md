# Quiz Aula 2 - Movimentação com pulo

Atividade da disciplina de **Desenvolvimento de Jogos Digitais** (Ciência da Computação – UNISAGRADO).

Autor: **Heitor Lupino**

Movimentação básica de um personagem 2D na Unity: andar para os lados e pular, usando os inputs do Input Manager e a física 2D da Unity (Rigidbody2D + BoxCollider2D). Não tem animação, só a movimentação.

## Controles

| Ação | Tecla |
|------|-------|
| Andar | A / D ou setas |
| Pular | Espaço, W ou seta para cima |

## Como funciona

O script principal é o [`Assets/Scripts/MovimentoJogador.cs`](Assets/Scripts/MovimentoJogador.cs):

- No `Update` lê o eixo `Horizontal` e o botão `Jump` do Input Manager.
- No `FixedUpdate` altera a velocidade do `Rigidbody2D`: o eixo X vem do input e o eixo Y fica com a gravidade.
- O pulo só acontece se o personagem estiver no chão. Isso é verificado com um `Physics2D.OverlapBoxAll` logo abaixo dos pés.
- A rotação do Rigidbody fica travada para o personagem não tombar, e o sprite vira para o lado em que está andando.

A cena tem um chão, três plataformas para testar o pulo e paredes nas pontas.

## Assets

Sprites em pixel art 16x16 (personagem, grama, terra e plataforma) do meu pack usado no jogo [A Aventura de Pip](https://github.com/heitorlupino/a-aventura-de-pip).

## Como abrir

1. Abra o projeto no Unity Hub com a **Unity 6.6** (`6000.6.9f1`).
2. Na primeira vez que o projeto abre, a cena `Assets/Scenes/Movimentacao.unity` é montada automaticamente pelo script `Assets/Editor/MontarCena.cs`. Se precisar montar de novo, use o menu **Quiz > Montar Cena**.
3. Aperte **Play**.

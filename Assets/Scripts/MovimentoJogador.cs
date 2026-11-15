using UnityEngine;

// Movimentacao basica do personagem 2D: andar para os lados e pular.
// Usa o Rigidbody2D (fisica da Unity) e os eixos do Input Manager.
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovimentoJogador : MonoBehaviour
{
    public float velocidade = 6f;
    public float forcaPulo = 12f;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private float inputHorizontal;
    private bool querPular;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        rb.freezeRotation = true; // nao deixa o personagem tombar
    }

    void Update()
    {
        // A/D ou setas (eixo "Horizontal" do Input Manager)
        inputHorizontal = Input.GetAxisRaw("Horizontal");

        // Espaco (botao "Jump" do Input Manager) ou W / seta para cima
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            querPular = true;
        }
    }

    void FixedUpdate()
    {
        // movimento horizontal: muda so o X e deixa a gravidade cuidar do Y
        rb.linearVelocity = new Vector2(inputHorizontal * velocidade, rb.linearVelocity.y);

        // so pula se estiver encostando no chao
        if (querPular && EstaNoChao())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, forcaPulo);
        }
        querPular = false;

        // vira o sprite para o lado que esta andando
        if (inputHorizontal != 0)
        {
            Vector3 escala = transform.localScale;
            escala.x = Mathf.Abs(escala.x) * Mathf.Sign(inputHorizontal);
            transform.localScale = escala;
        }
    }

    bool EstaNoChao()
    {
        // caixa fininha logo abaixo dos pes do personagem
        Bounds b = col.bounds;
        Vector2 centro = new Vector2(b.center.x, b.min.y - 0.05f);
        Vector2 tamanho = new Vector2(b.size.x * 0.9f, 0.1f);

        Collider2D[] colisores = Physics2D.OverlapBoxAll(centro, tamanho, 0f);
        foreach (Collider2D c in colisores)
        {
            if (c != col) return true;
        }
        return false;
    }
}

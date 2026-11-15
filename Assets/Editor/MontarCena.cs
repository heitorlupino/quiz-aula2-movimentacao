using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// Monta a cena de teste (chao, plataformas, camera e personagem).
// Roda sozinho na primeira vez que o projeto abre, ou pelo menu Quiz > Montar Cena.
[InitializeOnLoad]
public static class MontarCena
{
    const string CaminhoCena = "Assets/Scenes/Movimentacao.unity";

    static MontarCena()
    {
        EditorApplication.delayCall += () =>
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
            if (!System.IO.File.Exists(CaminhoCena)) Montar();
        };
    }

    [MenuItem("Quiz/Montar Cena")]
    public static void Montar()
    {
        var cena = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        Sprite grama = Carregar("grama");
        Sprite terra = Carregar("terra");
        Sprite plataforma = Carregar("plataforma");
        Sprite jogador = Carregar("jogador_parado");

        // camera
        var camGO = new GameObject("Main Camera");
        camGO.tag = "MainCamera";
        var cam = camGO.AddComponent<Camera>();
        cam.orthographic = true;
        cam.orthographicSize = 6f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.45f, 0.75f, 0.95f);
        camGO.transform.position = new Vector3(0f, 2f, -10f);

        // chao: uma faixa de terra com grama em cima
        Bloco("Chao (terra)", terra, new Vector2(0f, -3f), new Vector2(30f, 3f), 0, true);
        Bloco("Chao (grama)", grama, new Vector2(0f, -1f), new Vector2(30f, 1f), 0, true);

        // plataformas para testar o pulo
        Bloco("Plataforma 1", plataforma, new Vector2(-5f, 1.5f), new Vector2(3f, 1f), 0, true);
        Bloco("Plataforma 2", plataforma, new Vector2(0f, 3.5f), new Vector2(3f, 1f), 0, true);
        Bloco("Plataforma 3", plataforma, new Vector2(5f, 1.5f), new Vector2(3f, 1f), 0, true);

        // paredes invisiveis nas pontas para o personagem nao cair
        Parede("Parede Esquerda", -15.5f);
        Parede("Parede Direita", 15.5f);

        // personagem
        var player = new GameObject("Jogador");
        player.transform.position = new Vector3(-8f, 0.5f, 0f);
        var sr = player.AddComponent<SpriteRenderer>();
        sr.sprite = jogador;
        sr.sortingOrder = 5;
        var rb = player.AddComponent<Rigidbody2D>();
        rb.gravityScale = 3f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        var col = player.AddComponent<BoxCollider2D>();
        col.size = new Vector2(0.8f, 1f);
        col.sharedMaterial = SemAtrito();
        player.AddComponent<MovimentoJogador>();

        System.IO.Directory.CreateDirectory("Assets/Scenes");
        EditorSceneManager.SaveScene(cena, CaminhoCena);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(CaminhoCena, true) };
        Debug.Log("Cena montada em " + CaminhoCena + ". Aperte Play para testar.");
    }

    static Sprite Carregar(string nome)
    {
        return AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/" + nome + ".png");
    }

    static void Bloco(string nome, Sprite sprite, Vector2 pos, Vector2 tamanho, int ordem, bool colisao)
    {
        var go = new GameObject(nome);
        go.transform.position = pos;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = ordem;
        sr.drawMode = SpriteDrawMode.Tiled; // repete o sprite de 16x16 no tamanho do bloco
        sr.size = tamanho;
        if (colisao)
        {
            var box = go.AddComponent<BoxCollider2D>();
            box.size = tamanho;
        }
    }

    static void Parede(string nome, float x)
    {
        var go = new GameObject(nome);
        go.transform.position = new Vector3(x, 4f, 0f);
        go.AddComponent<BoxCollider2D>().size = new Vector2(1f, 20f);
    }

    // material sem atrito: evita o personagem ficar "grudado" na lateral das plataformas
    static PhysicsMaterial2D SemAtrito()
    {
        const string caminho = "Assets/SemAtrito.physicsMaterial2D";
        var mat = AssetDatabase.LoadAssetAtPath<PhysicsMaterial2D>(caminho);
        if (mat == null)
        {
            mat = new PhysicsMaterial2D("SemAtrito") { friction = 0f, bounciness = 0f };
            AssetDatabase.CreateAsset(mat, caminho);
        }
        return mat;
    }
}
